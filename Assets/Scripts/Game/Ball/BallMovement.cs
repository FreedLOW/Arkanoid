using Core.Enums;
using NM.Core.Interface;
using NM.Game.Interface;
using UnityEngine;
using Zenject;

namespace NM.Game.Ball
{
    public class BallMovement : MonoBehaviour, IBallMovement
    {
        [SerializeField] private Rigidbody2D ballBody;
        [SerializeField] private float force;
        
        private readonly RigidbodyType2D dynamicBall = RigidbodyType2D.Dynamic;
        private readonly RigidbodyType2D kinematicBall = RigidbodyType2D.Kinematic;
        private readonly float xForce = 150f;
        private readonly float limitMaxSpeed = 7f;
        private readonly float limitMinSpeed = 5f;
        
        private bool isBallActive;
        private float lastBallPositionX;
        
        public bool IsBallActive { get => isBallActive; set => isBallActive = value; }
        public Rigidbody2D BallBody { get => ballBody; set => ballBody = value; }

        private IEventListenerService eventListenerService;
        private IBallSpawnerService ballSpawnerService;
        private ILevelDataService levelDataService;
        private IAudioService audioService;

        [Inject]
        private void Construct(IEventListenerService eventListenerService, IBallSpawnerService ballSpawnerService,
            ILevelDataService levelDataService, IAudioService audioService)
        {
            this.eventListenerService = eventListenerService;
            this.ballSpawnerService = ballSpawnerService;
            this.levelDataService = levelDataService;
            this.audioService = audioService;
        }

        private void Awake()
        {
            eventListenerService.OnMultipleBalls += OnMultipleBalls;
        }

        private void OnDestroy()
        {
            eventListenerService.OnMultipleBalls -= OnMultipleBalls;
        }

        private void Update()
        {
            if (ballBody.velocity.magnitude > limitMaxSpeed)
            {
                ballBody.velocity = Vector2.ClampMagnitude(ballBody.velocity, limitMaxSpeed);
            }

            if (ballBody.velocity.magnitude < limitMinSpeed)
            {
                ballBody.velocity = Vector2.ClampMagnitude(ballBody.velocity, limitMinSpeed);
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && !isBallActive)
            {
                ActivateBall();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ballPositionX = transform.position.x;

            if (collision.gameObject.TryGetComponent(out IPlayerService player))
            {
                if (ballPositionX < lastBallPositionX + 0.05f && ballPositionX > lastBallPositionX - 0.05f)
                {
                    CalculateBallReflect(collision, player.PlayerBody.transform);
                }
                
                audioService.PlayOneShotAudioSound(AudioKey.HitBall);
            }
            
            lastBallPositionX = ballPositionX;
        }

        public void CalculateBallReflect(Collision2D collision, Transform collideObject)
        {
            var collisionPointX = collision.contacts[0].point.x;
            ballBody.velocity = Vector2.zero;
            var playerCenterPosition = collideObject.position.x;
            var difference = playerCenterPosition - collisionPointX;
            var direction = collisionPointX < playerCenterPosition ? -1 : 1;
            var forceDirection = new Vector2(direction * Mathf.Abs(difference * (force / 2)), force);
            ballBody.AddForce(forceDirection);   
        }
        
        public void ActivateBall()
        {
            lastBallPositionX = transform.position.x;
            isBallActive = true;
            transform.SetParent(null);
            ballBody.bodyType = dynamicBall;
            var randomDirection = new Vector2(Random.value > 0 ? xForce : -xForce, force);
            ballBody.AddForce(randomDirection);
        }

        public void DestroyBall()
        {
            if (levelDataService.Balls.Contains(gameObject))
                levelDataService.Balls.Remove(gameObject);
            Destroy(gameObject);
        }
        
        private void OnMultipleBalls()
        {
            for (int i = 0; i < 2; i++)
            {
                ballSpawnerService.CreateBall(transform.position);
            }
        }
    }
}