using Core.Enums;
using NM.Core.Interface;
using UnityEngine;
using Zenject;

namespace NM.Game.Level
{
    public class Bonus : MonoBehaviour
    {
        [SerializeField] private BonusType bonusType;
        [SerializeField] private Rigidbody2D bonsBody;
        [SerializeField] private float force;

        private int playerLayer;

        private IEventListenerService eventListenerService;

        [Inject]
        private void Construct(IEventListenerService eventListenerService)
        {
            this.eventListenerService = eventListenerService;
        }

        private void Start()
        {
            playerLayer = LayerMask.NameToLayer("Player");
            bonsBody.AddForce(Vector2.down * force);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == playerLayer)
            {
                eventListenerService.InvokeOnTakeBonus(bonusType);
                Destroy(gameObject);
            }
        }
    }
}