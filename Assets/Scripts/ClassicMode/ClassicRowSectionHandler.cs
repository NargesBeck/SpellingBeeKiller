using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicRowSectionHandler : MonoBehaviour
{
    [SerializeField] private GameObject LetterBoxPrefab;

    private Action<char[]> OnSubmit;
    private List<ClassicLetterBoxHandler> LetterBoxHandlers = new List<ClassicLetterBoxHandler>();

    public void Setup(int count, Action<char[]> onSubmit)
    {
        OnSubmit = onSubmit;
        for (int i = 0; i < count; i++)
        {
            CreateLetterBox();
        }
    }

    private void CreateLetterBox()
    {
        GameObject letterbox = Instantiate(LetterBoxPrefab, transform);
        LetterBoxHandlers.Add(letterbox.GetComponent<ClassicLetterBoxHandler>());
    }

    public void TrySubmitSection()
    {
        var na = LetterBoxHandlers.Find(x => x.CurrentState == ClassicLevelMenu.LetterStates.NA);
        if (na != null)
        {
            Debug.Log("Failed to submit");
            return;
        }
        char[] guess = new char[LetterBoxHandlers.Count];
        for (int i = 0; i < guess.Length; i++)
            guess[i] = LetterBoxHandlers[i].GetLetter();

        OnSubmit?.Invoke(guess);
    }

    public void GetResultOnInput(ClassicLevelMenu.LetterStates[] letterStates)
    {
        for (int i = 0; i < letterStates.Length; i++)
        {
            LetterBoxHandlers[i].SetState(letterStates[i]);
        }
    }
}
