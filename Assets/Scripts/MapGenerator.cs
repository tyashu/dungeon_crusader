using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {
	
	public GameSceneManager GameSceneManager;

	public GameObject MapPrefab;

	public Transform MapParent;
	public GameObject DefaultWallPrefab;
	public GameObject WallPrefab;
	public GameObject WarpPrefab;
	public GameObject DoorPrefab;

	private Transform wallParent;
	private Transform objParent;

	private int default_x_max = 1000;
	private int default_z_max = 1000;

	private bool moveFlg = false;
	private int movePos = -100;


	// Use this for initialization
	void Awake () {
	}

	void Start(){
	}
	
	private void createWall(GameObject prefab, Vector3 position){
		GameObject obj = Instantiate(prefab, position, Quaternion.identity) as GameObject;
		obj.transform.SetParent (wallParent);
		obj.layer = LayerMask.NameToLayer ("Wall");
	}

	private void createWarpPoint(GameObject prefab, Vector3 position){
		GameObject obj = Instantiate(prefab, position, prefab.transform.rotation) as GameObject;
		obj.transform.SetParent (objParent);
		obj.layer = LayerMask.NameToLayer ("Wall");
	}

	private void createDoor(GameObject prefab, Vector3 position, bool directionX){
		GameObject obj = Instantiate(prefab, position, prefab.transform.rotation) as GameObject;
		if (directionX) {
			obj.transform.eulerAngles = new Vector3 (0,90,0);
		}
		obj.transform.SetParent (objParent); 
	}

	//マップを作るメソッド
	public void CreateMap()
	{
		GameObject map = Instantiate (MapPrefab);
		map.name = "Map";
		map.transform.SetParent (MapParent);
		wallParent = map.transform.GetChild (0);
		objParent = map.transform.GetChild (1);

		MapData mapDataManager = new MapData();
		List<List<int>> mapData = mapDataManager.GetMapObjectList(GameSceneManager.MapData.CurrentFloor);
		 
		for(int z = 0; z < mapData.Count; z++){
			List<int> rowData = mapData[z];
			for(int x = 0; x < rowData.Count; x++){
				int val = rowData[x];

				if(val == 0){
					createWall(WallPrefab, new Vector3(x*4, 1.5f, -z*4));
				}else if(val == 10){
					if(!moveFlg){
						GameSceneManager.CameraController.SetPosition(x*4,-z*4);
						GameSceneManager.CameraController.currentX = x;
						GameSceneManager.CameraController.currentZ = -z;
					}
				}else if(val >= 30 && val < 40){
					createWarpPoint(WarpPrefab, new Vector3(x*4, -0.5f, -z*4));
				}else if(val == 3){
					bool directionX = true;
					if(rowData[x+1]==0){
						directionX = false;
					}
					createDoor(DoorPrefab, new Vector3(x*4, 1.5f, -z*4), directionX);
				}

				if(val == movePos){
					GameSceneManager.CameraController.SetPosition(x*4,-z*4);
					GameSceneManager.CameraController.currentX = x;
					GameSceneManager.CameraController.currentZ = -z;
				}
			}
		}
		moveFlg = false;
	}
	
	public void ReCreateMap(int MovePos )
	{
		moveFlg = true;
		movePos = MovePos;
		DestroyImmediate(MapParent.FindChild("Map").gameObject);
		CreateMap ();
	}
}