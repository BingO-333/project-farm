using UnityEngine;

namespace Game
{
    public class TimeScaleController : MonoBehaviour
    {
        [SerializeField, Range(1f, 5f)] float _timeScale = 1f;

#if UNITY_EDITOR
        private void Update()
        {
            Time.timeScale = _timeScale;
        }
#endif
    }
}
