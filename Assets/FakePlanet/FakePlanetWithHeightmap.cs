using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (PlanetGenerator))]
[RequireComponent (typeof (Heightmap))]
public class FakePlanetWithHeightmap : MonoBehaviour {

	public int iterations;
	public Vector3 rotation;
	private int i = 0;
	private PlanetGenerator generator;
	private Heightmap heightmap;

	private Renderer renderer;

	public GameObject preview;

	private Text text;
	
	void Start()
	{
		generator = GetComponent<PlanetGenerator>();
		generator.Create();

		heightmap = GetComponent<Heightmap>();
		heightmap.Init();

		renderer = preview.GetComponent<Renderer>();

		//heightmap.Cut();
		//renderer.material.mainTexture = heightmap.Texture;

		text = FindObjectOfType<Text>();
	}

	void Update()
	{
		if(i < iterations)
		{
			text.text = i.ToString();
			heightmap.Cut();
			renderer.material.mainTexture = heightmap.Texture;
			i++;
		}


		//transform.Rotate(rotation * Time.deltaTime);
	}
}