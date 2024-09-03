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
            MeshFilter meshFilter = target.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                Debug.LogError("MeshFilter component not found on target GameObject.");
                return;
            }

            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            List<Vector3> leftVertices = new List<Vector3>();
            List<int> leftTriangles = new List<int>();

            List<Vector3> rightVertices = new List<Vector3>();
            List<int> rightTriangles = new List<int>();

            // Convert slice points to local space
            Vector2 localSliceStart = target.transform.InverseTransformPoint(sliceStart);
            Vector2 localSliceEnd = target.transform.InverseTransformPoint(sliceEnd);

            // Iterate through each triangle
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v0 = vertices[triangles[i]];
                Vector3 v1 = vertices[triangles[i + 1]];
                Vector3 v2 = vertices[triangles[i + 2]];

                // Determine on which side each vertex lies
                int v0Side = SideOfPointFromSlice(sliceStart, sliceEnd, v0);
                int v1Side = SideOfPointFromSlice(sliceStart, sliceEnd, v1);
                int v2Side = SideOfPointFromSlice(sliceStart, sliceEnd, v2);

                // Classify the triangle based on vertex positions
                if (v0Side >= 0 && v1Side >= 0 && v2Side >= 0)
                {
                    // Entire triangle is on the left side
                    AddTriangleToMesh(v0, v1, v2, leftVertices, leftTriangles);
                }
                else if (v0Side <= 0 && v1Side <= 0 && v2Side <= 0)
                {
                    // Entire triangle is on the right side
                    AddTriangleToMesh(v0, v1, v2, rightVertices, rightTriangles);
                }
                else
                {
                    // Triangle is intersected by the slice
                    HandleIntersection(v0, v1, v2, v0Side, v1Side, v2Side, sliceStart, sliceEnd, leftVertices, leftTriangles, rightVertices, rightTriangles);
                }
            }

            // Create and apply new meshes to GameObjects
            Mesh leftMesh = CreateMesh(leftVertices, leftTriangles);
            Mesh rightMesh = CreateMesh(rightVertices, rightTriangles);

            // Assuming leftObject and rightObject are predefined GameObjects to hold the sliced parts
            ApplyMeshToObject(leftMesh, leftObject);
            ApplyMeshToObject(rightMesh, rightObject);
        }

            // Create and apply new meshes to GameObjects
            Mesh leftMesh = CreateMesh(leftVertices, leftTriangles);
            Mesh rightMesh = CreateMesh(rightVertices, rightTriangles);

            ApplyMeshToObject(leftMesh, leftObject);
            ApplyMeshToObject(rightMesh, rightObject);
        }

        // checks if point c is left of line a-b
        int SideOfPointFromSlice(Vector2 a, Vector2 b, Vector3 c)
        {
            // Convert Vector3 to Vector2 by ignoring the z-axis
            Vector2 c2D = new Vector2(c.x, c.y);

            // Calculate the cross product
            float cross = (b.x - a.x) * (c2D.y - a.y) - (b.y - a.y) * (c2D.x - a.x);

            if (cross > 0)
            {
                return 1;
            }

            else if (cross < 0)
            {
                return -1;
            }

            else
            {
                return 0;
            }
        }
        void HandleIntersection(
            Vector3 v0, Vector3 v1, Vector3 v2,
            int v0Side, int v1Side, int v2Side,
            Vector2 sliceStart, Vector2 sliceEnd,
            List<Vector3> leftVertices, List<int> leftTriangles,
            List<Vector3> rightVertices, List<int> rightTriangles)
        {
            // Collect vertices and their sides
            List<(Vector3 vertex, int side)> verts = new List<(Vector3, int)>
            {
                (v0, v0Side),
                (v1, v1Side),
                (v2, v2Side)
            };

            // Separate vertices into positive and negative sides
            List<(Vector3 vertex, int side)> positive = verts.FindAll(v => v.side > 0);
            List<(Vector3 vertex, int side)> negative = verts.FindAll(v => v.side < 0);
            List<(Vector3 vertex, int side)> onLine = verts.FindAll(v => v.side == 0);

            // Depending on the number of vertices on each side, handle accordingly
            if (positive.Count == 2 && negative.Count == 1)
            {
                // Two vertices on positive side, one on negative
                SplitTriangle(positive, negative[0], sliceStart, sliceEnd, leftVertices, leftTriangles, rightVertices, rightTriangles);
            }
            else if (negative.Count == 2 && positive.Count == 1)
            {
                // Two vertices on negative side, one on positive
                SplitTriangle(negative, positive[0], sliceStart, sliceEnd, rightVertices, rightTriangles, leftVertices, leftTriangles);
            }
            else if (onLine.Count > 0)
            {
                // Handle cases where one or more vertices lie exactly on the slice line
                // This requires careful handling to avoid duplicating vertices
                // Implementation depends on specific requirements
            }
            // Additional cases can be handled as needed
        }

        void SplitTriangle(
            List<(Vector3 vertex, int side)> positive,
            (Vector3 vertex, int side) negative,
            Vector2 sliceStart, Vector2 sliceEnd,
            List<Vector3> targetMeshVertices, List<int> targetMeshTriangles,
            List<Vector3> oppositeMeshVertices, List<int> oppositeMeshTriangles)
        {
            // Find intersection points between the slice line and the triangle edges
            Vector3 intersection1 = GetIntersection(positive[0].vertex, negative.vertex, sliceStart, sliceEnd);
            Vector3 intersection2 = GetIntersection(positive[1].vertex, negative.vertex, sliceStart, sliceEnd);

            // Add the original positive vertices and the intersection points to the target mesh
            targetMeshVertices.Add(positive[0].vertex);
            targetMeshVertices.Add(positive[1].vertex);
            targetMeshVertices.Add(intersection1);
            targetMeshVertices.Add(intersection2);

            int baseIndex = targetMeshVertices.Count - 4;

            // Create two new triangles for the target mesh
            targetMeshTriangles.Add(baseIndex);
            targetMeshTriangles.Add(baseIndex + 2);
            targetMeshTriangles.Add(baseIndex + 3);

            targetMeshTriangles.Add(baseIndex);
            targetMeshTriangles.Add(baseIndex + 3);
            targetMeshTriangles.Add(baseIndex + 1);

            // Add the negative vertex and intersection points to the opposite mesh
            oppositeMeshVertices.Add(negative.vertex);
            oppositeMeshVertices.Add(intersection1);
            oppositeMeshVertices.Add(intersection2);

            int oppBaseIndex = oppositeMeshVertices.Count - 3;

            // Create one new triangle for the opposite mesh
            oppositeMeshTriangles.Add(oppBaseIndex);
            oppositeMeshTriangles.Add(oppBaseIndex + 1);
            oppositeMeshTriangles.Add(oppBaseIndex + 2);
        }

        Vector3 GetIntersection(Vector3 vertexA, Vector3 vertexB, Vector2 sliceStart, Vector2 sliceEnd)
        {
            Vector2 a = new Vector2(vertexA.x, vertexA.y);
            Vector2 b = new Vector2(vertexB.x, vertexB.y);
            Vector2 c = sliceStart;
            Vector2 d = sliceEnd;

            float t1 = (d.x - c.x);
            float t2 = (d.y - c.y);
            float t3 = (b.x - a.x);
            float t4 = (b.y - a.y);
            float denominator = t1 * t4 - t2 * t3;

            if (denominator == 0)
            {
                // Lines are parallel; handle accordingly
                return Vector3.zero;
            }

            float t = ((a.x - c.x) * t4 - (a.y - c.y) * t3) / denominator;
            // float u = ((a.x - c.x) * t2 - (a.y - c.y) * t1) / denominator; // Not used here

            Vector2 intersection = c + t * new Vector2(t1, t2);
            return new Vector3(intersection.x, intersection.y, vertexA.z); // Assuming z remains the same
        }
    }
}