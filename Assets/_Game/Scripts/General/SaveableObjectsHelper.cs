using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SaveableObjectsHelper : MonoBehaviour
    {
        [Button] void CheckForSameNames()
        {
            ISaveable[] saveableObjects = GetComponentsInChildren<ISaveable>(true);

            bool hasSameNames = false;

            foreach (var s in saveableObjects)
            {
                string key = s.GetSaveKey();

                var objectsWithSameKey = saveableObjects.Where(saveableObject => saveableObject.GetSaveKey() == key).ToArray();

                if (objectsWithSameKey.Length > 1)
                {
                    Debug.Log($"Has identity key : {key}, count : {objectsWithSameKey.Length}");
                    hasSameNames = true;
                }
            }

            if (hasSameNames == false)
                Debug.Log("No objects with the same names were found");
        }

        [Button] void RenameAllSaveableObjects()
        {
            ISaveable[] saveableObjects = GetComponentsInChildren<ISaveable>(true);

            Dictionary<string, int> tagsCount = new Dictionary<string, int>();

            foreach (var s in saveableObjects)
            {
                string key = s.PrefsBaseTag;

                if (tagsCount.ContainsKey(key))
                    tagsCount[key] = tagsCount[key] + 1;
                else
                    tagsCount.Add(key, 0);

                int tagCount = tagsCount[key];

                s.SetSaveKey(key + (tagCount + 1));
            }
        }
    }
}
