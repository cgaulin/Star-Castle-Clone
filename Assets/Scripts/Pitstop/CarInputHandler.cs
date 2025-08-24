using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    //Components
    TopDownCarController topDownCarController;

    //Awake is called when the script instance is being loaded
    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Create Vector2 input
        Vector2 inputVector = Vector2.zero;

        //Store the inputs in the horizontal and vertical axis
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        //Using SetInputFactor() to update the input vectors
        topDownCarController.SetInputFactor(inputVector);
    }
}
