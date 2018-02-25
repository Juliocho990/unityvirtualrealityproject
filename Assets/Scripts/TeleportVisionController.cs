using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportVisionController : MonoBehaviour {

	// Computing angle
	Vector3 followHeadPosition, followHeadDirection, teleportPointPosition, teleportPointDirection;
	Vector3 prevHeadPosition,prevHeadDirection;
	GameObject followHead;
	float visionAngle;

	// Changing alpha with angle
	const float minVisionAngle = 30f;
	const float maxVisionAngle = 15f;
	float fullTintAlpha = 0.0f;
	float meshAlphaPercent = 1.0f;
	int tintColorID = 0;
	Color tintColor = Color.clear;
	Color titleColor = Color.clear;
	float fullTitleAlpha = 0.0f;
	MeshRenderer markerMesh;
	MeshRenderer switchSceneIcon;
	MeshRenderer moveLocationIcon;
	MeshRenderer lockedIcon;
	Text titleText;
	Material pointVisibleMaterial;

	// Use this for initialization
	void Start () {
		
		teleportPointPosition = transform.parent.transform.position;
		followHead = GameObject.Find ("FollowHead");
		prevHeadPosition = new Vector3 (0f, 0f, 0f);
		prevHeadDirection = new Vector3 (0f, 0f, 0f);

		// Changing alpha with distance
		markerMesh = GetComponent<MeshRenderer>(); //transform.Find( "teleport_marker_mesh" ).GetComponent<MeshRenderer>();
		switchSceneIcon = transform.parent.Find( "teleport_marker_lookat_joint/teleport_marker_icons/switch_scenes_icon" ).GetComponent<MeshRenderer>();
		moveLocationIcon = transform.parent.Find( "teleport_marker_lookat_joint/teleport_marker_icons/move_location_icon" ).GetComponent<MeshRenderer>();
		lockedIcon = transform.parent.Find( "teleport_marker_lookat_joint/teleport_marker_icons/locked_icon" ).GetComponent<MeshRenderer>();
		titleText = transform.parent.Find( "teleport_marker_lookat_joint/teleport_marker_canvas/teleport_marker_canvas_text" ).GetComponent<Text>();
		pointVisibleMaterial = GameObject.Find ("Teleporting").GetComponent<Valve.VR.InteractionSystem.Teleport>().pointVisibleMaterial;

		tintColorID = Shader.PropertyToID( "_TintColor" );
		fullTintAlpha = .5f; //pointVisibleMaterial.GetColor( tintColorID ).a;
	}


	// Update is called once per frame
	void Update () {

		//Assessing current state
		followHeadPosition = followHead.transform.position;
		followHeadDirection = followHead.transform.forward;
		//Only working on horizontal plane (Y not considered)
		followHeadPosition = new Vector3 (followHeadPosition.x, 0f, followHeadPosition.z);
		followHeadDirection = new Vector3 (followHeadDirection.x, 0f, followHeadDirection.z);

		//Only updating transparency when player moves or looks in a different direction
		if (!(V3Equal(followHeadPosition,prevHeadPosition)) || !(V3Equal(followHeadDirection,prevHeadDirection)) ||
			AlphaExternallyChanged(fullTintAlpha * meshAlphaPercent))
		{
			teleportPointDirection = teleportPointPosition - followHeadPosition;
			//Forcing same Y plane
			teleportPointDirection = new Vector3 (teleportPointDirection.x, 0f, teleportPointDirection.z);

			//Computing transparency factor to be applied
			visionAngle = Vector3.Angle (followHeadDirection, teleportPointDirection);
			if (visionAngle < maxVisionAngle) {
				meshAlphaPercent = 1.0f;
			} else if (visionAngle > minVisionAngle) {
				meshAlphaPercent = 0.0f;
			} else {
				meshAlphaPercent = Mathf.Lerp (0.0f, 1.0f, (minVisionAngle - visionAngle) / minVisionAngle);
			}

	//		Debug.Log ("visionAngle = " + visionAngle + " ( head=" + followHeadDirection + " vs teleportPoint=" 
	//			+ teleportPointDirection + " ) / alpha = " + meshAlphaPercent);

			//Applying transparency
			SetAlpha (fullTintAlpha * meshAlphaPercent, meshAlphaPercent);

			//Preparing next frame
			prevHeadPosition = followHeadPosition;
			prevHeadDirection = followHeadDirection;
		}
	}		

	private bool V3Equal(Vector3 a, Vector3 b){
		return Vector3.SqrMagnitude (a - b) < 0.0001f;
	}

	//Alpha reset when starting teleport mode!!! Necessary to  correct it...
	private bool AlphaExternallyChanged( float tintAlpha)
	{
		return !(Mathf.Approximately(markerMesh.material.GetColor( tintColorID ).a,tintAlpha));
	}

	private void SetAlpha( float tintAlpha, float alphaPercent )
	{		
		
		tintColor = markerMesh.material.GetColor( tintColorID );
		tintColor.a = tintAlpha;

		markerMesh.material.SetColor( tintColorID, tintColor );
		switchSceneIcon.material.SetColor( tintColorID, tintColor );
		moveLocationIcon.material.SetColor( tintColorID, tintColor );
		lockedIcon.material.SetColor( tintColorID, tintColor );

		titleColor.a = fullTitleAlpha * alphaPercent;
		titleText.color = titleColor;
	}
}
