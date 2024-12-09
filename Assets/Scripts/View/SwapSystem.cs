using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class SwapSystem : MonoBehaviour
//, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static event Action<int> OnDragStart;
    public static event Action<int> OnDragEnd;

    Vector2 currentSwipe;
    public bool StartMove;
    public bool ScrollChange;
    public float movmentspeed;
    public float Multiplier;
    private Vector3 screenPoint;
    private Vector3 offset;

    public Transform BasePanel;
    public List<Transform> PanelsSector;
    public int IndexPanel = -1;
    public float StartToMoveDistance = 30;
    public float MovmentThreshold = 70;
    
    public void OnValueScrollVerticalChange()
    {
        ScrollChange = true;
    }

    public void ChangeToPanel(int index)
    {
        IndexPanel = index;
        Vector2 NextPos = new Vector2(-PanelsSector[index].transform.localPosition.x, 0);
        BasePanel.transform.DOLocalMove(NextPos, 0.5f).SetEase(Ease.OutExpo)/*.OnComplete(()=> { SwapFix(); })*/;
        OnPanelChange(index);
    }

    private void OnPanelChange(int panelIndex)
    {
        switch (panelIndex)
        {
            case 1: // home panel
                HomeMenu.Instance.OnActivated(); 
                break;
        }
    }
}