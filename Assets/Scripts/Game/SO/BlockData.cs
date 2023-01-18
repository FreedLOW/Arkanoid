using Core.Enums;
using UnityEngine;

namespace NM.Game.SO
{
    [CreateAssetMenu(fileName = "Block Data", menuName = "GameData/Block", order = 51)]
    public class BlockData : ScriptableObject
    {
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private int score;
        [SerializeField] private BlockType blockType;
        [SerializeField] private float blockHeight;
        [SerializeField] private float blockWidth;

        public BlockType BlockType => blockType;
        public GameObject BlockPrefab => blockPrefab;
        public int Score => score;
        public float BlockHeight => blockHeight;
        public float BlockWidth => blockWidth;
    }
}