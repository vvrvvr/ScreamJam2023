using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

namespace CustomProximity
{
    public class ProximityTriggerEvent : MonoBehaviour
    {
        public enum ColliderType
        {
            Box,
            Sphere,
            Capsule,
            // Add other collider types as needed
        }

        public enum GizmosDrawType
        {
            Wire,
            Filled
        }

        public ColliderType colliderType;
        public Vector3 colliderSize;
        public Vector3 colliderPosition;
        public Vector3 colliderRotation;
        public Color gizmosColor = Color.yellow;
        public GizmosDrawType gizmosDrawType = GizmosDrawType.Wire;
        public UnityEvent onTriggerEnterEvent = new UnityEvent();
        public string triggerTags = "Untagged"; // Array of tags that trigger the event
        public LayerMask triggerLayer; // Layer mask for triggering the event

        private void OnEnable()
        {
            AddCollider();
        }

        private void AddCollider()
        {
            Collider existingCollider = GetComponent<Collider>();

            if (existingCollider != null)
            {
                Destroy(existingCollider);
            }

            switch (colliderType)
            {
                case ColliderType.Box:
                    BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                    boxCollider.size = colliderSize;
                    boxCollider.center = colliderPosition;
                    boxCollider.transform.rotation = Quaternion.Euler(colliderRotation);
                    break;

                case ColliderType.Sphere:
                    SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
                    sphereCollider.radius = colliderSize.x / 2f; // Assuming colliderSize represents diameter
                    sphereCollider.center = colliderPosition;
                    sphereCollider.transform.rotation = Quaternion.Euler(colliderRotation);
                    break;

                case ColliderType.Capsule:
                    CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
                    capsuleCollider.height = colliderSize.x;
                    capsuleCollider.radius = colliderSize.y;
                    capsuleCollider.center = colliderPosition;
                    capsuleCollider.transform.rotation = Quaternion.Euler(colliderRotation);
                    break;

                default:
                    Debug.LogError("Unsupported collider type");
                    break;
            }
        }

        private Mesh CreateCapsuleMesh(float height, float radius)
        {
            int segments = 16;
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[segments * 2 + 2];
            int[] triangles = new int[segments * 6];

            for (int i = 0; i < segments; i++)
            {
                float phi = Mathf.PI * 2 * i / segments;
                float y = Mathf.Sin(phi);
                float x = Mathf.Cos(phi);

                vertices[i * 2] = new Vector3(x * radius, y * radius + height * 0.5f, 0);
                vertices[i * 2 + 1] = new Vector3(x * radius, y * radius - height * 0.5f, 0);

                if (i < segments - 1)
                {
                    triangles[i * 6] = i * 2;
                    triangles[i * 6 + 1] = (i + 1) * 2;
                    triangles[i * 6 + 2] = i * 2 + 1;

                    triangles[i * 6 + 3] = (i + 1) * 2;
                    triangles[i * 6 + 4] = (i + 1) * 2 + 1;
                    triangles[i * 6 + 5] = i * 2 + 1;
                }
                else
                {
                    // Connect the last and first vertices to close the loop
                    triangles[i * 6] = i * 2;
                    triangles[i * 6 + 1] = 0;
                    triangles[i * 6 + 2] = i * 2 + 1;

                    triangles[i * 6 + 3] = 0;
                    triangles[i * 6 + 4] = 1;
                    triangles[i * 6 + 5] = i * 2 + 1;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            return mesh;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmosColor;

            switch (colliderType)
            {
                case ColliderType.Box:
                    if (gizmosDrawType == GizmosDrawType.Wire)
                    {
                        // Draw wire box gizmos
                        Gizmos.matrix = Matrix4x4.TRS(transform.position + colliderPosition, Quaternion.Euler(colliderRotation), colliderSize);
                        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
                    }
                    else
                    {
                        // Draw filled box gizmos
                        Gizmos.matrix = Matrix4x4.TRS(transform.position + colliderPosition, Quaternion.Euler(colliderRotation), colliderSize);
                        Gizmos.DrawCube(Vector3.zero, Vector3.one);
                    }
                    break;

                case ColliderType.Sphere:
                    if (gizmosDrawType == GizmosDrawType.Wire)
                    {
                        // Draw wire sphere gizmos
                        Gizmos.matrix = Matrix4x4.TRS(transform.position + colliderPosition, Quaternion.Euler(colliderRotation), Vector3.one);
                        Gizmos.DrawWireSphere(Vector3.zero, colliderSize.x / 2f);
                    }
                    else
                    {
                        // Draw filled sphere gizmos
                        Gizmos.matrix = Matrix4x4.TRS(transform.position + colliderPosition, Quaternion.Euler(colliderRotation), Vector3.one);
                        Gizmos.DrawSphere(Vector3.zero, colliderSize.x / 2f);
                    }
                    break;

                case ColliderType.Capsule:
                    // Draw capsule gizmos
                    DrawCapsuleGizmos();
                    break;

                default:
                    Debug.LogError("Unsupported collider type");
                    break;
            }
        }

        private void DrawCapsuleGizmos()
        {
            if (gizmosDrawType == GizmosDrawType.Wire)
            {
                // Draw wire capsule gizmos
                DrawWireCapsule(transform.position + colliderPosition, Quaternion.Euler(colliderRotation), colliderSize.y, colliderSize.x, gizmosColor);
            }
            else
            {
                // Draw filled capsule gizmos
                DrawFilledCapsule(transform.position + colliderPosition, Quaternion.Euler(colliderRotation), colliderSize.y, colliderSize.x, gizmosColor);
            }
        }

        public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _height, float _radius, Color _color = default(Color))
        {
            // The wire capsule drawing method goes here
            if (_color != default(Color))
                Handles.color = _color;

            Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);

            using (new Handles.DrawingScope(angleMatrix))
            {
                var pointOffset = (_height - (_radius * 2)) / 2;

                // Draw sideways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
                Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
                Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);

                // Draw frontways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
                Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);

                // Draw center
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);
            }
        }

        public static void DrawFilledCapsule(Vector3 _pos, Quaternion _rot, float _height, float _radius, Color _color = default(Color))
        {
            // The filled capsule drawing method goes here
            if (_color != default(Color))
                Handles.color = _color;

            Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);

            using (new Handles.DrawingScope(angleMatrix))
            {
                var pointOffset = (_height - (_radius * 2)) / 2;

                // Draw sideways
                Handles.DrawSolidArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
                Handles.DrawSolidRectangleWithOutline(new Rect(-_radius, pointOffset, _radius * 2, -_height), _color, _color);

                // Draw frontways
                Handles.DrawSolidArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
                Handles.DrawSolidRectangleWithOutline(new Rect(-_radius, pointOffset, _radius * 2, -_height), _color, _color);

                // Draw center
                Handles.DrawSolidDisc(Vector3.up * pointOffset, Vector3.up, _radius);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(triggerTags) && !System.Array.Exists(triggerTags.Split(','), tag => tag == other.tag))
            {
                // Skip if the tag is not in the triggerTags array
                return;
            }

            if ((triggerLayer.value & (1 << other.gameObject.layer)) == 0)
            {
                // Skip if the layer is not in the triggerLayer mask
                return;
            }

            // Perform actions when trigger is entered
            onTriggerEnterEvent.Invoke();
        }
    }
}
