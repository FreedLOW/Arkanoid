using UnityEngine;

namespace NM.Game.Interface
{
    public interface IBallMovement
    {
        public bool IsBallActive { get; set; }
        public Rigidbody2D BallBody { get; set; }
        public void ActivateBall();
        public void DestroyBall();
        public void CalculateBallReflect(Collision2D collision, Transform collideObject);
    }
}