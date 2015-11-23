using UnityEngine;
using System.Collections;

public class PanelSelect : MonoBehaviour {

	
	public GameSceneManager GameSceneManager;

	public PanelName CurrentPanel = PanelName.Status;

	public Transform StatusPanel;
	public Transform ItemPanel;
	public Transform MapPanel;

	public void SelectPanel(PanelName name){
		CurrentPanel = name;

		switch (CurrentPanel) {
		case PanelName.Status:
			SelectStatusPanel();
			break;
		case PanelName.Item:
			SelectItemPanel();
			break;
		case PanelName.Map:
			SelectMapPanel();
			break;
		default:
			break;
		}
	}

	private void SelectStatusPanel(){
		StatusPanel.SetAsLastSibling();
		GameSceneManager.MiniMapManager.HideMiniMap ();
	}

	
	private void SelectItemPanel(){
		ItemPanel.SetAsLastSibling();
		GameSceneManager.MiniMapManager.HideMiniMap ();
	}
	
	private void SelectMapPanel(){
		MapPanel.SetAsLastSibling();
		GameSceneManager.MiniMapManager.ShowMiniMap ();
	}
}
