using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour {

	[SerializeField] private GameObject gf;
	public int abc;

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.GetComponent<DotMarker>() != null) {
			Destroy(other.gameObject);
			GameFunctions.AddToScore();
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
