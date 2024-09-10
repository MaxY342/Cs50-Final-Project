using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            if (Input.GetKeyDown("r"))
            {
                Vector2 playerPosition = transform.position;
                Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D[] hits = Physics2D.LinecastAll(playerPosition, mouseWorldPosition);
                
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null)
                    {
                        ConvertSpriteToMesh(hit.collider.gameObject);
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
                int v0Side = SideOfPointFromSlice(localSliceStart, localSliceEnd, v0);
                int v1Side = SideOfPointFromSlice(localSliceStart, localSliceEnd, v1);
                int v2Side = SideOfPointFromSlice(localSliceStart, localSliceEnd, v2);

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
                    HandleIntersection(v0, v1, v2, v0Side, v1Side, v2Side, localSliceStart, localSliceEnd, leftVertices, leftTriangles, rightVertices, rightTriangles);
                }
            }

            // Create and apply new meshes to GameObjects
            Mesh leftMesh = CreateMesh(leftVertices, leftTriangles);
            Mesh rightMesh = CreateMesh(rightVertices, rightTriangles);

            // create left and right slice gameobjects
            GameObject leftObject = new GameObject("LeftSlice");
            leftObject.AddComponent<MeshFilter>().mesh = leftMesh;
            leftObject.AddComponent<MeshRenderer>().material = target.GetComponent<MeshRenderer>().material;

            GameObject rightObject = new GameObject("RightSlice");
            rightObject.AddComponent<MeshFilter>().mesh = rightMesh;
            rightObject.AddComponent<MeshRenderer>().material = target.GetComponent<MeshRenderer>().material;

            leftObject.transform.position = target.transform.position;
            rightObject.transform.position = target.transform.position;
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

        void HandleIntersection(Vector3 v0, Vector3 v1, Vector3 v2, int v0Side, int v1Side, int v2Side, Vector2 sliceStart, Vector2 sliceEnd,
            List<Vector3> leftVertices, List<int> leftTriangles,
            List<Vector3> rightVertices, List<int> rightTriangles)
        {
            // Store vertices on either side
            List<Vector3> leftSideVertices = new List<Vector3>();
            List<Vector3> rightSideVertices = new List<Vector3>();
        
            // Determine which vertices are on which side
            if (v0Side > 0) leftSideVertices.Add(v0); else rightSideVertices.Add(v0);
            if (v1Side > 0) leftSideVertices.Add(v1); else rightSideVertices.Add(v1);
            if (v2Side > 0) leftSideVertices.Add(v2); else rightSideVertices.Add(v2);
        
            // We should have two vertices on one side and one on the other.
            if (leftSideVertices.Count == 2 && rightSideVertices.Count == 1)
            {
                // Slice the triangle
                Vector3 intersection1 = GetIntersectionPoint(leftSideVertices[0], rightSideVertices[0], sliceStart, sliceEnd);
                Vector3 intersection2 = GetIntersectionPoint(leftSideVertices[1], rightSideVertices[0], sliceStart, sliceEnd);
        
                // Add two triangles to the left side
                AddTriangleToMesh(leftSideVertices[0], intersection1, intersection2, leftVertices, leftTriangles);
                AddTriangleToMesh(leftSideVertices[0], leftSideVertices[1], intersection2, leftVertices, leftTriangles);
        
                // Add one triangle to the right side
                AddTriangleToMesh(rightSideVertices[0], intersection1, intersection2, rightVertices, rightTriangles);
            }
            else if (rightSideVertices.Count == 2 && leftSideVertices.Count == 1)
            {
                // Slice the triangle
                Vector3 intersection1 = GetIntersectionPoint(rightSideVertices[0], leftSideVertices[0], sliceStart, sliceEnd);
                Vector3 intersection2 = GetIntersectionPoint(rightSideVertices[1], leftSideVertices[0], sliceStart, sliceEnd);
        
                // Add two triangles to the right side
                AddTriangleToMesh(rightSideVertices[0], intersection1, intersection2, rightVertices, rightTriangles);
                AddTriangleToMesh(rightSideVertices[0], rightSideVertices[1], intersection2, rightVertices, rightTriangles);
        
                // Add one triangle to the left side
                AddTriangleToMesh(leftSideVertices[0], intersection1, intersection2, leftVertices, leftTriangles);
            }
        }
        
        Vector3 GetIntersectionPoint(Vector3 start, Vector3 end, Vector2 sliceStart, Vector2 sliceEnd)
        {
            // Convert slice direction to local space if necessary
            Vector2 sliceDir = sliceEnd - sliceStart;
            Vector2 edgeDir = end - start;

            // Calculate the intersection point using parametric equation of the line
            float t = ((sliceStart.x - start.x) * (sliceStart.y - sliceEnd.y) - (sliceStart.y - start.y) * (sliceStart.x - sliceEnd.x)) / ((start.x - end.x) * (sliceStart.y - sliceEnd.y) - (start.y - end.y) * (sliceStart.x - sliceEnd.x));

            // Calculate intersection point along the edge
            return start + t * (end - start);
        }
        
        void AddTriangleToMesh(Vector3 v0, Vector3 v1, Vector3 v2, List<Vector3> vertices, List<int> triangles)
        {
            int index = vertices.Count;
            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);
        
            triangles.Add(index);
            triangles.Add(index + 1);
            triangles.Add(index + 2);
        }

        Mesh CreateMesh(List<Vector3> vertices, List<int> triangles)
        {
            Mesh mesh = new Mesh();
            
            // Assign vertices and triangles to the mesh
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            
            // Recalculate normals and bounds for proper rendering and physics interactions
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            
            return mesh;
        }

        void ConvertSpriteToMesh(GameObject target)
        {
            SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;

            // Add or get existing MeshFilter and MeshRenderer
            MeshFilter meshFilter = target.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();

            // Convert sprite data to mesh (using sprite vertices)
            Vector2[] spriteVertices = spriteRenderer.sprite.vertices;
            ushort[] spriteTriangles = spriteRenderer.sprite.triangles;

            // Convert 2D vertices to 3D by adding a z value of 0
            Vector3[] vertices = new Vector3[spriteVertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(spriteVertices[i].x, spriteVertices[i].y, 0f);
            }

            // Assign vertices and triangles to mesh
            mesh.vertices = vertices;
            mesh.triangles = Array.ConvertAll(spriteTriangles, i => (int)i);

            // Assign UVs for texturing
            mesh.uv = spriteRenderer.sprite.uv;

            // Optionally assign normals (for lighting, 2D games often don't need this)
            Vector3[] normals = new Vector3[vertices.Length];
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = Vector3.back; // Normals point to -Z for 2D sprites
            }
            mesh.normals = normals;

            // Assign the generated mesh to the MeshFilter
            meshFilter.mesh = mesh;

            // Set the same material as the SpriteRenderer
            meshRenderer.material = spriteRenderer.material;

            // Remove the SpriteRenderer after conversion (optional, if you don't need it anymore)
            Destroy(spriteRenderer);
        }
    }
}
