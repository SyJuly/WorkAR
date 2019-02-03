using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.XR.WSA.Input;
using UnityEngine.EventSystems;
using HoloToolkit.Unity.InputModule;
using Vuforia;

public class PhotoCaptureWithHolograms : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    GameObject photoObject;
    
    private CameraOverlay cameraOverlay;

    PhotoCapture photoCaptureObject = null;
    InputNote inputNote = null;
    public Texture2D targetTexture = null;

    bool isCapturing = false;

    public bool isPhotoReadyToSend = false;

    private void Start()
    {
        inputNote = GetComponentInParent<InputNote>();
        cameraOverlay = Camera.main.GetComponentInChildren<CameraOverlay>();
        cameraOverlay.Capturer = this;
    }

    void StartCapturing()
    {
        VuforiaBehaviour.Instance.enabled = false;
        isCapturing = true;
        cameraOverlay.GetComponent<BoxCollider>().enabled = true;
        inputNote.StartCapturingMode();
    }

    void StopCapturing()
    {
        VuforiaBehaviour.Instance.enabled = true;
        isCapturing = false;
        cameraOverlay.GetComponent<BoxCollider>().enabled = false;
        inputNote.StopCapturingMode();
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
        StopCapturing();
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown our photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
        Debug.Log("DISPOSED AND STOPPED PHOTO MODE");
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!isCapturing)
        {
            StartCapturing();
        }
    }

    public void TakePhoto()
    {
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        // Create a PhotoCapture object
        PhotoCapture.CreateAsync(true, delegate (PhotoCapture captureObject) {
            Debug.Log("CREATE AND BEFORE START PHOTO MODE");
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