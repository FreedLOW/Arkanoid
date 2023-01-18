using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Structures;
using NM.Constant;
using NM.Core.Interface;
using NM.Game.SO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace NM.UI
{
    public class GameMenu : MonoBehaviour
    {
        [Header("Pause")]
        [SerializeField] private CanvasGroup pausePanel;
        [SerializeField] private Button backButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button quitButton;

        [Header("Score")]
        [SerializeField] private TMP_Text currentScoreText;
        [SerializeField] private TMP_Text highScoreText;

        [Header("Level up")] 
        [SerializeField] private CanvasGroup levelUpPanel;
        [SerializeField] private TMP_Text levelText;
        
        [Header("Lives")]
        [SerializeField] private List<GameObject> playerLives;

        [Header("Game over")] 
        [SerializeField] private CanvasGroup gameOverPanel;
        [SerializeField] private TMP_Text scoreResultText;
        [SerializeField] private Button restartButton;

        private readonly int menSceneIndex = 0;
        private readonly string scoreResultPattern = "Your result \n{0}";
        private readonly string levelUpPattern = "Round  {0} \n \n \nReady";

        private int currentScore;
        private int highScore;
        private int livesIndex = 2;

        private IEventListenerService eventListenerService;
        private IGameDataService gameDataService;
        private ILevelDataService levelDataService;
        private IAudioService audioService;

        [Inject]
        private void Construct(IEventListenerService eventListenerService, IGameDataService gameDataService,
            ILevelDataService levelDataService, IAudioService audioService)
        {
            this.eventListenerService = eventListenerService;
            this.gameDataService = gameDataService;
            this.levelDataService = levelDataService;
            this.audioService = audioService;
        }

        private void OnEnable()
        {
            eventListenerService.OnPlayerHealthChange += OnPlayerHealthChange;
            eventListenerService.OnBlockDestroy += OnBlockDestroy;
            eventListenerService.OnGameOver += OnGameOver;
            eventListenerService.OnLevelUp += OnLevelUp;
        }

        private void OnDisable()
        {
            eventListenerService.OnPlayerHealthChange -= OnPlayerHealthChange;
            eventListenerService.OnBlockDestroy -= OnBlockDestroy;
            eventListenerService.OnGameOver -= OnGameOver;
            eventListenerService.OnLevelUp -= OnLevelUp;
        }

        private void Start()
        {
            ChangeTimeScale(false);
            if (gameDataService.IsGameContinue)
            {
                ScoreDataLoad();
                HealthDataLoad();   
            }
            else
            {
                var savedData = gameDataService.PlayerData;
                HighScoreLoad(savedData);
            }
            LevelCanvasUpdate();

            backButton.onClick.AddListener(BackToGame);
            saveButton.onClick.AddListener(SaveGameData);
            quitButton.onClick.AddListener(QuitToMenu);
            restartButton.onClick.AddListener(Restart);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && pausePanel.interactable == false)
            {
                ChangeStateOfGame(false);
            }
        }

        private void LevelCanvasUpdate()
        {
            levelText.text = String.Format(levelUpPattern, levelDataService.LevelIndex);
            StartCoroutine(UITool.State(levelUpPanel, true, 3f, 
                canvas =>
                {
                    UITool.State(ref canvas, false);
                    ChangeTimeScale(true);
                }));
        }

        private void ScoreDataLoad()
        {
            var savedData = gameDataService.PlayerData;
            currentScore = savedData.CurrentScore;
            currentScoreText.text = currentScore.ToString();
            levelDataService.LevelIndex = savedData.CurrentLevel;
            HighScoreLoad(savedData);
        }

        private void HighScoreLoad(PlayerData savedData)
        {
            highScore = savedData.MaxScore;
            highScoreText.text = highScore.ToString();
        }

        private void HealthDataLoad()
        {
            var savedData = gameDataService.PlayerData;
            var lives = savedData.CurrentHealth;

            if (lives == 1)
            {
                playerLives[^1].SetActive(false);
            }
            else if (lives == 0)
            {
                for (int i = playerLives.Count - 1; i > lives; i--)
                {
                    playerLives[i].SetActive(false);
                }
            }

            livesIndex = lives;
        }
        
        private void OnLevelUp()
        {
            ChangeTimeScale(false);
            audioService.PlayOneShotAudioSound(AudioKey.RoundEnd);
            LevelCanvasUpdate();
        }
        
        private void Restart()
        {
            gameDataService.LoadPlayerData();
            gameDataService.IsGameContinue = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        private void OnGameOver()
        {
            UITool.State(ref gameOverPanel, true);
            scoreResultText.text = string.Format(scoreResultPattern, currentScore);
            SavePlayerScoreData();
            Time.timeScale = 0;
        }

        private void OnBlockDestroy(BlockData blockData)
        {
            currentScore += blockData.Score;
            currentScoreText.text = currentScore.ToString();
        }
        
        private void OnPlayerHealthChange(int value)
        {
            playerLives[livesIndex].SetActive(false);
            livesIndex--;
        }

        private void QuitToMenu()
        {
            gameDataService.LoadPlayerData();
            gameDataService.LoadBlocksData();
            SceneManager.LoadScene(menSceneIndex);
        }

        private void SaveGameData()
        {
            if (!PlayerPrefs.HasKey(ConstantHolder.GameSaveKey))
                PlayerPrefs.SetInt(ConstantHolder.GameSaveKey, 0);

            BlockPositionsData blockPositionsData = new BlockPositionsData()
            {
                BlockPositions = levelDataService.BlocksPosition,
                BlockColors = levelDataService.BlockColors,
                FieldIndex = levelDataService.CurrentFieldIndex
            };
            gameDataService.SaveBlocksData(blockPositionsData);
            
            SavePlayerScoreData();
        }

        private void SavePlayerScoreData()
        {
            if (currentScore > highScore) highScore = currentScore;

            if (livesIndex <= 0)
            {
                livesIndex = 0;
            }

            PlayerData playerData = new PlayerData()
            {
                CurrentHealth = livesIndex,
                CurrentScore = currentScore,
                MaxScore = highScore,
                CurrentLevel = levelDataService.LevelIndex
            };
            gameDataService.SavePlayerData(playerData);
        }

        private void BackToGame()
        {
            ChangeStateOfGame(true);
        }

        private void ChangeStateOfGame(bool state)
        {
            ChangeTimeScale(state);
            UITool.State(ref pausePanel, !state);
        }

        private void ChangeTimeScale(bool isPlay)
        {
            Time.timeScale = isPlay ? 1 : 0;
        }
    }
}