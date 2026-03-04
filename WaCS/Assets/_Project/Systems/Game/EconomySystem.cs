using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Core.Event;
using Project.Systems.SaveLoad;

namespace _Project.Systems.Game
{
    [Serializable]
    public class EconomySaveData
    {
        public decimal Balance;
    }

    public readonly struct EconomyTransactionCompleted
    {
        public readonly string TransactionId; // id của giao dịch
        public readonly decimal Revenue; // số tiền của giao dịch
        public readonly DateTime Time; // thời gian giao dịch

        public EconomyTransactionCompleted(string transactionId, decimal revenue)
        {
            TransactionId = transactionId;
            Revenue = revenue;
            Time = DateTime.UtcNow;
        }
    }


    // Hệ thống quản lý kinh tế trong game
    public class EconomySystem : IManager, ISaveable
    {
        private decimal _balance;
        private decimal _dailyRevenue;

        private readonly List<EconomyTransactionCompleted> _transactions = new();

        private EventManager _eventManager;

        public decimal Balance => _balance;
        public decimal DailyRevenue => _dailyRevenue;
        public IReadOnlyList<EconomyTransactionCompleted> Transactions => _transactions.AsReadOnly();

        public EconomySystem(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public async Task<bool> InitAsync()
        {
            _eventManager.Register<CheckoutCompleted>(OnTransactionCompleted);
            _eventManager.Register<CheckoutFailed>(OnTransactionFailed);
            SaveLoadService.Instance.RegisterSaveData(this);
            await Task.CompletedTask;
            return true;
        }

        // Thêm các phương thức quản lý kinh tế ở đây
        private void OnTransactionCompleted(CheckoutCompleted e)
        {
            _balance += e.Result.Total;
            _dailyRevenue += e.Result.Total;

            var transaction = new EconomyTransactionCompleted(
                e.Result.Customer,
                e.Result.Total
            );
            _transactions.Add(transaction);
        }

        private void OnTransactionFailed(CheckoutFailed e)
        {
            // Xử lý khi giao dịch thất bại nếu cần
        }

        public void ResetDailyRevenue()
        {
            _dailyRevenue = 0;
        }

        public void SaveState(GameData gameData)
        {
            gameData.Economy ??= new EconomySaveData();
            gameData.Economy.Balance = _balance;

        }

        public void LoadState(GameData gameData)
        {
            if(gameData.Economy == null)
                return;
            _balance = gameData.Economy.Balance;
        }
    }
}
