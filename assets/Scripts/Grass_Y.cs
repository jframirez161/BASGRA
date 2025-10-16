using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass_Y : MonoBehaviour {

	public GameObject grass_; 

	void Start () {
		StartCoroutine(CreateWorld());
	}


	IEnumerator CreateWorld() {    

		GameObject areas = GameObject.FindGameObjectWithTag ("Generator");
		int areaa = areas.GetComponent<Generator> ().worldWidth;
			
			for(int x =0; x<areaa; x++) {
			yield return new WaitForSeconds(0.1f);	
			Instantiate(grass_, new Vector3(this.transform.position.x,0, this.transform.position.z + x), Quaternion.identity);

		}

	}
}
