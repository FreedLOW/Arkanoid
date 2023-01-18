using System.Collections.Generic;
using Core.Enums;
using NM.Game.SO;
using UnityEngine;

namespace NM.Core.Interface
{
    public interface IBlockSpawnerService
    {
        public void SpawnBlocks();
        public void SpawnBlocks(List<BlockColor> blockColors, List<Vector3> blockPositions);
        public void InstantiateBlockObject(BlockData blockData, Vector3 position);
        public void ShuffleBlockData();
    }
}