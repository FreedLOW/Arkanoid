using Core.Structures;

namespace NM.Core.Interface
{
    public interface IGameDataService
    {
        public bool IsGameContinue { get; set; }
        public PlayerData PlayerData { get; }
        public BlockPositionsData BlockPositionsData { get; }
        
        public void SavePlayerData(PlayerData playerDataToSave);
        public void SaveBlocksData(BlockPositionsData blockPositionsDataToSave);
        public void LoadPlayerData();
        public void LoadBlocksData();
    }
}