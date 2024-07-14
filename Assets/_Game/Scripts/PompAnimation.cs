using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Game
{
	public class PompAnimation : MonoBehaviour
	{
		[SerializeField] Transform _pompTube;
		[SerializeField] Transform _pompYPoint;
		[SerializeField] Transform _pompHandle;
		[Space]
		[SerializeField] float _handleAngle = 35f;

		private Tweener _animationTweener;

        private void OnDisable()
        {
			_animationTweener.KillIfActiveAndPlaying();
        }

        public void StartAnimation()
        {
			_animationTweener.KillIfActiveAndPlaying();
			_animationTweener = _pompHandle.DOLocalRotate(new Vector3(0, 0, _handleAngle), 0.5f)
				.ChangeStartValue(Vector3.zero)
				.SetLoops(-1, LoopType.Yoyo)
				.OnUpdate(() => {
					Vector3 pompTubePos = _pompTube.position;
					pompTubePos.y = _pompYPoint.position.y;

					_pompTube.position = pompTubePos;
				} );
        }

		public void StopAnimation()
        {
			_animationTweener.KillIfActiveAndPlaying();
        }
	}
}

