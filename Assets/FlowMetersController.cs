using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using System;

public class FlowMetersController : MonoBehaviour {
    public GUIController controller;
    private AudioSource audio;

    [Header("Air Variables")]
    public GameObject knobAir;
    private float knobAngleAir;
    private float valueIndicatorBigAir;
    private CircularDrive driverKnobAir;
    public GameObject indicatorConeBigAir;
    public int indexOfCurrentValueIndicatorBigAir = 0;
    private int prevIndexOfCurrentValueIndicatorBigAir = 0;
    public float[] valuesIndicatorBigAir = new float[16] { -0.0261f, -0.02021f, -0.01674f, -0.01343f, -0.0098f, -0.00665f, -0.00326f, 0.00029f, 0.0036f, 0.00699f, 0.01046f, 0.01385f, 0.01716f, 0.02076f, 0.02393f, 0.0272f };
    public Text AirText;
    private float[] flowAirValues = new float[16]{0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f};
    public bool barometerAirEnabled = true;
	public GameObject indicatorPressureAir;
	public float valueIndicatorPressureAir = 0f;
	private float maxAngleIndicatorPressureAir = 0f;

	[Header("N2O Variables")]
	public GameObject knobN2O;
	public float knobAngleN2O;
	private float valueIndicatorSmallN2O;
	private CircularDrive driverKnobN2O;
	public GameObject indicatorConeSmallN2O;
	public Vector2 rangeConeSmallN2O;
	public GameObject indicatorConeBigN2O;
	public int indexOfCurrentValueIndicatorBigN2O = 0;
    private int prevIndexOfCurrentValueIndicatorBigN2O = 0;
    public float[] valuesIndicatorBigN2O = new float[11]{ -0.0253f, -0.01573f, -0.01121f, -0.00654f, -0.00202f, 0.0025f, 0.00709f, 0.01161f, 0.01605f, 0.02072f, 0.02531f};
    public Text N2OText;
    private float[] flowN2OValues = new float[11] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f};
    public bool barometerN2OEnabled = true;
	public GameObject indicatorPressureN2O;
	public float valueIndicatorPressureN2O = 0f;
	private float maxAngleIndicatorPressureN2O = 0f;


	[Header("O2 Variables")]
	public GameObject knobO2;
	private float knobAngleO2;
	private float valueIndicatorSmallO2;
	private CircularDrive driverKnobO2;
	public GameObject indicatorConeSmallO2;
	public Vector2 rangeConeSmallO2;
	public GameObject indicatorConeBigO2;
	public int indexOfCurrentValueIndicatorBigO2 = 0;
    private int prevIndexOfCurrentValueIndicatorBigO2 = 0;
    public float [] valuesIndicatorBigO2 = new float[11]{ -0.0261f, -0.0216f, -0.0181f, -0.0147f, -0.01122f, -0.0078f, -0.00448f, -0.001f, 0.00233f, 0.00574f, 0.00915f};
    public Text O2Text;
    private  float[] flowO2Values = new  float[11] { 0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f};
    public bool barometerO2Enabled = true;
	public GameObject indicatorPressureO2;
	public float valueIndicatorPressureO2 = 0f;
	private float maxAngleIndicatorPressureO2 = 0f;



	// Use this for initialization
	void Start () {
		InitFlowMeters ();
        audio = GetComponent<AudioSource>();

    }

	// Update is called once per frame
	void Update () {
		
		valueIndicatorBigAir = getValueIndicatorConeBigAir ();
		valueIndicatorSmallN2O = getValueIndicatorConeSmallN2O ();
		valueIndicatorSmallO2 = getValueIndicatorConeSmallO2 ();
		valueIndicatorPressureAir = getValueIndicatorPressureAir ();
		valueIndicatorPressureN2O = getValueIndicatorPressureN2O ();
		valueIndicatorPressureO2 = getValueIndicatorPressureO2 ();
	}



	public void InitFlowMeters(){
		//Init Air
		driverKnobAir = knobAir.GetComponent<CircularDrive>();
		maxAngleIndicatorPressureAir = driverKnobAir.minAngle;
		valueIndicatorPressureAir = getValueIndicatorPressureAir ();
		StartCoroutine (PointerPositionAirStateMachineCoroutine());

		valueIndicatorBigAir = getValueIndicatorConeBigAir ();
		StartCoroutine (ConePositionBigAirStateMachineCoroutine());

		//Init N2O
		driverKnobN2O = knobN2O.GetComponent<CircularDrive> ();
		maxAngleIndicatorPressureN2O = driverKnobN2O.minAngle;
		valueIndicatorPressureN2O = getValueIndicatorPressureN2O ();
        StartCoroutine(PointerPositionN2OStateMachineCoroutine());

		valueIndicatorSmallN2O = getValueIndicatorConeSmallN2O ();
		StartCoroutine (ConePositionSmallN2OStateMachineCoroutine());
		StartCoroutine (ConePositionBigN2OStateMachineCoroutine ());

		//Init O2
		driverKnobO2 = knobO2.GetComponent<CircularDrive> ();
		maxAngleIndicatorPressureO2 = driverKnobO2.minAngle;
		valueIndicatorPressureO2 = getValueIndicatorPressureO2 ();
        StartCoroutine(PointerPositionO2StateMachineCoroutine());

		valueIndicatorSmallO2 = getValueIndicatorConeSmallO2 ();
		StartCoroutine (ConePositionSmallO2StateMachineCoroutine());
		StartCoroutine (ConePositionBigO2StateMachineCoroutine ());
	
	}






    public void UpdateTextValueAir(int index) {
		if (index < flowAirValues.Length) {
			AirText.text = flowAirValues [index].ToString ();
			controller.UserInputStateMachine (0, flowAirValues [index]);
		}
    }

    public void UpdateTextValueN2O(int index)
    {
		if (index < flowN2OValues.Length) {
			N2OText.text = flowN2OValues [index].ToString ();
			controller.UserInputStateMachine (1, flowN2OValues [index]);
		}
    }
    public void UpdateTextValueO2(int index)
    {
		if (index < flowO2Values.Length) {
			float temp = flowO2Values [index];
			string str = flowO2Values [index].ToString ();
			O2Text.text = flowO2Values [index].ToString ();
			controller.UserInputStateMachine (2, flowO2Values [index]);
		}
    }


    public float getValueIndicatorPressureAir(){
		return -170f * ( knobAngleAir / ( (maxAngleIndicatorPressureAir != 0f)?maxAngleIndicatorPressureAir:1f));
	}

	public float getValueIndicatorPressureN2O(){
		return -170f * ( knobAngleN2O / ( (maxAngleIndicatorPressureO2 != 0f)?maxAngleIndicatorPressureN2O:1f));
	}


	public float getValueIndicatorPressureO2(){
		return -170f * ( knobAngleO2 / ( (maxAngleIndicatorPressureO2 != 0f)?maxAngleIndicatorPressureO2:1f));
	}



	public float getValueIndicatorConeBigAir(){
		float knobAnglePercentage = 0f;
		knobAngleAir = driverKnobAir.outAngle;
		knobAnglePercentage =  Mathf.Abs (knobAngleAir) / 45f;//360f
		indexOfCurrentValueIndicatorBigAir = (int)knobAnglePercentage;
        if (prevIndexOfCurrentValueIndicatorBigAir != indexOfCurrentValueIndicatorBigAir) {
            UpdateTextValueAir(indexOfCurrentValueIndicatorBigAir);
            audio.Stop();
            audio.Play();
        }
		if(knobAnglePercentage != 0f){
			knobAnglePercentage = (knobAnglePercentage%1f>0f)?knobAnglePercentage%1f:1f; // aqui donde se puede modificar el grado de acertado en el nivel del cono
		}
        prevIndexOfCurrentValueIndicatorBigAir = indexOfCurrentValueIndicatorBigAir;


        if (indexOfCurrentValueIndicatorBigAir < (valuesIndicatorBigAir.Length - 1)) {
			return valuesIndicatorBigAir [indexOfCurrentValueIndicatorBigAir] + knobAnglePercentage * (valuesIndicatorBigAir [indexOfCurrentValueIndicatorBigAir + 1] - valuesIndicatorBigAir [indexOfCurrentValueIndicatorBigAir]);
		} else {
			return valuesIndicatorBigAir [valuesIndicatorBigAir.Length-1];
		}
	}

	public float getValueIndicatorConeSmallN2O(){
		float knobAnglePercentage = 0f; 
		knobAngleN2O = driverKnobN2O.outAngle;
		knobAnglePercentage = Mathf.Abs (knobAngleN2O) / 45f;//360f
		indexOfCurrentValueIndicatorBigN2O = (int)knobAnglePercentage;


       if (prevIndexOfCurrentValueIndicatorBigN2O != indexOfCurrentValueIndicatorBigN2O){
            UpdateTextValueN2O(indexOfCurrentValueIndicatorBigN2O);
            audio.Stop();
            audio.Play();
        }

        if (knobAnglePercentage != 0f){
			knobAnglePercentage = (knobAnglePercentage%1f>0f)?knobAnglePercentage%1f:1f; // aqui donde se puede modificar el grado de acertado en el nivel del cono
		}
        prevIndexOfCurrentValueIndicatorBigN2O = indexOfCurrentValueIndicatorBigN2O;

        return (rangeConeSmallN2O.y - rangeConeSmallN2O.x) * (knobAnglePercentage ) + rangeConeSmallN2O.x;

	}



	public float getValueIndicatorConeSmallO2(){
		float knobAnglePercentage = 0f; 
		knobAngleO2 = driverKnobO2.outAngle;
		knobAnglePercentage = Mathf.Abs (knobAngleO2) / 45f;//360f
		indexOfCurrentValueIndicatorBigO2 = (int)knobAnglePercentage;

        if (prevIndexOfCurrentValueIndicatorBigO2 != indexOfCurrentValueIndicatorBigO2) {
            UpdateTextValueO2(indexOfCurrentValueIndicatorBigO2);
            audio.Stop();
            audio.Play();
        }

		if(knobAnglePercentage != 0f){
			knobAnglePercentage = (knobAnglePercentage%1f>0f)?knobAnglePercentage%1f:1f; // aqui donde se puede modificar el grado de acertado en el nivel del cono
		}
        prevIndexOfCurrentValueIndicatorBigO2 = indexOfCurrentValueIndicatorBigO2;


        return (rangeConeSmallO2.y - rangeConeSmallO2.x) * (knobAnglePercentage ) + rangeConeSmallO2.x;

	}


	public Vector3 transformYLocalPosition(GameObject obj ,float value){

		return new Vector3 (obj.transform.localPosition.x , value, obj.transform.localPosition.z);
	}





	public IEnumerator PointerPositionAirStateMachineCoroutine(){
		float initAnglePointer = 0f, targetAnglePointer = 0f, currentAnglePointer = 0f;
		float rate = 0f;
		float time2Interpolate = 2f;
		yield return new WaitForSeconds (.1f);
		while (Application.isPlaying) {
			initAnglePointer = valueIndicatorPressureAir;
			targetAnglePointer = valueIndicatorPressureAir;
			float i = 0f;
			rate = 1f / time2Interpolate * 2f;
			while (i < 1f) {
				i += Time.deltaTime * rate;
				currentAnglePointer = Mathf.Lerp (initAnglePointer, getValueIndicatorPressureAir(), i);
				indicatorPressureAir.transform.localEulerAngles = new Vector3 (currentAnglePointer, 0f, 0f);
				yield return null;
			}
			i = 0;
			yield return null;
		}
	
	}


    public IEnumerator PointerPositionN2OStateMachineCoroutine()
    {
        float initAnglePointer = 0f, targetAnglePointer = 0f, currentAnglePointer = 0f;
        float rate = 0f;
        float time2Interpolate = 2f;
        yield return new WaitForSeconds(.1f);
        while (Application.isPlaying)
        {
            initAnglePointer = valueIndicatorPressureN2O;
            targetAnglePointer = valueIndicatorPressureN2O;
            float i = 0f;
            rate = 1f / time2Interpolate * 2f;
            while (i < 1f)
            {
                i += Time.deltaTime * rate;
                currentAnglePointer = Mathf.Lerp(initAnglePointer, getValueIndicatorPressureN2O(), i);
                indicatorPressureN2O.transform.localEulerAngles = new Vector3(currentAnglePointer, 0f, 0f);
                yield return null;
            }
            i = 0;
            yield return null;
        }

    }

    public IEnumerator PointerPositionO2StateMachineCoroutine()
    {
        float initAnglePointer = 0f, targetAnglePointer = 0f, currentAnglePointer = 0f;
        float rate = 0f;
        float time2Interpolate = 2f;
        yield return new WaitForSeconds(.1f);
        while (Application.isPlaying)
        {
            initAnglePointer = valueIndicatorPressureO2;
            targetAnglePointer = valueIndicatorPressureO2;
            float i = 0f;
            rate = 1f / time2Interpolate * 2f;
            while (i < 1f)
            {
                i += Time.deltaTime * rate;
                currentAnglePointer = Mathf.Lerp(initAnglePointer, getValueIndicatorPressureO2(), i);
                indicatorPressureO2.transform.localEulerAngles = new Vector3(currentAnglePointer, 0f, 0f);
                yield return null;
            }
            i = 0;
            yield return null;
        }

    }


    public IEnumerator ConePositionBigAirStateMachineCoroutine(){
		Vector3 initPositionAir, targetPositionAir;
		float rate = 0f;
		float time2Interpolate = 2f;
		yield return new WaitForSeconds (.1f);
		while (Application.isPlaying) {
			initPositionAir = indicatorConeBigAir.transform.localPosition;
			targetPositionAir = transformYLocalPosition (indicatorConeBigAir, valueIndicatorBigAir);
			float i = 0f;
			rate = 1f / time2Interpolate * 2f;
			while (i < 1f) {
				i += Time.deltaTime * rate;
				indicatorConeBigAir.transform.localPosition = Vector3.Lerp (initPositionAir,  targetPositionAir, i);
				yield return null;
			}
			i = 0;
			yield return null;
		}
	}





	public IEnumerator ConePositionSmallO2StateMachineCoroutine(){
		Vector3 initPositionO2, targetPositionO2;
		float rate = 0f;
		float time2Interpolate = 2f;
		yield return new WaitForSeconds (.1f);
		while (Application.isPlaying) {
			initPositionO2 = indicatorConeSmallO2.transform.localPosition;
			targetPositionO2 = transformYLocalPosition (indicatorConeSmallO2, valueIndicatorSmallO2);
			float i = 0f;
			rate = 1f / time2Interpolate * 2f;
			while (i < 1f) {
				i += Time.deltaTime * rate;
				indicatorConeSmallO2.transform.localPosition = Vector3.Lerp (initPositionO2,  targetPositionO2, i);
				yield return null;
			}
			i = 0;
			yield return null;
		}
	}




	public IEnumerator ConePositionBigO2StateMachineCoroutine(){
		Vector3 initPositionO2, targetPositionO2;
		float rate = 0f;
		float time2Interpolate = 2f;
		yield return new WaitForSeconds (.1f);
		while (Application.isPlaying) {
			initPositionO2 = indicatorConeBigO2.transform.localPosition;
			targetPositionO2 = transformYLocalPosition (indicatorConeBigO2, valuesIndicatorBigO2[indexOfCurrentValueIndicatorBigO2]);
			float i = 0f;
			rate = 1f / time2Interpolate * 2f;
			while (i < 1f) {
				i += Time.deltaTime * rate;
				indicatorConeBigO2.transform.localPosition = Vector3.Lerp (initPositionO2,  targetPositionO2, i);
				yield return null;
			}
			i = 0;
			yield return null;
		}
	}
		



	public IEnumerator ConePositionSmallN2OStateMachineCoroutine(){
		Vector3 initPositionN2O, targetPositionN2O;
		float rate = 0f;
		float time2Interpolate = 2f;
		yield return new WaitForSeconds (.1f);
		while (Application.isPlaying) {
			initPositionN2O = indicatorConeSmallN2O.transform.localPosition;
			targetPositionN2O = transformYLocalPosition (indicatorConeSmallN2O, valueIndicatorSmallN2O);
			float i = 0f;
			rate = 1f / time2Interpolate * 2f;
			while (i < 1f) {
				i += Time.deltaTime * rate;
				indicatorConeSmallN2O.transform.localPosition = Vector3.Lerp (initPositionN2O,  targetPositionN2O, i);
				yield return null;
			}
			i = 0;
			yield return null;
		}
	}






	public IEnumerator ConePositionBigN2OStateMachineCoroutine(){
		Vector3 initPositionN2O, targetPositionN2O;
		float rate = 0f;
		float time2Interpolate = 2f;
		yield return new WaitForSeconds (.1f);
		while (Application.isPlaying) {
			initPositionN2O = indicatorConeBigN2O.transform.localPosition;
			targetPositionN2O = transformYLocalPosition (indicatorConeBigN2O, valuesIndicatorBigN2O[indexOfCurrentValueIndicatorBigN2O]);
			float i = 0f;
			rate = 1f / time2Interpolate * 2f;
			while (i < 1f) {
				i += Time.deltaTime * rate;
				indicatorConeBigN2O.transform.localPosition = Vector3.Lerp (initPositionN2O,  targetPositionN2O, i);
				yield return null;
			}
			i = 0;
			yield return null;
		}
	}

}
