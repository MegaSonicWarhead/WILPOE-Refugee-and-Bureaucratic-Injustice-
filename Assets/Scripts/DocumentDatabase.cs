using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentDatabase : MonoBehaviour
{
    public static DocumentDatabase Instance;

    [System.Serializable]
    public struct DocumentEntry
    {
        public DocumentType documentType;
        public InventoryItemData itemData;
    }

    public List<DocumentEntry> documents;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Get the InventoryItemData for a DocumentType
    public InventoryItemData GetItemDataForDocument(DocumentType docType)
    {
        foreach (var entry in documents)
        {
            if (entry.documentType == docType)
                return entry.itemData;
        }
        return null;
    }
}
