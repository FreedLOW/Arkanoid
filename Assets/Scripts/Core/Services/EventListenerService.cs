using System;
using Core.Enums;
using NM.Core.Interface;
using NM.Game.SO;
using UnityEngine;

namespace NM.Core.Services
{
    public class EventListenerService : MonoBehaviour, IEventListenerService
    {
        public event Action<int> OnPlayerHealthChange;
        public event Func<bool> OnIsGameOver;
        public event Action OnGameOver;
        public event Action<BlockData> OnBlockDestroy;
        public event Action<BonusType> OnTakeBonus;
        public event Action OnMultipleBalls;
        public event Action OnLevelUp;

        public void InvokeOnPlayerHealthChange(int value)
        {
            OnPlayerHealthChange?.Invoke(value);    
        }
        
        public bool InvokeIsGameOver()
        {
            var isGameOver = (bool) OnIsGameOver?.Invoke();
            return isGameOver;
        }

        public void InvokeGameOver()
        {
            OnGameOver?.Invoke();
        }

        public void InvokeOnBlockDestroy(BlockData blockData)
        {
            OnBlockDestroy?.Invoke(blockData);
        }

        public void InvokeOnTakeBonus(BonusType bonusType)
        {
            OnTakeBonus?.Invoke(bonusType);
        }

        public void InvokeOnMultipleBalls()
        {
            OnMultipleBalls?.Invoke();
        }

        public void InvokeOnLevelUp()
        {
            OnLevelUp?.Invoke();
        }
    }
}