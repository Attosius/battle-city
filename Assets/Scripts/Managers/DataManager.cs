using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.TanksData
{
    public class DataManager : MonoBehaviour
    {
        public List<TankPropertiesData> TankPropertiesDataList = new List<TankPropertiesData>();

        public  TankPropertiesData PlayerLvl1;
        public  TankPropertiesData PlayerLvl2;
        public  TankPropertiesData PlayerLvl3;
        public  TankPropertiesData PlayerLvl4;
        public  TankPropertiesData EnemyLvl1;
        public  TankPropertiesData EnemyLvl2Fast;

        public static DataManager Instance;

        protected void Awake()
        {
            Initialize();
        }

        

        private void LoadData()
        {
            var assetNames = AssetDatabase.FindAssets("*", new[] { "Assets/Prefabs/PropsData" });
            foreach (var soName in assetNames)
            {
                var soPath = AssetDatabase.GUIDToAssetPath(soName);
                var character = AssetDatabase.LoadAssetAtPath<TankPropertiesData>(soPath);
                if (character == null)
                {
                    continue;
                }
                TankPropertiesDataList.Add(character);
            }

            PlayerLvl1 = TankPropertiesDataList.First(o => o.DataOwner == TankPropertiesData.PlayerLvl1);
            PlayerLvl2 = TankPropertiesDataList.First(o => o.DataOwner == TankPropertiesData.PlayerLvl2);
            PlayerLvl3 = TankPropertiesDataList.First(o => o.DataOwner == TankPropertiesData.PlayerLvl3);
            PlayerLvl4 = TankPropertiesDataList.First(o => o.DataOwner == TankPropertiesData.PlayerLvl4);
            EnemyLvl1 = TankPropertiesDataList.First(o => o.DataOwner == TankPropertiesData.EnemyLvl1);
            EnemyLvl2Fast = TankPropertiesDataList.First(o => o.DataOwner == TankPropertiesData.EnemyLvl2Fast);
        }

        private void Initialize()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            if (TankPropertiesDataList.Count == 0)
            {
                LoadData();
            }
        }
    }


}
