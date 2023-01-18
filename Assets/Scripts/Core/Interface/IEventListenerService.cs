using System;
using Core.Enums;
using NM.Game.SO;

namespace NM.Core.Interface
{
    public interface IEventListenerService
    {
        public event Action<int> OnPlayerHealthChange;
        public event Func<bool> OnIsGameOver;
        public event Action OnGameOver;
        public event Action<BlockData> OnBlockDestroy;
        public event Action<BonusType> OnTakeBonus;
        public event Action OnMultipleBalls;
        public event Action OnLevelUp;

        public void InvokeOnPlayerHealthChange(int value);
        public bool InvokeIsGameOver();
        public void InvokeGameOver();
        public void InvokeOnBlockDestroy(BlockData blockData);
        public void InvokeOnTakeBonus(BonusType bonusType);
        public void InvokeOnMultipleBalls();
        public void InvokeOnLevelUp();
    }
}