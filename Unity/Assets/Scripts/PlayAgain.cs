using UnityEngine;
using System.Collections;

public class PlayAgain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Clicked() {
		Application.LoadLevel(Application.loadedLevel);
	}
}
