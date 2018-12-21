using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.XR.WSA.Input;
using UnityEngine.EventSystems;
using HoloToolkit.Unity.InputModule;

public class PhotoCaptureWithHolograms : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    GameObject photoObject;

    PhotoCapture photoCaptureObject = null;
    public Texture2D targetTexture = null;

    bool isCapturing = false;

    public bool isPhotoReadyToSend = false;

    void StartCapturing()
    {
        isCapturing = true;
        InputManager.Instance.AddGlobalListener(gameObject);
    }

    void StopCapturing()
    {
        isCapturing = false;
        if (InputManager.Instance != null)
        {
            InputManager.Instance.RemoveGlobalListener(gameObject);
        }
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        // Copy the raw image data into our target texture
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);

        // Create a gameobject that we can apply our texture to
        Renderer quadRenderer = photoObject.GetComponent<Renderer>() as Renderer;
        quadRenderer.material = new Material(Shader.Find("UI/Default"));

        quadRenderer.material.SetTexture("_MainTex", targetTexture);
        isPhotoReadyToSend = true;
        // Deactivate our camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        OnStoppedPhotoMode(result);
        StopCapturing();
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown our photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!isCapturing)
        {
            StartCapturing();
        } else
        {
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        // Create a PhotoCapture object
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 1.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
                // Take a picture
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }
}