using Assets.Scripts.TanksData;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameLoader : MonoBehaviour
    {
        public DataManager DataManagerInstance;

        void Awake()
        {
            if (DataManager.Instance == null)
            {
                //Instantiate(DataManagerInstance);
                DataManagerInstance = gameObject.AddComponent<DataManager>();
            }
        }
    }

}
