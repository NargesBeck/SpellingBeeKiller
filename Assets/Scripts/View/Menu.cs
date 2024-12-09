using System;

public abstract class Menu<T> : BaseMenu where T : Menu<T>
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = MenuManager.Instance.CreateInstance<T>().GetComponent<T>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    protected virtual void Awake()
    {
        _instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }

    public virtual void Show()
    {
        MenuManager.Instance.OpenMenu(Instance);
        Instance.gameObject.SetActive(true);
        Instance.OnActivated();
        //if (HasInOutAnimation) { }
    }

    public virtual void Show<F>(F variable) where F : class
    {
        MenuManager.Instance.OpenMenu(Instance);
        Instance.gameObject.SetActive(true);
        Instance.OnActivated();
    }
    public virtual void Hide(Action onComplete = null)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        MenuManager.Instance.RemoveMenuFromTopOfStack(Instance);
        //if (HasInOutAnimation) { } else
        MenuManager.Instance.RemoveMenu(Instance);
        onComplete?.Invoke();
    }
}