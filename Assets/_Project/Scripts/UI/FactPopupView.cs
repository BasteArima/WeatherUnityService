using _Project.Scripts.Features.Facts.Domain;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class FactPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private GameObject _loadingIndicator;

        public void ShowLoading()
        {
            _loadingIndicator.SetActive(true);
            gameObject.SetActive(true);
            _title.text = "";
            _description.text = "";
        }

        public void ShowDetail(FactDetail detail)
        {
            _loadingIndicator.SetActive(false);
            gameObject.SetActive(true);
            _title.text = detail.Name;
            _description.text = detail.Description;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}