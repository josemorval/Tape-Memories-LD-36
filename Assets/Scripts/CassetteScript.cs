using UnityEngine;
using System.Collections;

public class CassetteScript : MonoBehaviour {

	public GameObject wheelLeft;
	public GameObject wheelRight;

	public GameObject tapeLeft;
	public GameObject tapeRight;

	float widthTape;
	float rot = 0f;
	float vel = 0f;

	public float thresholdVel = 500f;
	public float upVel = 0.5f;
	public float downVel = 0.5f;
	public float paramTimeTape = 5000f;

	public void SetupCassette(int level){
		widthTape = 0f;
		rot = 0f;
		vel = 0f;
		thresholdVel = 500f-300f*level/18f;
		upVel = 0.5f+2.5f*level/18f;
		downVel = 4f-3.7f*level/18f;

		//paramTimeTape = 3000f + Random.Range(0,4)*3000f;
		paramTimeTape = 5000f;

		wheelLeft.transform.localRotation = Quaternion.identity;
		wheelRight.transform.localRotation = Quaternion.identity;

		UpdateShaderTape();
	}


	public void UpdateValuesCassette(){


		vel+=2000f*Time.deltaTime;

		if(vel>600f){
			vel=600f;
		}


	}

	public void UpdateCassette(){

		RotateCassette();
		UpdateShaderTape();

	}


	void UpdateShaderTape(){
		widthTape = Mathf.Clamp01(widthTape);
		float f = 0.27f*widthTape;
		tapeLeft.GetComponent<MeshRenderer>().material.SetFloat("_Width",f);
		tapeRight.GetComponent<MeshRenderer>().material.SetFloat("_Width",Mathf.Sqrt(Mathf.Clamp(0.0729f-f*f,0f,0.27f)));
	}

	void RotateCassette(){

		if(vel>thresholdVel){
			transform.localPosition += upVel*new Vector3(0f,1f,0f) * Time.deltaTime;
		}else{
			transform.localPosition -= downVel*new Vector3(0f,1f,0f) * Time.deltaTime;
		}

		if(transform.localPosition.y<4.5f){
			transform.localPosition = new Vector3(0f,4.5f,0f);
		}

		widthTape+=vel*Time.deltaTime/paramTimeTape;
		wheelRight.transform.Rotate(new Vector3(0f,0f,-4f*vel*Time.deltaTime),Space.Self);

		vel-=100f*Time.deltaTime;
		if(vel<0f){
			vel=0f;
		}
			

		transform.GetChild(0).transform.rotation = transform.parent.transform.rotation;
		transform.Rotate(new Vector3(0f,0f,vel*Time.deltaTime),Space.Self);

	}


	public bool CheckEscape(){
		return transform.localPosition.y>6.5f;
	}
	public bool CheckEndRolling(){
		return widthTape>=1f;
	}
}
