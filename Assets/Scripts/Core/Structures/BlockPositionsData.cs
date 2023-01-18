using System;
using System.Collections.Generic;
using Core.Enums;
using UnityEngine;

namespace Core.Structures
{
    [Serializable]
    public struct BlockPositionsData
    {
        public List<BlockColor> BlockColors;
        public List<Vector3> BlockPositions;
        public int FieldIndex;
    }
}