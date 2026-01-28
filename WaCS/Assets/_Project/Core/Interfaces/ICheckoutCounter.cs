using _Project.Gameplay.Customer;

public interface ICheckoutCounter
{
    bool IsBusy { get; }
    void StartCheckout(Customer customer);
}