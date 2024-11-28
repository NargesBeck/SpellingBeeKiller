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

    private enum States { WaitingForInput, GradeInput, Won, Lost }
    public enum LetterStates { Correct, IncorrectPlace, Incorrect }

    private States CurrentState;
    private int AttemptsLeft;
    private string WordToGuess;

    [SerializeField] private RectTransform RowsParent;
    [SerializeField] private GameObject ClassicRowSectionPrefab;

    private List<ClassicRowSectionHandler> SectionHandlers;

    private void Start()
    {
        Show("Hello");
    }

    public void Show(string wordToGuess)
    {
        AttemptsLeft = 5;
        WordToGuess = wordToGuess;
        SectionHandlers = new List<ClassicRowSectionHandler>();
        SetMode(States.WaitingForInput);
    }

    private void SetMode(States state)
    {
        CurrentState = state;

        switch(state)
        {
            case States.WaitingForInput:
                GameObject row = Instantiate(ClassicRowSectionPrefab);
                SectionHandlers.Add(row.GetComponent<ClassicRowSectionHandler>());
                SectionHandlers.LastOrDefault().Setup(WordToGuess.Length, ReadInputFromRow);
                break;
        }
    }

    private void ReadInputFromRow(char[] guess)
    {
        SectionHandlers.LastOrDefault().GetResultOnInput(null);
    }
}
