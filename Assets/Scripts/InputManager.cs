using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public enum OnButtonState{
		Default,
		OnUp,
		OnRight,
		OnLeft
	}

	public OnButtonState CurrentButtonState = OnButtonState.Default;


	// Update is called once per frame
	void Update () {
	
	}

	public void OnUpButtonDown()
	{
		CurrentButtonState = OnButtonState.OnUp;
	}

	
	public void OnButtonUp()
	{
		CurrentButtonState = OnButtonState.Default;
	}

	
	public void OnRightButtonDown()
	{
		CurrentButtonState = OnButtonState.OnRight;
	}
	
	public void OnLeftButtonDown()
	{
		CurrentButtonState = OnButtonState.OnLeft;
	}
}
