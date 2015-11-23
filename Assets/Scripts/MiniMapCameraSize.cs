using UnityEngine;
using System.Collections;

public class MiniMapCameraSize : MonoBehaviour {

	public Camera Camera;

	// Use this for initialization
	void Start () {
		float baseAspect = 16f / 9f;
		float currentAspect = Screen.height / Screen.width;
		float fix = baseAspect / currentAspect;

		float x = 0.12f*fix;
		float y = 0.05f;
		float w = 1 - x * 2;
		float h = 0.27f;

		Camera.rect = new Rect(x,y,w,h);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
