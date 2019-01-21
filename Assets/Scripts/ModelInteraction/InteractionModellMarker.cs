using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class InteractionModellMarker : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;

    public InteractionModelLoader ModelLoader { get; set; }

    [SerializeField]
    GameObject loadingobject;

    void Awake()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (ModelLoader && newStatus == TrackableBehaviour.Status.TRACKED)
        {
            loadingobject.SetActive(true);
            bool validRequest = ModelLoader.Get3DModel();
        } else
        {
            loadingobject.SetActive(false);
        }
    }

}
