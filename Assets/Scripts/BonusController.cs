using Assets.Scripts.TanksData;
using UnityEngine;

namespace Assets.Scripts
{
    public class BonusController : MonoBehaviour
    {
        private PlayerInput playerInput;

        void Awake()
        {

            playerInput = GetComponent<PlayerInput>();
        }


        void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($" OnTriggerEnter2D {gameObject.name} Collidered with {collision.name} ");
            switch (playerInput.TankProperties.DataOwner)
            {
                case "PlayerLvl1":
                    playerInput.SetPropsData(DataManager.Instance.PlayerLvl2);
                    playerInput.animator.SetTrigger("UpTo2");
                    break;
                case "PlayerLvl2":
                    playerInput.SetPropsData(DataManager.Instance.PlayerLvl3);
                    playerInput.animator.SetTrigger("UpTo3");
                    break;
                case "PlayerLvl3":
                    playerInput.SetPropsData(DataManager.Instance.PlayerLvl4);
                    playerInput.animator.SetTrigger("UpTo4");
                    break;
                case "PlayerLvl4":
                    playerInput.SetPropsData(DataManager.Instance.PlayerLvl1);
                    playerInput.animator.SetTrigger("UpTo1");
                    break;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {

            Debug.Log($"Collidered with {collision.gameObject.name} -> {collision.otherCollider.gameObject.name}");
        }

    }
}
