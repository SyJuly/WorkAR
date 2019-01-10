using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortButton : MonoBehaviour, IInputClickHandler
{
    Sorter sorter;

    bool isSortActivated;

    bool isFocused;

    public void OnFocusEnter()
    {
        isFocused = true;
    }

    public void OnFocusExit()
    {
        isFocused = false;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!isSortActivated)
        {
            sorter.ActivateSort();
        } else
        {
            sorter.DeactivateSort();
        }
        
    }
    
    void Start()
    {
        sorter = GetComponentInParent<Sorter>();
    }
}
