using UnityEngine;
using System.Collections;

public class Spot : MonoBehaviour {

	private int _spotID = 0;

	public int spotID {
		get{
			return _spotID;
		}
		set{
			_spotID = value;
		}
	}
	private LightProperty _lightProperty;

	public LightProperty lightProperty {
		get{
			return _lightProperty;
		}
		set{
			_lightProperty = value;
		}
	}

	public Spot ( int incID, LightProperty incLight ){
		_spotID = incID;
		_lightProperty = incLight;
	}

}
