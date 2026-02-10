using _Project.Core.Event;
using _Project.Systems.Game;
using UnityEngine;

public class CheckoutSystemTestRunner : MonoBehaviour
{
    private CheckoutSystem _checkoutSystem;
    private EventManager _eventManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _eventManager = EventManager.Instance;
        _checkoutSystem = new CheckoutSystem(_eventManager);


        // Example test sequence
        _eventManager.Register<CheckoutStarted>(OnCheckoutStarted);
        _eventManager.Register<CheckoutItemScanned>(OnItemScanned);
        _eventManager.Register<CheckoutScanFinished>(OnScanFinished);
        _eventManager.Register<CheckoutPaymentReceived>(OnPaymentReceived);
        _eventManager.Register<CheckoutCompleted>(OnCheckoutCompleted);
        _eventManager.Register<CheckoutFailed>(OnCheckoutFailed);

        RunTests();
    }

    private void RunTests()
    {
        _checkoutSystem.StartCheckout("Customer_001");
        var item1 = CreateItem("Item_001", "Apple", 1.5f);
        _checkoutSystem.TryScanItem(item1);
        var item2 = CreateItem("Item_002", "Banana", 0.75f);
        _checkoutSystem.TryScanItem(item2);
        _checkoutSystem.TryFinishScan();
        _checkoutSystem.TryReceiveMoney(5.0m);
        CheckoutResult result;
        _checkoutSystem.TryCompleteCheckout(out result);
        _checkoutSystem.StartCheckout("Customer_002");
        _checkoutSystem.CancelCheckout();
    }

    // Các hàm listener để cập nhật UI
    private void OnCheckoutStarted(CheckoutStarted evt)
    {
        Debug.Log($"[UI] Bắt đầu phiên thanh toán cho khách: {evt.CustomerId}");
        // Cập nhật UI: Hiển thị thông tin khách hàng
    }

    private void OnItemScanned(CheckoutItemScanned evt)
    {
        Debug.Log($"[UI] Quét item: {evt.ItemId}, Giá: {evt.ItemPrice}, Tổng: {evt.NewTotal}, Số lượng: {evt.ItemCount}");
        // Cập nhật UI: Hiển thị item vừa quét và tổng tiền
    }

    private void OnScanFinished(CheckoutScanFinished evt)
    {
        Debug.Log($"[UI] Đã quét xong, Tổng tiền: {evt.FinalTotal}, Số lượng: {evt.ItemCount}");
        // Cập nhật UI: Hiển thị tổng tiền và số lượng item
    }

    private void OnPaymentReceived(CheckoutPaymentReceived evt)
    {
        Debug.Log($"[UI] Nhận tiền: {evt.PaidAmount}, Đã trả: {evt.TotalPaid}, Còn thiếu: {evt.Remaining}");
        // Cập nhật UI: Hiển thị số tiền đã trả và còn thiếu
    }

    private void OnCheckoutCompleted(CheckoutCompleted evt)
    {
        Debug.Log($"[UI] Hoàn tất thanh toán! Khách: {evt.Result.Customer}, Tổng: {evt.Result.Total}, Đã trả: {evt.Result.Paid}, Tiền thừa: {evt.Result.Change}");
        // Cập nhật UI: Hiển thị kết quả thanh toán
    }

    private void OnCheckoutFailed(CheckoutFailed evt)
    {
        Debug.Log($"[UI] Thanh toán thất bại cho khách: {evt.CustomerId}, Lý do: {evt.Reason}");
        // Cập nhật UI: Hiển thị lỗi
    }

    private ItemData CreateItem(string id, string name, float price)
    {
        var item = ScriptableObject.CreateInstance<ItemData>();
        item.ItemID = id;
        item.DisplayName = name;
        item.Price = price;
        return item;
    }
}
