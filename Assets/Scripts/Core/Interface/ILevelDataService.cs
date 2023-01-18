using System.Collections.Generic;
using Core.Enums;
using NM.Game.Interface;
using NM.Game.Level;
using UnityEngine;

namespace NM.Core.Interface
{
    public interface ILevelDataService
    {
        public int LevelIndex { get; set; }
        public int CurrentFieldIndex { get; }
        public List<Block> Blocks { get; set; }
        public List<Vector3> BlocksPosition { get; set; }
        public List<BlockColor> BlockColors { get; set; }
        public List<GameObject> Balls { get; set; }

        public void LevelUp();
        public void RemoveBlock(Block block);
    }
}