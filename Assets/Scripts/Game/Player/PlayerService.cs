using Core.Enums;
using NM.Constant;
using NM.Core.Interface;
using UnityEngine;
using Zenject;

namespace NM.Game.Player
{
    public class PlayerService : MonoBehaviour, IPlayerService
    {
        [SerializeField] private Rigidbody2D playerBody;
        [SerializeField] private float movementSpeed;
        [SerializeField] private Transform ballPoint;
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private BoxCollider2D boxCollider2D;

        private float normalScaleX;
        private float normalColliderSizeX;
        private Vector3 startPosition;
        private bool isHasBonus;

        public Rigidbody2D PlayerBody => playerBody;
        public Transform BallPoint => ballPoint;
        public int PlayerHealth { get; set; } = 3;

        private IEventListenerService eventListenerService;
        private IGameDataService gameDataService;
        private IAudioService audioService;

        [Inject]
        private void Construct(IEventListenerService eventListenerService, IGameDataService gameDataService,
            IAudioService audioService)
        {
            this.eventListenerService = eventListenerService;
            this.gameDataService = gameDataService;
            this.audioService = audioService;
        }

        private void OnEnable()
        {
            eventListenerService.OnPlayerHealthChange += OnPlayerHealthChange;
            eventListenerService.OnIsGameOver += OnIsGameOver;
            eventListenerService.OnTakeBonus += OnTakeBonus;
        }

        private void OnDisable()
        {
            eventListenerService.OnPlayerHealthChange -= OnPlayerHealthChange;
            eventListenerService.OnIsGameOver -= OnIsGameOver;
            eventListenerService.OnTakeBonus -= OnTakeBonus;
        }

        private void Start()
        {
            normalScaleX = playerSprite.size.x;
            normalColliderSizeX = boxCollider2D.size.x;
            startPosition = transform.position;
            if (gameDataService.IsGameContinue)
            {
                PlayerHealth = gameDataService.PlayerData.CurrentHealth + 1;
            }
        }

        private void FixedUpdate()
        {
            PlayerMovement();
        }

        public void PlayerMovement()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal == 0) return;
            var horizontalMovement = playerBody.position.x + horizontal * movementSpeed * Time.fixedDeltaTime;
            horizontalMovement = isHasBonus ? 
                Mathf.Clamp(horizontalMovement, ConstantHolder.MinGameBorderWithBonus, ConstantHolder.MaxGameBorderWithBonus) : 
                Mathf.Clamp(horizontalMovement, ConstantHolder.MinGameBorder, ConstantHolder.MaxGameBorder);
            var direction = new Vector2(horizontalMovement, playerBody.position.y);
            playerBody.MovePosition(direction);
        }

        public void ResetPlayer()
        {
            playerSprite.size = new Vector2(normalScaleX, playerSprite.size.y);
            boxCollider2D.size = new Vector2(normalColliderSizeX, boxCollider2D.size.y);
            isHasBonus = false;
            transform.position = startPosition;
        }

        private void OnTakeBonus(BonusType bonusType)
        {
            switch (bonusType)
            {
                case BonusType.MultipleBalls:
                    eventListenerService.InvokeOnMultipleBalls();
                    break;
                case BonusType.LargePlayer:
                    if (isHasBonus) return;
                    playerSprite.size = new Vector2(normalScaleX + 1f, playerSprite.size.y);
                    boxCollider2D.size = new Vector2(normalColliderSizeX + 1f, boxCollider2D.size.y);
                    isHasBonus = true;
                    break;
            }
            audioService.PlayOneShotAudioSound(AudioKey.TakePowerUp);
        }

        private void OnPlayerHealthChange(int damage)
        {
            PlayerHealth -= damage;
        }
        
        private bool OnIsGameOver()
        {
            return PlayerHealth <= 0;
        }
    }
}