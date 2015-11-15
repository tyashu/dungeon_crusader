using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	internal enum MoveDirection
	{
		NoMove,
		XPlus,
		XMinus,
		ZPlus,
		ZMinus
	}
	
	
	public float walkSpeed = 100.0f; //歩く速度
	public float gravity = 10.0f;//重力加速度
	public float rotateSpeed=10f;
	
	public float MoveUnit = 4f;
	public Vector3 velocity;//現在の速度

	
	public Transform TargetCamera;
	public InputManager inputManager;

	public CharacterController cController;


	public int goalRotateY= 0;
	public int currentRotateY = 0;
	public bool rotateYPlus = false;
	public bool inRotate = false;

	MoveDirection moveDirection = MoveDirection.NoMove;
	private Vector3 goalPos;
	public float currentX = 0;
	public float currentZ = 0;
	public bool inMove = false;
	public float moveCount = 0;
	public float inMoveCount = 0;
	
	void Awake (){
	}
	
	void FixedUpdate () {
		if (rotate()) {
			return;
		}

		if (move ()) {
			return;
		}

		JudgeInput ();
	}

	private bool rotate()
	{
		if (inRotate) {
			if (currentRotateY != goalRotateY) {
				if (rotateYPlus) {
					currentRotateY += 10;
					if (currentRotateY >= 360) {
						currentRotateY = currentRotateY - 360;
					}
				} else {
					currentRotateY -= 10;
					if (currentRotateY < 0) {
						currentRotateY = 360 + currentRotateY;
					}
				}
				TargetCamera.eulerAngles = new Vector3 (0, (float)currentRotateY, 0);
				if (currentRotateY == goalRotateY) {
					inRotate = false;
				}
			} else {
				
				inRotate = false;
			}
			
			return true;
		}
		return false;
	}

	private bool move(){
		if (inMove) {
			float SpeedY = -gravity, SpeedZ = 0f;
			velocity = Vector3.zero;

			bool goal = false;

			if(isOverMove()){
				goal = true;
			}
			if(currentX == TargetCamera.position.x && currentZ == TargetCamera.position.z){
				moveCount += Time.deltaTime;
				if(moveCount > 0.5f){
					goal = true;
					moveCount = 0;
				}
			}else{
				moveCount = 0;
			}

			if(goal){
				moveGoal();
			}else{
				SpeedZ = 1f;
				velocity.y += SpeedY;
				velocity += TargetCamera.forward * SpeedZ;

			}
			
			//キャラクターコントローラーの移動
			cController.Move(velocity * walkSpeed * Time.deltaTime);
			
			currentX = TargetCamera.position.x;
			currentZ = TargetCamera.position.z;

			return true;
		}
		return false;
	}

	private bool isOverMove(){
		bool over = false;
		switch(moveDirection)
		{
		case MoveDirection.XMinus:
			if( TargetCamera.position.x <= goalPos.x){
				over = true; 
			}
			break;
		case MoveDirection.XPlus:
			if(TargetCamera.position.x >= goalPos.x){
				over = true;
			}
			break;
		case MoveDirection.ZMinus:
			if(TargetCamera.position.z <= goalPos.z){
				over = true;
			}
			break; 
		case MoveDirection.ZPlus:
			if(TargetCamera.position.z >= goalPos.z){
				over = true;
			}
			break;
		default:
			break;
		}
		return over;
	}

	private void moveGoal(){
		inMove = false;
		float fixX;
		float fixZ;
		if((int) TargetCamera.position.x % 4 <= 2){
			fixX = TargetCamera.position.x - (int) TargetCamera.position.x;
		}else{
			fixX =  (TargetCamera.position.x - (int) TargetCamera.position.x) - 1f;
		}
		if((int) TargetCamera.position.z % 4 <= 2){
			fixZ = TargetCamera.position.z - (int) TargetCamera.position.z;
		}else{
			fixZ =  (TargetCamera.position.z - (int) TargetCamera.position.z) - 1f;
		}
		TargetCamera.position += new Vector3(-fixX, 0, -fixZ);
		inputManager.CurrentButtonState = InputManager.OnButtonState.Default;
	}


	private void JudgeInput(){
		/////キー入力確認 各キーが押されているか
		if (Input.GetKey(KeyCode.RightArrow) || inputManager.CurrentButtonState == InputManager.OnButtonState.OnRight){
			goalRotateY = goalRotateY + 90;
			if (goalRotateY >= 360) {
				goalRotateY = goalRotateY - 360;
			}
			inRotate = true;
			rotateYPlus = true;
		}
		else if (Input.GetKey(KeyCode.LeftArrow)|| inputManager.CurrentButtonState == InputManager.OnButtonState.OnLeft){
			goalRotateY = goalRotateY - 90;
			if (goalRotateY < 0) {
				goalRotateY = 360 + goalRotateY;
			}
			
			inRotate = true;
			rotateYPlus = false;
		}
		else if (Input.GetKey(KeyCode.UpArrow)|| inputManager.CurrentButtonState == InputManager.OnButtonState.OnUp){
			inMove = true; 
			currentX = TargetCamera.position.x;
			currentZ = TargetCamera.position.z;
			goalPos = TargetCamera.position + TargetCamera.forward * MoveUnit;
			if(currentX < goalPos.x){
				moveDirection = MoveDirection.XPlus;
			}else if(currentX > goalPos.x){
				moveDirection = MoveDirection.XMinus;
			}else if(currentZ < goalPos.z){
				moveDirection = MoveDirection.ZPlus;
			}else if(currentZ > goalPos.z){
				moveDirection = MoveDirection.ZMinus;
			}else{
				moveDirection = MoveDirection.NoMove;
			}
			moveCount = 0;
			inMoveCount = MoveUnit;

		}
	}
}
