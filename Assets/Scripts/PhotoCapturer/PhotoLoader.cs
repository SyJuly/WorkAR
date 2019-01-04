/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_WINMD_SUPPORT
using System;
using Windows.Storage;
using System.Windows.Media.Imaging;
#endif

public class PhotoLoader : MonoBehaviour {

    List<Texture2D> targetTextures = null;

    void Start () {
        Debug.Log("Starting photo loader");
        targetTextures = new List<Texture2D>();
#if ENABLE_WINMD_SUPPORT
        checkFiles();
#endif
    }

#if ENABLE_WINMD_SUPPORT
    private async void VisualisePhotos(StorageFile file)
    {
        Image myImage = new Image();
        // load file from document library. Note: select document library in capabilities and declare .png file type

        BitmapImage img = new BitmapImage();
        img = await LoadImage(file);
        myImage.Source = img;

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        quadRenderer.material = new Material(Shader.Find("UI/Default"));

        quad.transform.parent = this.transform;
        quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);
        quad.AddComponent(myImage);
    }

    private async Task<BitmapImage> LoadImage(StorageFile file)
    {
        BitmapImage bitmapImage = new BitmapImage();
        FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

        bitmapImage.SetSource(stream);

        return bitmapImage;

    }


    private async void checkFiles()
    {
            Debug.Log("CAMERA ROLL:\n");
            StorageFolder folder = KnownFolders.CameraRoll;
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            if (files.Count == 0)
            {
                Debug.Log("no files...?");
            }
            else
            {
                foreach (var file in files)
                {
                    Debug.Log(file.Name + "\n");
                }
            }
    }
#endif
}*/
