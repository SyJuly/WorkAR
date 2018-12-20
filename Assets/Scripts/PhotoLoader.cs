using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_WINMD_SUPPORT
using System;
using Windows.Storage;
#endif

public class PhotoLoader : MonoBehaviour {
    
	void Start () {
        Debug.Log("Starting photo loader");
#if ENABLE_WINMD_SUPPORT
        checkFiles();
#endif
    }

#if ENABLE_WINMD_SUPPORT
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
}
