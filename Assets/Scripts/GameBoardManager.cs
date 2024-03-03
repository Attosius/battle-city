using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameBoardManager : MonoBehaviour
{
    public GameObject groundPf;
    void Awake()
    {
        for (float i = 0f; i < 13/2f; i+=0.5f)
        {
            for (float j = 0; j < 13/2f; j+=0.5f)
            {
                var position = new Vector3(i, j, 0);
                //GUI.Label(new Rect(position, new Vector2(20, 20)), position.ToString());
                //Debug.Log(position);
                //Gizmos.
                Instantiate(groundPf, position, Quaternion.identity);
            }

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//[CustomPropertyDrawer(typeof(HexCoordinates))]
//public class HexCoordinatesDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        var coord = new HexCoordinates(
//            property.FindPropertyRelative("x").intValue,
//            property.FindPropertyRelative("z").intValue
//        );
//        position = EditorGUI.PrefixLabel(position, label);
//        GUI.Label(position, coord.ToString());
//    }
//}