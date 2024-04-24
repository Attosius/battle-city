using UnityEngine;

namespace Assets.Scripts
{
    public class OnGUIController : MonoBehaviour
    {

        private Vector3 resets = Vector3.back;
        private Vector3 worldPosition = Vector3.back;

        private void OnGUI()
        {
            var screenPosition = Input.mousePosition;
            //Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            //Vector3 worldPosition = Vector3.zero;


            //Plane plane = new Plane(Vector3.down, -5);
            //if (plane.Raycast(ray, out float distance))
            //{
            //    worldPosition = ray.GetPoint(distance);
            //}
            worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var  ray = Camera.main.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                resets = hit.point;
            }

            // Get the mouse position from Event.
            // Note that the y position from Event is inverted.
            //mousePos.x = currentEvent.mousePosition.x;
            //mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

            //point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
            //Vector3 InverseTransformPoint = cam.transform.InverseTransformPoint(point);
            //Vector3 screenPos = cam.WorldToScreenPoint(point);

            Vector3 distanceFromCam = new Vector3(Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                0);
            Plane plane = new Plane(Vector3.forward, distanceFromCam);

            GUILayout.BeginArea(new Rect(20, 20, 550, 120));


            GUILayout.Label("mousePosition: " + screenPosition.ToString("F3"));
            GUILayout.Label("worldPosition: " + worldPosition.ToString("F3"));
            GUILayout.Label("resets: " + resets.ToString("F3"));
            GUILayout.EndArea();
        }
    }
}
