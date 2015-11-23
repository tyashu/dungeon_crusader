using UnityEngine;
using System.Collections;

public class MiniMapManager : MonoBehaviour {

	public GameObject MiniMapCamera;

	public GameObject PlayerPoint;

	void Awake(){
		MiniMapCamera.SetActive (false);
		PlayerPoint.SetActive (false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowMiniMap(){
		MiniMapCamera.SetActive (true);
		PlayerPoint.SetActive (true);

	}

	public void HideMiniMap(){
		MiniMapCamera.SetActive (false);
		PlayerPoint.SetActive (false);
	}
}
