using UnityEngine;

namespace NM.Core.Interface
{
    public interface IBallSpawnerService
    {
        public void CreateBall();
        public void CreateBall(Vector3 position);
    }
}