using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scripProductoCruz : MonoBehaviour {
	Vector3 vecNormal = new Vector3 (0,1,0);
	Vector3 vec1 = new Vector3(1f,3f, 4f);
	Vector3 vec1Proyected, vec2Proyected;
	Vector3 vec2 = new Vector3(1f,3f, 4.1f);
	Vector3 vecCruz;
	// Use this for initialization
	void Start () {
		vec1 = vec1.normalized;
		vec2 = vec2.normalized;
		vec1Proyected = Vector3.ProjectOnPlane( vec1, vecNormal ).normalized; 
		vec2Proyected = Vector3.ProjectOnPlane (vec2, vecNormal).normalized;
		vecCruz = Vector3.Cross (vec1Proyected,vec1Proyected).normalized;
		Debug.LogError (vecCruz);
	}
	

}
