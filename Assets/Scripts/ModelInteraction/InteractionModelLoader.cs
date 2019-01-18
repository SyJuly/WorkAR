using AsImpL;
using System.Collections;
using UnityEngine;

public class InteractionModelLoader : ObjectImporter
{
    [SerializeField]
    GameObject interactionModelPrefab;

    [SerializeField]
    GameObject notLoadingCube;

    [SerializeField]
    GameObject loadingCube;

    [SerializeField]
    float modelScaling = 1f;

    InteractionModel interactionModelParent;

    private bool isImporting;

    private string url;

    private GameObject tempModel;

    private void Import3DModel()
    {
        isImporting = true;
        WebManager.Instance.Trello.ModelLoader.Get3DModel(OnUrlReceive);
    }

    private void OnUrlReceive(string result)
    {
        StartCoroutine(ImportObjFromUrl(result));
    }

    protected override void OnImportingComplete()
    {
        if (isImporting)
        {
            ActivateSingleCollider(interactionModelParent.gameObject);
            //TranslateModelInFrontOfWidget();
            isImporting = false;
            ShowNotLoading();
        }
    }

    private void TranslateModelInFrontOfWidget()
    {
        interactionModelParent.transform.Translate(1,1,1, Camera.main.transform);
    }

    private void ActivateSingleCollider(GameObject gameObject)
    {
        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Collider colliderToDeactivate = meshFilters[i].GetComponent<Collider>();
            if (colliderToDeactivate)
            {
                colliderToDeactivate.enabled = false;
            }

            i++;
        }
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        if(filter == null)
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }
        filter.mesh = new Mesh();
        filter.mesh.CombineMeshes(combine);
        MeshCollider collider = gameObject.GetComponent<MeshCollider>();
        if (collider == null)
        {
            gameObject.AddComponent<MeshCollider>();
        }
        collider.enabled = true;
        collider.sharedMesh = filter.mesh;

    }

    IEnumerator ImportObjFromUrl(string url)
    {
        ObjectImporter objectLoadedImporter = this;
        ImportOptions options = new ImportOptions();
        options.reuseLoaded = true;
        options.inheritLayer = false;
        options.modelScaling = modelScaling;
        options.localPosition = Vector3.zero;
        Debug.Log("start import model async");
        objectLoadedImporter.ImportModelAsync("model", url, interactionModelParent.transform, options);
        yield return null;
    }

    public void Get3DModel()
    {
        if (tempModel == null)
        {
            ShowLoading();
            Initialise3DModel();
            Import3DModel();
        }
    }

    private void Initialise3DModel()
    {
        tempModel = Instantiate(interactionModelPrefab);
        interactionModelParent = tempModel.GetComponentInChildren<InteractionModel>();
    }

    private void ShowLoading()
    {
        notLoadingCube.SetActive(false);
        loadingCube.SetActive(true);
        
    }

    private void ShowNotLoading()
    {
        loadingCube.SetActive(false);
        notLoadingCube.SetActive(true);
    }
}
