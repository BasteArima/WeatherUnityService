using _Project.Scripts.Core.Network;
using _Project.Scripts.Features.Facts.Domain;
using _Project.Scripts.Features.Facts.Service;
using _Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Features.Facts.Presentation
{
    public class FactsPresenter : MonoBehaviour
    {
        [SerializeField] private FactsView _factsView;
        [SerializeField] private FactPopupView _popupView;

        [Inject] private IFactsService _factsService;
        [Inject] private IRequestQueue _requestQueue;

        private IRequest _currentFactsRequest;
        private IRequest _currentDetailRequest;

        private void OnEnable()
        {
            LoadFacts();
        }

        private void OnDisable()
        {
            CancelAllRequests();
        }

        private void LoadFacts()
        {
            CancelAllRequests();
            _factsView.ShowLoading(true);

            var req = new FactsRequest(_factsService,
                onSuccess: (facts) =>
                {
                    _factsView.ShowLoading(false);
                    _factsView.ShowFacts(facts, OnFactClicked);
                },
                onError: (ex) =>
                {
                    _factsView.ShowLoading(false);
                    Debug.LogError($"Facts error: {ex.Message}");
                }
            );

            _currentFactsRequest = req;
            _requestQueue.Enqueue(req);
        }

        private void OnFactClicked(FactInfo fact)
        {
            if (_currentDetailRequest != null)
            {
                _requestQueue.Remove(_currentDetailRequest);
                _requestQueue.CancelCurrent();
                _currentDetailRequest = null;
            }

            _popupView.ShowLoading();

            var detailReq = new FactDetailRequest(
                _factsService,
                fact.Id,
                onSuccess: (detail) =>
                {
                    if (_popupView != null)
                    {
                        _popupView.ShowDetail(detail);
                    }
                },
                onError: (ex) =>
                {
                    if (_popupView != null)
                        _popupView.Hide();
                    Debug.LogError($"Fact detail error: {ex.Message}");
                }
            );
            _currentDetailRequest = detailReq;
            _requestQueue.Enqueue(detailReq);
        }

        private void CancelAllRequests()
        {
            if (_currentFactsRequest != null)
            {
                _requestQueue.Remove(_currentFactsRequest);
                _requestQueue.CancelCurrent();
                _currentFactsRequest = null;
            }

            if (_currentDetailRequest != null)
            {
                _requestQueue.Remove(_currentDetailRequest);
                _requestQueue.CancelCurrent();
                _currentDetailRequest = null;
            }

            _factsView.ShowLoading(false);
            _popupView.Hide();
        }
    }
}