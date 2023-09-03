using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    private Quaternion originLocalRotattion;
    //Quaternion son variables que guardan la rotacion de x,y,z

    // Start is called before the first frame update
    void Start()
    {
        originLocalRotattion = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        float t_xLookInput = Input.GetAxis("Mouse X");
        float t_yLookInput = Input.GetAxis("Mouse Y");
        //Calculate the wepon rotation
        Quaternion t_xAngleAdjustment = Quaternion.AngleAxis(-t_xLookInput * 1.45f, Vector3.up);
        Quaternion t_yAngleAdjustment = Quaternion.AngleAxis(t_yLookInput * 1.45f,Vector3.right);
        // si quiero que swea mas brusca pongo mas de 1.45.......
        Quaternion t_targetRotation = originLocalRotattion * t_xAngleAdjustment * t_yAngleAdjustment;
        // rotacion al target de pocision
        transform.localRotation = Quaternion.Lerp( transform.localRotation, t_targetRotation, Time.deltaTime*10f);
        // Quaternion.Lerp pasa de una rotacion especifica a otra en un tiempo determinado, por eso el 10f
    }
}
