using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float walkSpeed = 7.0f; //歩く速度
	public float gravity = 10.0f;//重力加速度
	public float rotateSpeed=10f;
	public Vector3 velocity;//現在の速度


	public Transform TargetCamera;
	public CharacterController Controller;

	public int goalRotateY= 0;
	public int currentRotateY = 0;
	private bool rotateYPlus = false;
	private bool inRotate = false;

	void Awake (){
	}

	void Update () {

		if (currentRotateY != goalRotateY) {
			if (rotateYPlus) {
				currentRotateY += 2;
				if (currentRotateY > 360) {
					currentRotateY = currentRotateY - 360;
				}
			} else {
				currentRotateY -= 2;
				if (currentRotateY < 0) {
					currentRotateY = 360 - currentRotateY;
				}
			}
			TargetCamera.eulerAngles = new Vector3 (0, (float)currentRotateY, 0);
		} else {
			
			inRotate = false;
		}

		if (inRotate) {
			return;
		}



		float SpeedX = 0f, SpeedY = -gravity, SpeedZ = 0f;
		velocity = Vector3.zero;
		
		/////キー入力確認 各キーが押されているか
		if (Input.GetKey(KeyCode.RightArrow)){
			goalRotateY = goalRotateY + 90;
			if (goalRotateY > 360) {
				goalRotateY = goalRotateY - 360;
			}
			inRotate = true;
			rotateYPlus = true;
			 
		}
		else if (Input.GetKey(KeyCode.LeftArrow)){
			goalRotateY = goalRotateY - 90;
			if (goalRotateY <= 0) {
				goalRotateY = 360 + goalRotateY;
			}
			
			inRotate = true;
			rotateYPlus = false;
		}
		if (Input.GetKey(KeyCode.UpArrow)){
			SpeedZ = 1f;
		}
		else if (Input.GetKey(KeyCode.DownArrow)){
			SpeedZ = -1f;
		}

		velocity.y += SpeedY;
		velocity += TargetCamera.forward * SpeedZ;
		
		
		//キャラクターコントローラーの移動
		Controller.Move(velocity * walkSpeed * Time.deltaTime);
	}

}
