using UnityEngine;
using System.Collections;

public class Plasma : MonoBehaviour {

	public int size;
	
	public int seed;
	public float rough;

	private int _size;

	private Texture2D noiseTex;
	private float [,] heights;
	private Renderer targetRenderer;
	
	// Use this for initialization
	void Start () {
		_size = (int)Mathf.Pow(2,size);

		Debug.Log("_size: " + _size);
		
		noiseTex = new Texture2D(_size, _size);
		targetRenderer= GetComponent<Renderer>();
		
		// init corner points
		// heights[0,0] = Random.Range(0f,1f);
		// heights[0,size-1] = Random.Range(0f,1f);
		// heights[size-1,0] = Random.Range(0f,1f);
		// heights[size-1,size-1] = Random.Range(0f,1f);

		// Debug.Log(string.Format("r0[{0}, {0}] | r1[{0}, {1}] | r2[{1}, {0}] | r3[{1}, {1}]", 0, size-1));

		heights = DiamondSquare(2, 1, 1);

		UpdateTexture();
	}

	void DiamondSquareFill(int rmin, int cmin, int rmax, int cmax)
	{
		Debug.Log(string.Format("r0[{0}, {1}] | r1[{0}, {3}] | r2[{2}, {1}] | r3[{2}, {3}]", rmin, cmin, rmax, cmax));
		
		if(rmin+1 <= rmax || cmin+1 <= cmax)
			return;

		float c1 = heights[rmin,cmin];
		float c2 = heights[rmin,cmax];
		float c3 = heights[rmax,cmin];
		float c4 = heights[rmax,cmax];
		
		// diamond step
		heights[rmax/2, cmax/2] = Average(c1, c2, c3, c4) + Random.Range(0f,1f);

		// square step
		heights[rmin, cmax/2] = Average(c1, c2, c3, c4) + Random.Range(0f,1f);
		heights[rmax/2, cmin] = Average(c1, c2, c3, c4) + Random.Range(0f,1f);
		heights[rmax/2, cmax] = Average(c1, c2, c3, c4) + Random.Range(0f,1f);
		heights[rmax, cmax/2] = Average(c1, c2, c3, c4) + Random.Range(0f,1f);

		DiamondSquareFill(rmin, cmin, rmax/2, cmax/2);
		// DiamondSquareFill(rmin, cmax/2, rmax/2, cmax);
		// DiamondSquareFill(rmax/2, cmin, rmax, cmax/2);
		// DiamondSquareFill(rmax/2, cmin/2, rmax, cmax);

		return;

	}

	float[,] DiamondSquare(int size, float rough, int seed)
	{
		int depth = size-1;
		float[,] dsMap = new float[(int) Mathf.Round(Mathf.Pow(2, size)+1), (int) Mathf.Round(Mathf.Pow(2, size)+1)];
		dsMap[0,0] = rough * (2 * Random.Range(0f, 1f)-1);
		dsMap[0,size-1] = rough * (2 * Random.Range(0f, 1f)-1);
		dsMap[size-1,0] = rough * (2 * Random.Range(0f, 1f)-1);
		dsMap[size-1,size-1] = rough * (2 * Random.Range(0f, 1f)-1);

		int iteration;
		bool putX, putZ;

		while(depth > -1)
		{
			iteration = (int) Mathf.Round(Mathf.Pow(2, depth));
			putX = false;

			for(int i = 0; i<dsMap.GetLength(0); i+=iteration)
			{
				putZ = false;
				for(int j = 0; j < dsMap.GetLength(1); j+=iteration)
				{
					if(putX == true && putZ == true)
					{
						// put diamond
						dsMap[i,j] = (  dsMap[i-iteration,j-iteration] +
										dsMap[i+iteration,j-iteration] +
										dsMap[i-iteration,j+iteration] +
										dsMap[i+iteration,j+iteration] ) / 4 
										+ rough * (2 * Random.Range(0f,1f )-1);
					}
					if(putX != putZ)
					{
						// put squares
						if(putX == true)
						{
							dsMap[i,j] = (  dsMap[i-iteration,j] +
											dsMap[i+iteration,j] )/ 2
											+ rough * (2 * Random.Range(0f,1f )-1);
						}
						else
						{
							dsMap[i,j] = (  dsMap[i,j-iteration] +
											dsMap[i,j+iteration] )/ 2
											+ rough * (2 * Random.Range(0f,1f )-1);
						}
					}
					putZ = !putZ;
				}
				putX = !putX;
			}
			rough/=2;
			depth--;
		}
		for(int i = 0; i<dsMap.GetLength(0); i++)
		{
			for(int j = 0; j < dsMap.GetLength(1); j++)
			{
				dsMap[i,j] = (float) (dsMap[i,j] <= 0? -Mathf.Sqrt(Mathf.Abs(dsMap[i,j])) : dsMap[i,j]);
			}
		}

		return dsMap;
	}

	float Average(float f1, float f2, float f3, float f4)
	{
		return ((f1+f2+f3+f4)/4);
	}
	
	void UpdateTexture()
	{
		Color[] colors = new Color[_size * _size];
		
		for(int r = 0; r < _size; r++)
		{
			for(int c = 0; c < _size; c++)
			{
				Debug.Log("r: " + r + " | c: " + c);

				float sample = heights[r, c];

				colors[r*(_size)+c] = new Color(sample, sample, sample);
			}
		}

		noiseTex.SetPixels(colors);
		noiseTex.Apply();
		targetRenderer.material.mainTexture = noiseTex;
	}

}
