using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {
	
	public GameObject DefaultWallPrefab;
	public GameObject WallPrefab;

	public Transform WallParent;

	private int default_x_max = 19;
	private int default_z_max = 19;


	// Use this for initialization
	void Awake () {
		//これがマップの元になるデータ
		string map_matrix  = "022520000000000000:022220000000000000:022250000000000000:025251111111111110:022220000000000010:000100000000000010:000110000000000010:000010022500000010:000010032200000010:000010052200000010:000011152200000010:000000022200000010:000000000000000010:000000000000002522:000000000000015222:000000000000002222:000000000000000000:000000000000000000";


		//枠を作るメソッド呼び出し
		CreateMapWaku();

		// 引数にこれを入れてマップ生成する
		CreateMap(map_matrix);
	}

	
	private void createWall(GameObject prefab, Vector3 position){
		GameObject obj = Instantiate(prefab, position, Quaternion.identity) as GameObject;
		obj.transform.SetParent (WallParent);
	}

	
	//枠を作るメソッド
	void CreateMapWaku()
	{
		//ループしながらz軸の上と下２列に枠を作ります
		for(int dx = 0; dx <= default_x_max; dx++){
			createWall(DefaultWallPrefab, new Vector3(dx, 1.5f, 0));
			createWall(DefaultWallPrefab, new Vector3(dx, 1.5f, default_z_max));
		}

		//同じくx軸に右と左に枠を作ります
		for(int dz = 0; dz <= default_z_max; dz++){
			createWall(DefaultWallPrefab, new Vector3(0, 1.5f, dz));
			createWall(DefaultWallPrefab, new Vector3(default_x_max, 1.5f, dz));
		}
		
	}

	//マップを作るメソッド
	void CreateMap(string map_matrix)
	{
		//「:」をデリミタとして、mapp_matrix_arrに配列として分割していれます
		string[] map_matrix_arr = map_matrix.Split(':');

		//map_matrix_arrの配列の数を最大値としてループ
		for(int x = 0; x < map_matrix_arr.Length; x++){
			//xを元に配列の要素を取り出す
			string x_map = map_matrix_arr[x];
			//１配列に格納されている文字の数でx軸をループ
			for(int z = 0; z < x_map.Length; z++){
				//配列から取り出した１要素には022520000000000000こんな値が入っているのでこれを１文字づつ取り出す
				int obj = int.Parse(x_map.Substring(z, 1));
				
				//もしも０だったら壁ということで壁のプレハブをインスタンス化してループして出したx座標z座標を指定して設置
				if(obj == 0){
					createWall(WallPrefab, new Vector3(x + 1, 1.5f, z  + 1));
				}
			}
		}
	}
}