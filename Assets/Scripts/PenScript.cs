using UnityEngine;
using System.Collections;

public class PenScript : MonoBehaviour {

	float rot = 0f;
	float time = 0f;
	float vel = 0f;

	public void SetupPen(){
		rot = 0f;
		time = 0f;
		vel = 0f;
	}

	public void UpdateValuesPen(){
		
		vel+=1f;

		if(vel>20f){
			vel=20f;
		}


	}

	public void UpdatePen(){
		RotatePen();
	}

	void RotatePen () {

		vel-=0.05f;

		if(vel<0f){
			vel = 0f;
		}

		time += vel*Time.deltaTime;
		rot = 3f*Mathf.Sin(time);

		transform.rotation = Quaternion.identity;
		transform.Rotate(new Vector3(0f,0f,rot));
	}
}
