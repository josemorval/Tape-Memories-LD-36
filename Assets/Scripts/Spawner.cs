using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {




	// Use this for initialization
	void Start () {
	
		for(int i=0;i<transform.childCount;i++){
			Vector3 v = new Vector3(Random.Range(-4f,4f),Random.Range(-7f,7f),11f+1f*i);
			transform.GetChild(i).localPosition = v;
			float f = Random.Range(4,10)/10f;
			transform.GetChild(i).localScale = new Vector3(f,f,f);
			transform.GetChild(i).Rotate(0f,0f,360f*Random.value);
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
