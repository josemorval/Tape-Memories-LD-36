using UnityEngine;
using System.Collections;

public class TestTape : MonoBehaviour {

	public GameObject leftTape;
	public GameObject rightTape;

	public float maxWidth;
	public float width;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		width = 0.24f/2f*(1f + Mathf.Sin(Time.time));
		leftTape.GetComponent<MeshRenderer>().material.SetFloat("_Width",width);
		rightTape.GetComponent<MeshRenderer>().material.SetFloat("_Width",Mathf.Sqrt(Mathf.Clamp(maxWidth-width*width,0f,maxWidth)));

	}
}
