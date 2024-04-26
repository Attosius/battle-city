using UnityEngine;

namespace Assets.Scripts.TanksData
{
    [CreateAssetMenu(fileName = "NewTankProperties", menuName = "Data/TankPropertiesData")]
    public class TankPropertiesData: ScriptableObject
    {
        public static string PlayerLvl1 = "PlayerLvl1";
        public static string PlayerLvl2 = "PlayerLvl2";
        public static string PlayerLvl3 = "PlayerLvl3";
        public static string PlayerLvl4 = "PlayerLvl4";

        public static string EnemyLvl1 = "EnemyLvl1";
        public static string EnemyLvl2Fast = "EnemyLvl2Fast";
        public static string EnemyLvl3LightSmart = "EnemyLvl3LightSmart";
        public static string EnemyLvl4Heavy = "EnemyLvl4Heavy";

        public string DataOwner = "";

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
