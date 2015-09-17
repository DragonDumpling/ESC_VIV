using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{

	public GameObject controlPannel;
	public LightingConcole lightingCon;
	public GameObject dynamicButton;
	public Color buttonColor_normal;
	public Color buttonColor_active;
	private List<GameObject> activeButtons;
	public LightingColor lightingColors;

	void Start ()
	{

		activeButtons = new List<GameObject> ();
		Create_ShowControlButton ();
		controlPannel.SetActive(false);
	}

	public void Create_ShowControlButton ()
	{
		Button newButton = NewButton ("Show Control", buttonColor_normal);
		newButton.onClick.AddListener (delegate { 
			MainMenu ();
		});
	}

	public void MainMenu ()
	{
		ClearActiveButtons ();
		Create_ShowControlButton ();
		Button teamSetupButton = NewButton ("Team Setup", buttonColor_normal);
		teamSetupButton.onClick.AddListener (delegate {
			TeamSetupMenu ();
		});

		Button CuesSelectionButton = NewButton("Cues", buttonColor_normal);
		CuesSelectionButton.onClick.AddListener(delegate {
			Menu_Cues();
		});


	}

	public void Menu_Cues(){
		ClearActiveButtons();
		Create_ShowControlButton();

		Button FindSpotQ = NewButton("Find Location",buttonColor_normal);
		FindSpotQ.onClick.AddListener(delegate {
			lightingCon.Cue_FindLocation();
		});

		Button TrainingModeButton = NewButton("Training Mode",buttonColor_normal);
		TrainingModeButton.onClick.AddListener(delegate {
			lightingCon.Cue_Training_Mode();
		});

		Button HighlightSpot = NewButton("Highlight Spot", buttonColor_normal);
		HighlightSpot.onClick.AddListener(delegate {
			Menu_HighlightSpot();
		});

		Button Tention_FastButton = NewButton("Tention_Fast",buttonColor_normal);
		Tention_FastButton.onClick.AddListener(delegate {
			lightingCon.Cue_TentionFast();
		});

		Button Tention_MediumButton = NewButton("Tention_Medium",buttonColor_normal);
		Tention_MediumButton.onClick.AddListener(delegate {
			lightingCon.Cue_TentionMedium();
		});

		Button Tention_SlowButton = NewButton("Tention_Slow",buttonColor_normal);
		Tention_SlowButton.onClick.AddListener(delegate {
			lightingCon.Cue_TentionSlow();
		});

		Button SetWasherButton = NewButton("SetWasher",buttonColor_normal);
		SetWasherButton.onClick.AddListener(delegate {
			Menu_SetWasher(0);
		});

		Button lightingPulseButton = NewButton("SetLightingPulse",buttonColor_normal);
		lightingPulseButton.onClick.AddListener(delegate {
			Menu_SetLightingPulse();
		});



	}
	public void Menu_SetWasher(int activeTiming){
		Debug.Log("Menu_SetWasher: " + activeTiming);
		ClearActiveButtons();
		Create_ShowControlButton();
		Color invis = Color.black;
		invis.a = 0.05f;
		Button emptyButton = NewButton("Timing",invis);
		foreach(LightingConcole.Timing pulse in System.Enum.GetValues(typeof(LightingConcole.Timing))){
			Button TimingButton = NewButton (pulse.ToString (),(int)pulse == activeTiming? buttonColor_active: buttonColor_normal);
			int pulseIndex = (int)pulse;
			TimingButton.onClick.AddListener (delegate {
				Menu_SetWasher (pulseIndex);
			});
		}
		foreach(LightingColorE lColor in System.Enum.GetValues(typeof(LightingColorE))){
			Button lighColorButton = NewButton_Nested(lColor.ToString(),lightingColors.GetColor(lColor));
			LightingColorE exitColor = lColor;
			lighColorButton.onClick.AddListener(delegate {
				lightingCon.SetLightingWash(exitColor,(LightingConcole.Timing)activeTiming);
			});
		}

	}
	public void Menu_SetLightingPulse(){
		ClearActiveButtons();
		Create_ShowControlButton();
		foreach(LightingConcole.Pulse pulse in System.Enum.GetValues(typeof(LightingConcole.Pulse))){
			LightingConcole.Pulse pu = pulse;
			Button pulseSelection = NewButton(pu.ToString(),buttonColor_normal);
			pulseSelection.onClick.AddListener(delegate {
				lightingCon.SetLightingPulse(pu);
			});
		}
	}

	public void Menu_HighlightSpot(){
		ClearActiveButtons();
		Create_ShowControlButton();
		for(int i = 1 ; i < 7; i ++){
			int spotId = i;
			Button spotHighlightButton = NewButton(spotId.ToString(),buttonColor_normal);
			spotHighlightButton.onClick.AddListener(delegate {
				lightingCon.HighlightPlayer(spotId,LightingConcole.Duration.Short);
			});
		}
		for(int i = 7 ; i < 31; i ++){
			int spotId = i;
			Button spotHighlightButton = NewButton_Nested(spotId.ToString(),buttonColor_normal);
			spotHighlightButton.onClick.AddListener(delegate {
				lightingCon.HighlightPlayer(spotId,LightingConcole.Duration.Short);
			});
		}
	}

	public void TeamSetupMenu ()
	{
		ClearActiveButtons ();
		Create_ShowControlButton ();
		Button setTeamNumberButton = NewButton ("Team Count", buttonColor_normal);
		setTeamNumberButton.onClick.AddListener (delegate {
			TeamCountSetupMenu ();
		});
		Button setTeamColorButton = NewButton("Team Colors", buttonColor_normal);
		setTeamColorButton.onClick.AddListener(delegate {
			Menu_SetTeamColors(1);
		
		});
	}

	public void Menu_SetTeamColors(int activeTeam){
		ClearActiveButtons ();
		Create_ShowControlButton ();
		for (int i = 1; i < 7; i ++) {
			Button teamNumButton = NewButton (i.ToString (),i == activeTeam? buttonColor_active: buttonColor_normal);
			int teamCountToSet = i;
			teamNumButton.onClick.AddListener (delegate {
				Menu_SetTeamColors (teamCountToSet);
			});
		}
		foreach(LightingColorE lColor in System.Enum.GetValues(typeof(LightingColorE))){
			Button lighColorButton = NewButton_Nested(lColor.ToString(),lightingColors.GetColor(lColor));
			LightingColorE exitColor = lColor;
			lighColorButton.onClick.AddListener(delegate {
				SetTeamColorsTo(activeTeam,exitColor);
			});
		}
	}

	public void TeamCountSetupMenu ()
	{
		ClearActiveButtons ();
		Create_ShowControlButton ();
		for (int i = 1; i < 7; i ++) {
			Button teamNumButton = NewButton (i.ToString (), buttonColor_normal);
			int teamCountToSet = i;
			teamNumButton.onClick.AddListener (delegate {
				SetTeamCountTo (teamCountToSet);
			});
		}
	}

	public void ClearActiveButtons ()
	{
		for (int i = activeButtons.Count - 1; i > -1; i--) {
			Destroy (activeButtons [i]);
		}
		activeButtons.Clear();
	}

	public void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Return)) {
			controlPannel.SetActive (!controlPannel.activeSelf);
		}
	}

	public void SetTeamCountTo (int teamCount)
	{
		lightingCon.SetTeams (teamCount);
		ClearActiveButtons();
		Create_ShowControlButton();
	}
	public void SetTeamColorsTo (int TeamID, LightingColorE incColor){
		Debug.Log("Reqeusting lightcolor: "+ incColor.ToString() + " for team: " + TeamID);
		lightingCon.SetLightingForTeam(TeamID, incColor);
		//ClearActiveButtons();
		//Create_ShowControlButton();
	}

	Button NewButton (string labelText, Color buttonColor)
	{
		GameObject newButton = Instantiate (dynamicButton) as GameObject;
		activeButtons.Add (newButton);
		Vector3 buttonPosition = new Vector3 (newButton.GetComponent<RectTransform> ().sizeDelta.x / 2 + 10, ((newButton.GetComponent<RectTransform> ().sizeDelta.y / -2) * activeButtons.Count) - (13 * activeButtons.Count), 0);
		newButton.transform.SetParent (controlPannel.transform);
		newButton.GetComponent<RectTransform> ().localPosition = buttonPosition;
		newButton.GetComponentInChildren<Text> ().text = labelText;
		newButton.GetComponentInChildren<Image> ().color = buttonColor;
		return newButton.GetComponent<Button> ();
	}
	Button NewButton_Nested (string labelText, Color buttonColor)
	{
		int nested = 1;
		int yMultiply = activeButtons.Count;

		if(activeButtons.Count> 27){
			nested = 4;
			yMultiply -= 28;
		}
		else if(activeButtons.Count>20){
			nested = 3;
			yMultiply -= 21;
		}
		else if(activeButtons.Count> 13){
			nested = 2;
			yMultiply -= 14;
		}
		else if(activeButtons.Count> 5){
			yMultiply -= 7;
			nested = 1;
		}

		GameObject newButton = Instantiate (dynamicButton) as GameObject;
		float initPosX = (newButton.GetComponent<RectTransform> ().sizeDelta.x /2  + 10);
		float xPos = (newButton.GetComponent<RectTransform> ().sizeDelta.x  + 10);
		xPos = initPosX + xPos * nested;
		activeButtons.Add (newButton);
		Vector3 buttonPosition = new Vector3 (xPos, ((newButton.GetComponent<RectTransform> ().sizeDelta.y / -2) * yMultiply) - (13 * yMultiply) - newButton.GetComponent<RectTransform>().sizeDelta.y, 0);
		newButton.transform.SetParent (controlPannel.transform);
		newButton.GetComponent<RectTransform> ().localPosition = buttonPosition;
		newButton.GetComponentInChildren<Text> ().text = labelText;
		newButton.GetComponentInChildren<Image> ().color = buttonColor;
		return newButton.GetComponent<Button> ();
	}
}
