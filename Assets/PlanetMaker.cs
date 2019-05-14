using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Mesh))]
[RequireComponent(typeof(MeshRenderer))]
public class PlanetMaker : MonoBehaviour
{
    private MeshFilter filter;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;

    private const float X = .525731112119133606f;
    private const float Z = .850650808352039932f;

    void Start()
    {
        PlanetMaker maker = GetComponent<PlanetMaker>();
        maker.Create();
    }

    public void Create()
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
            filter = gameObject.AddComponent<MeshFilter>();
        else
            filter = gameObject.GetComponent<MeshFilter>();

        mesh = filter.mesh;
        mesh.Clear();

        vertices = new[]
        {
            new Vector3(-X, 0.0f, Z), new Vector3(X, 0.0f, Z), new Vector3(-X, 0.0f, -Z), new Vector3(X, 0.0f, -Z),    
            new Vector3(0.0f, Z, X), new Vector3(0.0f, Z, -X), new Vector3(0.0f, -Z, X), new Vector3(0.0f, -Z, -X),    
            new Vector3(Z, X, 0.0f), new Vector3(-Z, X, 0.0f), new Vector3(Z, -X, 0.0f), new Vector3(-Z, -X, 0.0f)
        };

        triangles = new[]
        {
            0,1,4,  0,4,9,  9,4,5,  4,8,5,  4,1,8,
            8,1,10, 8,10,3, 5,8,3,  5,2,2,  2,3,7,
            7,3,10, 7,10,6, 7,6,11, 11,6,0, 0,6,1,
            6,10,1, 9,11,0, 9,2,11, 9,5,2,  7,11,2
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
