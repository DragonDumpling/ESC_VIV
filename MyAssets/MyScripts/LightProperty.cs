using UnityEngine;
using System.Collections;

public class LightProperty : MonoBehaviour
{
	public Renderer lightSurface;
	public Light lightsettings;
	private Material materialCopy;
	public Color assignedColor;
	public LightState lightState = LightState.off;
	private float assignedIntencity = 8.0f;
	public LensFlare lensFlare;
	public int spotID = 01;
	private Color lastColor = Color.black;
	private float interpliation = 0.0f;
	private float speed = 1f;
	private float intencityInterp = 0f;
	private float intencitySpeed = 1f;
	private float oldIntencity = 0f;
	private float lastCheckTime = 0f;
	public Texture[] cookies;

	// Use this for initialization
	void Start ()
	{
		lightsettings = gameObject.GetComponentInChildren<Light> ();
		materialCopy = Instantiate (lightSurface.material);
		lightSurface.material = materialCopy;
		SetLight(LightState.off,Color.blue,1f);
		StartCoroutine(Diffuse());
	}

	public void SetLight (LightState state, Color color, float incSpeed)
	{
		lastCheckTime = Time.time;
		speed = incSpeed;
		switch (state) {
		case(LightState.off):
			lensFlare.enabled = false;
			lightSurface.enabled = false;
			assignedColor = Color.black;
			assignedIntencity = 0f;
			break;
		case(LightState.dim):
			lensFlare.enabled = true;
			lightSurface.enabled = true;
			assignedIntencity = 3f;
			assignedColor = color;
			lensFlare.color = color;
			break;
		case(LightState.on):
			lensFlare.enabled = true;
			lightSurface.enabled = true;
			assignedIntencity = 6f;
			assignedColor = color;
			lensFlare.color = color;
			break;
		case(LightState.colorOnly):
			assignedColor = color;
			lensFlare.color = color;
			break;
		}
	}

	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.I)) {
			StartCoroutine (Diffuse ());
		}
		if (Input.GetKeyDown (KeyCode.O)) {
			StartCoroutine (Focus ());
		}

		if (lastCheckTime + 5 < Time.time)
			return;
		if (assignedColor != lightsettings.color) {
			Color newColor = Color.Lerp (lastColor, assignedColor, interpliation);
			lightsettings.color = newColor;
			interpliation += Time.deltaTime * speed;
		} else {
			interpliation = 0;
			lastColor = assignedColor;
		}
		if (assignedIntencity != lightsettings.intensity) {
			float newIntencity = Mathf.Lerp (oldIntencity, assignedIntencity, intencityInterp);
			lightsettings.intensity = newIntencity;
			intencityInterp += Time.deltaTime * speed;
		} else {
			intencityInterp = 0f;
			oldIntencity = lightsettings.intensity;
		}
	}



	public void HighLight (float durration)
	{
		Debug.Log ("highlighting player on spot " + spotID);
		StartCoroutine (TriggerHighLight (durration));
	}

	public IEnumerator TriggerHighLight (float durration)
	{

		speed = 10;
		lastCheckTime = Time.time;
		float savedIntensity = lightsettings.intensity;
		bool previousEnabled = lensFlare.enabled;
		//lightsettings.enabled = true;
		lensFlare.enabled = true;
		lightSurface.enabled = true;
		//lightsettings.intensity = 8.0f;
		assignedIntencity = 8.0f;
		yield return new WaitForSeconds (durration);
		lightsettings.enabled = previousEnabled;
		lensFlare.enabled = previousEnabled;
		lightSurface.enabled = previousEnabled;
		assignedIntencity = savedIntensity;
	}

	public IEnumerator Diffuse ()
	{
		for (int i = 0; i < cookies.Length; i ++) {
			lightsettings.cookie = cookies [i];
			for (var k = 0; k < 10; k++) {
				if (lightsettings.spotAngle < 30)
					lightsettings.spotAngle += 0.5f;
				else
					lightsettings.cookie = cookies [cookies.Length - 1];
				yield return new WaitForEndOfFrame ();
			}
		}
	}

	public IEnumerator Focus ()
	{
		for (int i = cookies.Length -1; i > -1; i --) {
			lightsettings.cookie = cookies [i];
			for (var k = 0; k < 10; k++) {
				if (lightsettings.spotAngle > 13)
					lightsettings.spotAngle -= 0.5f;
				else
					lightsettings.cookie = cookies [0];
				yield return new WaitForEndOfFrame ();
			}
		}
	}
	

}
