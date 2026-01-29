using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public class Customer
    {
        public Cart Cart { get; set; }

        public void OnCheckoutCompleted()
        {
            // TODO: Add event
        }

        public void OnCheckoutFailed()
        {
            // TODO: Add event
        }
    }
}