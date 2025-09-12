using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentDatabase : MonoBehaviour
{
    public static DocumentDatabase Instance;

    [System.Serializable]
    public class DocumentEntry
    {
        public DocumentType documentType;
        public InventoryItemData itemData;
    }

    [Header("All Documents")]
    public List<DocumentEntry> documents = new List<DocumentEntry>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public InventoryItemData GetItemDataForDocument(DocumentType docType)
    {
        var entry = documents.Find(d => d.documentType == docType);
        return entry != null ? entry.itemData : null;
    }
}
