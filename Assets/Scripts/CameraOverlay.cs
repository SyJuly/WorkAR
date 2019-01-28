using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOverlay : MonoBehaviour,IInputClickHandler
{
    public PhotoCaptureWithHolograms Capturer { private get; set; }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (Capturer != null)
        {
            Capturer.TakePhoto();
        }
    }
}
