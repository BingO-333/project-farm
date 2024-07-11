using System.Collections;
using System.Linq;
using UnityEngine;

namespace SmartUI 
{
	public class PageTurning_SUI : MonoBehaviour
	{
		[SerializeField] private bool _closeAllOnStart = true;

		[SerializeField] private Page_SUI[] _pages;

		private Coroutine _actionCoroutine;

		private int _currentPageIndex;

		private void Awake()
		{
			if (_closeAllOnStart)
				CloseAll();
		}

		public void ShowNextPage()
		{
			if (_actionCoroutine != null)
				StopCoroutine(_actionCoroutine);

			_actionCoroutine = StartCoroutine(ShowingNextPage());
		}

		public void ShowPreviousPage()
		{
            if (_actionCoroutine != null)
                StopCoroutine(_actionCoroutine);

            _actionCoroutine = StartCoroutine(ShowingPreviousPage());
        }

		public void CloseAll()
		{
			Page_SUI[] shownPages = _pages
				.Where(p => p.Status == PageStatus_SUI.Shown || p.Status == PageStatus_SUI.Showing).ToArray();

			foreach (Page_SUI page in shownPages)
				page.StartHiding();
		}

		private IEnumerator ShowingNextPage()
		{
            CloseAll();

            _currentPageIndex++;
            if (_currentPageIndex >= _pages.Length)
                _currentPageIndex = 0;

            yield return new WaitUntil(() => _pages.All(p => p.Status == PageStatus_SUI.Hidden));

            _pages[_currentPageIndex].StartShowing();
        }

		private IEnumerator ShowingPreviousPage()
		{
            CloseAll();

            _currentPageIndex--;
            if (_currentPageIndex < 0)
                _currentPageIndex = _pages.Length - 1;

            yield return new WaitUntil(() => _pages.All(p => p.Status == PageStatus_SUI.Hidden));

            _pages[_currentPageIndex].StartShowing();
        }
	}
}