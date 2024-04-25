using UnityEngine;

namespace Assets.Scripts.TanksData
{
    [CreateAssetMenu(fileName = "NewTankProperties", menuName = "Data/TankPropertiesData")]
    public class TankPropertiesData : ScriptableObject
    {
        public TurretPropertiesData TurretPropertiesData;

        public float MaxSpeed = 1f;

        public static float MapTankWidth = 0.5f;

        public float MovePoint = 0.5f / 2; // part of move to move smooth 1/8 square

    }

    //[CreateAssetMenu(fileName = "NewTankProperties", menuName = "Data/TankPropertiesData")]
    //public class TankPropertiesData : ScriptableObject
    //{
    //    public float ReloadDelay = 1.5f; //sec
    //    public float ReloadDelayAfterHitPlayerSec = 0.25f;
    //    public float ReloadDelayAfterHitEnemySec = 0.5f;
    //    public float ReloadDelayAfterHitBulletSec = 0.05f;

    //    public float MaxSpeed = 1f;

    //    public static float MapTankWidth = 0.5f;

    //    public float MovePoint = 0.5f / 2; // part of move to move smooth 1/8 square

    //    public float BulletSpeed = 2;
    //}

}
