using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorter : MonoBehaviour
{
    [SerializeField]
    Material[] sortMaterials;

    private NoteColumnSortModifier[] listSortModifiers;
    private List<NoteSortModifier> noteSortModifiers;
    private Dictionary<string, string> cardIdWithListId;
    private Dictionary<string, string> changedCardIdWithListId;
    private string[] listIdsOfColumns;

    private Material defaultColumnMaterial;
    private Material defaultCellMaterial;
    private TrelloBoardManager trelloBoardManager;
    private WriteToTrello writer;

    private string activeColumnId;

    private void Start()
    {
        trelloBoardManager = GetComponent<TrelloBoardManager>();
        writer = WriteToTrello.Instance;
    }

    void SetUpSorter()
    {

        listSortModifiers = GetComponentsInChildren<NoteColumnSortModifier>();
        noteSortModifiers = new List<NoteSortModifier>();
        listIdsOfColumns = new string[listSortModifiers.Length];
        cardIdWithListId = new Dictionary<string, string>();
        changedCardIdWithListId = new Dictionary<string, string>();
        for (int i = 0; i < listSortModifiers.Length; i++)
        {
            listSortModifiers[i].sorter = this;
        }
        if (listSortModifiers.Length > 0)
        {
            defaultColumnMaterial = listSortModifiers[0].meshRenderer.material;
        }
    }
    
    public void ActivateSort()
    {
        trelloBoardManager.isResorting = true;
        SetUpSorter();
        for (int i = 0; i < listSortModifiers.Length; i++)
        {
            listSortModifiers[i].ActivateSortMode();
            listSortModifiers[i].meshRenderer.material = sortMaterials[i];
            NoteColumnSortModifier columnSortModifier = listSortModifiers[i];
            BoardColumn boardColumn = columnSortModifier.boardColumn;
            boardColumn.dictationNoteColumn.SetActive(false);
            string listId = boardColumn.list.id;
            listIdsOfColumns[i] = listId;
            NoteSortModifier[] noteSortModifiersOfList = boardColumn.GetComponentsInChildren<NoteSortModifier>();
            for (int j = 0; j < noteSortModifiersOfList.Length; j++)
            {
                NoteSortModifier noteSortModifier = noteSortModifiersOfList[j];
                if (defaultCellMaterial == null)
                {
                    defaultCellMaterial = noteSortModifier.meshRenderer.material;
                }
                noteSortModifiers.Add(noteSortModifier);
                noteSortModifier.sorter = this;
                cardIdWithListId.Add(noteSortModifier.note.cardId, listId);
                noteSortModifier.meshRenderer.material = sortMaterials[i];
            }
            
        }
        SetActiveColumn(0);
    }

    public void DeactivateSort()
    {
        trelloBoardManager.isResorting = false;
        for (int i = 0; i < listSortModifiers.Length; i++)
        {
            NoteColumnSortModifier noteColumnSortModifier = listSortModifiers[i];
            noteColumnSortModifier.DeactivateSortMode();
            noteColumnSortModifier.meshRenderer.material = defaultColumnMaterial;
            noteColumnSortModifier.boardColumn.dictationNoteColumn.SetActive(true);
        }
        foreach (NoteSortModifier card in noteSortModifiers)
        {
            card.meshRenderer.material = defaultCellMaterial;
        }
        foreach (KeyValuePair<string, string> changedCardPair in changedCardIdWithListId)
        {
            writer.SendReorderedCardToTrello(changedCardPair.Key, changedCardPair.Value);
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
            noteSortModifier.meshRenderer.material = sortMaterials[index];
        }
    }

    public void ClickedList(NoteColumnSortModifier noteColumnSortModifier) {
        int index = System.Array.IndexOf(listSortModifiers, noteColumnSortModifier);
        SetActiveColumn(index);
    }

    private void SetActiveColumn(int index)
    {
        activeColumnId = listIdsOfColumns[index];
        CursorFeedback.Instance.ToggleSortModeFeedback(sortMaterials[index]);
    }
}
