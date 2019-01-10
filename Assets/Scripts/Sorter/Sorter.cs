using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorter : MonoBehaviour
{
    [SerializeField]
    Material[] sortMaterials;

    private NoteColumnSortModifier[] sortModifier;
    private Renderer[] noteColumnsRenderer;
    private Dictionary<string, string> cardIdWithListId;
    private Dictionary<string, string> changedCardIdWithListId;
    private string[] listIdsOfColumns;

    private Material defaultMaterial;
    private TrelloBoardManager trelloBoardManager;

    private string activeColumnId;

    private void Start()
    {
        trelloBoardManager = GetComponent<TrelloBoardManager>();
    }

    void SetUpSorter()
    {
        
        sortModifier = GetComponentsInChildren<NoteColumnSortModifier>();
        noteColumnsRenderer = new Renderer[sortModifier.Length];
        listIdsOfColumns = new string[sortModifier.Length];
        cardIdWithListId = new Dictionary<string, string>();
        changedCardIdWithListId = new Dictionary<string, string>();
        for (int i = 0; i < sortModifier.Length; i++)
        {
            NoteColumnSortModifier columnSortModifier = sortModifier[i];
            columnSortModifier.sorter = this;
            noteColumnsRenderer[i] = columnSortModifier.GetComponent<Renderer>();
        }
        if (noteColumnsRenderer.Length > 0)
        {
            defaultMaterial = noteColumnsRenderer[0].material;
        }
    }
    
    public void ActivateSort()
    {
        trelloBoardManager.isResorting = true;
        SetUpSorter();
        for (int i = 0; i < sortModifier.Length; i++)
        {
            sortModifier[i].ActivateSortMode();
            noteColumnsRenderer[i].material = sortMaterials[i];
            NoteColumnSortModifier columnSortModifier = sortModifier[i];
            BoardColumn boardColumn = columnSortModifier.GetComponentInParent<BoardColumn>();
            string listId = boardColumn.list.id;
            listIdsOfColumns[i] = listId;
            NoteSortModifier[] notes = boardColumn.GetComponentsInChildren<NoteSortModifier>();
            for (int j = 0; j < notes.Length; j++)
            {
                NoteSortModifier noteSortModifier = notes[j];
                noteSortModifier.sorter = this;
                cardIdWithListId.Add(noteSortModifier.note.cardId, listId);
                noteSortModifier.SetColumnColor(sortMaterials[i]);
            }
            
        }
        activeColumnId = listIdsOfColumns[0];
    }

    public void DeactivateSort()
    {
        trelloBoardManager.isResorting = false;
        for (int i = 0; i < sortModifier.Length; i++)
        {
            sortModifier[i].DeactivateSortMode();
            noteColumnsRenderer[i].material = defaultMaterial;
        }
    }

    public void ClickedNote(NoteSortModifier noteSortModifier)
    {
        string previousListId = cardIdWithListId[noteSortModifier.note.cardId];
        bool listOfCardWasAlreadyChanged = changedCardIdWithListId.ContainsKey(noteSortModifier.note.cardId);
        if (previousListId != activeColumnId || listOfCardWasAlreadyChanged)
        {
            if (listOfCardWasAlreadyChanged)
            {
                changedCardIdWithListId.Remove(noteSortModifier.note.cardId);
            }
            changedCardIdWithListId.Add(noteSortModifier.note.cardId, activeColumnId);
            int index = System.Array.IndexOf(listIdsOfColumns, activeColumnId);
            noteSortModifier.SetColumnColor(sortMaterials[index]);
        }
    }

    public void ClickedList(NoteColumnSortModifier noteColumnSortModifier) {
        int index = System.Array.IndexOf(sortModifier, noteColumnSortModifier);
        activeColumnId = listIdsOfColumns[index];
    }
}
