using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimeDisolving : MonoBehaviour
{
    public Material characterMaterial;
    public bool shouldDisolve = false;
    public float currentTime_ = 0.0f;
    public float endTime_ = 2.0f;
    bool reset = false;

    void Start()
    {
        characterMaterial = GetComponentInChildren<Renderer>().material;
    }

    void Update()
    {
        if (shouldDisolve)
        {
            reset = true;
            currentTime_ += Time.deltaTime;
            characterMaterial.SetFloat("_DisolveValue", Mathf.Lerp(0.0f, 1.0f, currentTime_ / endTime_));

            if(currentTime_ >= endTime_)
            {
                shouldDisolve = false;
            }
        }
        else if (reset)
        {
            reset = false;
            currentTime_ = 0.0f;
            characterMaterial.SetFloat("_DisolveValue", 0.0f);
        }
    }
}
