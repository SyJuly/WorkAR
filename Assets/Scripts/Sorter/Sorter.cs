using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorter : MonoBehaviour
{
    [SerializeField]
    Material[] sortMaterials;

    private NoteColumnSortModifier[] sortModifier;
    private Renderer[] noteColumnsRenderer;

    private Material defaultMaterial;


    void SetUpSorter()
    {
        sortModifier = GetComponentsInChildren<NoteColumnSortModifier>();
        noteColumnsRenderer = new Renderer[sortModifier.Length];
        for(int i = 0; i < sortModifier.Length; i++)
        {
            noteColumnsRenderer[i] = sortModifier[i].GetComponent<Renderer>();
        }
        if (noteColumnsRenderer.Length > 0) {
            defaultMaterial = noteColumnsRenderer[0].material;
        }
    }
    
    public void ActivateSort()
    {
        SetUpSorter();
        for (int i = 0; i < sortModifier.Length; i++)
        {
            sortModifier[i].ActivateSortMode();
            SetSortMaterial(i);
        }
    }

    public void DeactivateSort()
    {
        for (int i = 0; i < sortModifier.Length; i++)
        {
            sortModifier[i].DeactivateSortMode();
            noteColumnsRenderer[i].material = defaultMaterial;
        }
    }

    public void SetSortMaterial(int i)
    {
        noteColumnsRenderer[i].material = sortMaterials[i];
    }
}
