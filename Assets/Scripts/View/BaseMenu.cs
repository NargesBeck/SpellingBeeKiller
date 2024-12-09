using UnityEngine;

public abstract class BaseMenu : MonoBehaviour
{
    [Tooltip("Is this menu colsable by pressing back/secape button?")]
    public bool Closable = false;

    public bool HasInOutAnimation = false;
    internal Transform RootObject = null;
    internal UnityEngine.UI.Image BlackScreen;

    [Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
    public bool DestroyWhenClosed = true;

    [Tooltip("Disable menus that are under this one in the stack")]
    public bool DisableMenusUnderneath = true;

    public void GetAnimateAsset()
    {
        if (HasInOutAnimation)
        {
            BlackScreen = transform.GetComponent<UnityEngine.UI.Image>();
            RootObject = transform.GetChild(0);
        }
    }

    public abstract void OnBackPressed();

    public abstract void OnActivated();
}