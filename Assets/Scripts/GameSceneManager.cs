using UnityEngine;
using System.Collections;

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

	void Awake () {

		Application.targetFrameRate = 20;
	}

	// Use this for initialization
	void Start () {
		SoundManager.Instance.PlayBGM("bgm1");
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

}
