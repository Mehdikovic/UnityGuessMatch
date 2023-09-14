using System.Collections.Generic;
using UnityEngine;

namespace ResourceItem {
    public static class DatabaseUtility {
        public static T GetItemById<T>(string path, ref Dictionary<int, T> id2Items, int id) where T : DatabaseItemSO {
            if (id2Items != null) {
                return id2Items[id];
            }

            id2Items = new();
            T[] listOfItems = null;

            GetAllItemsList(path, ref listOfItems);

            foreach (var item in listOfItems) {
                if (id2Items.ContainsKey(item.Id)) {
                    Debug.LogError(string.Format("There are some duplicated id for the type {0}", typeof(T).Name));
                    break;
                }
                id2Items.Add(item.Id, item);
            }

            return id2Items[id];
        }

        public static Dictionary<int, T> Build<T>(string path) where T : DatabaseItemSO {
            Dictionary<int, T> itemLookup = new();

            T[] listOfItems = null;
            GetAllItemsList(path, ref listOfItems);

            foreach (var item in listOfItems) {
                if (itemLookup.ContainsKey(item.Id)) {
                    Debug.LogError(string.Format("There are some duplicated id for the type {0}", typeof(T).Name));
                    break;
                }
                itemLookup.Add(item.Id, item);
            }

            return itemLookup;
        }

        public static void GetAllItemsList<T>(string path, ref T[] items) where T : DatabaseItemSO {
            if (items != null) return;
            items = Resources.LoadAll<T>(path);
        }
    }
}