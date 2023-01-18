using System;
using System.IO;
using Core.Structures;
using NM.Core.Interface;
using UnityEngine;

namespace NM.Core.Services
{
    public class GameDataService : MonoBehaviour, IGameDataService
    {
        private string playerSavePath;
        private string playerSaveFileName = "ScoreData.json";
        private string blockSavePath;
        private string blockSaveFileName = "BlockData.json";

        public bool IsGameContinue { get; set; }
        public PlayerData PlayerData { get; private set; }
        public BlockPositionsData BlockPositionsData { get; private set; }

        private void Awake()
        {
            playerSavePath = Path.Combine(Application.dataPath, playerSaveFileName);
            blockSavePath = Path.Combine(Application.dataPath, blockSaveFileName);
            
            LoadBlocksData();
            LoadPlayerData();
        }

        public void SavePlayerData(PlayerData playerDataToSave)
        {
            PlayerData playerData = new PlayerData()
            {
                CurrentHealth = playerDataToSave.CurrentHealth,
                CurrentScore = playerDataToSave.CurrentScore,
                MaxScore = playerDataToSave.MaxScore,
                CurrentLevel = playerDataToSave.CurrentLevel
            };
            
            var json = JsonUtility.ToJson(playerData, true);
            try
            {
                File.WriteAllText(playerSavePath, json);
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
        }
        
        public void SaveBlocksData(BlockPositionsData blockPositionsDataToSave)
        {
            BlockPositionsData blockPositionsData = new BlockPositionsData()
            {
                BlockColors = blockPositionsDataToSave.BlockColors,
                BlockPositions = blockPositionsDataToSave.BlockPositions,
                FieldIndex = blockPositionsDataToSave.FieldIndex
            };
            
            var json = JsonUtility.ToJson(blockPositionsData, true);
            try
            {
                File.WriteAllText(blockSavePath, json);
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
        }

        public void LoadPlayerData()
        {
            if (!File.Exists(playerSavePath)) return;

            try
            {
                var json = File.ReadAllText(playerSavePath);
                PlayerData = JsonUtility.FromJson<PlayerData>(json);
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
        }
        
        public void LoadBlocksData()
        {
            if (!File.Exists(blockSavePath)) return;

            try
            {
                var json = File.ReadAllText(blockSavePath);
                BlockPositionsData = JsonUtility.FromJson<BlockPositionsData>(json);
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
        }
    }
}