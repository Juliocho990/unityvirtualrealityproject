using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorBlendShape : MonoBehaviour {


    public SkinnedMeshRenderer blendShapeRenderer;

    private bool increasing = true;
	private float currentWeghtValue = 0;

	void Start () {
        StartCoroutine(OscilationBlenShapeCoroutine(16));
	}


    public IEnumerator OscilationBlenShapeCoroutine(int samplesPerMinute) {
        float elapsedTime = 0f;

		float period = 60f / samplesPerMinute;

        while (Application.isPlaying) {

			while (elapsedTime < period/2f) {
                if (increasing) {
                    if (elapsedTime % .05f < 0.03f) {
						currentWeghtValue = Mathf.Clamp (elapsedTime/(period/2f) * 100f, 0f, 100f);
						blendShapeRenderer.SetBlendShapeWeight(0, currentWeghtValue);
                    }
                }
                else {
                    if (elapsedTime % .05f < 0.03f)
                    {
						blendShapeRenderer.SetBlendShapeWeight(0, 100f - Mathf.Clamp(elapsedTime/(period/2f) * 100f, 0f, 100f));
					
                    }
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;
            increasing = !increasing;
            yield return null;

        }

    }
	
	
}
