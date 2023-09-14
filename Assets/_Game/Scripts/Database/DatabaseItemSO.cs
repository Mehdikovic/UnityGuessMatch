using UnityEngine;

namespace ResourceItem {
    public class DatabaseItemSO : ScriptableObject, ISerializationCallbackReceiver {
        [field: SerializeField] public int Id { get; private set; } = 0;
        [field: SerializeField] public string NameString { get; private set; }

#if UNITY_EDITOR
        static private readonly System.Collections.Generic.Dictionary<int, DatabaseItemSO> id2DatabaseItem = new();

        private bool IsUnique(int candidateID, DatabaseItemSO databaseItem) {
            if (candidateID == 0) { return false; }

            if (!id2DatabaseItem.ContainsKey(candidateID)) {
                return true;
            }

            if (databaseItem != null && id2DatabaseItem[candidateID] == databaseItem) {
                return true;
            }

            if (id2DatabaseItem[candidateID] == null) {
                id2DatabaseItem.Remove(candidateID);
                return true;
            }

            return false;
        }

        private void Add(int candidateID, DatabaseItemSO databaseItem) {
            if (candidateID == 0) { return; }
            if (id2DatabaseItem.ContainsKey(candidateID)) { return; }
            
            id2DatabaseItem[candidateID] = databaseItem;
        }
#endif

        public void OnBeforeSerialize() {
#if UNITY_EDITOR
            if (Id == 0) {
                do {
                    int newId = System.Guid.NewGuid().GetHashCode();
                    if (IsUnique(newId, this)) {
                        Add(newId, this);
                        Id = newId;
                        break;
                    }
                }
                while (Id == 0 || !IsUnique(Id, this));
            } else {
                while (!IsUnique(Id, this)) {
                    int newId = System.Guid.NewGuid().GetHashCode();
                    if (IsUnique(newId, this)) {
                        Id = newId;
                        break;
                    }
                }
                Add(Id, this);
            }
#endif
        }

        public void OnAfterDeserialize() {
#if UNITY_EDITOR
#endif
        }
    }
}