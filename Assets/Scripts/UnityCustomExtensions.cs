using UnityEngine;

namespace Assets.Scripts
{
    public static class UnityCustomExtensions
    {
        public static Rect CreateRectFromCenter(Vector2 center, float width, float height)
        {
            float halfWidth = width / 2;
            float halfHeight = height / 2;

            float xMin = center.x - halfWidth;
            float yMin = center.y - halfHeight;

            return new Rect(xMin, yMin, width, height);
        }

        public static void DrawRect(Rect rect, Color color, float Rotation)
        {
            Gizmos.color = color;
            Gizmos.matrix = Matrix4x4.TRS(rect.center, Quaternion.Euler(0, 0, Rotation), Vector3.one);
            Gizmos.DrawWireCube(Vector2.zero, rect.size);



            Gizmos.matrix = Matrix4x4.TRS(Vector2.zero, Quaternion.identity, Vector3.one);
            Gizmos.DrawSphere(rect.center, 0.03f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(rect.position, 0.03f);
        }

        public static LayerMask GetLayerMaskByName(string layerName)
        {
            var layerMask = new LayerMask();
            int layerId = LayerMask.NameToLayer(layerName);
            if (layerId != -1)
            {
                layerMask = 1 << layerId;
            }
            else
            {
                Debug.LogWarning("Layer not found: " + layerName);
            }
            return layerMask;
        }
    }
}
