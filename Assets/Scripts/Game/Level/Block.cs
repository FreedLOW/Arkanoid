using System.Collections.Generic;
using Core.Enums;
using NM.Core.Interface;
using NM.Game.SO;
using UnityEngine;
using Zenject;

namespace NM.Game.Level
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private BlockData blockData;
        [SerializeField] private Animator animator;
        [SerializeField] private List<Bonus> bonusList;
        [SerializeField] private BlockColor blockColor;
        
        private const int SpawnBonusChance = 10;
        private readonly int hitTrigger = Animator.StringToHash("Hit");

        private int blockHealth = 2;
        private int ballLayer;
        private bool isHasBonus;

        public Vector3 BlockPosition => transform.position;
        public BlockType BlockType => blockData.BlockType;
        public BlockColor BlockColor => blockColor;

        private IEventListenerService eventListenerService;
        private ILevelDataService levelDataService;
        private IAudioService audioService;
        private DiContainer container;

        [Inject]
        private void Construct(IEventListenerService eventListenerService, ILevelDataService levelDataService,
            IAudioService audioService, DiContainer container)
        {
            this.eventListenerService = eventListenerService;
            this.levelDataService = levelDataService;
            this.audioService = audioService;
            this.container = container;
        }

        private void Start()
        {
            ballLayer = LayerMask.NameToLayer("Ball");
            if (BlockColor.Equals(BlockType.NonDestructible)) return;
            var random = Random.Range(0, 101);
            isHasBonus = random % SpawnBonusChance == 0;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.layer == ballLayer)
            {
                switch (BlockType)
                {
                    case BlockType.Destructible:
                        DestroyBlock();
                        break;
                    case BlockType.NonDestructible:
                        if (animator != null) PlayAnimation();
                        audioService.PlayOneShotAudioSound(AudioKey.BlockDamage);
                        break;
                    case BlockType.TwoTimeDestructible:
                        blockHealth--;
                        if (blockHealth == 0)
                        {
                            DestroyBlock();
                            break;
                        }
                        if (animator != null) PlayAnimation();
                        audioService.PlayOneShotAudioSound(AudioKey.BlockDamage);
                        break;
                }
            }
        }

        private void DestroyBlock()
        {
            if (isHasBonus)
            {
                var bonus = bonusList[Random.Range(0, bonusList.Count)];
                var bonusGameObject = container.InstantiatePrefab(bonus);
                bonusGameObject.transform.position = transform.position;
            }
            audioService.PlayOneShotAudioSound(AudioKey.BlockDestroy);
            eventListenerService.InvokeOnBlockDestroy(blockData);
            levelDataService.RemoveBlock(this);
            Destroy(gameObject);
        }

        private void PlayAnimation()
        {
            animator.SetTrigger(hitTrigger);
        }
    }
}