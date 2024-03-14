using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameBoardManager : MonoBehaviour
{
    public GameObject mapGround;
    public GameObject mapOuter;
    public int mapWidth = 13;
    void Awake()
    {
        for (int i = -1; i < mapWidth + 1 ; i++)
        {
            for (int j = -1; j < mapWidth + 1; j++)
            {
                var position = new Vector3(i / 2f, j / 2f, 0);
                if (i == -1 || i == mapWidth || j == -1 || j == mapWidth)
                {
                    Instantiate(mapOuter, position, Quaternion.identity, transform);
                    continue;
                }
                Instantiate(mapGround, position, Quaternion.identity, transform);
            }

        }
        for (float i = 0f; i < mapWidth; i++)
        {
            for (float j = 0; j < mapWidth; j++)
            {
                var position = new Vector3(i / 2f, j / 2f, 0);
                //GUI.Label(new Rect(position, new Vector2(20, 20)), position.ToString());
                //Debug.Log(position);
                //Gizmos.
                Instantiate(mapGround, position, Quaternion.identity, transform);
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