using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class QuadGenerator : MonoBehaviour
{

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Texture2D _texture;

    public int CutIterations;
    public float ColorDelta;
    public Vector2i TextureSize;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        MakeQuad();

        _texture = MakeTexture(TextureSize.x, TextureSize.y);
        _meshRenderer.material.SetTexture("_MainTex", _texture);

        for (int i = 0; i < CutIterations; i++)
        {
            // CutTextureDDA(ref _texture);
			CutTextureBresenham(ref _texture);
        }
    }

    public void MakeQuad()
    {
        List<Vector3> vertexList = new List<Vector3>();
        List<int> triangleList = new List<int>();
        List<Vector2> uvList = new List<Vector2>();

        // Make first triangle.
        vertexList.Add(new Vector3(-0.5f, -0.5f, 0.0f));
        vertexList.Add(new Vector3(0.5f, -0.5f, 0.0f));
        vertexList.Add(new Vector3(0.5f, 0.5f, 0.0f));
        vertexList.Add(new Vector3(-0.5f, 0.5f, 0.0f));

        triangleList.Add(0);
        triangleList.Add(2);
        triangleList.Add(1);

        triangleList.Add(0);
        triangleList.Add(3);
        triangleList.Add(2);

        uvList.Add(new Vector2(0, 0));
        uvList.Add(new Vector2(1, 0));
        uvList.Add(new Vector2(1, 1));
        uvList.Add(new Vector2(0, 1));


        Mesh mesh = new Mesh();
        mesh.vertices = vertexList.ToArray();
        mesh.triangles = triangleList.ToArray();
        mesh.uv = uvList.ToArray();

        _meshFilter.mesh.Clear();
        _meshFilter.mesh = mesh;
        _meshFilter.mesh.RecalculateNormals();
        _meshFilter.mesh.RecalculateBounds();
    }

    public Texture2D MakeTexture(int width, int height)
    {
        Texture2D tex = new Texture2D(width, height);
        Color[] colors = new Color[tex.width * tex.height];

        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                colors[y * tex.width + x] = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        }
        
		tex.SetPixels(colors);
        tex.Apply();

		foreach(Color c in tex.GetPixels())
		{
			Debug.Log(string.Format("<color=magenta>color: {0}</color>", c));
		}

        return tex;
    }


 public void CutTextureBresenham(ref Texture2D texture)
    {
        Vector2 start = new Vector2(0, UnityEngine.Random.Range(0, texture.height));
        Vector2 end = new Vector2(texture.width, UnityEngine.Random.Range(0, texture.height));

        int x = (int)start.x;
        int y = (int)start.y;
		
		texture.SetPixel(x, y, Color.red);

        int dx = (int)end.x - (int)start.x;
        int dy = (int)end.y - (int)start.y;

		int twody = dy*2;
		int twodytwodx = dy*2 -dx*2;

		int p = twody-dx;

        for (int k = 0; k < dx; k++)
        {
			x+=1;

			if(p < 0)
			{
				texture.SetPixel(x, y, Color.red);
				p += twody;
			}
			else
			{
				y+=1;

				texture.SetPixel(x, y, Color.red);
				p += twodytwodx; 
			}
        }

        texture.Apply();

		foreach(Color c in texture.GetPixels())
		{
			Debug.Log(string.Format("<color=orange>color: {0}</color>", c));
		}
    }

    public void CutTextureDDA(ref Texture2D texture)
    {
        Vector2 start = new Vector2(0, UnityEngine.Random.Range(0, texture.height));
        Vector2 end = new Vector2(texture.width, UnityEngine.Random.Range(0, texture.height));

        float dx = end.x - start.x;
        float dy = end.y - start.y;

        float m = dy / dx;

        float x = start.x;
        float y = start.y;

        Color col = texture.GetPixel(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        col.r -= ColorDelta;
        col.g -= ColorDelta;
        col.b -= ColorDelta;
        col.a = 1;

        texture.SetPixel(Mathf.RoundToInt(x), Mathf.RoundToInt(y), col);

        // below line
        for (int i = Mathf.RoundToInt(y)-1; i > 0; i--)
        {
            Color col1 = texture.GetPixel(Mathf.RoundToInt(x), i);
            col1.r -= ColorDelta;
            col1.g -= ColorDelta;
            col1.b -= ColorDelta;
            col1.a = 1;

            texture.SetPixel(Mathf.RoundToInt(x), i, col1);
        }

        // above line
        for (int i = Mathf.RoundToInt(y)+1; i < texture.height; i++)
        {
            Color col1 = texture.GetPixel(Mathf.RoundToInt(x), i);
            col1.r += ColorDelta;
            col1.g += ColorDelta;
            col1.b += ColorDelta;
            col1.a = 1;

            texture.SetPixel(Mathf.RoundToInt(x), i, col1);
        }


        for (int k = 0; k < dx; k++)
        {
            x += 1;
            y += m;

            Color col1 = texture.GetPixel(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            col1.r -= ColorDelta;
            col1.g -= ColorDelta;
            col1.b -= ColorDelta;

            texture.SetPixel(Mathf.RoundToInt(x), Mathf.RoundToInt(y), col1);

            // below line
            for (int i = Mathf.RoundToInt(y)-1; i > 0; i--)
            {
                Color col2  = texture.GetPixel(Mathf.RoundToInt(x), i);
                col2.r -= ColorDelta;
                col2.g -= ColorDelta;
                col2.b -= ColorDelta;

                texture.SetPixel(Mathf.RoundToInt(x), i, col2);
            }

            // above line
            for (int i = Mathf.RoundToInt(y)+1; i < texture.height; i++)
            {
                Color col2 = texture.GetPixel(Mathf.RoundToInt(x), i);
                col2.r += ColorDelta;
                col2.g += ColorDelta;
                col2.b += ColorDelta;

                texture.SetPixel(Mathf.RoundToInt(x), i, col2);
            }
        }

        texture.Apply();

		foreach(Color c in texture.GetPixels())
		{
			Debug.Log(string.Format("<color=orange>color: {0}</color>", c));
		}
    }
}

[Serializable]
public class Vector2i
{
    public int x;
    public int y;
}
