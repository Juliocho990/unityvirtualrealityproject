using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPointEnabler : MonoBehaviour {

	GameObject followHead;
	Vector2 followHeadPosition, prevHeadPosition;
	GameObject[] teleportPoints;
	int nPoints;
	Vector2[] teleport2DTransforms;
	const float maxDistance = 3f;

	// Use this for initialization
	void Start () {
		followHead = GameObject.Find ("FollowHead");
		prevHeadPosition = new Vector3 (0f, 0f, 0f);

		teleportPoints = GameObject.FindGameObjectsWithTag ("TeleportPoint");
		nPoints = teleportPoints.Length;
		teleport2DTransforms = new Vector2[nPoints];
		for (int i = 0; i < nPoints; i++) {
			teleport2DTransforms [i] = new Vector2 (teleportPoints [i].transform.position.x, teleportPoints [i].transform.position.z);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//Assessing current state
		//Only working on horizontal plane (Y not considered)
		followHeadPosition = new Vector2 (followHead.transform.position.x, followHead.transform.position.z);

		//Only enabling/disabling teleportPoints when player moves
		if (!(V2Equal(followHeadPosition,prevHeadPosition))){

			for (int i = 0; i < nPoints; i++) {
				teleport2DTransforms [i] = new Vector2 (teleportPoints [i].transform.position.x, teleportPoints [i].transform.position.z);
				if (V2CloserThan (followHeadPosition, teleport2DTransforms [i], maxDistance)) {
					teleportPoints [i].SetActive (true);
				} else {
					teleportPoints [i].SetActive (false);
				}
			}

			//Preparing next frame
			prevHeadPosition = followHeadPosition;
		}
	}

	private bool V2Equal(Vector2 a, Vector2 b){
		return Vector2.SqrMagnitude (a - b) < 0.0001f;
	}

	private bool V2CloserThan(Vector2 a, Vector2 b, float distance){
		return Vector2.SqrMagnitude (a - b) < (distance*distance);
	}
}
