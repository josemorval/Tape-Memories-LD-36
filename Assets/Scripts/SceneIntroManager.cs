using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneIntroManager : MonoBehaviour {

	public SpriteRenderer whiteSquare;

	public GameObject wheelLeft;
	public GameObject wheelRight;

	public GameObject tapeLeft;
	public GameObject tapeRight; 

	public GameObject rewindObject;
	public AnimationCurve animRewindObject;
	public float timeRewind;
	public float heightAnimRewind;

	public TextMesh[] texts;
	public SpriteRenderer[] sprites;


	// Use this for initialization
	void Start () {
		StartCoroutine(IntroGame());
	}
	
	IEnumerator IntroGame(){

		float widthTape = 0.8f;
		float f = 0.27f*widthTape;
		tapeLeft.GetComponent<MeshRenderer>().material.SetFloat("_Width",f);
		tapeRight.GetComponent<MeshRenderer>().material.SetFloat("_Width",Mathf.Sqrt(Mathf.Clamp(0.0729f-f*f,0f,0.27f)));

		float time = 0f;
		bool tapstart = false;
		Vector3 v = rewindObject.transform.position;

		float timeAnim = 3f;
		float vel = 360f/timeAnim;

		Color c = whiteSquare.color;
		c.a =1f;
		whiteSquare.color = c;

		while(time<timeAnim){

			widthTape-=0.6f*(Time.deltaTime/timeAnim);
			f = 0.27f*widthTape;

			tapeLeft.GetComponent<MeshRenderer>().material.SetFloat("_Width",f);
			tapeRight.GetComponent<MeshRenderer>().material.SetFloat("_Width",Mathf.Sqrt(Mathf.Clamp(0.0729f-f*f,0f,0.27f)));

			wheelRight.transform.Rotate(new Vector3(0f,0f,-vel*Time.deltaTime),Space.Self);
			wheelLeft.transform.Rotate(new Vector3(0f,0f,-vel*Time.deltaTime),Space.Self);

			c = whiteSquare.color;
			c.a = Mathf.Lerp(1f,0f,Mathf.Sqrt(time/timeAnim));
			whiteSquare.color = c;

			time+=Time.deltaTime;
			yield return null;
		}

		wheelRight.transform.localRotation = Quaternion.identity;
		wheelLeft.transform.localRotation = Quaternion.identity;


		c = whiteSquare.color;
		c.a =0f;
		whiteSquare.color = c;

		sprites[5].transform.parent.transform.Rotate(new Vector3(0f,0f,10f),Space.Self);
		sprites[6].transform.parent.transform.Rotate(new Vector3(0f,0f,-15f),Space.Self);

		time = 0f;
		timeAnim = 2f;
		while(time<timeAnim){

			c = texts[0].color;
			c.a = Mathf.Lerp(0f,1f,Mathf.Sqrt(time/timeAnim));
			texts[0].color = c;

			c = sprites[5].color;
			c.a = Mathf.Lerp(0f,1f,Mathf.Sqrt(time/timeAnim));
			sprites[5].color = c;

			c = sprites[6].color;
			c.a = Mathf.Lerp(0f,1f,Mathf.Sqrt(time/timeAnim));
			sprites[6].color = c;

			time+=Time.deltaTime;
			yield return null;
		}


		c = texts[0].color;
		c.a = 1f;
		texts[0].color = c;

		c = sprites[5].color;
		c.a = 1f;
		sprites[5].color = c;

		c = sprites[6].color;
		c.a = 1f;
		sprites[6].color = c;

		time = 0f;
		timeAnim = 1f;
		while(time<timeAnim){

			for(int i=0;i<5;i++){
				c = sprites[i].color;
				c.a = Mathf.Lerp(0f,0.5f,Mathf.Sqrt(time/timeAnim));
				sprites[i].color = c;
			}

			time+=Time.deltaTime;
			yield return null;
		}

		for(int i=0;i<5;i++){
			c = sprites[i].color;
			c.a = 0.5f;
			sprites[i].color = c;
		}

		yield return null;

		StartCoroutine(BouncingRewind());
		StartCoroutine(OscillatingPicture());

		time = 0f;
		timeAnim = 1f;

		while(time<timeAnim){

			c = texts[1].color;
			c.a = Mathf.Lerp(0f,1f,Mathf.Sqrt(time/timeAnim));
			texts[1].color = c;

			c = texts[2].color;
			c.a = Mathf.Lerp(0f,1f,Mathf.Sqrt(time/timeAnim));
			texts[2].color = c;

			c = texts[3].color;
			c.a = Mathf.Lerp(0f,1f,Mathf.Sqrt(time/timeAnim));
			texts[3].color = c;

			time+=Time.deltaTime;
			yield return null;
		}

		c = texts[1].color;
		c.a = 1f;
		texts[1].color = c;

		c = texts[2].color;
		c.a = 1f;
		texts[2].color = c;

		c = texts[3].color;
		c.a = 1f;
		texts[3].color = c;



		while(!tapstart){
			

			if(CheckInput()){
				tapstart = true;
				StartCoroutine(EndIntro());
			}

			yield return null;

		}

	}

	IEnumerator BouncingRewind(){

		float time = 0f;
		Color c = sprites[1].color;
		c.a = 1f;
		sprites[1].color = c;
		Vector3 v;
		v = rewindObject.transform.position;
		while(true){

			rewindObject.transform.position = v + heightAnimRewind*animRewindObject.Evaluate(time/timeRewind)*transform.up;
			time+=Time.deltaTime;

			if(time>timeRewind){
				time = 0f;
				yield return new WaitForSeconds(0.8f);
			}
			yield return null;
		}

	}

	IEnumerator OscillatingPicture(){

		float time = 0f;

		float angle0 = 10f;
		float angle1 = -15f;
//		float vel0 = 0f;
//		float vel1 = 0f;

		while(true){

			angle0 = 10f*Mathf.Cos(time)*Mathf.Exp(-0.05f*time);
			angle1 = -15f*Mathf.Cos(time)*Mathf.Exp(-0.1f*time);

			sprites[5].transform.parent.transform.localRotation = Quaternion.identity;
			sprites[6].transform.parent.transform.localRotation = Quaternion.identity;

			sprites[5].transform.parent.transform.Rotate(new Vector3(0f,0f,angle0),Space.Self);
			sprites[6].transform.parent.transform.Rotate(new Vector3(0f,0f,angle1),Space.Self);

			time+=4f*Time.deltaTime;
			yield return null;
		}


	}

	IEnumerator EndIntro(){

		float time = 0f;
		Color c;
		while(time<2f){

			for(int i=0;i<texts.Length;i++){
				c = texts[i].color;
				c.a = 1f-time/2f;
				texts[i].color = c;
			}

			for(int i=0;i<sprites.Length;i++){

				if(i==0 || i==2 || i==3 || i==4){
					c = sprites[i].color;
					c.a = 0.5f-0.5f*time/2f;
					sprites[i].color = c;
				}else{
					c = sprites[i].color;
					c.a = 1f-time/2f;
					sprites[i].color = c;
				}

			}

			time+=Time.deltaTime;
			yield return null;

		}

		for(int i=0;i<texts.Length;i++){
			c = texts[i].color;
			c.a = 0f;
			texts[i].color = c;
		}


		for(int i=0;i<sprites.Length;i++){
			c = sprites[i].color;
			c.a = 0f;
			sprites[i].color = c;
		}

		UnityEngine.SceneManagement.SceneManager.LoadScene(1);


	}



	bool CheckInput(){

		#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE || UNITY_STANDALONE_OSX

		if(Input.GetKeyDown(KeyCode.Space)){
			return true;
		}else{
			return false;
		}


		#elif UNITY_ANDROID || UNITY_IOS
		if(Input.touchCount>0){
		if(Input.GetTouch(0).phase == TouchPhase.Began){
		return true;
		}

		return false;

		}else{
		return false;
		}
		#endif

	}
}
