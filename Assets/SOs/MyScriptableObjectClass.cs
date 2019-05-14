using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "CG/ScriptableObj", order = 1)]
public class MyScriptableObjectClass : ScriptableObject {
    public string objectName = "New MyScriptableObject";
    public bool colorIsRandom = false;
    public Color thisColor = Color.white;
    public Vector3[] spawnPoints;
	public GameObject gameObj;
}