using _Project.Systems.Game;
using UnityEngine;

namespace _Project.Gameplay.Customer
{
    public class Customer : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private CustomerMovement movement;

        private QueueSystem _queueSystem;
        private CheckoutSystem _checkoutSystem;


    }
}