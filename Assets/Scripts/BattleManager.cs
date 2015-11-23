using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {

	public GameObject Battle;

	void Awake(){
		Battle.SetActive (false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartBattle(){
		Battle.SetActive (true);
	
	}

	public void FinishBattle(){
		Battle.SetActive(false);
	}

}
