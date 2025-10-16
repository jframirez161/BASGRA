using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vagar : MonoBehaviour {
	
	float elapsed;
	float elapsed_ = 1;
	public bool pre_gr;
	public bool ordenoPM;

   
	void Start(){
		pre_gr = true;

	}

	void Update () {

		elapsed += Time.deltaTime;
		if (elapsed >= elapsed_) {
		elapsed = 0;
		elapsed_ = Random.Range (10f, 25f);


				if (pre_gr == true) {		
					pre_mover (); 				
				} else {
					float rand = Random.Range (0, 100f);			
					if (rand <= 20.0f) {
						mover (); // move to random position
					} else {
						move (); // move to best position
					}
				}


		}
	}


	void mover(){		

		GameObject[] hitColliders1= GameObject.FindGameObjectsWithTag("grass");
		int rand = Random.Range(0,hitColliders1.Length);
		this.transform.position = hitColliders1[rand].gameObject.GetComponent<Transform> ().position;
	}


	void pre_mover(){	
		this.transform.position = new Vector3(Random.Range(0,50),0,Random.Range(-5,-15));
	}


	void move(){
	
		//Find near grass
		int layerMask0 = 1 << 8; //Layer 8 pasto
		Collider[] hitColliders2 = Physics.OverlapSphere (this.transform.position, 20.0f, layerMask0);
		 
		if (hitColliders2.Length > 0) {
			float ms_ = hitColliders2 [0].gameObject.GetComponent<BIOMASS> ().MS;
			float dm_ = hitColliders2 [0].gameObject.GetComponent<BIOMASS> ().damage;
			float sum = ms_ + dm_;
			int selected = 0;
	
			for (int i = 1; i < hitColliders2.Length; i++) {
				float ms_0 = hitColliders2 [i].gameObject.GetComponent<BIOMASS> ().MS;
				float ms_1 = hitColliders2 [i].gameObject.GetComponent<BIOMASS> ().damage;
				float ms_2 = ms_0 + ms_1;

				if (ms_2 > sum) {
					sum = ms_2;
					selected = i;
				}	

			}

			this.transform.position = hitColliders2 [selected].GetComponent<Transform> ().position;

		} else {
			mover ();		
		}
	}
}
