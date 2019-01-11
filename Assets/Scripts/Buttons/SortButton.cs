using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortButton : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    Sorter sorter;

    bool isSortActivated;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!isSortActivated)
        {
            sorter.ActivateSort();
            isSortActivated = true;
        } else
        {
            sorter.DeactivateSort();
            isSortActivated = false;
        }
        
    }
}
