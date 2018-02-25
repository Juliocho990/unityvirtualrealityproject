using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knobScaleControl : MonoBehaviour {


	public Vector3 offsetAxisOrigin = Vector3.zero;
	public Vector3 scaleHoverBegin = new Vector3(.7f, .7f, .7f);


	private Vector3 initAxisOrigin;
	private Vector3 scaleHoverEnd;


	// Use this for initialization
	void Start () {
		initAxisOrigin = transform.localPosition;
		scaleHoverEnd = transform.localScale;
	}
	

	public void setScaleHover(bool isHoverBegin){
		if (isHoverBegin) {
			transform.localPosition += offsetAxisOrigin; 
			transform.localScale = scaleHoverBegin;

		} else {
			
			transform.localScale = scaleHoverEnd;
			transform.localPosition = initAxisOrigin;
		}
		
	}
}
