using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapData {

	private Dictionary<int, List<List<int>>> mapObjectDictionary = null;
	public Dictionary<int, List<List<int>>> MapObjectDictionary{
		get{
			if(mapObjectDictionary == null){
				SetMapData();
			}
			return mapObjectDictionary;
		}
		set{
			mapObjectDictionary = value;
		}
	}
	
	public int CurrentFloor = 1;

	public MapData(){
		SetMapData ();
	}

	public void SetMapData()
	{
		mapObjectDictionary = new Dictionary<int, List<List<int>>> ();

		TextAsset[] mapDataFiles = Resources.LoadAll<TextAsset> ("CSV/MapData") ;

		for (int floor=0; floor<mapDataFiles.Length; floor++) {
			TextAsset csv = Array.Find<TextAsset>(mapDataFiles, (txt) => txt.name.Equals("map_f" + floor));
			StringReader reader = new StringReader(csv.text);
			
			List<List<int>> mapObjectList = new List<List<int>> ();
			
			while (reader.Peek() > -1) {
				List<int> cols = new List<int>();
				
				string line = reader.ReadLine();
				string[] values = line.Split(',');
				for(int j=0; j<values.Length; j++){
					cols.Add(int.Parse(values[j]));
				}
				mapObjectList.Add(cols);
			}

			mapObjectDictionary.Add(floor, mapObjectList);
		}
	}

	public List<List<int>> GetMapObjectList(int floor){
		return MapObjectDictionary [floor];
	}

	public int GetValOfPos(int x, int z)
	{
		return mapObjectDictionary[CurrentFloor] [-z] [x];
	}

	public int FindFloorWithJumpNum(int num){
		int result = -100;
		if (CurrentFloor + 1 < mapObjectDictionary.Count) {
			List<List<int>> nextFloor = mapObjectDictionary [CurrentFloor + 1];
			for(int z=0; z<nextFloor.Count; z++){
				for(int x=0;x<nextFloor[z].Count;x++){
					if(nextFloor[z][x] == num){
						return CurrentFloor +1;
					}
				}
			}
		}
		if (CurrentFloor - 1 >= 0) {
			List<List<int>> prevFloor = mapObjectDictionary [CurrentFloor - 1];
			for(int z=0; z<prevFloor.Count; z++){
				for(int x=0;x<prevFloor[z].Count;x++){
					if(prevFloor[z][x] == num){
						return CurrentFloor -1;
					}
				}
			}
		}
		for(int i=0; i<mapObjectDictionary.Count; i++){
			List<List<int>> floor = mapObjectDictionary[i];
			for(int z=0; z<floor.Count; z++){
				for(int x=0;x<floor[z].Count;x++){
					if(floor[z][x] == num){
						return i;
					}
				}
			}
		}
		return result;
	}
}
