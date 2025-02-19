using UnityEngine;
using UnityEngine.UI;
using System;
using _Project.Scripts.Features.Facts.Domain;
using TMPro;

namespace _Project.Scripts.UI
{
    public class FactsItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private FactInfo _fact;
        private Action<FactInfo> _onClick;

        public void Setup(FactInfo fact, Action<FactInfo> onClick)
        {
            _fact = fact;
            _onClick = onClick;
            _text.text = $"{fact.Name}";
        }

        public void OnClick()
        {
            _onClick?.Invoke(_fact);
        }
    }
}