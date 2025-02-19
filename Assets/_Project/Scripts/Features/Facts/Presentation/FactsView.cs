using UnityEngine;
using System;
using _Project.Scripts.Features.Facts.Domain;
using _Project.Scripts.UI;

namespace _Project.Scripts.Features.Facts.Presentation
{
    public class FactsView : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private FactsItemView _factItemPrefab;
        [SerializeField] private GameObject _loadingIndicator;

        public void ShowLoading(bool show)
        {
            _loadingIndicator.SetActive(show);
        }

        public void ShowFacts(FactInfo[] facts, Action<FactInfo> onFactClick)
        {
            ClearItems();
            if (facts == null) return;

            foreach (var fact in facts)
            {
                var itemView = Instantiate(_factItemPrefab, _container);
                itemView.Setup(fact, onFactClick);
            }
        }

        private void ClearItems()
        {
            for (int i = _container.childCount - 1; i >= 0; i--)
                Destroy(_container.GetChild(i).gameObject);
        }
    }
}