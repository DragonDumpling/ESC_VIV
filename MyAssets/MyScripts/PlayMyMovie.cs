using UnityEngine;
using System.Collections;

public class PlayMyMovie : MonoBehaviour {
	public MovieTexture myMovieTexture;

	// Use this for initialization
	void Start () {
		//MovieTexture myMovieTexture = (MovieTexture)gameObject.GetComponent<MeshRenderer>().material.mainTexture;
		myMovieTexture.Play();
		myMovieTexture.loop = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
