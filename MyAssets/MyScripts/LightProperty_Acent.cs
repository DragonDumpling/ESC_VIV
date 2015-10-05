using UnityEngine;
using System.Collections;

public class LightProperty_Acent : MonoBehaviour
{
	public Renderer lightSurface;
	public Light lightsettings;
	private Material materialCopy;
	public Color assignedColor;
	public LightState lightState = LightState.off;
	private float assignedIntencity;
	public LensFlare lensFlare;
	public int spotID = 01;
	private Color lastColor = Color.black;
	private float interpliation = 0.0f;
	private float speed = 1f;
	private float intencityInterp = 0f;
	private float intencitySpeed = 1f;
	private float oldIntencity = 0f;
	private float lastCheckTime = 0f;
	public Transform rotationController;
	public Vector3[] presetRotationsEulers;
	private Vector3 assignedRotation;
	private Vector3 lastRotation;
	public GameObject[] lightingCones;

	//public Material[] coneMats;

	public float onDur = 1f;
	public float waitDur = 0f;
	public Color setColor = Color.white;
	public float washerIntencity = 8.0f;
	private float randomChangeRotation = 0f;
	private int randomRotInt = 0;
	public Texture[] cookies;
	public Transform lookatTarget;
	private float rotationSpeed = 5;

	// Use this for initialization
	void Start ()
	{
		assignedRotation = presetRotationsEulers [0];
		lastRotation = presetRotationsEulers [0];
		lightsettings = gameObject.GetComponentInChildren<Light> ();
		materialCopy = Instantiate (lightSurface.material);
		lightSurface.material = materialCopy;
		SetLight (LightState.off, Color.blue, 1f);
		StartCoroutine ("Swing");
	}

	IEnumerator Swing ()
	{

		while (true) {
			while (lookatTarget != null)
				yield return null;
			Debug.Log ("swinging");
			float randomDelay = Random.Range (1, 3);
			rotationSpeed = randomDelay;
			assignedRotation = presetRotationsEulers[Random.Range(0,presetRotationsEulers.Length)];
			Debug.Log ("assigned rotation: " + assignedRotation.ToString ());
			yield return new WaitForSeconds (randomDelay);
		}
	}

	public void SetLight (LightState state, Color color, float incSpeed)
	{
		lastCheckTime = Time.time;
		speed = incSpeed;
		switch (state) {
		case(LightState.off):
			lensFlare.enabled = false;
			lightSurface.enabled = false;
			foreach (GameObject cone in lightingCones) {
				cone.SetActive (false);
			}
			assignedColor = Color.black;
			assignedIntencity = 0f;
			break;
		case(LightState.dim):
			lensFlare.enabled = true;
			lightSurface.enabled = true;
			foreach (GameObject cone in lightingCones) {
				cone.SetActive (true);
			}
			assignedIntencity = 3f;
			assignedColor = color;
			lensFlare.color = color;
			break;
		case(LightState.on):
			lensFlare.enabled = true;
			lightSurface.enabled = true;
			foreach (GameObject cone in lightingCones) {
				cone.SetActive (true);
			}
			assignedIntencity = 6f;
			assignedColor = color;
			lensFlare.color = color;
			break;
		case(LightState.colorOnly):
			assignedColor = color;
			lensFlare.color = color;
			break;
		}
		foreach (GameObject cone in lightingCones) {
			cone.GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", color);
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.M)) {
			this.SetLight (LightState.on, Color.blue, 0.5f);
			ChangeCoookie(0);
		}
		if (Input.GetKeyDown (KeyCode.T)) {
			RotateTo (0);
		}
		Vector3 newDirection = Quaternion.Euler (assignedRotation) * Vector3.forward;
		rotationController.forward = Vector3.RotateTowards (rotationController.forward, newDirection, rotationSpeed * Time.deltaTime, 0.0f);
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

	public void LightUp ()
	{
		Debug.Log ("highlighting player on spot " + spotID);
		StartCoroutine (TriggerHighLight ());
	}

	public IEnumerator TriggerHighLight ()
	{
		yield return new WaitForSeconds (waitDur);
		speed = 10;
		lastCheckTime = Time.time;
		float savedIntensity = lightsettings.intensity;
		bool previousEnabled = lensFlare.enabled;
		lensFlare.color = setColor;
		assignedColor = setColor;

		//lightsettings.color = setColor;
		lensFlare.enabled = true;
		lightSurface.enabled = true;
		assignedIntencity = washerIntencity;
		foreach (GameObject cone in lightingCones) {
			cone.SetActive (true);
			cone.GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", setColor);
		}
		yield return new WaitForSeconds (onDur);
		lightsettings.enabled = previousEnabled;
		lensFlare.enabled = previousEnabled;
		lightSurface.enabled = previousEnabled;
		assignedIntencity = savedIntensity;
		foreach (GameObject cone in lightingCones) {
			cone.SetActive (previousEnabled);
		}
	}

	public void RotateTo (int rotationIndex)
	{
		lastCheckTime = Time.time;
		assignedRotation = presetRotationsEulers [rotationIndex];
	}

	private Vector3 v3Clamp (Vector3 incV3)
	{
		Vector3 refurbishedV3 = incV3;
		if (refurbishedV3.x > 360)
			refurbishedV3.x = 0;
		if (refurbishedV3.x < 0)
			refurbishedV3.x = 360;

		if (refurbishedV3.y > 360)
			refurbishedV3.y = 0;
		if (refurbishedV3.y < 0)
			refurbishedV3.y = 360;

		if (refurbishedV3.z > 360)
			refurbishedV3.z = 0;
		if (refurbishedV3.z < 0)
			refurbishedV3.z = 360;
		return refurbishedV3;
	}

	public void ChangeCoookie (int incCookieIndex)
	{
		lightsettings.cookie = cookies [incCookieIndex];
	}
	

}
