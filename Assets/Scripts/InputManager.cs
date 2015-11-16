using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public GameSceneManager GameSceneManager;


	public MoveButtonState CurrentButtonState = MoveButtonState.Default;


	// Update is called once per frame
	void Update () {
	
	}


	/** Move Buttons */


	public void OnUpButtonDown()
	{
		CurrentButtonState = MoveButtonState.OnUp;
	}

	
	public void OnButtonUp()
	{
		CurrentButtonState = MoveButtonState.Default;
	}

	
	public void OnRightButtonDown()
	{
		CurrentButtonState = MoveButtonState.OnRight;
	}
	
	public void OnLeftButtonDown()
	{
		CurrentButtonState = MoveButtonState.OnLeft;
	}



	/** Panel select */

	public void OnSelectStatusPanel()
	{
		GameSceneManager.SelectPanelStatus (PanelName.Status);
	}
	
	public void OnSelectItemPanel()
	{
		GameSceneManager.SelectPanelStatus (PanelName.Item);
	}

	public void OnSelectMapPanel()
	{
		GameSceneManager.SelectPanelStatus (PanelName.Map);
	}

}
