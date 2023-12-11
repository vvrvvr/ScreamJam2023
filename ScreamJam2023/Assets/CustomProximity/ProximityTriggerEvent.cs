using UnityEngine;
using UnityEngine.Events;

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
        public GizmosDrawType gizmosDrawType = GizmosDrawType.Wire; // New property for Gizmos draw type
        public UnityEvent onTriggerEnterEvent = new UnityEvent();

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

                // Add other cases for additional collider types

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
                    Gizmos.matrix = Matrix4x4.TRS(transform.position + colliderPosition, Quaternion.Euler(colliderRotation), colliderSize);
                    if (gizmosDrawType == GizmosDrawType.Wire)
                        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
                    else
                        Gizmos.DrawCube(Vector3.zero, Vector3.one);
                    break;

                case ColliderType.Sphere:
                    Gizmos.matrix = Matrix4x4.TRS(transform.position + colliderPosition, Quaternion.Euler(colliderRotation), Vector3.one);
                    if (gizmosDrawType == GizmosDrawType.Wire)
                        Gizmos.DrawWireSphere(Vector3.zero, colliderSize.x / 2f);
                    else
                        Gizmos.DrawSphere(Vector3.zero, colliderSize.x / 2f);
                    break;

                case ColliderType.Capsule:
                    Gizmos.matrix = Matrix4x4.TRS(transform.position + colliderPosition, Quaternion.Euler(colliderRotation), Vector3.one);
                    if (gizmosDrawType == GizmosDrawType.Wire)
                        Gizmos.DrawWireMesh(CreateCapsuleMesh(colliderSize.x, colliderSize.y));
                    else
                        Gizmos.DrawMesh(CreateCapsuleMesh(colliderSize.x, colliderSize.y));
                    break;

                // Add other cases for additional collider types

                default:
                    Debug.LogError("Unsupported collider type");
                    break;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            // Perform actions when trigger is entered
            onTriggerEnterEvent.Invoke();
        }
    }
}
