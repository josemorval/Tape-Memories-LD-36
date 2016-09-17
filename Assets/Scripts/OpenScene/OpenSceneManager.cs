using UnityEngine;
using System.Collections;

public class OpenSceneManager : MonoBehaviour {

	public Transform wheelleft;
	public Transform wheelright;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		wheelleft.Rotate(new Vector3(0,0,100f*Time.deltaTime),Space.Self);
		wheelright.Rotate(new Vector3(0,0,100f*Time.deltaTime),Space.Self);
	}
}
