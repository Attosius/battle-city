using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class TextCoordinate : MonoBehaviour
    {
        public TMP_Text begin;
        public TMP_Text end;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            begin.text = $"{transform.position.x}, {transform.position.y}";
            end.text = "-";
        }
    }
}
