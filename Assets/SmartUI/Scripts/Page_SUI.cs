using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace SmartUI 
{
	[RequireComponent(typeof(CanvasGroup))]
	public class Page_SUI : MonoBehaviour
	{
		public PageStatus_SUI Status { get; private set; }

        private BaseElement_SUI[] _uiElements;

		private CanvasGroup _canvasGroup;

		private Coroutine _actionCoroutine;

		private bool _initialized;

		public virtual void StartShowing()
		{
			if (_initialized == false)
				Initialize();

			if (Status == PageStatus_SUI.Shown || Status == PageStatus_SUI.Showing)
				return;
			
			Status = PageStatus_SUI.Showing;

            gameObject.SetActive(true);            

            if (_actionCoroutine != null)
				StopCoroutine(_actionCoroutine);

			_actionCoroutine = StartCoroutine(Showing());
        }

		public virtual void StartHiding()
		{
            if (_initialized == false)
                Initialize();

            if (Status == PageStatus_SUI.Hidden || Status == PageStatus_SUI.Hiding)
				return;
            
            Status = PageStatus_SUI.Hiding;

            if (_actionCoroutine != null)
                StopCoroutine(_actionCoroutine);

            _actionCoroutine = StartCoroutine(Hiding());
        }

		protected virtual void OnShown() { }
		protected virtual void OnHidden() { }

		private void Initialize()
		{
            _canvasGroup = GetComponent<CanvasGroup>();
            _uiElements = GetComponentsInChildren<BaseElement_SUI>(true);

            Status = gameObject.activeSelf ? PageStatus_SUI.Shown : PageStatus_SUI.Hidden;

			_initialized = true;
        }

		private IEnumerator Showing()
		{
            foreach (var uiElement in _uiElements)
                uiElement.Show();
			            
			yield return new WaitUntil(() => _uiElements.All(e => e.Status == ElementStatus_SUI.Shown));

            _canvasGroup.interactable = true;

			Status = PageStatus_SUI.Shown;

			OnShown();
        }

		private IEnumerator Hiding()
		{
            _canvasGroup.interactable = false;

            foreach (var uiElement in _uiElements)
                uiElement.Hide();

			yield return new WaitUntil(() => _uiElements.All(e => e.Status == ElementStatus_SUI.Hidden));

            gameObject.SetActive(false);

			Status = PageStatus_SUI.Hidden;

            OnHidden();
        }
	}
}