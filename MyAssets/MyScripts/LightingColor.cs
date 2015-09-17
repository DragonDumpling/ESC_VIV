using UnityEngine;
using System.Collections;
public enum LightingColorE
{
	Red = 0,
	Orange = 1,
	Amber = 2,
	Yellow = 3,
	Green = 4,
	Cyan = 5,
	Blue = 6,
	Light_Blue = 7,
	Magenta = 8,
	Lavender = 9,
	Pink = 10,
	Warm_White = 11,
	Neatural_White = 12,
	Cool_White = 13,
	Black = 14,
	Red_Amber_Yellow = 15,
	Blue_Cyan_Green = 16,
	Red_Blue = 17,
	Amber_Blue = 18,
	Red_White_Blue = 19,
	Red_Pink_Purple = 20
}

public class LightingColor : MonoBehaviour
{


	public Color[] colors;

	public Color GetColor (LightingColorE colorEnum)
	{
		return colors [(int)colorEnum];
	}


}
