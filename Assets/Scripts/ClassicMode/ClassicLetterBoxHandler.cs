using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassicLetterBoxHandler : MonoBehaviour
{
    private ClassicLevelMenu.LetterStates currentState = ClassicLevelMenu.LetterStates.NA;
    public ClassicLevelMenu.LetterStates CurrentState
    {
        get
        {
            if (currentState == ClassicLevelMenu.LetterStates.NA &&
                string.IsNullOrEmpty(TextUI.text) == false)
                currentState = ClassicLevelMenu.LetterStates.Filled;
            return currentState;
        }
    }

    [SerializeField] private Image BoxUI;
    [SerializeField] private Color ColorCorrect;
    [SerializeField] private Color ColorIncorrect;
    [SerializeField] private Color ColorIncorrectPlace;
    [SerializeField] private RTLTextMeshPro TextUI;

    public char GetLetter()
    {
        return TextUI.text[0];
    }

    public void SetState(ClassicLevelMenu.LetterStates state)
    {
        switch (state)
        {
            case ClassicLevelMenu.LetterStates.Correct: BoxUI.color = ColorCorrect; break;
            case ClassicLevelMenu.LetterStates.Incorrect: BoxUI.color = ColorIncorrect; break;
            case ClassicLevelMenu.LetterStates.IncorrectPlace: BoxUI.color = ColorIncorrectPlace; break;
        }
    }
}
