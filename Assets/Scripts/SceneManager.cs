using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

	public Texture2D[] texs; 


	public GameObject penObject;
	public GameObject tapeObject;

	public AnimationCurve penAppearAnimation;
	public Vector3 penAppearInitialPos;
	public Vector3 penAppearEndPos;
	public float penAppearTime;

	Vector3 penMovementInitialPos;
	public float initialPenLimit;
	public float initialPenVel;


	public AnimationCurve succeedThrowAnimation;
	public AnimationCurve succeedThrowAllocation;
	public Vector3 succeedThrowEndPos;
	public float succeedThrowTime;

	public int numberRewinds = 0;
	public TextMesh textRewinds;

	public bool firstTimeHole = true;
	public bool firstTimeRoll = true;


	public AnimationCurve animIndicatorHole;
	public SpriteRenderer indicatorHole;
	public TextMesh textHole;
	public TextMesh textSpin;

	public CassetteBackgroundSpawner cassetteBackgroundSpawner;

	float timeToComplete = 0f;

	public SpriteRenderer whiteScreen;
	public TextMesh finalText;

	void Start(){
		tapeObject.GetComponent<CassetteScript>().SetupCassette(0);
		penObject.GetComponent<PenScript>().SetupPen();
		initialPenVel = 2f;

		StartCoroutine(PenAppear());
	}

	IEnumerator PenAppear(){

		float time = 0f;
		penObject.transform.position = penAppearInitialPos;

		while(time<penAppearTime){

			float alpha = penAppearAnimation.Evaluate(time/penAppearTime);
			penObject.transform.position = penAppearInitialPos*(1f-alpha) + alpha*penAppearEndPos;
			time+=Time.deltaTime;
			yield return null;

		}

		penObject.transform.position = penAppearEndPos;
		penMovementInitialPos = penAppearEndPos;
		StartCoroutine(PenMovement());

		if(firstTimeHole){
			StartCoroutine(ShowIndicatorHole());
		}


	}

	IEnumerator PenMovement(){

		float time = 0f;
		bool throwPen = false;

		while(!throwPen){

			float offset = initialPenLimit*Mathf.Sin(time);

			Vector3 v = penAppearEndPos + new Vector3(offset,0f,0f);
			penObject.transform.position = v;
			time+=initialPenVel*Time.deltaTime;

			if(CheckInput()){
				throwPen = true;

				if(Mathf.Abs(offset+0.7f)>0.3f){
					StartCoroutine(FailThrow());
				}else{

					if(firstTimeHole){
						StopCoroutine(ShowIndicatorHole());
						StartCoroutine(HideIndicatorHole());
					}

					firstTimeHole = false;

					StartCoroutine(SucceedThrow());
				}
			}

			yield return null;
		}

		yield return null;

	}

	IEnumerator FailThrow(){

		float time = 0f;
		Vector3 v = tapeObject.transform.position;
		v.z = 100f;
		tapeObject.transform.position = v;

		while(time<1f){

			penObject.transform.position+=30f*Time.deltaTime*transform.up;
			time+=Time.deltaTime;
			yield return null;
		}

		v = tapeObject.transform.position;
		v.z = 0f;
		tapeObject.transform.position = v;

		StartCoroutine(PenAppear());

	}

	IEnumerator SucceedThrow(){


		float time = 0f;
		Vector3 v = penObject.transform.position;

		while(time<succeedThrowTime){

			float alpha = succeedThrowAnimation.Evaluate(time/succeedThrowTime);
			penObject.transform.position = v*(1f-alpha) + alpha*succeedThrowEndPos;

			time+=Time.deltaTime;
			yield return null;
		}

		penObject.transform.position = succeedThrowEndPos;
		tapeObject.transform.parent = penObject.transform;
		v = succeedThrowEndPos;
		v.x = 0f;

		time = 0f;
		float maxTime = 0.4f;
		while(time<maxTime){

			float alpha = succeedThrowAllocation.Evaluate(time/maxTime);
			penObject.transform.position = (1f-alpha)*succeedThrowEndPos + alpha*v;

			time += Time.deltaTime;
			yield return null;
		}

		penObject.transform.position = v;

		StartCoroutine(GameLoop());

	}

	IEnumerator GameLoop(){

		if(firstTimeRoll){
			StartCoroutine(ShowSpinIndicator());
		}

		timeToComplete = 0f;

		bool inGameLoop = true;
		CassetteScript cassetteScript = tapeObject.GetComponent<CassetteScript>();
		PenScript penScript = penObject.GetComponent<PenScript>();

		while(inGameLoop){

			if(CheckInput()){
				penScript.UpdateValuesPen();
				cassetteScript.UpdateValuesCassette();	
			}

			penScript.UpdatePen();
			cassetteScript.UpdateCassette();

			if(cassetteScript.CheckEscape()){

				if(firstTimeRoll){
					StopCoroutine(ShowSpinIndicator());
					StartCoroutine(HideSpinIndicator());
				}

				firstTimeRoll = false;

				inGameLoop = false;	
				StartCoroutine(GameOver());


			}else if(cassetteScript.CheckEndRolling()){
				if(firstTimeRoll){
					StopCoroutine(ShowSpinIndicator());
					StartCoroutine(HideSpinIndicator());
				}

				firstTimeRoll = false;
			
				inGameLoop = false;
				StartCoroutine(GoNextCassette());
			}


			timeToComplete+=Time.deltaTime;

			yield return null;
		}

	}

	IEnumerator GoNextCassette(){

		numberRewinds++;
		textRewinds.text=numberRewinds.ToString()+"\n"+"rewinds";
		initialPenVel = 2f*Mathf.Lerp(1f,2f,cassetteBackgroundSpawner.numberCassettes/18f);

		float time = 0f;
		tapeObject.transform.parent = null;

		while(time<2f){

			penObject.transform.position -= 5f*Time.deltaTime*Vector3.up;
			tapeObject.transform.position -= 5f*Time.deltaTime*Vector3.right;

			time+=Time.deltaTime;
			yield return null;
		}


		cassetteBackgroundSpawner.SpawnCassette(tapeObject.transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture as Texture2D,timeToComplete);

		Vector3 v0 = new Vector3(-8f,0f,0f);
		Vector3 v1 = new Vector3(-0.7f,0f,0f);
		tapeObject.transform.position = v0;
		tapeObject.transform.rotation = Quaternion.identity;
		penObject.transform.rotation = Quaternion.identity;

		penObject.GetComponent<PenScript>().SetupPen();
		tapeObject.GetComponent<CassetteScript>().SetupCassette(cassetteBackgroundSpawner.numberCassettes);
		tapeObject.transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture = texs[Random.Range(0,texs.Length)];


		if(cassetteBackgroundSpawner.numberCassettes==18){
			yield return new WaitForSeconds(3f);
			StartCoroutine(GoodEndGame());
		}else{
			time = 0f;

			while(time<1f){

				float alpha = penAppearAnimation.Evaluate(time);
				tapeObject.transform.position = (1f-alpha)*v0 + alpha*v1;

				time+=Time.deltaTime;
				yield return null;
			}



			tapeObject.transform.position = v1;

			StartCoroutine(PenAppear());

		}


	}

	IEnumerator BadEndGame(){

		float time = 0f;
		Color c = whiteScreen.color;
		c.a = 0f;
		whiteScreen.color = c;

		while(time<1f){

			c = whiteScreen.color;
			c.a = time/1f;
			whiteScreen.color = c;

			time+=Time.deltaTime;
			yield return null;
		}

		c = whiteScreen.color;
		c.a = 1f;
		whiteScreen.color = c;

		time = 0f;

		finalText.text = "you have only remembered a little fragment\nof your memories. you need more tapes...you need more time.";

		while(time<1f){

			c = finalText.color;
			c.a = time/1f;
			finalText.color = c;

			time+=Time.deltaTime;
			yield return null;
		}

		c = finalText.color;
		c.a = 1;
		finalText.color = c;

		yield return new WaitForSeconds(4f);
		time = 0f;

		while(time<1f){

			c = finalText.color;
			c.a = 1f-time/1f;
			finalText.color = c;

			time+=Time.deltaTime;
			yield return null;
		}


		UnityEngine.SceneManagement.SceneManager.LoadScene(0);



	}

	IEnumerator GoodEndGame(){

		float time = 0f;
		Color c = whiteScreen.color;
		c.a = 0f;
		whiteScreen.color = c;

		while(time<1f){

			c = whiteScreen.color;
			c.a = time/1f;
			whiteScreen.color = c;

			time+=Time.deltaTime;
			yield return null;
		}

		c = whiteScreen.color;
		c.a = 1f;
		whiteScreen.color = c;

		time = 0f;

		finalText.text = "you have remembered all your memories\nin just "+cassetteBackgroundSpawner.allTime.ToString("#.00")+" seconds. Isn't it wonderful?";

		while(time<1f){

			c = finalText.color;
			c.a = time/1f;
			finalText.color = c;

			time+=Time.deltaTime;
			yield return null;
		}

		c = finalText.color;
		c.a = 1;
		finalText.color = c;

		yield return new WaitForSeconds(5f);
		time = 0f;

		while(time<1f){

			c = finalText.color;
			c.a = 1f-time/1f;
			finalText.color = c;

			time+=Time.deltaTime;
			yield return null;
		}


		UnityEngine.SceneManagement.SceneManager.LoadScene(0);



	}



	IEnumerator GameOver(){

		float time = 0f;
		tapeObject.transform.parent = null;

		while(time<2f){

			penObject.transform.position -= 5f*Time.deltaTime*Vector3.up;
			tapeObject.transform.position -= 5f*Time.deltaTime*Vector3.right;
			penObject.transform.GetChild(0).Rotate(new Vector3(0,0,100f*Time.deltaTime),Space.Self);
			tapeObject.transform.Rotate(0,0,500f*Time.deltaTime);

			time+=Time.deltaTime;
			yield return null;

		}


		StartCoroutine(BadEndGame());



	}


	IEnumerator ShowIndicatorHole(){

		float time = 0;
		float timeAnim = 1f;

		Color c;
		c = indicatorHole.color;
		c.a=1f;
		indicatorHole.color = c;

		c=textHole.color;
		c.a=1f;
		textHole.color = c;

		Vector3 v = indicatorHole.transform.position;

		while(true){

			indicatorHole.transform.position = v + 0.3f*transform.up*animIndicatorHole.Evaluate(time/timeAnim);

			time+=Time.deltaTime;
			yield return null;
		}


	}

	IEnumerator HideIndicatorHole(){

		Color c;
		c = indicatorHole.color;
		c.a=0f;
		indicatorHole.color = c;

		c=textHole.color;
		c.a=0f;
		textHole.color = c;

		yield return null;


	}

	IEnumerator ShowSpinIndicator(){

		Color c;
		c = textSpin.color;
		c.a=1f;
		textSpin.color = c;

		Vector3 v = textSpin.transform.localScale;
		float s = 1f;
		while(true){

			if(CheckInput()){
				s=1.2f;
			}

			s-=1f*Time.deltaTime;

			if(s<1f){
				s=1f;
			}

			textSpin.transform.localScale = s*v;

			yield return null;
		}

	}

	IEnumerator HideSpinIndicator(){

		Color c;
		c = textSpin.color;
		c.a=0f;
		textSpin.color = c;

		yield return null;
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



