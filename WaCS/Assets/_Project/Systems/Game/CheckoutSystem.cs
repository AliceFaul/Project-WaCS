using System;
using _Project.Gameplay.Customer;

namespace _Project.Systems.Game
{
    // tạo một struct dùng để lưu lại kết quả khi hoàn thành
    public readonly struct CheckoutResult
    {
        public readonly Customer Customer;
        public readonly float Change;
        public readonly float Total;
        public readonly float Paid;

        public CheckoutResult(CheckoutSession session)
        {
            Customer = session.Customer;
            Change = session.Change;
            Total = session.TotalPrice;
            Paid = session.Paid;
        }
    }

    public class CheckoutSystem
    {
        private CheckoutSession _currentSession;
        private bool IsBusy => _currentSession != null;

        // checkout event
        public event Action<CheckoutSession> OnCheckoutStarted;
        public event Action<CheckoutResult> OnCheckoutCompleted;
        public event Action<CheckoutSession> OnCheckoutFailed;

        public CheckoutSession CurrentSession { get { return _currentSession; } }
        
        // bắt đầu một session thanh toán
        public bool StartCheckout(Customer customer)
        {
            // nếu không có customer hoặc đang trong một session rồi
            // thì không thể bắt đầu một session mới
            if(customer == null || IsBusy) return false;
            _currentSession = new CheckoutSession(customer);
            OnCheckoutStarted?.Invoke(_currentSession);
            return true;
        }

        // hàm hoàn thành việc scanning item
        public void FinishScanning()
        {
            if(_currentSession == null) return;
            if(_currentSession.State != CheckoutState.Scanning) return;
            _currentSession.ScanCompleted();
        }

        // hàm nhận tiền từ customer
        public void ReceiveMoney(float money)
        {
            if(_currentSession == null) return;
            // state phải ở waitingForPayment thì người chơi mới nhận tiền được
            if(_currentSession.State != CheckoutState.WaitingForPayment) return;
            _currentSession.Pay(money);
        }

        // hàm hoàn thành session
        public bool TryConfirmCheckout()
        {
            if(_currentSession == null) return false;
            if(_currentSession.State != CheckoutState.WaitingForPayment) return false;
            if(!_currentSession.IsPaymentEnough()) return false;
            FinishCheckout();
            return true;
        }

        private void FinishCheckout()
        {
            _currentSession.CompleteSession();
            OnCheckoutCompleted?.Invoke(new CheckoutResult(_currentSession));
            _currentSession = null;
        }
        
        public void FailCheckout()
        {
            if(_currentSession == null) return;
            _currentSession.FailSession();
            OnCheckoutFailed?.Invoke(_currentSession);
            _currentSession = null;
        }
    }
}