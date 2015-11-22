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
	
	public GameSceneManager GameSceneManager;
	
	public float walkSpeed = 100.0f; //歩く速度
	public float gravity = 10.0f;//重力加速度
	public float rotateSpeed=10f;
	
	public float MoveUnit = 4f;
	public Vector3 velocity;//現在の速度

	
	public Transform TargetCamera;

	public CharacterController cController;


	public int goalRotateY= 0;
	public int currentRotateY = 0;
	public bool rotateYPlus = false;
	public bool inRotate = false;

	MoveDirection moveDirection = MoveDirection.NoMove;
	private Vector3 goalPos;
	public float currentX = 0;
	public float currentZ = 0;
	public bool wallFlg = false;
	public bool inMove = false;
	private bool moveStart = false;
	public float moveCount = 0;
	public float inMoveCount = 0;

	public float DefaultMoveWaitCount = 0.5f;
	public float MoveWaitCount ;
	
	void Awake (){
		MoveWaitCount = DefaultMoveWaitCount;
		wallFlg = false;
	}
	
	void Update () {
		if (rotate()) {
			MoveWaitCount = DefaultMoveWaitCount; 
			return;
		}

		if (move ()) {
			MoveWaitCount = DefaultMoveWaitCount;
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

			if (isOverMove ()) {
				goal = true;
			}else if((goalPos - TargetCamera.position).magnitude < 0.2f){
				goal = true;
			}
 
			if (moveCount > 0.05f) {
				goal = true;
				wallFlg = true;
				moveCount = 0;
			}

			if (goal) {
				moveGoal ();
			} else {
				SpeedZ = 1f;
				velocity.y += SpeedY;
				velocity += TargetCamera.forward * SpeedZ;

			}
			
			//キャラクターコントローラーの移動
			cController.Move (velocity * walkSpeed * Time.deltaTime);

			if (currentX == TargetCamera.position.x && currentZ == TargetCamera.position.z) {
				moveCount += Time.deltaTime;
			} else {
				if(moveStart){
					SoundManager.Instance.PlaySE("se1");
					moveStart = false;
				}
				moveCount = 0;
			}


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
		GameSceneManager.SetMoveButtonState (MoveButtonState.Default);

	}


	private void JudgeInput(){
		/////キー入力確認 各キーが押されているか
		if (Input.GetKey (KeyCode.RightArrow) || GameSceneManager.GetMoveButtonState () == MoveButtonState.OnRight) {
			rotateRight();
		} else if (Input.GetKey (KeyCode.LeftArrow) || GameSceneManager.GetMoveButtonState () == MoveButtonState.OnLeft) {
			rotateLeft();
		} else if (Input.GetKey (KeyCode.UpArrow) || GameSceneManager.GetMoveButtonState () == MoveButtonState.OnUp) {
			moveUp();

		} else {
			if(!wallFlg){
				MoveWaitCount -= Time.deltaTime;
				if(MoveWaitCount < 0){
					MoveWaitCount = DefaultMoveWaitCount;
					moveUp();
				}
			}
		}
	}

	private void rotateRight(){
		goalRotateY = goalRotateY + 90;
		if (goalRotateY >= 360) {
			goalRotateY = goalRotateY - 360;
		}
		inRotate = true;
		wallFlg = false;
		rotateYPlus = true;
	}

	private void rotateLeft(){
		goalRotateY = goalRotateY - 90;
		if (goalRotateY < 0) {
			goalRotateY = 360 + goalRotateY;
		}
		
		inRotate = true;
		wallFlg = false;
		rotateYPlus = false;
	}

	private void moveUp(){
		if (wallFlg) {
			return;
		}
		inMove = true; 
		currentX = TargetCamera.position.x;
		currentZ = TargetCamera.position.z;
		goalPos = TargetCamera.position + TargetCamera.forward * MoveUnit;
		if (currentX < goalPos.x) {
			moveDirection = MoveDirection.XPlus;
		} else if (currentX > goalPos.x) {
			moveDirection = MoveDirection.XMinus;
		} else if (currentZ < goalPos.z) {
			moveDirection = MoveDirection.ZPlus;
		} else if (currentZ > goalPos.z) {
			moveDirection = MoveDirection.ZMinus;
		} else {
			moveDirection = MoveDirection.NoMove;
		}
		moveCount = 0;
		moveStart = true;
		inMoveCount = MoveUnit;

	}
}
