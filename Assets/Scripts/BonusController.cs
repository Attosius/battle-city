using Assets.Scripts.TanksData;
using UnityEngine;

namespace Assets.Scripts
{
    public class BonusController : MonoBehaviour
    {
        void Awake()
        {

            //playerInput = GetComponent<PlayerInput>();
        }

        // if using for enemy, maybe set this controller to bonus?
        void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($" OnTriggerEnter2D {gameObject.name} Collidered with {collision.name} ");
            var tank = collision.gameObject;
            switch (tank.tag)
            {
                case PlayerInput.Tag:
                    GetAsPlayer(tank);


                    break;
                case EnemyController.Tag:
                    break;
            }


        }

        private void GetAsPlayer(GameObject tank)
        {
            var playerInput = tank.GetComponent<PlayerInput>();

            playerInput.animator.StartPlayback(); // ?
            switch (playerInput.TankProperties.DataOwner)
            {
                case TankPropertiesData.PlayerLvl1: // current state
                    playerInput.SetPropsData(DataManager.Instance.PlayerLvl2);
                    playerInput.animator.SetTrigger("UpTo2");
                    break;
                case TankPropertiesData.PlayerLvl2:
                    playerInput.SetPropsData(DataManager.Instance.PlayerLvl3);
                    playerInput.animator.SetTrigger("UpTo3");
                    break;
                case TankPropertiesData.PlayerLvl3:
                    playerInput.SetPropsData(DataManager.Instance.PlayerLvl4);
                    playerInput.animator.SetTrigger("UpTo4");
                    break;
                case TankPropertiesData.PlayerLvl4:
                    playerInput.SetPropsData(DataManager.Instance.PlayerLvl1);
                    playerInput.animator.SetTrigger("UpTo1");
                    break;
            }
            gameObject.SetActive(false);
            playerInput.animator.StopPlayback();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {

            Debug.Log($"Collidered with {collision.gameObject.name} -> {collision.otherCollider.gameObject.name}");
        }

    }
}
