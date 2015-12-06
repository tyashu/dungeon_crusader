using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public enum MoveDirection
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
	public MoveDirection CurrentDirection = MoveDirection.NoMove;

	MoveDirection moveDirection = MoveDirection.NoMove;
	public Vector3 goalPos;
	public int currentX = 0;
	public int currentZ = 0;
	public bool inMove = false;
	public bool AutoMoveFlg = true;

	public float DefaultMoveWaitCount = 1f;
	public float MoveWaitCount ;
	
	void Awake (){
		MoveWaitCount = DefaultMoveWaitCount;
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

	public void SetPosition(float x, float z)
	{
		TargetCamera.position = new Vector3 (x, TargetCamera.position.y, z);
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

			if (goal) {
				moveGoal ();
			} else {
				SpeedZ = 1f;
				velocity.y += SpeedY;
				velocity += TargetCamera.forward * SpeedZ;

			}
			
			//キャラクターコントローラーの移動
			cController.Move (velocity * walkSpeed * Time.deltaTime);

			return true;
		} 
		cController.Move (velocity*0);
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
		TargetCamera.position = new Vector3 (currentX*4, TargetCamera.position.y, currentZ*4);
		GameSceneManager.SetMoveButtonState (MoveButtonState.Default);
		GameSceneManager.OnMoveEnd ();
	}


	private void JudgeInput(){
		/////キー入力確認 各キーが押されているか
		if (Input.GetKey (KeyCode.RightArrow) || GameSceneManager.GetMoveButtonState () == MoveButtonState.OnRight) {
			rotateRight();
		} else if (Input.GetKey (KeyCode.LeftArrow) || GameSceneManager.GetMoveButtonState () == MoveButtonState.OnLeft) {
			rotateLeft();
		} else if (Input.GetKey (KeyCode.UpArrow) || GameSceneManager.GetMoveButtonState () == MoveButtonState.OnUp) {
			AutoMoveFlg = true;
			GameSceneManager.InputManager.HideMoveButton();
			moveUp();

		} else {
			if(AutoMoveFlg){
				MoveWaitCount -= Time.deltaTime;
				if(MoveWaitCount < 0){
					MoveWaitCount = DefaultMoveWaitCount;
					GameSceneManager.InputManager.HideMoveButton();
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
		rotateYPlus = true;
		SetCurrentForward (false);
	}

	private void rotateLeft(){
		goalRotateY = goalRotateY - 90;
		if (goalRotateY < 0) {
			goalRotateY = 360 + goalRotateY;
		}
		
		inRotate = true;
		rotateYPlus = false;
		SetCurrentForward (true);
	}
	
	private void SetCurrentForward(bool right)
	{
		switch (CurrentDirection) {
		case MoveDirection.XPlus:
			if(right){
				CurrentDirection = MoveDirection.ZPlus;
			}else{
				CurrentDirection = MoveDirection.ZMinus;
			}
			break;
		case MoveDirection.XMinus:
			if(right){
				CurrentDirection = MoveDirection.ZMinus;
			}else{
				CurrentDirection = MoveDirection.ZPlus;
			}
			break;
		case MoveDirection.ZPlus:
			if(right){
				CurrentDirection = MoveDirection.XMinus;
			}else{
				CurrentDirection = MoveDirection.XPlus;
			}
			break;
		case MoveDirection.ZMinus:
			if(right){
				CurrentDirection = MoveDirection.XPlus;
			}else{
				CurrentDirection = MoveDirection.XMinus;
			}
			break;
		default:
			CurrentDirection = MoveDirection.XPlus;
			break;
		}
	}

	private void moveUp(){
		inMove = true; 
		if (CurrentDirection == MoveDirection.XPlus) {
			
			if(!CanMoveToPos(currentX + 1, currentZ)){ 
				GameSceneManager.InputManager.ShowMoveButton();
				return;
			}
			moveDirection = MoveDirection.XPlus;
			currentX++;

		} else if (CurrentDirection == MoveDirection.XMinus) {
			if(!CanMoveToPos(currentX - 1, currentZ)){
				GameSceneManager.InputManager.ShowMoveButton();
				return;
			} 
			currentX--;
			moveDirection = MoveDirection.XMinus;

		} else if (CurrentDirection == MoveDirection.ZPlus) {
			if(!CanMoveToPos(currentX, currentZ + 1)){
				GameSceneManager.InputManager.ShowMoveButton();
				return;
			}
			currentZ++;
			moveDirection = MoveDirection.ZPlus;


		} else if (CurrentDirection == MoveDirection.ZMinus) {
			if(!CanMoveToPos(currentX, currentZ - 1)){
				GameSceneManager.InputManager.ShowMoveButton();
				return;
			}
			currentZ--;
			moveDirection = MoveDirection.ZMinus;

		} else {
			moveDirection = MoveDirection.NoMove;
			return;
		}
		goalPos = new Vector3(currentX*4, TargetCamera.position.y, currentZ*4);
		
		SoundManager.Instance.PlaySE("se1");
	}

	public bool CanMoveToNextPos(){
		if (CurrentDirection == MoveDirection.XPlus) {

			return CanMoveToPos(currentX + 1, currentZ);
			
		} else if (CurrentDirection == MoveDirection.XMinus) {
			return CanMoveToPos(currentX - 1, currentZ);
			
		} else if (CurrentDirection == MoveDirection.ZPlus) {
			return CanMoveToPos(currentX, currentZ + 1);
			
		} else if (CurrentDirection == MoveDirection.ZMinus) {
			return CanMoveToPos(currentX, currentZ - 1);
		} else {
			return false;
		}
	}
	public bool CanMoveToRightPos(){
		if (CurrentDirection == MoveDirection.XPlus) {
			
			return CanMoveToPos(currentX, currentZ + 1);
			
		} else if (CurrentDirection == MoveDirection.XMinus) {
			return CanMoveToPos(currentX, currentZ - 1);
			
		} else if (CurrentDirection == MoveDirection.ZPlus) {
			return CanMoveToPos(currentX + 1, currentZ);
			
		} else if (CurrentDirection == MoveDirection.ZMinus) {
			return CanMoveToPos(currentX - 1, currentZ);
		} else {
			return false;
		}
	}
	public bool CanMoveToLeftPos(){
		if (CurrentDirection == MoveDirection.XPlus) {
			
			return CanMoveToPos(currentX, currentZ - 1);
			
		} else if (CurrentDirection == MoveDirection.XMinus) {
			return CanMoveToPos(currentX, currentZ + 1);
			
		} else if (CurrentDirection == MoveDirection.ZPlus) {
			return CanMoveToPos(currentX - 1, currentZ);
			
		} else if (CurrentDirection == MoveDirection.ZMinus) {
			return CanMoveToPos(currentX + 1, currentZ);
		} else {
			return false;
		}
	}

	public bool CanMoveToPos(int x, int z){
		bool result = true; 
		int posVal = GameSceneManager.MapData.GetValOfPos(x,z);
		switch (posVal) {
		case 0:
			result = false;
			break;
		default:
			break;
		}
		return result;
	}
}
