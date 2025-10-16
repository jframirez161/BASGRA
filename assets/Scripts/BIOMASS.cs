using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIOMASS : MonoBehaviour {

	public float MS;
	public float damage;
	public bool PM;

	public float R;
	public float G;
	public float B;

	void Start() {
		MS = 0.01f;
		damage = 0;
		PM=true;
	}

	void Update () {

		if (MS <= 0) {
			Destroy (this.gameObject);
		}
	}


}
