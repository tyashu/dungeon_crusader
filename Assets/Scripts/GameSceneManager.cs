using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum MoveButtonState{
	Default,
	OnUp,
	OnRight,
	OnLeft
}

public enum PanelName{
	Status,
	Item,
	Map
}


public class GameSceneManager : MonoBehaviour {

	public CameraController CameraController;

	public InputManager InputManager;

	public MapGenerator MapGenerator;

	public PanelSelect PanelSelect;
	
	public BattleManager BattleManager;

	public MiniMapManager MiniMapManager;

	public MapData MapData;

	public int CurrentPosVal;

	public Text FloorText;

	void Awake () {
		Application.targetFrameRate = 20;
		MapData = new MapData ();
		MapGenerator.CreateMap ();
		InputManager.HideMoveButton ();

		SetFloorText ();
	}

	// Use this for initialization
	void Start () {
		SoundManager.Instance.PlayBGM("bgm1");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SetFloorText(){
		string floor;
		if (MapData.CurrentFloor == 0) {
			floor = "B1";
		} else {
			floor = MapData.CurrentFloor.ToString();
		}

		FloorText.text = floor + "F";
	}

	// move button status

	public MoveButtonState GetMoveButtonState(){
		return InputManager.CurrentButtonState;
	}

	
	public void SetMoveButtonState(MoveButtonState state){
		InputManager.CurrentButtonState = state;
	}

	// panel status
	
	public PanelName GetPanleStatus(){
		return PanelSelect.CurrentPanel;
	}

	public void SelectPanelStatus(PanelName name){
		PanelSelect.SelectPanel (name);
	}

	public void OnMoveEnd(){
		CurrentPosVal = MapData.GetValOfPos (CameraController.currentX, CameraController.currentZ);

		if (CurrentPosVal >= 30 && CurrentPosVal < 40) {
			
			CameraController.AutoMoveFlg = false;
			InputManager.ShowMoveButton ();
			int floor = MapData.FindFloorWithJumpNum(CurrentPosVal);
			MapData.CurrentFloor = floor;
			MapGenerator.ReCreateMap(CurrentPosVal);
			SetFloorText ();

		}else if (CurrentPosVal == 2) {
			CameraController.AutoMoveFlg = false; 
			InputManager.ShowMoveButton ();
		} else if (!CameraController.CanMoveToNextPos ()) {
			CameraController.AutoMoveFlg = false;
			InputManager.ShowMoveButton ();
		}else if(CameraController.CanMoveToRightPos() || CameraController.CanMoveToLeftPos()){
			CameraController.AutoMoveFlg = false;
			InputManager.ShowMoveButton ();

		}else{
			CameraController.AutoMoveFlg = true;
			InputManager.HideMoveButton();
		}
	}
}
