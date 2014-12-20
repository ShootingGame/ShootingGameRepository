using UnityEngine;
using System.Collections;

public class EndButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EndOnClick(){
		Application.LoadLevel("01StartMenu");
	}

}
