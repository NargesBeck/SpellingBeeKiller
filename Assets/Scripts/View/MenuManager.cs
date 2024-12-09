using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public static event Action<BaseMenu> OnMenuOpened;

    private GameObject EventSystem
    {
        get
        {
            var eventSystem = GameObject.FindObjectOfType<EventSystem>(true);
            if (eventSystem != null)
            {
                return eventSystem.gameObject;
            }
            Debug.LogError("Event System not found!");
            return null;
        }
    }

    public List<GameObject> MenuPrefabs;

    public Stack<BaseMenu> MenuStack = new Stack<BaseMenu>();

    public static MenuManager Instance { get; set; }

    private List<BaseMenu> Stack = new List<BaseMenu>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        // On Android the back button is sent as Esc
        if (Input.GetKeyDown(KeyCode.Escape) && MenuStack.Count > 0 && EventSystem.activeInHierarchy)
        {
            MenuStack.Peek().OnBackPressed();
        }
    }

    public GameObject CreateInstance<T>() where T : BaseMenu
    {
        var prefab = GetPrefab<T>();

        var menu = Instantiate<GameObject>(prefab.gameObject, transform);

        menu.SetActive(false);

        return menu;
    }

    public void OpenMenu(BaseMenu instance)
    {
        // De-activate top menu
        if (MenuStack.Count > 0)
        {
            var lastMenu = MenuStack.Peek();

            if (lastMenu == instance)
                return;

            if (instance.DisableMenusUnderneath)
            {
                foreach (var menu in MenuStack)
                {
                    menu.gameObject.SetActive(false);

                    if (menu.DisableMenusUnderneath)
                        break;
                }
            }

            instance.transform.SetAsLastSibling();
        }

        OnMenuOpened?.Invoke(instance);

        MenuStack.Push(instance);
    }

    private T GetPrefab<T>() where T : BaseMenu
    {
        foreach (var menuPrefab in MenuPrefabs)
        {
            if (menuPrefab == null)
            {
                Debug.LogError($"Prefab serialized field is null for type {typeof(T)}");
                // no need to handle. let it load from resources.
            }

            if (menuPrefab.GetComponent<T>() != null)
            {
                return menuPrefab.GetComponent<T>();
            }
        }

        // Load from resources
        string[] a = typeof(T).ToString().Split('.');
        string prefabName = a[a.Length - 1];
        GameObject loadprefab = Resources.Load<GameObject>("MenuPrefab/" + prefabName);

        if (loadprefab != null)
        {
            return loadprefab.GetComponent<T>();
        }
        else
            throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }

    public bool PrefabExists<T>() where T : BaseMenu
    {
        foreach (var menuPrefab in MenuPrefabs)
        {
            if (menuPrefab.GetComponent<T>() != null)
            {
                return true;
            }
        }

        // Load from resource
        GameObject loadprefab = Resources.Load<GameObject>("MenuPrefab/" + typeof(T).ToString());
        if (loadprefab != null)
        {
            return true;
        }
        else
            return false;
    }

    public void RemoveMenuFromTopOfStack(BaseMenu menu)
    {
        if (MenuStack.Count == 0)
        {
            string error = $"Bad menu managment, {menu.GetType()} " +
                $"cannot be closed because menu stack is empty, currentStack : {GetStackDetails()}";
            Debug.LogError(error);
            //AnalyticsManager.SendErrorEvent(GameAnalyticsSDK.GAErrorSeverity.Critical, error);
            throw new System.Exception(error);
        }

        if (MenuStack.Peek() != menu)
        {
            // check if we can fix stack
            var menusInStack = MenuStack.ToList();
            if (menusInStack.Remove(menu))
            {
                Debug.Log("recreated stack to remove something in the middle of stack");
                // recreate the stack
                MenuStack = new Stack<BaseMenu>(menusInStack);
                return;
            }
            else
            {
                // menu not found in stack
                // ok just ignore it
                return;
            }
        }

        // remove last item
        MenuStack.Pop();
    }

    public string GetStackDetails()
    {
        var list = MenuStack.ToList();
        StringBuilder output = new StringBuilder();
        foreach (var item in list)
        {
            output.Append(item.GetType());
        }
        return output.ToString();
    }

    public void RemoveMenu(BaseMenu menu)
    {
        if (menu.DestroyWhenClosed)
            Destroy(menu.gameObject);
        else
            menu.gameObject.SetActive(false);

        ReactivateTopMenu();
    }

    public void CloseAllMenus()
    {
        for (int i = 0; i < MenuStack.Count; i++)
        {
            var instance = MenuStack.Pop();

            if (instance.DestroyWhenClosed)
                Destroy(instance.gameObject);
            else
                instance.gameObject.SetActive(false);
        }
    }

    public BaseMenu PeekLastMenu()
    {
        if (MenuStack.Count == 0)
        {
            return null;
        }
        return MenuStack.Peek();
    }

    public void ReactivateTopMenu()
    {
        // Re-activate top menu
        // If a re-activated menu is an overlay we need to activate the menu under it
        foreach (var menu in MenuStack)
        {
            menu.gameObject.SetActive(true);
            menu.OnActivated();

            if (menu.DisableMenusUnderneath)
                break;
        }
    }

    public bool CheckIfMenuIsCurrentActiveMenu<T>() where T : BaseMenu
    {
        if (MenuStack.Count == 0)
        {
            return false;
        }
        var currentMenu = MenuStack.Peek();
        return currentMenu is T;
    }

    public bool CheckIfMenuIsInStack<T>() where T : BaseMenu
    {
        var list = MenuStack.ToList();
        foreach (var item in list)
        {
            if (item is T)
            {
                return true;
            }
        }
        return false;
    }

    public void SetActiveEventSystem(bool value)
    {
        EventSystem.SetActive(value);
    }
}