using System.Collections;
using System.Linq;
using UnityEngine;

namespace SmartUI 
{
	public class SwitchPage_SUI : MonoBehaviour
	{
		[SerializeField] private Page_SUI _showPage;
		[SerializeField] private Page_SUI _hidePage;

		private bool _isChanging;

		private void OnDisable()
		{
			 _isChanging = false;
		}

		public void ChangePages()
		{
			if (_isChanging)
				return;

			StartCoroutine(Changing());
		}

		private IEnumerator Changing()
		{
			_isChanging = true;

            _hidePage.StartHiding();

			yield return new WaitUntil(() =>
			{
				if (_hidePage.Status == PageStatus_SUI.Showing)
				{
					_isChanging = false; 
					StopAllCoroutines();
                }
				
				return _hidePage.Status == PageStatus_SUI.Hidden;
            });

            _showPage.StartShowing();

			_isChanging = false;
        }
	}
}