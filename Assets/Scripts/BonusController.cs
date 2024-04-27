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

            playerInput.SetPropsData(DataManager.Instance.PlayerLvl2);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {

            Debug.Log($"Collidered with {collision.gameObject.name} -> {collision.otherCollider.gameObject.name}");
        }

    }
}
