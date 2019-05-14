using UnityEngine;
using System.Collections;

public class PlanetGenerator : MonoBehaviour {

	public float radius;
	
	// Longitude |||
	public int divisionLong;
	
	// Latitude ---
	public int divisionsLat;

	public float elevationThreshold;

	public float colorDelta;

	private MeshFilter filter;
	private Mesh mesh;
	private Vector3[] vertices;
	private Vector3[] normals;
	private Vector2[] uvs;
	private int[] triangles;
	private Color[] colors;

	
	public void Create()
	{		
		if(gameObject.GetComponent<MeshFilter>() == null)
			filter = gameObject.AddComponent<MeshFilter>();
		else
			filter = gameObject.GetComponent<MeshFilter>();

		mesh = filter.mesh;
		mesh.Clear();
		
		#region Vertices
		vertices = new Vector3[(divisionLong+1) * divisionsLat + 2];
		float _pi = Mathf.PI;
		float _2pi = _pi * 2f;
		
		vertices[0] = Vector3.up * radius;
		for( int lat = 0; lat < divisionsLat; lat++ )
		{
			float a1 = _pi * (float)(lat+1) / (divisionsLat+1);
			float sin1 = Mathf.Sin(a1);
			float cos1 = Mathf.Cos(a1);
		
			for( int lon = 0; lon <= divisionLong; lon++ )
			{
				float a2 = _2pi * (float)(lon == divisionLong ? 0 : lon) / divisionLong;
				float sin2 = Mathf.Sin(a2);
				float cos2 = Mathf.Cos(a2);
		
				vertices[ lon + lat * (divisionLong + 1) + 1] = new Vector3( sin1 * cos2, cos1, sin1 * sin2 ) * radius;
			}
		}
		vertices[vertices.Length-1] = Vector3.up * -radius;
		#endregion

		#region Vertex colors
		colors = new Color[vertices.Length];
		#endregion
		
		#region Normales		
		normals = new Vector3[vertices.Length];
		for( int n = 0; n < vertices.Length; n++ )
			normals[n] = vertices[n].normalized;
		#endregion
		
		#region UVs
		uvs = new Vector2[vertices.Length];
		uvs[0] = Vector2.up;
		uvs[uvs.Length-1] = Vector2.zero;
		for( int lat = 0; lat < divisionsLat; lat++ )
			for( int lon = 0; lon <= divisionLong; lon++ )
				uvs[lon + lat * (divisionLong + 1) + 1] = new Vector2( (float)lon / divisionLong, 1f - (float)(lat+1) / (divisionsLat+1) );
		#endregion
		
		#region Triangles
		int nbFaces = vertices.Length;
		int nbTriangles = nbFaces * 2;
		int nbIndexes = nbTriangles * 3;
		triangles = new int[ nbIndexes ];


		
		//Top Cap
		int i = 0;
		for( int lon = 0; lon < divisionLong; lon++ )
		{
			triangles[i++] = lon+2;
			triangles[i++] = lon+1;
			triangles[i++] = 0;
		}
		
		//Middle
		for( int lat = 0; lat < divisionsLat - 1; lat++ )
		{
			for( int lon = 0; lon < divisionLong; lon++ )
			{
				int current = lon + lat * (divisionLong + 1) + 1;
				int next = current + divisionLong + 1;
		
				triangles[i++] = current;
				triangles[i++] = current + 1;
				triangles[i++] = next + 1;
		
				triangles[i++] = current;
				triangles[i++] = next + 1;
				triangles[i++] = next;
			}
		}
		
		//Bottom Cap
		for( int lon = 0; lon < divisionLong; lon++ )
		{
			triangles[i++] = vertices.Length - 1;
			triangles[i++] = vertices.Length - (lon+2) - 1;
			triangles[i++] = vertices.Length - (lon+1) - 1;
		}
		#endregion
		
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.colors = colors;
		
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	public void DivideSphere()
	{
		float elevation = Random.Range(0f, elevationThreshold);

		Vector3 randomVec = vertices[Random.Range(0, vertices.Length)];

		Vector3 tColorVec = Vector3.zero;
		Vector3 ntVector = Vector3.zero;

		for(int i = 0; i < vertices.Length; i++)
		{
			Vector3 tVector = vertices[i]-transform.position;

			if(Vector3.Dot(randomVec, tVector) >= 0)
			{
				ntVector = tVector.normalized;
				vertices[i] += ntVector * elevation;
				tColorVec = new Vector3(colors[i].r + colorDelta, colors[i].b + colorDelta, colors[i].g + colorDelta);
			}
			else
			{
				ntVector = tVector.normalized;
				vertices[i] -= ntVector * elevation;
				tColorVec = new Vector3(colors[i].r - colorDelta, colors[i].b - colorDelta, colors[i].g - colorDelta);			
			}

			//tColor /=2;
			colors[i] = new Color(tColorVec.x, tColorVec.y, tColorVec.z);

			/*colors[i].r = Mathf.Clamp(colors[i].r, minColor.r, maxColor.r);
			colors[i].g = Mathf.Clamp(colors[i].g, minColor.g, maxColor.g);
			colors[i].b = Mathf.Clamp(colors[i].b, minColor.b, maxColor.b);*/
		}

		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.colors = colors;
		
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	public void SetBaseColor(Color baseColor)
	{
		for(int i = 0; i < colors.Length; i++)
		{
			colors[i] = baseColor;
		}
	}
}
