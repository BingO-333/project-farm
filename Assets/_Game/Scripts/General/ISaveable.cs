using UnityEngine;

namespace Game
{
    public interface ISaveable
    {
        public string PrefsBaseTag { get; }

        public string GetSaveKey();
        public void SetSaveKey(string key);
    }
}
