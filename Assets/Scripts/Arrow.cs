using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		Destroy (gameObject);
	}
}
