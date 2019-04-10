using UnityEngine;
using System.Collections;

public class SelectionIndicator : MonoBehaviour {

	MouseManager mm;

	// Use this for initialization
	void Start () {
		mm = GameObject.FindObjectOfType<MouseManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(mm.selectedObject != null) {

            this.transform.position = mm.selectedObject.transform.position;
			
		}
	}
}
 