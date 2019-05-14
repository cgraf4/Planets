using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (PlanetGenerator))]
public class FakePlanet : MonoBehaviour {

	public Color baseColor;
	public int iterations;
	public Vector3 rotation;
	private int i = 0;
	private PlanetGenerator generator;
		
	public Text text;

	
	void Start()
	{
		generator = GetComponent<PlanetGenerator>();
		generator.Create();
		generator.SetBaseColor(baseColor);

		// text = FindObjectOfType<Text>();

		StartCoroutine("Terraform", iterations);

	}

	IEnumerator Terraform(int iterations)
	{
		int i=0;
		while(i<iterations)
		{
			text.text = i.ToString();
			generator.DivideSphere();
			i++;
			yield return null;
		}
	}

	void Update()
	{
		// if(i < iterations)
		// {
		// 	text.text = i.ToString();
		// 	generator.DivideSphere();
		// 	//Application.CaptureScreenshot("mars" + i + ".png");
		// 	i++;
		// }


		transform.Rotate(rotation * Time.deltaTime);
	}
}