using UnityEngine;
using System.Collections;

public class CassetteBackgroundSpawner : MonoBehaviour {

	public GameObject cassette;
	public AnimationCurve appearCassette;

	public int numberCassettes=0;
	public float allTime;


	public void SpawnCassette(Texture2D tex, float val){
		int j=numberCassettes%3;
		int i=(numberCassettes-j)/3;

		GameObject g;

		if(numberCassettes==0){
			g = GameObject.Instantiate(cassette,new Vector3(-2.5f,4.8f,120f),Quaternion.identity) as GameObject;
			numberCassettes=2;
		}else if(numberCassettes==2){
			g = GameObject.Instantiate(cassette,new Vector3(2.5f,4.8f,120f),Quaternion.identity) as GameObject;
			numberCassettes++;
		}else{
			g = GameObject.Instantiate(cassette,new Vector3(2.5f*(j-1),4.8f-i*1.8f,120f),Quaternion.identity) as GameObject;
			numberCassettes++;
		}

		g.transform.parent = transform;
		g.transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture = tex;
		g.transform.GetChild(4).GetComponent<TextMesh>().text = val.ToString("#0.00")+" sec";
		allTime+=val;

		StartCoroutine(AppearCassette(g));
	}

	IEnumerator AppearCassette(GameObject g){

		float time = 0f;
		float animTime = 0.4f;
		Vector3 v = g.transform.localScale;

		while(time<animTime){

			g.transform.localScale = 0.45f*appearCassette.Evaluate(time/animTime)*v;

			time+=Time.deltaTime;
			yield return null;

		}

		g.transform.localScale = 0.45f*v;

	}
}
