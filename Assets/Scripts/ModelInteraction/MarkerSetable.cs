using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSetable : MonoBehaviour, IFocusable, IInputClickHandler {

    [SerializeField]
    Marker markerPrefab;

    private bool markerIsSettable = false;

    public void OnFocusEnter()
    {
        markerIsSettable = true;
    }

    public void OnFocusExit()
    {
        markerIsSettable = false;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (markerIsSettable)
        {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward);
            int hitIndex = GetHitIndexOnInteractableModel(hits);
            if(hitIndex >= 0) {
                Vector3 hitPoint = hits[hitIndex].point;
                GameObject markerGO = Instantiate(markerPrefab.gameObject);
                markerGO.transform.position = hitPoint;
                markerGO.transform.parent = transform;
            }
        }
    }

    private int GetHitIndexOnInteractableModel(RaycastHit[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            MarkerSetable hitModel = hits[i].collider.GetComponent<MarkerSetable>(); //attachedRigidbody.GetComponent
            if (hitModel != null)
            {
                return i;
            }
        }
        return -1;
    }
}
