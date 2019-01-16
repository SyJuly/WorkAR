using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class InteractionModellMarker : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;

    private InteractionModelLoader modelLoader;

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        modelLoader = GetComponentInParent<InteractionModelLoader>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.TRACKED)
        {
            modelLoader.Get3DModel();
        }
    }

}
