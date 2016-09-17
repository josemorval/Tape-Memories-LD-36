using UnityEngine;
using System.Collections;

public class PenHideScript : MonoBehaviour {

	public Transform cassette;
	public float orientation;
	
	void Update () {

		GetComponent<MeshRenderer>().material.SetFloat("_Orientation",orientation);
		GetComponent<MeshRenderer>().material.SetVector("_PosCassette",cassette.position);

	}
}
