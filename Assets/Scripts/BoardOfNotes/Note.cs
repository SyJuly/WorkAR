using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, ICancelButton {

    [SerializeField]
    public GameObject photoObject;

    public bool isUsed;

    public string cardId;

    private CancelButton cancelButton;

    public void OnCancel()
    {
        WriteToTrello.Instance.SendDeleteCardToTrello(cardId);
        DeactivateNote();
    }

    private void DeactivateNote()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        cancelButton = GetComponentInChildren<CancelButton>();
        cancelButton.receiver = this;
    }
}
