using _Project.Core.Event;
using _Project.Systems.Game;
using TMPro;
using UnityEngine;

public class CheckoutUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text totalText;
    [SerializeField] private TMP_Text remainingText;
    [SerializeField] private Transform scannedItemList;
    [SerializeField] private GameObject itemRowPrefab;

    private EventManager _eventManager;

    public void Init(EventManager eventManager)
    {
        _eventManager = eventManager;

        _eventManager.Register<CheckoutItemScanned>(OnItemScanned);
        _eventManager.Register<CheckoutScanFinished>(OnScanFinished);
        _eventManager.Register<CheckoutPaymentReceived>(OnPayment);
        _eventManager.Register<CheckoutCompleted>(OnCompleted);
    }

    private void OnItemScanned(CheckoutItemScanned e)
    {
        totalText.text = $"Total: {e.NewTotal:C}";
        AddItemUI(e.ItemId, e.ItemPrice);
    }

    private void OnScanFinished(CheckoutScanFinished e)
    {
        remainingText.text = $"Remaining: {e.FinalTotal:C}";
    }

    private void OnPayment(CheckoutPaymentReceived e)
    {
        remainingText.text = $"Remaining: {e.Remaining:C}";
    }

    private void OnCompleted(CheckoutCompleted e)
    {
        ClearUI();
    }

    private void AddItemUI(string itemId, decimal price)
    {
        var row = Instantiate(itemRowPrefab, scannedItemList);

        var texts = row.GetComponentsInChildren<TMP_Text>();
        if (texts.Length >= 2)
        {
            texts[0].text = itemId;
            texts[1].text = price.ToString("C");
        }
    }

    private void ClearUI()
    {
        foreach (Transform child in scannedItemList)
        {
            Destroy(child.gameObject);
        }

        totalText.text = "Total: 0";
        remainingText.text = "";
    }
}
