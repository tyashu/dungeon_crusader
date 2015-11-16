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
			StatusPanel.SetAsLastSibling();
			break;
		case PanelName.Item:
			ItemPanel.SetAsLastSibling();
			break;
		case PanelName.Map:
			MapPanel.SetAsLastSibling();
			break;
		default:
			break;
		}
	}
}
