using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using NM.Core.Interface;
using NM.Game.Level;
using NM.Game.SO;
using UnityEngine;
using Zenject;

namespace NM.Core.Services
{
    public class BlockSpawnerService : MonoBehaviour, IBlockSpawnerService
    {
        [SerializeField] private List<BlockData> blocks;

        private int blocksInLine = 13;
        private int rightToLeftBlocksInLine = 14;
        private int leftToRightBlocksInLine = -1;

        private IGameDataService gameDataService;
        private ILevelDataService levelDataService;
        private DiContainer container;

        [Inject]
        private void Construct(IGameDataService gameDataService,
            ILevelDataService levelDataService, DiContainer container)
        {
            this.gameDataService = gameDataService;
            this.levelDataService = levelDataService;
            this.container = container;
        }
        
        private void Awake()
        {
            if (gameDataService.IsGameContinue)
            {
                var blockTypes = gameDataService.BlockPositionsData.BlockColors;
                var blockPositions = gameDataService.BlockPositionsData.BlockPositions;
                SpawnBlocks(blockTypes, blockPositions);
            }
            else
            {
                SpawnBlocks();
            }
        }

        public void SpawnBlocks()
        {
            while (blocks[0].BlockType.Equals(BlockType.NonDestructible))
            {
                ShuffleBlockData();
            }
            
            rightToLeftBlocksInLine = 14;
            var index = 0;

            foreach (var block in blocks)
            {
                if (block.BlockType.Equals(BlockType.NonDestructible) && 
                    (levelDataService.LevelIndex == 1 || levelDataService.LevelIndex == 2)) continue;

                for (int i = 0; i < blocksInLine; i++)
                {
                    if ((i <= leftToRightBlocksInLine || i >= rightToLeftBlocksInLine - 1) &&
                        levelDataService.LevelIndex % 3 == 0)
                    {
                        index++;
                        continue;
                    }

                    if (i <= leftToRightBlocksInLine && levelDataService.LevelIndex % 4 == 0)
                    {
                        rightToLeftBlocksInLine = 14;
                        index++;
                        continue;
                    }

                    if (i >= rightToLeftBlocksInLine - 1 && levelDataService.LevelIndex % 2 == 0)
                    {
                        leftToRightBlocksInLine = -1;
                        index++;
                        continue;
                    }
                    
                    var x = (index % blocksInLine) * block.BlockWidth;
                    var y = (index / blocksInLine) * block.BlockHeight;
                    var position = new Vector3(x, y, 0);
                    InstantiateBlockObject(block, position);
                    index++;
                }
                
                rightToLeftBlocksInLine--;
                leftToRightBlocksInLine++;
            }
        }

        public void SpawnBlocks(List<BlockColor> blockColors, List<Vector3> blockPositions)
        {
            for (int i = 0; i < blockColors.Count; i++)
            {
                var index = i;
                var data = blocks.
                    Select(blockData => blockData).
                    Where(blockData => 
                    blockData.BlockPrefab.GetComponent<Block>().BlockColor.Equals(blockColors[index]));
                foreach (var blockData in data)
                {
                    InstantiateBlockObject(blockData, blockPositions[index]);
                }
            }
        }

        public void InstantiateBlockObject(BlockData blockData, Vector3 position)
        {
            var blockPrefab = blockData.BlockPrefab;
            var blockGameObject = container.InstantiatePrefab(blockPrefab);
            blockGameObject.transform.position = position;
            blockGameObject.transform.parent = transform;
            var block = blockGameObject.GetComponent<Block>();
            if (block.BlockType.Equals(BlockType.Destructible) || block.BlockType.Equals(BlockType.TwoTimeDestructible))
                levelDataService.Blocks.Add(block);
            levelDataService.BlocksPosition.Add(position);
            levelDataService.BlockColors.Add(block.BlockColor);
        }

        public void ShuffleBlockData()
        {
            var random = new System.Random();
 
            for (int i = blocks.Count - 1; i >= 1; i--)
            {
                var j = random.Next(i + 1);
                (blocks[j], blocks[i]) = (blocks[i], blocks[j]);
            }
        }
    }
}