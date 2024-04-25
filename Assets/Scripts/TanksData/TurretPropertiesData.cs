using UnityEngine;

namespace Assets.Scripts.TanksData
{
    [CreateAssetMenu(fileName = "NewTurretPropertiesData", menuName = "Data/TurretPropertiesData")]
    public class TurretPropertiesData : ScriptableObject
    {
        public float ReloadDelay = 1.5f; //sec
        public float ReloadDelayAfterHitPlayerSec = 0.25f;
        public float ReloadDelayAfterHitEnemySec = 0.5f;
        public float ReloadDelayAfterHitBulletSec = 0.05f;


        public float BulletSpeed = 2;
    }
}