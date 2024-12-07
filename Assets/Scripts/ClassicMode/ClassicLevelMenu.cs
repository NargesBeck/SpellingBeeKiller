using RTLTMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ClassicLevelMenu : MonoBehaviour
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
    [SerializeField] private RTLTextMeshPro MockInputTextUI;

    private List<ClassicRowSectionHandler> SectionHandlers = new List<ClassicRowSectionHandler>();

    public void MochShow()
    {
        Show(MockInputTextUI.text.Trim());
    }

    public void Show(string wordToGuess)
    {
        AttemptsLeft = 5;
        WordToGuess = new char[wordToGuess.Length - 1];
        for (int i = 0; i < WordToGuess.Length; i++) 
        {
            WordToGuess[i] = wordToGuess[i];
        }
        SectionHandlers = new List<ClassicRowSectionHandler>();
        SetMode(States.WaitingForInput);
    }

    private void SetMode(States state)
    {
        CurrentState = state;

        switch(state)
        {
            case States.WaitingForInput:
                GameObject row = Instantiate(ClassicRowSectionPrefab, RowsParent);
                SectionHandlers.Add(row.GetComponent<ClassicRowSectionHandler>());
                SectionHandlers.LastOrDefault().Setup(WordToGuess.Length, ReadInputFromRow);
                break;
        }
    }

    private void ReadInputFromRow(char[] guess)
    {
        var output = new LetterStates[guess.Length];
        for (int i = 0; i <= guess.Length; i++) 
        {
            // is correct
            if (guess[i] == WordToGuess[i])
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
}
