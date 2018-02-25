using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class GUIController : MonoBehaviour {

	public Transform teleportPointStart;
	public Transform teleportPointAnesthesiaMachine;
	public GameObject UI_Intro; 
	public GameObject UI_AnesthesiaMachine;

    public GameObject flowMetersObj, sevofluraneObj, desfluraneObj;

	GameObject followHead;
	Vector2 followHeadPosition, prevHeadPosition;
	Vector2 teleport2DTransformStart,teleport2DTransformAnesthesiaMachine;
	const float maxDistance = .5f;
	int GUIstate,prevGUIstate;
	const int GUI_NONE = 0,GUI_INTRO = 1, GUI_ANESTHESIA_0 = 2, GUI_ANESTHESIA_1 = 3,GUI_ANESTHESIA_2 = 4,GUI_ANESTHESIA_3 = 5; 
	Text[] UI_Anesthesia_Texts; 
	float elapsedTime;
    private const int FLOWMETERS_INDEX_AIR = 0, FLOWMETERS_INDEX_N2O = 1, FLOWMETERS_INDEX_O2 = 2, SEVOFLURANE_INDEX = 3, DESFLURANE_INDEX = 4;
    private bool s_flowmeter = false, s_sevoflurane = false, s_desflurane = false;

    // Use this for initialization
    void Start () {
		followHead = GameObject.Find ("FollowHead");
		prevHeadPosition = new Vector3 (0f, 0f, 0f);

		teleport2DTransformStart =  new Vector2 (teleportPointStart.transform.position.x, teleportPointStart.transform.position.z);
		teleport2DTransformAnesthesiaMachine =  new Vector2 (teleportPointAnesthesiaMachine.transform.position.x, teleportPointAnesthesiaMachine.transform.position.z);
        
        GUIstate = GUI_NONE;
		prevGUIstate = GUI_NONE;
		UI_Intro.SetActive (false);
		UI_Anesthesia_Texts = UI_AnesthesiaMachine.GetComponentsInChildren<Text> ();
		for (int i = 0; i < UI_Anesthesia_Texts.Length; i++) {
			UI_Anesthesia_Texts [i].enabled = false;
		}
			
		elapsedTime = 0f;
	}




    
    public void UserInputStateMachine(int index, float value) {
       

        if (prevGUIstate >= GUI_ANESTHESIA_3)
            return;
        
         switch (index) {
            /*
            case FLOWMETERS_INDEX_AIR:
                if(value == -1 && !s_flowmeter)
                {
                    GUIstate++;
                    s_flowmeter = true;
                }
                
                break;
            case FLOWMETERS_INDEX_N2O:
                if (value == 4 && !s_flowmeter)
                {
                    GUIstate++;
                    s_flowmeter = true;
                }

                break;
            case FLOWMETERS_INDEX_O2:
                if (value == -1 && !s_flowmeter)
                {
                    GUIstate++;
                    s_flowmeter = true;
                    
                }

                break;
            case SEVOFLURANE_INDEX:
                if(value == 1.5f && !s_sevoflurane) {
                    GUIstate++;
                    s_sevoflurane = true;
                }
               
                break;
            case DESFLURANE_INDEX:
                if(value == 3 && !s_desflurane){
                    GUIstate++;
                    s_desflurane = true;
                    break;
                }*/
               
                
            default:
                break;

        }

    }



	
	// Update is called once per frame
	void Update () {
		
		//Assessing current position
		//Only working on horizontal plane (Y not considered)
		followHeadPosition = new Vector2 (followHead.transform.position.x, followHead.transform.position.z);

		//Only checking if close to a GUI teleportPoints when player moves
		if (!(V2Equal(followHeadPosition,prevHeadPosition))){

			if (V2CloserThan (followHeadPosition, teleport2DTransformStart,maxDistance))
            {
				GUIstate = GUI_INTRO;
			} else 
            if (V2CloserThan (followHeadPosition, teleport2DTransformAnesthesiaMachine,maxDistance))
            {
                      /*
				if (prevGUIstate < GUI_ANESTHESIA_0) {
                    if (s_sevoflurane)
                        GUIstate = GUI_ANESTHESIA_3;
                    else if (s_flowmeter)
                        GUIstate = GUI_ANESTHESIA_2;
                    else
                    }*/
                        GUIstate = GUI_ANESTHESIA_0;

                
			} else {
				GUIstate = GUI_NONE;
			}								

			//Preparing next frame
			prevHeadPosition = followHeadPosition;
		}
        
		if (prevGUIstate == GUI_ANESTHESIA_0) {
			if (prevGUIstate < GUI_ANESTHESIA_3) {
				elapsedTime += Time.deltaTime;
				if (elapsedTime > 5f) {
					//GUIstate = GUI_ANESTHESIA_1;
				}
			}
		}

		//GUI State Machine
		if (prevGUIstate != GUIstate) {
			switch (GUIstate) {
			case GUI_INTRO:
				UI_Intro.SetActive (true);
				for (int i = 0; i < UI_Anesthesia_Texts.Length; i++) {
					UI_Anesthesia_Texts [i].enabled = false;
				}
                    
                
                    elapsedTime = 0f;
				break;
			case GUI_ANESTHESIA_0:
				UI_Intro.SetActive (false);
				UI_Anesthesia_Texts [0].enabled = true;
                    SetChildComponentsEnabled(true, "flowmeters");
                    SetChildComponentsEnabled(true, "sevoflurane");
                    SetChildComponentsEnabled(true, "desflurane");
                    

                    elapsedTime = 0f;
				break;
                    /*
			case GUI_ANESTHESIA_1:
				UI_Anesthesia_Texts [0].enabled = false;
				UI_Anesthesia_Texts [1].enabled = true;
                    //SetChildComponentsEnabled(true, "flowmeters");
                    elapsedTime = 0f;
				break;
			case GUI_ANESTHESIA_2:
				UI_Anesthesia_Texts [1].enabled = false;
				UI_Anesthesia_Texts [2].enabled = true;
                   // SetChildComponentsEnabled(false, "flowmeters");
                    //SetChildComponentsEnabled(true, "sevoflurane");
                    elapsedTime = 0f;
				break;
			case GUI_ANESTHESIA_3:
				UI_Anesthesia_Texts [2].enabled = false;
				UI_Anesthesia_Texts [3].enabled = true;
                    SetChildComponentsEnabled(true, "flowmeters");
                    SetChildComponentsEnabled(true, "sevoflurane");
                    SetChildComponentsEnabled(true, "desflurane");
                    elapsedTime = 0f;
				break;*/
                default: break;
			case GUI_NONE:
				UI_Intro.SetActive (false);
				for (int i = 0; i < UI_Anesthesia_Texts.Length; i++) {
					UI_Anesthesia_Texts [i].enabled = false;
				}
				break;
			}
			prevGUIstate = GUIstate;
		}
	}



    void SetChildComponentsEnabled(bool value, string tag)
    {

        var parentObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (var gameObject in parentObjects)
        {

            var components = gameObject.GetComponentsInChildren<Component>();

            foreach (var component in components)
            {
                var collider = component as Collider;
                if (collider) collider.enabled = value;

                //var renderer = component as Renderer;
                //if (renderer) renderer.enabled = value;

                var behaviour = component as Behaviour;
                if (behaviour) behaviour.enabled = value;
            }
        }
    }


    private bool V2Equal(Vector2 a, Vector2 b){
		return Vector2.SqrMagnitude (a - b) < 0.0001f;
	}

	private bool V2CloserThan(Vector2 a, Vector2 b, float distance){
		return Vector2.SqrMagnitude (a - b) < (distance*distance);
	}
}
