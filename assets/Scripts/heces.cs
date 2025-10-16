using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heces : MonoBehaviour {

	//Grass Contamination
	void Start () {
		int layerMask = 1 << 9; //Layer 9 pasto
		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 0.5f, layerMask);

		for(int x=0; x<hitColliders.Length; x++){
			hitColliders[x].gameObject.layer = 10;
		}
		
	}
	

}
