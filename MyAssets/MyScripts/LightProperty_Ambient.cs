using UnityEngine;
using System.Collections;

public class LightProperty_Ambient : MonoBehaviour {
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



	// Use this for initialization
	void Start () {
		lightsettings = gameObject.GetComponentInChildren<Light>();
		materialCopy = Instantiate(lightSurface.material);
		lightSurface.material = materialCopy;
		SetLight(LightState.off,Color.blue,1f);
	}
	public void SetLight (LightState state, Color color, float incSpeed){
		lastCheckTime = Time.time;
		speed = incSpeed;
		switch(state){
		case(LightState.off):
			lensFlare.enabled = false;
			lightSurface.enabled = false;
			assignedColor = Color.black;
			assignedIntencity = 0f;
			break;
		case(LightState.dim):
			lensFlare.enabled = true;
			lightSurface.enabled = true;
			assignedIntencity = 0.2f;
			assignedColor = color;
			lensFlare.color = color;
			break;
		case(LightState.on):
			lensFlare.enabled = true;
			lightSurface.enabled = true;
			assignedIntencity = 0.5f;
			assignedColor = color;
			lensFlare.color = color;
			break;
		case(LightState.colorOnly):
			assignedColor = color;
			lensFlare.color = color;
			break;
		}
	}
	void Update(){
		if(lastCheckTime + 5 < Time.time)
			return;
		if(assignedColor != lightsettings.color)
		{
			Color newColor = Color.Lerp(lastColor,assignedColor,interpliation);
			lightsettings.color = newColor;
			interpliation += Time.deltaTime * speed;
		}
		else{
			interpliation = 0;
			lastColor = assignedColor;
		}
		if(assignedIntencity != lightsettings.intensity){
			float newIntencity = Mathf.Lerp(oldIntencity,assignedIntencity,intencityInterp);
			lightsettings.intensity = newIntencity;
			intencityInterp += Time.deltaTime * speed;
		}
		else{
			intencityInterp = 0f;
			oldIntencity = lightsettings.intensity;
		}

	}
}


