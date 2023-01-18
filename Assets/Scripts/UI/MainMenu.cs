using NM.Constant;
using NM.Core.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace NM.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button exitButton;

        private readonly int gameSceneIndex = 1;

        private IGameDataService gameDataService;

        [Inject]
        private void Construct(IGameDataService gameDataService)
        {
            this.gameDataService = gameDataService;
        }

        private void Start()
        {
            if (!PlayerPrefs.HasKey(ConstantHolder.GameSaveKey))
            {
                continueButton.interactable = false;
            }
            
            startButton.onClick.AddListener(StartGame);
            continueButton.onClick.AddListener(ContinueGame);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void ContinueGame()
        {
            PrepareLevel(true);
        }
        
        private void StartGame()
        {
            PrepareLevel(false);
        }
        
        private void PrepareLevel(bool isGameContinue)
        {
            gameDataService.IsGameContinue = isGameContinue;
            SceneManager.LoadScene(gameSceneIndex);
        }
    }
}