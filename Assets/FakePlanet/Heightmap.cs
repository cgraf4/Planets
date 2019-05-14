using UnityEngine;
using System.Collections;

public class Heightmap : MonoBehaviour {

	public int width;
	public int height;
	private float[,] data;

	public float elevationDelta;

	private	float maxVal = 1;


	public void Init()
	{
		data = new float[height, width];
		
		for(int h = 0; h < height; h++)
		{
			for(int w = 0; w < width; w++)
			{
				data[h, w] = 0.5f;
			}
		}
	}

	public void Cut()
	{
		Vector2 pStart = new Vector2(Random.Range(0,width), Random.Range(0,height));
		Vector2 pEnd = new Vector2(Random.Range(0,width), Random.Range(0,height));

		if(pStart.x < pEnd.x)
		{
			Vector2 help = pStart;
			pStart = pEnd;
			pEnd = help;
		}

		Vector2 delta = pEnd-pStart;
		delta /= (pEnd.x-pStart.x);
		
		
		
		// int rand = Random.Range(0,2);

		// Vector2 pStart = Vector2.zero;
		// Vector2 pEnd = Vector2.zero;

		// float elevation = Random.Range(0f, elevationDelta);
		// maxVal = 1;

		// //vertical
		// if(rand == 0)
		// {
		// 	int col = Random.Range(0, width);
		// 	pStart = new Vector2(0, col);
		// 	pEnd = new Vector2(height-1, Random.Range(0,width));

		// 	Vector2 dir = pEnd - pStart;
		// 	dir/= (pEnd.x-pStart.x);



		// 	// Debug.Log("pStart: " + pStart + " | pEnd: " + pEnd + " | dir: " + dir);

		// 	for(int h = 0; h < height; h++)
		// 	{
		// 		Vector2 dot = pStart + dir*h;

		// 		for(int w = 0; w < width; w++)
		// 		{
		// 			// Debug.Log("[h,w]: [" + h + "," + w + "] | dot: " + dot);

		// 			if(w < Mathf.RoundToInt(dot.y))
		// 				data[h,w] -= elevation;
		// 			else
		// 				data[h,w] += elevation;
					
		// 			data[h,w] /= maxVal;
		// 		}
		// 	}
		// }
		// else
		// {
		// 	int row = Random.Range(0, height);
		// 	pStart = new Vector2(row, 0);
		// 	pEnd = new Vector2(Random.Range(0, height), width-1);

		// 	Vector2 dir = pEnd - pStart;
		// 	dir/= (pEnd.y-pStart.y);

		// 	// Debug.Log("pStart: " + pStart + " | pEnd: " + pEnd + " | dir: " + dir);

		// 	for(int w = 0; w < width; w++)
		// 	{
		// 		Vector2 dot = pStart + dir*w;

		// 		for(int h = 0; h < height; h++)
		// 		{
		// 			// Debug.Log("[h,w]: [" + h + "," + w + "] | dot: " + dot);

		// 			if(h < Mathf.RoundToInt(dot.x))
		// 				data[h,w] -= elevation;
		// 			else
		// 				data[h,w] += elevation;

		// 			data[h,w] /= maxVal;
		// 		}
		// 	}
		// }
	}

	public Texture2D Texture
	{
		get
		{
			Texture2D tex = new Texture2D(width, height);
			Color[] colors = new Color[tex.width * tex.height];
		
			for(int y = 0; y < tex.height; y++) 
			{
				for (int x=0; x < tex.width; x++) 
				{
					float sample = data[y, x];
					colors[y * tex.width + x] = new Color(sample, sample, sample);
				}
			}
			tex.SetPixels(colors);
			tex.Apply();

			return tex;
		}
	}

}
