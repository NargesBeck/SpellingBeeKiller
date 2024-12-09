using RTLTMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ClassicLevelMenu : Menu<ClassicLevelMenu>
{
    // instantiate row
    // row instantiates letter boxes
    // grade row
    // if won, end game
    // if attempts left, repeat

    public enum LetterStates { NA, Filled, Correct, IncorrectPlace, Incorrect }
    private enum States { WaitingForInput, GradeInput, Won, Lost }

    private States CurrentState;
    private int AttemptsLeft;
    private char[] WordToGuess;

    [SerializeField] private RectTransform RowsParent;
    [SerializeField] private GameObject ClassicRowSectionPrefab;

    private List<ClassicRowSectionHandler> SectionHandlers = new List<ClassicRowSectionHandler>();

    public void MockShow()
    {
        Show(new char[] { 'ز', 'ن', 'ب', 'و', 'ر' });
    }

    public void Show(char[] wordToGuess)
    {
        base.Show();
        AttemptsLeft = 5;
        WordToGuess = wordToGuess;
        SectionHandlers = new List<ClassicRowSectionHandler>();
        SetNewRow();
    }

    private void SetNewRow()
    {
        CurrentState = States.WaitingForInput;
        GameObject row = Instantiate(ClassicRowSectionPrefab, RowsParent);
        SectionHandlers.Add(row.GetComponent<ClassicRowSectionHandler>());
        SectionHandlers.LastOrDefault().Setup(WordToGuess.Length, ReadInputFromRow);
    }

    private void ReadInputFromRow(char[] guess)
    {
        CurrentState = States.GradeInput;
        var output = new LetterStates[guess.Length];

        for (int i = 0; i < guess.Length; i++)
        {
            // is correct
            char a = guess[i];
            char b = WordToGuess[i];
            if (a == b)
            {
                output[i] = LetterStates.Correct;
                continue;
            }

            // could find
            if (WordToGuess.Contains(guess[i]))
            {
                output[i] = LetterStates.IncorrectPlace;
                continue;
            }

            // letter not found
            output[i] = LetterStates.Incorrect;
        }
        SectionHandlers.LastOrDefault().GetResultOnInput(output);

        bool won = true;
        foreach (var state in output)
        {
            if (state != LetterStates.Correct)
                won = false;
        }
        OnRowSectionDone(won);
    }

    public void SubmitGuessClick()
    {
        if (SectionHandlers.Count == 0)
        {
            Debug.Log("Submit what section exactly, sir?");
            return;
        }

        SectionHandlers.LastOrDefault().TrySubmitSection();
    }

    private void OnRowSectionDone(bool won = false)
    {
        if (won)
        {
            CurrentState = States.Won;
            GameOverMenu.Instance.Show(won, 5, 3 + 3 * AttemptsLeft, AttemptsLeft);
            OnBackPressed();
            return;
        }

        AttemptsLeft--;

        if (AttemptsLeft <= 0)
        {
            CurrentState = States.Lost;
            GameOverMenu.Instance.Show(false, 1, 3 + 3 * AttemptsLeft, AttemptsLeft);
            return;
        }

        SetNewRow();
    }

    public override void OnBackPressed()
    {
        Hide();
    }

    public override void OnActivated()
    {
    }
}
