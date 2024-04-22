using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class GridEditorManager : MonoBehaviour
{
    public GameObject mapGround2;
    public GameObject mapOuter;
    public int mapWidth = 13;
    public List<Vector2> mapObjects = new List<Vector2>();
    public bool IsMapShow = true;
    void Awake()
    {
        for (int i = -1; i < mapWidth + 1 ; i++)
        {
            for (int j = -1; j < mapWidth + 1; j++)
            {
                var position = new Vector2(i / 2f - BaseMovingObject.MapTankWidth / 2 , j / 2f - BaseMovingObject.MapTankWidth / 2);
                mapObjects.Add(position);
                
                //Instantiate(mapGround, position, Quaternion.identity, transform);
            }

        }
        //for (float i = 0f; i < mapWidth; i++)
        //{
        //    for (float j = 0; j < mapWidth; j++)
        //    {
        //        var position = new Vector3(i / 2f, j / 2f, 0);
        //        //GUI.Label(new Rect(position, new Vector2(20, 20)), position.ToString());
        //        //Debug.Log(position);
        //        //Gizmos.
        //        //Instantiate(mapGround, position, Quaternion.identity, transform);
        //    }

        //}

    }

    private void OnDrawGizmos()
    {
        if (!IsMapShow)
        {
            return;
        }
        Gizmos.color = Color.gray;
        var thickness = 1;

        for (int i = 0; i < mapObjects.Count; i++)
        {
            if (i == 0)
            {
                //Debug.Log(mapObjects[i] + " " + mapObjects[i + 1]);
            }

            if (i == mapObjects.Count - 1)
            {
                continue;
            }
            if (mapObjects[i].x < -0.5f || mapObjects[i].y < -0.5f)
            {
                continue;
            }
            var obj = mapObjects[i];
            var nextObj = mapObjects[i + 1];
            if (Math.Abs(obj.y - nextObj.y) > 1)
            {
                continue;
            }                                                                        
            Handles.DrawBezier(obj, nextObj,
                obj, nextObj, Color.gray, null, thickness);
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(obj, 0.02f);

        }
        //Handles.DrawBezier(Rect.center, RectTo.center, Rect.center, RectTo.center, Color.blue, null, thickness);
        //Gizmos.DrawLine(Rect.center, RectTo.center);

        ////////////////////
        //Gizmos.DrawSphere(nextRect.center, 0.03f);
        //UnityCustomExtensions.DrawRect(Rect, Color.yellow, Rotation);
        //UnityCustomExtensions.DrawRect(RectTo, Color.green, Rotation);
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