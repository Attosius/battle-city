using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class EditorMode : MonoBehaviour
{
    
    void Start()
    {
        SceneView.duringSceneGui += SceneView_duringSceneGui;
    }
    public Vector3 MousePosition = Vector3.back;
    public string MousePositionString = "1";
    private void SceneView_duringSceneGui(SceneView sceneView)
    {
        //mousePosition = Event.current.mousePosition;
        //worldPosition2 = Event.current.mousePosition;
        //Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        //worldPosition = ray.origin;
        ////if (Physics.Raycast(ray, out RaycastHit hit))
        ////{
        ////    worldPosition = hit.point;
        ////}
        //worldPosition2.y = sceneView.camera.pixelHeight - mousePosition.y; // Flip y
        //Ray ray2 = sceneView.camera.ScreenPointToRay(worldPosition2);
        //worldPosition2 = ray2.origin;


        MousePosition = Event.current.mousePosition;
        MousePosition.y = sceneView.camera.pixelHeight - MousePosition.y;
        MousePosition = sceneView.camera.ScreenToWorldPoint(MousePosition);

        MousePositionString = ((Vector2)MousePosition).ToString("F2");


        //Ray ray = sceneView.camera.ScreenPointToRay(mousePosition);
    }
}
