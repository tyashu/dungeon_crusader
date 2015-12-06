using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {
	
	public GameSceneManager GameSceneManager;

	public GameObject MapPrefab;

	public Transform MapParent;
	public GameObject DefaultWallPrefab;
	public GameObject WallPrefab;

	public Transform WallParent;

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
		obj.transform.SetParent (WallParent);
		obj.layer = LayerMask.NameToLayer ("Wall");
	}

	//マップを作るメソッド
	public void CreateMap()
	{
		GameObject map = Instantiate (MapPrefab);
		map.name = "Map";
		map.transform.SetParent (MapParent);
		WallParent = map.transform.GetChild (0);

		MapData mapDataManager = new MapData();
		List<List<int>> mapData = mapDataManager.GetMapObjectList(GameSceneManager.MapData.CurrentFloor);
		 
		for(int z = 0; z < mapData.Count; z++){
			List<int> rowData = mapData[z];
			for(int x = 0; x < rowData.Count; x++){
				int val = rowData[x];

				switch(val){
				case 0:
					createWall(WallPrefab, new Vector3(x*4, 1.5f, -z*4));
					break;
				case 10:
					if(!moveFlg){
						GameSceneManager.CameraController.SetPosition(x*4,-z*4);
						GameSceneManager.CameraController.currentX = x;
						GameSceneManager.CameraController.currentZ = -z;
					}
					break;
				default:
					break;
				}
				if(val == movePos){
					GameSceneManager.CameraController.SetPosition(x*4,-z*4);
					GameSceneManager.CameraController.currentX = x;
					GameSceneManager.CameraController.currentZ = -z;
					break;
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