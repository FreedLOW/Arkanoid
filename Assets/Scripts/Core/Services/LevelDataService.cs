using System.Collections.Generic;
using Core.Enums;
using NM.Core.Interface;
using NM.Game.Level;
using UnityEngine;
using Zenject;

namespace NM.Core.Services
{
    public class LevelDataService : MonoBehaviour, ILevelDataService
    {
        [SerializeField] private int levelIndex;
        [SerializeField] private List<GameObject> gameFields;

        private GameObject currentGameField;
        private int fieldIndex;

        private List<Vector3> blocksPosition = new List<Vector3>();
        private List<BlockColor> blockColors = new List<BlockColor>();
        private List<Block> blocks = new List<Block>();
        private List<GameObject> balls = new List<GameObject>();

        public int LevelIndex { get => levelIndex; set => levelIndex = value; }
        public int CurrentFieldIndex { get; private set; }
        public List<Block> Blocks { get => blocks; set => blocks = value; }
        public List<Vector3> BlocksPosition { get => blocksPosition; set => blocksPosition = value; }
        public List<BlockColor> BlockColors { get => blockColors; set => blockColors = value; }
        public List<GameObject> Balls { get => balls; set => balls = value; }

        private IBlockSpawnerService blockSpawnerService;
        private IEventListenerService eventListenerService;
        private IPlayerService playerService;
        private IBallSpawnerService ballSpawnerService;
        private IAudioService audioService;
        private IGameDataService gameDataService;

        [Inject]
        private void Construct(IBlockSpawnerService blockSpawnerService, IEventListenerService eventListenerService, 
            IPlayerService playerService, IBallSpawnerService ballSpawnerService, IAudioService audioService,
            IGameDataService gameDataService)
        {
            this.blockSpawnerService = blockSpawnerService;
            this.eventListenerService = eventListenerService;
            this.playerService = playerService;
            this.ballSpawnerService = ballSpawnerService;
            this.audioService = audioService;
            this.gameDataService = gameDataService;
        }

        private void Awake()
        {
            if (gameDataService.IsGameContinue)
            {
                fieldIndex = gameDataService.BlockPositionsData.FieldIndex;
            }
            CreateGameField();
            audioService.PlayOneShotAudioSound(AudioKey.RoundStart);
        }

        private void Update()
        {
            if (blocks.Count > 0) return;
            LevelUp();
        }

        public void LevelUp()
        {
            var bonus = FindObjectsOfType<Bonus>();
            if (bonus.Length > 0)
            {
                for (int i = 0; i < bonus.Length; i++)
                {
                    Destroy(bonus[i].gameObject);
                }
            }
            blocks.Clear();
            blocksPosition.Clear();
            levelIndex++;
            CreateGameField();
            blockSpawnerService.SpawnBlocks();
            playerService.ResetPlayer();
            for (int i = 0; i < balls.Count; i++)
            {
                Destroy(balls[i]);
            }
            balls.Clear();
            ballSpawnerService.CreateBall();
            audioService.PlayOneShotAudioSound(AudioKey.RoundStart);
            eventListenerService.InvokeOnLevelUp();
        }

        public void RemoveBlock(Block block)
        {
            if (blocks.Contains(block))
            {
                blocks.Remove(block);
                blocksPosition.Remove(block.BlockPosition);
                blockColors.Remove(block.BlockColor);
            }
        }

        private void CreateGameField()
        {
            if (fieldIndex == gameFields.Count) fieldIndex = 0;
            if (currentGameField != null) Destroy(currentGameField);
            currentGameField = Instantiate(gameFields[fieldIndex]);
            CurrentFieldIndex = fieldIndex;
            fieldIndex++;
        }
    }
}