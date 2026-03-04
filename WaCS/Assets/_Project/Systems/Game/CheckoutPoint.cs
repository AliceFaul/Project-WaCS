using _Project.Systems.Game;
using UnityEngine;

public class CheckoutPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var checkoutSystem = ServiceRegistry.Get<CheckoutSystem>();
        checkoutSystem?.RegisterCheckoutPosition(transform);
    }
}
