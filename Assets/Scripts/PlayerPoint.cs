using UnityEngine;
using System.Collections;

public class PlayerPoint : MonoBehaviour {

	public Transform PlayerCamera;
	public Transform MapCamera;
	public Transform Transform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float x = PlayerCamera.position.x;
		float z = PlayerCamera.position.z;
		Transform.position = new Vector3(x, Transform.position.y, z);
		MapCamera.position = new Vector3(x, MapCamera.position.y, z);
	}
}
