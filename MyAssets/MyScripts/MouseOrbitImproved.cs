using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour
{
	
	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	public float xMinLimit = -360f;
	public float xMaxLimit = 360f;
	public float distanceMin = .5f;
	public float distanceMax = 15f;
	public Material oclusionMaterial;
	public Dictionary<GameObject,Material> oclusionObjects;
	private Rigidbody rigidbody;
	float x = 0.0f;
	float y = 0.0f;
	
	// Use this for initialization
	void Start ()
	{
		oclusionObjects = new Dictionary<GameObject, Material> ();
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		rigidbody = GetComponent<Rigidbody> ();
		
		// Make the rigid body not change rotation
		if (rigidbody != null) {
			rigidbody.freezeRotation = true;
		}
	}
	
	void LateUpdate ()
	{
		if (target) {
			CheckOclusion ();
			if (Input.GetMouseButton (0)) {
				x += Input.GetAxis ("Mouse X") * xSpeed * distance * 0.02f;
				y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
			}
			
			y = ClampAngle (y, yMinLimit, yMaxLimit);
			x = ClampAngle (x, xMinLimit, xMaxLimit);
			
			Quaternion rotation = Quaternion.Euler (y, x, 0);
			
			distance = Mathf.Clamp (distance - Input.GetAxis ("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
			
			//RaycastHit hit;
			//if (Physics.Linecast (target.position, transform.position, out hit)) 
			//{
			//	distance -=  hit.distance;
			//}
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;
			
			transform.rotation = rotation;
			transform.position = position;
		}
	}

	void CheckOclusion ()
	{
		// Add to oclusion objects
		RaycastHit[] hits = Physics.RaycastAll (transform.position, transform.forward, Vector3.Distance (transform.position, Vector3.zero));
		List<GameObject> removalList = new List<GameObject> ();
		foreach (RaycastHit hit in hits) {
			if (!oclusionObjects.ContainsKey (hit.transform.gameObject)) {
				oclusionObjects.Add (hit.transform.gameObject, hit.transform.GetComponent<Renderer> ().material);
				hit.transform.GetComponent<Renderer> ().material = oclusionMaterial;
			}
		}
		foreach (KeyValuePair<GameObject,Material> ocludedObject in oclusionObjects) {
			bool stillInHits = false;
			foreach (RaycastHit hit in hits) {
				if (hit.transform.gameObject == ocludedObject.Key) {
					stillInHits = true;
				}
			}
			if (!stillInHits) {
				ocludedObject.Key.GetComponent<Renderer> ().material = ocludedObject.Value;
				removalList.Add (ocludedObject.Key);
			}

		}
		foreach (GameObject obj in removalList) {
			oclusionObjects.Remove (obj);
		}
	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}
}