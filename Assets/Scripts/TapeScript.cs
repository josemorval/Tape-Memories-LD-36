using UnityEngine;
using System.Collections;

public class TapeScript : MonoBehaviour {

	float vel;
	float timeTapping = 0f;
	Vector3 newLocalHeight;



	public void UpdateVel(){

		vel+=30f;

		if(vel>600f){
			vel=600f;
		}
			

			
			
	}

	void Update(){

		if(vel>500f){
			transform.localPosition += 0.5f*new Vector3(0f,1f,0f) * Time.deltaTime;
		}else{
			transform.localPosition -= 0.2f*new Vector3(0f,1f,0f) * Time.deltaTime;
		}

		if(transform.localPosition.y<2f){
			transform.localPosition = new Vector3(0f,2f,0f);
		}



		vel-=1.5f;
		if(vel<0f){
			vel=0f;
		}


		transform.GetChild(0).transform.rotation = Quaternion.identity;
		transform.GetChild(0).transform.Rotate(new Vector3(0f,0f,0f),Space.Self);

		transform.Rotate(new Vector3(0f,0f,vel*Time.deltaTime),Space.Self);
	}

}
