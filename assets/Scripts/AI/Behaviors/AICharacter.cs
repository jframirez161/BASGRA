using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AICharacter:MonoBehaviour {
	// Movement stats
	public float mass;
	public float maxForce;
	public float maxSpeed;
	Vector3 steeringForce;
	Vector3 acc;
	[HideInInspector]
	public Vector3 vel;
	List<BaseBehavior> currentAI;
	int totalActiveAI;
	float weightScaleFactor;
	
	Vector3 mHeading;
	Vector3 mSide;
	[HideInInspector]
	public AIManager aiManager;

	public float dmi_     = 0;
	public float dmi    = 0; //To show DMI in real time
	public float[] dmi_All;

	public int grazingTime_ = 0;
	public int grazingTime = 0;
	public int[] grazingTimeAll;

	Vector3 Actposit;
	public float distance_ = 0;
	private float distance = 0;
	public float[] distanceAll;

	int counter = 0;

	bool grazing_;

	public float startime;
	float t;

	float proba;

	float tempTime;
	float tempTime_g;
	float tempTime_end;
	public float start_g=0;

	public float T_max;
	public float T_min;
	public float F_max;
	public float F_min;

	public GameObject hece_;

	float rand;
	float noche;

	public float leche;

	public Material texture;

	void Awake() {
		currentAI = new List<BaseBehavior>();
		steeringForce = Vector3.zero;
		vel = Vector3.zero;
		acc = Vector3.zero;
		totalActiveAI = 0;
		weightScaleFactor = 1;
	}
	
	void Start() {
		aiManager = GameObject.FindWithTag("AIManager").GetComponent<AIManager>();
		aiManager.AddCharacter(this);

		mass = 1f;
		maxForce = 10f;
		maxSpeed = 5f;

		grazing_ = false;
		proba = 0;
	
		dmi_All = new float[24];
		grazingTimeAll = new int[24];
		distanceAll = new float[24];

		Actposit = transform.position;

	}
	
	/**************************************************
	  				 UPDATE
	**************************************************/
	    
public void UpdateAI() {	
    

			// Calculate the accumulated weighted prioritized force
		weightScaleFactor = totalActiveAI > 0 ? currentAI.Count / totalActiveAI : 1;
		steeringForce = Vector3.zero;

		foreach (BaseBehavior b in currentAI) {
			if (!b.paused) {
				if (b.UpdateAI ()) {						
					// Apply weights
					if (!b.ignoreWeightCalculations)
						b.steeringForce *= weightScaleFactor * b.weight;
					
					if (!AccumulateForce (b.steeringForce))
						break;
				}
			}
		}
		
		acc = (steeringForce / mass);
		vel += acc * Time.deltaTime;
		vel = limitTo (vel, maxSpeed);

		//Time
		t = Time.time-startime;


		//grass damage
		int layerMask0 = 1 << 9; //Layer 9 pasto
		Collider[] hitColliders0 = Physics.OverlapSphere(this.transform.position, 1.5f, layerMask0);

		if (hitColliders0.Length > 0) {
			for (var z = 0; z < hitColliders0.Length; z++) {
				float damage_ =	hitColliders0 [z].gameObject.GetComponent<BIOMASS> ().damage;
				hitColliders0 [z].gameObject.GetComponent<BIOMASS> ().damage = damage_ + 0.001f;
			}
		} else {
			grazing_ = false;
		}



		if (grazing_ == false) {

			Animation anim = GetComponent<Animation>();
			anim.CrossFade("walk");
			anim ["walk"].speed = vel.magnitude;
		
			float rest = Random.Range (0,100f);

			if (rest < 50 && t > 360 && t < 480 || t > noche) {
				transform.position += new Vector3 (0, 0, 0) * Time.deltaTime;
				gameObject.transform.LookAt (transform.position + vel);
			} else {
				transform.position += vel * Time.deltaTime;
				gameObject.transform.LookAt (transform.position + vel);
			}
					 
			//grazing probability, every second
			tempTime += Time.deltaTime;
			if (tempTime > 1) {
				tempTime = 0;		
				rand = Random.Range (1.0f, 140f);
				}


		//Find total biomass
		GameObject[] pastos = GameObject.FindGameObjectsWithTag ("grass");	
		float biomass = 0;
		for (int bb = 0; bb < pastos.Length; bb++) {
			biomass = biomass + pastos [bb].GetComponent<BIOMASS> ().MS;
		}

		//grazing probability
		noche = Random.Range (780f, 840f);
		if (t < noche) {
			proba = (100.0f * biomass) / start_g;
		} else {
			proba = Random.Range (0, 10);
		}

     
		if(rand <= proba ){
			grazing_ = true;
		}

	}


		if (grazing_ == true) {

		Animation anim = GetComponent<Animation>();
		anim.CrossFade("grazing");

		 tempTime_g += Time.deltaTime;
		 if (tempTime_g > 1) {

			int layerMask = 1 << 9; //Layer 9 pasto
			Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1.5f, layerMask);

			if (hitColliders.Length > 0) {
				transform.position += vel * Time.deltaTime * Random.Range (0, 0.5f);
				gameObject.transform.LookAt (transform.position + vel);
				
				float ms_ = hitColliders[0].gameObject.GetComponent<BIOMASS> ().MS;
		     	float dm_ = hitColliders[0].gameObject.GetComponent<BIOMASS> ().damage;
				float sum = ms_ + dm_;
				int selected = 0;

				for (int i=1; i < hitColliders.Length; i++){
				float ms_0 = hitColliders[i].gameObject.GetComponent<BIOMASS> ().MS;
				float ms_1 = hitColliders[i].gameObject.GetComponent<BIOMASS> ().damage;
				float ms_2 = ms_0 + ms_1;
					if (ms_2 > sum) {
						sum = ms_2;
						selected = i;
					}	
				}

			DMI (hitColliders[selected].gameObject);

			}else{
				grazing_ = false;
			}
				tempTime_g = 0;
				rand = Random.Range (1.0f, 140f); 

				if(rand > proba && grazing_ == true){
					grazing_ = false;
				}

				//Fecal 
				float ran_heces = Random.Range(0,1440);
				if(ran_heces <= 50){
				Instantiate(hece_,this.transform.position,Quaternion.identity);
				}
		 }
	 }

}


	public void stored(){		//Store values every hour
			
		dmi_All [counter]       = dmi;
		dmi        =0;

		grazingTimeAll[counter] = grazingTime;
		grazingTime =0;

		distanceAll[counter] = distance;
		distance =0;

		counter = counter + 1;
	}




	public void addBehavior(BaseBehavior b) {
		currentAI.Add(b);
		currentAI.Sort(SortBehavior);
	}
	
	int SortBehavior(BaseBehavior b1, BaseBehavior b2){		
		return b2.priority - b1.priority;
	}
	

	bool AccumulateForce(Vector3 force)	{		
		float combinedForceMagnitude = steeringForce.magnitude;		
		float remainingForceMagnitude = maxForce - combinedForceMagnitude;		
		if(remainingForceMagnitude<=0) return false;		
		float currentForceMagnitude = force.magnitude;
		if(currentForceMagnitude < remainingForceMagnitude){
			steeringForce += force;
		}
		else{
			force.Normalize();
			Vector3 newForce = force * remainingForceMagnitude;
			steeringForce += newForce;
		}		
		return true;
	}

		
	/*************************************************
	  				 HELPER FUNCTIONS
	*************************************************/
	public Vector3 limitTo(Vector3 vec, float val) {
		if(vec.magnitude > val) {
			vec = vec.normalized * val;
		}
		return vec;
	}
	
	public Vector3 Heading(){		
		return transform.forward;	
	}

	/*************************************************
	  				 NEW FUNCTIONS
	*************************************************/

	void DMI(GameObject other){

		byte R;
		byte G;
		byte B;
	
		grazingTime = grazingTime + 1;
		grazingTime_ = grazingTime_ + 1;
		distance = distance + Vector3.Distance (Actposit, transform.position);
		distance_ = distance_ + Vector3.Distance (Actposit, transform.position);
		Actposit = transform.position;

		if (other.name == "Alto") {
				R = (byte)(other.gameObject.GetComponent<BIOMASS> ().R + 15);
				G = (byte)(other.gameObject.GetComponent<BIOMASS> ().G);
				B = (byte)(other.gameObject.GetComponent<BIOMASS> ().B + 15);
				other.GetComponent<Renderer> ().material.color = new Color32 (R, G, B, 255);
		} else if (other.name == "Medio") {
			    R = (byte)(other.gameObject.GetComponent<BIOMASS> ().R + 15);
			    G = (byte)(other.gameObject.GetComponent<BIOMASS> ().G + 15);
				B = (byte)(other.gameObject.GetComponent<BIOMASS> ().B);
			    other.GetComponent<Renderer> ().material.color = new Color32 (R, G, B, 255); 
		} else if (other.name == "Bajo") {
				R = (byte)(other.gameObject.GetComponent<BIOMASS> ().R);
				G = (byte)(other.gameObject.GetComponent<BIOMASS> ().G);
				B = (byte)(other.gameObject.GetComponent<BIOMASS> ().B);
			    other.GetComponent<Renderer> ().material.color = new Color32 (R, G, B, 255); 
		} 

		    float T_cons0 = F_max - F_min;
		    float T_cons1 = other.gameObject.GetComponent<BIOMASS> ().MS/T_cons0;
		    float dmi0 = Mathf.Lerp (T_min, T_max, T_cons1);	

			float y_ = other.gameObject.GetComponent<Transform> ().localScale.y;
		    float t_scale = (dmi0 / other.gameObject.GetComponent<BIOMASS> ().MS)/2;
		    other.transform.localScale = new Vector3 (1.0f, y_ - (y_ * t_scale), 1.0f);

			other.gameObject.GetComponent<BIOMASS> ().MS = other.gameObject.GetComponent<BIOMASS> ().MS - dmi0;
			dmi = dmi + dmi0;
			dmi_ = dmi_ + dmi0;
	
		//Alto 
		//R = 50;
		//G = 200;
		//B = 0;  

		//Medio      
		//R = 100;
		//G = 150;
		//B = 50; 

		//bajo
		//R = 150;
		//G = 200;
		//B = 50; 
	}

}
