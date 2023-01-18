using System;

namespace Core.Structures
{
    [Serializable]
    public struct PlayerData
    {
        public int CurrentHealth;
        public int CurrentScore;
        public int MaxScore;
        public int CurrentLevel;
    }
}