using _Project.Gameplay.Player;
using _Project.Systems.Game;
using Project.Systems.Game;
using TMPro;
using UnityEngine;

namespace _Project.UI.Screens
{
    public class PriceUI : MonoBehaviour
    {
        [SerializeField] private GameObject priceTable;
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_InputField priceInput;

        private ShelfPoint _currentPoint;

        private void Start()
        {
            Close();
        }

        private void Update()
        {
            if (PlayerController.Instance.Context.Input.Escape &&
                priceTable.activeSelf)
            {
                Close();
            }
        }

        public void Open(ShelfPoint point)
        {
            _currentPoint = point;
            priceTable.SetActive(true);
            itemNameText.text = $"Enter price for {point.ItemType.DisplayName}";
            priceInput.text = point.Price.ToString("0.00");
            PauseSystem.PauseGame(true);
        }

        public void Confirm()
        {
            if (_currentPoint == null)
                return;
            if (float.TryParse(priceInput.text, out float newPrice))
            {
                _currentPoint.SetPrice(newPrice);
                Close();
            }
            else
            {
                Debug.LogWarning("Invalid price input: " + priceInput.text);
            }
        }

        public void Close()
        {
            priceTable.SetActive(false);
            _currentPoint = null;
            PauseSystem.PauseGame(false);
        }
    }
}
