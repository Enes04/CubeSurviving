using UnityEngine;

namespace Script
{
    public abstract class BackHit : EnemyBase
    {
        public abstract void BackHitAnim(GameObject obj);
        public abstract void SkillBackHitDisable(GameObject obj);
    }
}
