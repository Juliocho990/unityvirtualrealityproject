using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class VaporizersController : MonoBehaviour {

    public GUIController controller;

    private AudioSource audio;

    public Text vaporizerText;

	public enum vaporizer_t{SEVOFLURANE, DESFLURANE}

	public vaporizer_t vaporizerType = vaporizer_t.SEVOFLURANE;

	public float [] sevofluraneAngles = new float[15] {0f, -25f, -33.3f, -42.6f, -51.6f, -59.42f, -68.5f, -77.5f, -86.5f, -94.4f, -103.7f, -112.4f, -121.3f, -130.1f, -138.5f};
	private float [] sevofluraneValues = new float[15]{0f, 0.3f, 0.4f, 0.6f, 0.8f, 1f, 1.5f, 2f, 2.5f, 3f, 4f, 5f, 6f, 7f, 8f};

	private float [] desfluraneAngles = new float[13]{0f, -25.35f, -35.67f, -45.45f, -54.7f, -64.61f, -75f, -83.68f, -93.5f, -103.65f, -113.15f, -122.15f, -132.2f};
	private float [] desfluraneValues = {0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f};
	private CircularDrive driver;
	public float vaporizerValue = 0f;
    private float prevVaporizerValue = 0f;

	// Use this for initialization
	void Start () {
		driver = GetComponent<CircularDrive> ();
        audio = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
		if (vaporizerType == vaporizer_t.SEVOFLURANE) {
			for (int i = 0; i < (sevofluraneAngles.Length); i++) {
				if (driver.outAngle <= sevofluraneAngles [sevofluraneAngles.Length - 1]) {
					vaporizerValue = sevofluraneValues [sevofluraneValues.Length - 1];
				} else if (driver.outAngle <= sevofluraneAngles [i] && driver.outAngle > sevofluraneAngles [i + 1]) {
					vaporizerValue = sevofluraneValues [i];
				}
			}
		} else if(vaporizerType == vaporizer_t.DESFLURANE) {
			for (int i = 0; i < (desfluraneAngles.Length); i++) {
				if (driver.outAngle <= desfluraneAngles [desfluraneAngles.Length - 1]) {
					vaporizerValue = desfluraneValues [desfluraneAngles.Length - 1];
				} else if (driver.outAngle <= desfluraneAngles [i] && driver.outAngle > desfluraneAngles [i + 1]) {
					vaporizerValue = desfluraneValues [i];
				}
			}
        }
        if(prevVaporizerValue != vaporizerValue) {
            vaporizerText.text = vaporizerValue.ToString();
            audio.Stop();
            audio.Play();
        }
        
        prevVaporizerValue = vaporizerValue;
	}



    public void sendValueToGUIController() {

        controller.UserInputStateMachine((vaporizerType == vaporizer_t.SEVOFLURANE) ? 3 : 4, (vaporizerType == vaporizer_t.SEVOFLURANE) ? vaporizerValue : vaporizerValue);
       
    }
    
}
