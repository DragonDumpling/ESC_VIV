using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightingConcole : MonoBehaviour
{
	private LightProperty[] spotlights;
	private LightProperty_Washer[] washerLights;
	private Dictionary<int,Spot> spots;
	private Dictionary<int,List<int>> teams;
	public LightingColor colorPicker;
	public LightingColorE lightingColor;
	public Dictionary <int, LightingColorE > teamColors;
	public Dictionary <int, List<int>> rows;
	public GameObject controlPannel;

	delegate void RepeateMethod ();

	private RepeateMethod repeateCue;
	private List<object> repeateParamiters;


	// Washer lighting Groups ? Or Ambient lighting Groups
	public enum Grouping
	{
		All,
		Front,
		Back
	}
	;
	public Grouping grouping = Grouping.All;

	// Not sure what the level is for.
	public enum Level
	{
		Off,
		Low,
		Medium,
		High
	}
	;
	public Level level = Level.High;


	// Fan Speed
	public enum Speed
	{
		Off,
		Low,
		Medium,
		High

	}
	public Speed speed = Speed.High;

	// Overhead spot light, pulse durration
	public enum Duration
	{
		Short,
		Low,
		Medium,
		High
	}
	public Duration duration = Duration.High;


	// Strobe Patterns.
	public enum Pattern
	{
		LeftRight,
		RightLeft,
		Clockwise,
		CounterClockwise,
		Random1,
		Random2,
		OneBurst,
		Paparazzi
	}
	public Pattern pattern = Pattern.Random1;

	// Pulse for Washer lights.
	public enum Pulse
	{
		All_Full,
		All_50_Percent,
		All_25_Percent,
		Left_Right,
		Right_Left,
		Center_Out
	}

	public Pulse pulse = Pulse.Center_Out;

	// Avaiable timming for something!
	public enum Timing
	{
		Zero,
		One,
		Three,
		Five,
		Ten
	}

	public Timing timing = Timing.One;



	// Use this for initialization
	void Start ()
	{
		rows = new Dictionary<int, List<int>> ();

		rows.Add (1, new List<int>{1,11,21});
		rows.Add (2, new List<int>{2,12,22});
		rows.Add (3, new List<int>{3,13,23});
		rows.Add (4, new List<int>{4,14,24});
		rows.Add (5, new List<int>{5,15,25});
		rows.Add (6, new List<int>{6,16,26});
		rows.Add (7, new List<int>{7,17,27});
		rows.Add (8, new List<int>{8,18,28});
		rows.Add (9, new List<int>{9,19,29});
		rows.Add (10, new List<int>{10,20,30});

		repeateParamiters = new List<object> ();
		spots = new Dictionary<int, Spot> ();
		spotlights = gameObject.GetComponentsInChildren<LightProperty> ();
		washerLights = gameObject.GetComponentsInChildren<LightProperty_Washer>();
		teamColors = new Dictionary<int, LightingColorE> ();
		SetTeams (1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.A)) {
			TurnSpotLightsOn (0, colorPicker.GetColor (LightingColorE.Red),1f);
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			TurnSpotLightsOn (0, colorPicker.GetColor (LightingColorE.Blue),1f);
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			TurnSpotLightsOff (0);
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			TurnSpotLightsDim (0, colorPicker.GetColor (LightingColorE.Red),1f);
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (repeateCue != null && !controlPannel.activeSelf)
				repeateCue ();
		}

	}
	// Lighting Settings
	// Set up teams
	public void SetTeams (int numberOfTeams)
	{
		if (numberOfTeams < 0 || numberOfTeams > 6) {
			Debug.LogWarning ("Team count requested, out of range. Requested: " + numberOfTeams);
			return;
		}
		Debug.Log ("Team Count set to " + numberOfTeams);
		teams = new Dictionary<int, List<int>> ();
		teamColors = new Dictionary<int, LightingColorE> ();
		switch (numberOfTeams) {
		case(0):
			teams.Add (1, new List<int>{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30});
			teamColors.Add (1, LightingColorE.Blue);
			break;
		case(1):
			teams.Add (1, new List<int>{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30});
			teamColors.Add (1, LightingColorE.Blue);
			break;
		case(2):
			teams.Add (1, new List<int>{1,2,3,4,5,11,12,13,14,15,21,22,23,24,25});
			teams.Add (2, new List<int>{6,7,8,9,10,16,17,18,19,20,26,27,28,29,30});
			teamColors.Add (1, LightingColorE.Blue);
			teamColors.Add (2, LightingColorE.Blue);
			break;
		case(3):
			teams.Add (1, new List<int>{1,2,3,11,12,13,21,22,23,24});
			teams.Add (2, new List<int>{4,5,6,7,14,15,16,17,25,26});
			teams.Add (3, new List<int>{8,9,10,18,19,20,27,28,29,30});
			teamColors.Add (1, LightingColorE.Blue);
			teamColors.Add (2, LightingColorE.Blue);
			teamColors.Add (3, LightingColorE.Blue);
			break;
		case(4):
			teams.Add (1, new List<int>{1,2,11,12,13,21,22,23});
			teams.Add (2, new List<int>{3,4,5,14,15,24,25});
			teams.Add (3, new List<int>{6,7,8,16,17,26,27});
			teams.Add (4, new List<int>{9,10,18,19,20,28,29,30});
			teamColors.Add (1, LightingColorE.Blue);
			teamColors.Add (2, LightingColorE.Blue);
			teamColors.Add (3, LightingColorE.Blue);
			teamColors.Add (4, LightingColorE.Blue);
			break;
		case(5):
			teams.Add (1, new List<int>{1,2,11,12,21,22});
			teams.Add (2, new List<int>{3,4,13,14,23,24});
			teams.Add (3, new List<int>{5,6,15,16,25,26});
			teams.Add (4, new List<int>{7,8,17,18,27,28});
			teams.Add (5, new List<int>{9,10,19,20,29,30});
			teamColors.Add (1, LightingColorE.Blue);
			teamColors.Add (2, LightingColorE.Blue);
			teamColors.Add (3, LightingColorE.Blue);
			teamColors.Add (4, LightingColorE.Blue);
			teamColors.Add (5, LightingColorE.Blue);
			break;
		case(6):
			teams.Add (1, new List<int>{1,2,11,12,21});
			teams.Add (2, new List<int>{3,13,22,23,24});
			teams.Add (3, new List<int>{4,5,14,15,25});
			teams.Add (4, new List<int>{6,7,16,17,26});
			teams.Add (5, new List<int>{8,18,27,28,29});
			teams.Add (6, new List<int>{9,10,19,20,30});
			teamColors.Add (1, LightingColorE.Blue);
			teamColors.Add (2, LightingColorE.Blue);
			teamColors.Add (3, LightingColorE.Blue);
			teamColors.Add (4, LightingColorE.Blue);
			teamColors.Add (5, LightingColorE.Blue);
			teamColors.Add (6, LightingColorE.Blue);
			break;
		}
		ApplyColorToSpotLights ();
	}

	// Highlight Player for a some time. 
	public void HighlightPlayer (int spotID, Duration dur)
	{
		float highlightDurration = 1f;
		switch (dur) {
		case(Duration.High):
			highlightDurration = 3f;
			break;
		case(Duration.Medium):
			highlightDurration = 2f;
			break;
		case(Duration.Low):
			highlightDurration = 1f;
			break;
		case(Duration.Short):
			highlightDurration = 0.5f;
			break;

		}
		foreach (LightProperty lp in spotlights) {
			if (lp.spotID == spotID) {
				lp.HighLight (highlightDurration);
			}
		}
		repeateParamiters.Clear ();
		repeateParamiters.Add ((object)spotID);
		repeateParamiters.Add ((object)dur);
		repeateCue = RepeatHighlightPlayer;
	}

	public void RepeatHighlightPlayer ()
	{
		HighlightPlayer ((int)repeateParamiters [0], (Duration)repeateParamiters [1]);
	}

	// Set Default lighting color for a team. 
	public void SetLightingForTeam (int teamID, LightingColorE lightCol)
	{
		Debug.Log ("teamId = " + teamID);
		Debug.Log ("LgithCol = " + lightCol.ToString ());
		Debug.Log ("teamcolors length " + teamColors.Count);
		if (teamColors == null) {

			Debug.LogWarning ("Team ID out of range");
			return;
		}
		if (teamColors.Count < teamID) {
			Debug.LogWarning ("Team ID out of range");
			return;
		}
		teamColors [teamID] = lightCol;
		ApplyColorToSpotLights ();
	}
	

	// Temporarily highlight a team over a duration. Uses team colors
	public void HighlightTeam (int teamID, Duration dur)
	{

	}

	// Specify lighting accent color and duration
	public void SetLightingWash (LightingColorE lColor, Timing timing)
	{

		float dur = 1f;
		switch(timing){
		case(Timing.Ten):
			dur = 10;
			break;
		case(Timing.Five):
			dur = 5;
			break;
		case(Timing.Three):
			dur = 3;
			break;
		case(Timing.One):
			dur = 1;
			break;
		case(Timing.Zero):
			dur = 0.2f;
			break;

		}
		foreach(LightProperty_Washer lp in washerLights){
			lp.setColor = colorPicker.GetColor(lColor);
			lp.onDur = dur;
			lp.LightUp();
		}
	}

	// Specify lighting wash pulse
	public void SetLightingPulse (Pulse pl)
	{
		Dictionary<int,float> pulseMap = new Dictionary<int, float>();
		switch(pl){
		case(Pulse.All_Full):
			for(int i = 1; i < 31; i ++){
				int key = i;
				pulseMap.Add(key,0f);
			}
			break;
		case(Pulse.Center_Out):
			int center = washerLights.Length/2;
			float waitTime = 0.05f;
			for(int i = center; i < washerLights.Length + 1 ; i ++){
				int key = i;
				float newWaitTime = (key - center) * waitTime;
				pulseMap.Add(key,newWaitTime);
			}
			for(int i = center-1; i > 0 ; i --){
				int key = i;
				float newWaitTime = (center - key) * waitTime;
				pulseMap.Add(key,newWaitTime);
			}
			break;
		case(Pulse.Left_Right):
			float delay = 0.05f;
			for(int i = 1; i < washerLights.Length+1; i ++){
				int key = i;
				float newDelay = key * delay;
				pulseMap.Add(key, newDelay);
			}
			break;
		case(Pulse.Right_Left):
			float delay2 = 0.05f;
			for(int i = 1; i < washerLights.Length+1; i ++){
				int key = i;
				float newDelay = (washerLights.Length - key) * delay2;
				pulseMap.Add(key, newDelay);
			}
			break;

			// KEVIN! Add rest of the pulse options here.

		}
		foreach(LightProperty_Washer lp in washerLights){
			if(pulseMap.ContainsKey(lp.spotID)){
				lp.waitDur = pulseMap[lp.spotID];
				lp.LightUp();
			}
		}
	}

	// Set Ambient Color
	public void SetAmbientLighting (LightingColorE color, Timing tim)
	{

	}

	// Set Fans
	public void SetFans (Grouping group, Pattern patternValue)
	{

	}


	

	// Lighting Cues
	public void Cue_WalkIn ()
	{

	}

	public void Cue_GameLaunch ()
	{

	}

	public void Cue_FindLocation ()
	{
		StopAllCoroutines();
		if (teams == null) {
			Debug.LogWarning ("teams not set");
			return;
		}
		foreach (KeyValuePair<int, List<int>> team in teams) {
			foreach (int spotID in team.Value) {
				TurnSpotLightsOn (team.Key, colorPicker.GetColor (teamColors [team.Key]),0.5f);
			}
		}
		foreach(LightProperty lp in spotlights){
			lp.StartCoroutine("Focus");
		}
	}

	public void Cue_Identify_Teams ()
	{

	}

	public void Cue_Training_Mode ()
	{
		StopAllCoroutines();
		if (teams == null) {
			Debug.LogWarning ("teams not set");
			return;
		}
		foreach (KeyValuePair<int, List<int>> team in teams) {
			foreach (int spotID in team.Value) {
				TurnSpotLightsDim (team.Key, colorPicker.GetColor (teamColors [team.Key]),0.5f);
			}
		}
	}

	public void Cue_GameStart ()
	{

	}

	public void Cue_TentionSlow ()
	{
		StopAllCoroutines();
		StartCoroutine("TentionCoroutine",1f);
	}

	public void Cue_TentionMedium ()
	{
		StopAllCoroutines();
		StartCoroutine("TentionCoroutine",0.5f);
	}

	public void Cue_TentionFast ()
	{
		StopAllCoroutines();
		StartCoroutine("TentionCoroutine",0.2f);
	}

	public void Cue_TentionNone ()
	{


	}

	IEnumerator TentionCoroutine (float delay)
	{
		int itirationCount = 1;
		while (true) {
			foreach (LightProperty lp in spotlights) {
				if (rows [itirationCount].Contains (lp.spotID)) {
					lp.StartCoroutine("Diffuse");
					lp.SetLight (LightState.on, colorPicker.GetColor (teamColors [GetTeamID (lp.spotID)]),3f/delay);
				} else {
					lp.SetLight (LightState.off, Color.blue,5);
				}
			}
			itirationCount++;
			if (itirationCount > 10)
				itirationCount = 1;
			yield return new WaitForSeconds (delay);
		}
	}

	public void Cue_EndRound ()
	{

	}

	public void Cue_Intermission ()
	{

	}

	public void Cue_AnnounceLeadingTeam ()
	{

	}

	public void Cue_IdentifyLeadingPlayer ()
	{

	}

	public void Cue_Game_Finale ()
	{

	}

	// Turn Lights On
	public void ApplyColorToSpotLights ()
	{
		foreach (LightProperty spotLight in spotlights) {
			int spotID = 1;
			foreach (KeyValuePair<int,List<int>> kvp in teams) {
				if (kvp.Value.Contains (spotLight.spotID))
					spotID = kvp.Key;
			}
			spotLight.SetLight (LightState.colorOnly, colorPicker.GetColor (teamColors [spotID]),1f);

		}
	}

	// Turn Lights On
	void TurnSpotLightsOn (int teamID, Color incColor, float incSpeed)
	{
		foreach (LightProperty spotLight in spotlights) {
			if (teams [teamID].Contains (spotLight.spotID)) {
				spotLight.SetLight (LightState.on, incColor, incSpeed);
			}
		}
	}

	// Turn Lights Dim
	void TurnSpotLightsDim (int teamID, Color incColor, float incSpeed)
	{
		foreach (LightProperty spotLight in spotlights) {
			if (teams [teamID].Contains (spotLight.spotID)) {
				spotLight.SetLight (LightState.dim, incColor, incSpeed);
			}
		}
	}

	// Turn Lights Off
	void TurnSpotLightsOff (int teamID)
	{
		foreach (LightProperty spotLight in spotlights) {
			if (teams [teamID].Contains (spotLight.spotID)) {
				spotLight.SetLight (LightState.off, Color.blue, 1f);
			}
		}
	}

	int GetTeamID (int spotID)
	{

		foreach (KeyValuePair<int,List<int>> team in teams) {
			if (team.Value.Contains (spotID)) {						   
				return team.Key;
			}
		}
		return 1;				
	}

}
