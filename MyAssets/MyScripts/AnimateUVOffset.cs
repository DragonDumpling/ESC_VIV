using UnityEngine;
using System.Collections;

public class AnimateUVOffset : MonoBehaviour {

	public Material myMaterial;
	public Vector2 materialOffsetAnimation;

	// Use this for initialization
	void Start () {
		myMaterial = Instantiate( gameObject.GetComponent<Renderer>().material) as Material;
		gameObject.GetComponent<Renderer>().material = myMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		myMaterial.mainTextureOffset += materialOffsetAnimation * Time.deltaTime;
	}
}
