using UnityEngine;

namespace Player_v_Player_Game.Weapons.Sword
{
    public class UltimateAbility : MonoBehaviour
    {
        [SerializeField] private GameObject slicingParticleEffectPrefab;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown("e"))
            {
                Vector2 playerPosition = transform.position;
                Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D[] hits = Physics2D.LinecastAll(playerPosition, mouseWorldPosition);
                
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null)
                    {
                        SliceObject(hit.collider.gameObject, playerPosition, mouseWorldPosition);
                    }
                }
            }
        }

        public void SliceObject(GameObject target, Vector2 sliceStart, Vector2 sliceEnd)
        {
            PolygonCollider2D originalCollider = target.GetComponent<PolygonCollider2D>();
            Mesh mesh = target.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            List<Vector3> newVertices = new List<Vector3>();
            List<int> newTriangles = new List<int>();

            // Convert slice points to local space
            Vector2 localSliceStart = target.transform.InverseTransformPoint(sliceStart);
            Vector2 localSliceEnd = target.transform.InverseTransformPoint(sliceEnd);

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v0 = vertices[triangles[i]];
                Vector3 v1 = vertices[triangles[i + 1]];
                Vector3 v2 = vertices[triangles[i + 2]];

                // Convert vertices to world space if needed
                Vector3 worldV0 = target.transform.TransformPoint(v0);
                Vector3 worldV1 = target.transform.TransformPoint(v1);
                Vector3 worldV2 = target.transform.TransformPoint(v2);

                // Check if the slice line intersects this triangle
                if (IsLineIntersectingTriangle(localSliceStart, localSliceEnd, v0, v1, v2))
                {
                    // Add logic here to calculate the intersection points, create new triangles
                    // Add the new vertices and triangles to the lists
                }
                else
                {
                    // If no intersection, just add the triangle to the new mesh data
                    newVertices.Add(v0);
                    newVertices.Add(v1);
                    newVertices.Add(v2);

                    newTriangles.Add(newVertices.Count - 3);
                    newTriangles.Add(newVertices.Count - 2);
                    newTriangles.Add(newVertices.Count - 1);
                }
            }

            // After processing all triangles, apply the new mesh data
            Mesh newMesh = new Mesh();
            newMesh.vertices = newVertices.ToArray();
            newMesh.triangles = newTriangles.ToArray();
            newMesh.RecalculateNormals();

            // Create new GameObject for sliced part (or replace the original mesh)
            GameObject slicedObject = new GameObject("SlicedObject", typeof(MeshFilter), typeof(MeshRenderer));
            slicedObject.GetComponent<MeshFilter>().mesh = newMesh;
            slicedObject.transform.position = target.transform.position;
            slicedObject.transform.rotation = target.transform.rotation;
        }

        bool IsLineIntersectingTriangle(Vector2 p0, Vector2 p1, Vector3 v0, Vector3 v1, Vector3 v2)
        {
            float v0side = (p1.x - p0.x) * (v0.y - p0.y) - (p1.y - p0.y) * (v0.x - p0.x);
            float v1side = (p1.x - p0.x) * (v1.y - p0.y) - (p1.y - p0.y) * (v1.x - p0.x);
            float v2side = (p1.x - p0.x) * (v2.y - p0.y) - (p1.y - p0.y) * (v2.x - p0.x);

            if ((v0side >= 0 && v1side >= 0 && v2side >= 0) || (v0side <= 0 && v1side <= 0 && v2side <= 0))
            {
                return false;
            }

            return true;
        }

        void CalculateSlicedVertices(PolygonCollider2D originalCollider, Vector2 sliceStart, Vector2 sliceEnd, out Vector2[] vertices1, out Vector2[] vertices2)
        {
            // Calculate new vertices based on the slice line
            // This is complex and requires calculating where the slice line intersects with the polygon's edges
            vertices1 = new Vector2[0]; // Placeholder for actual slicing logic
            vertices2 = new Vector2[0]; // Placeholder for actual slicing logic
        }

        void ApplyPhysics(GameObject slicedPart)
        {
            Rigidbody2D rb = slicedPart.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1.0f;
            rb.mass = 1.0f;

            // Apply force or torque to simulate the slicing impact
            rb.AddForce(Random.insideUnitCircle * 2.0f, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-100.0f, 100.0f), ForceMode2D.Impulse);
        }

        void AddSliceVisualEffect(Vector2 position)
        {
            // Instantiate a particle effect at the position
            GameObject particleEffect = Instantiate(slicingParticleEffectPrefab, position, Quaternion.identity);
            Destroy(particleEffect, 2.0f); // Destroy the effect after 2 seconds
        }
    }
}