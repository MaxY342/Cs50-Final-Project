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

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v0 = vertices[triangles[i]];
                Vector3 v1 = vertices[triangles[i + 1]];
                Vector3 v2 = vertices[triangles[i + 2]];

                // Check if the slice line intersects this triangle
                // (v0, v1, v2 represent the three vertices of the triangle)
                if (IsLineIntersectingTriangle(sliceStart, sliceEnd, v0, v1, v2))
                {
                    
                }
            }

            if (originalCollider == null)
            {
                Debug.LogWarning("Object cannot be sliced as it doesn't have a PolygonCollider2D");
                return;
            }

            // Calculate the new vertices for the slice
            Vector2[] vertices1, vertices2;
            CalculateSlicedVertices(originalCollider, sliceStart, sliceEnd, out vertices1, out vertices2);

            // Create the first sliced part
            GameObject slice1 = Instantiate(target, target.transform.position, target.transform.rotation);
            slice1.GetComponent<PolygonCollider2D>().SetPath(0, vertices1);
            ApplyPhysics(slice1);

            // Create the second sliced part
            GameObject slice2 = Instantiate(target, target.transform.position, target.transform.rotation);
            slice2.GetComponent<PolygonCollider2D>().SetPath(0, vertices2);
            ApplyPhysics(slice2);

            // Destroy the original object
            Destroy(target);
        }

        bool IsLineIntersectingTriangle(Vector2 p0, Vector2 p1, Vector3 v0, Vector3 v1, Vector3 v2)
        {
            // Calculate the line equation coefficients
            float A = p1.y - p0.y;
            float B = p0.x - p1.x;
            float C = p1.x * p0.y - p0.x * p1.y;

            // Calculate the signed distances for the vertices
            float d0 = A * v0.x + B * v0.y + C;
            float d1 = A * v1.x + B * v1.y + C;
            float d2 = A * v2.x + B * v2.y + C;

            // Check if all vertices are on the same side of the line
            if ((d0 > 0 && d1 > 0 && d2 > 0) || (d0 < 0 && d1 < 0 && d2 < 0))
            {
                return false; // No intersection
            }

            return true; // Intersection occurs
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