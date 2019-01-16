using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortButton : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    Sorter sorter;

    [SerializeField]
    GameObject sortIcon;

    [SerializeField]
    GameObject confirmIcon;

    bool isSortActivated;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!isSortActivated)
        {
            sorter.ActivateSort();
            isSortActivated = true;
            ChangeIconToConfirm();
        } else
        {
            sorter.DeactivateSort();
            isSortActivated = false;
            SortIconToConfirm();
        }
        
    }

    private void ChangeIconToConfirm()
    {
        sortIcon.SetActive(false);
        confirmIcon.SetActive(true);
    }

    private void SortIconToConfirm()
    {
        confirmIcon.SetActive(false);
        sortIcon.SetActive(true);
    }
}
