using UnityEngine;

namespace NM.Core.Interface
{
    public interface IPlayerService
    {
        public Rigidbody2D PlayerBody { get; }
        public Transform BallPoint { get; }
        public int PlayerHealth { get; set; }

        public void PlayerMovement();
        public void ResetPlayer();
    }
}