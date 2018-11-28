using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {

    CharacterMovement characterMove;

    [Serializable]
    public class InputSettings
    {
        public string verticalAxis = "Vertical";
        public string horizontalAxis = "Horizontal";
        public string jump = "Jump";
    }
    [SerializeField]
    InputSettings input;

    [Serializable]
    public class OtherSettings
    {
        public float lookSpeed = 5.0f;
        public float lookDistance = 10.0f;
        public bool requireInputForTurn = true;
    }
    [SerializeField]
    public OtherSettings other;

    Camera mainCam;

	// Use this for initialization
	void Start ()
    {
        characterMove = GetComponent<CharacterMovement>();
        mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (characterMove)
        {
            characterMove.Animate(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

            if (Input.GetButtonDown("Jump"))
            {
                characterMove.Jump();
            }
        }
        
        if (mainCam)
        {
            if (other.requireInputForTurn)
            {
                if (Input.GetAxis(input.horizontalAxis) != 0 || Input.GetAxis(input.verticalAxis) != 0)
                {
                    CharacterLook();
                }
            }
            else
            {
                CharacterLook();
            }
        }
    }

    //Character looks at forward point from camera
    void CharacterLook()
    {
        Transform mainCamT = mainCam.transform;
        Transform pivotTransform = mainCamT.parent;
        Vector3 pivotPos = pivotTransform.position;
        Vector3 lookTarget = pivotPos + (pivotTransform.forward * other.lookDistance);
        Vector3 charPos = transform.position;
        Vector3 lookDirection = lookTarget - charPos;
        Quaternion charRotation = Quaternion.LookRotation(lookDirection);
        charRotation.x = 0;
        charRotation.z = 0;

        Quaternion newRotation = Quaternion.Lerp(transform.rotation, charRotation, Time.deltaTime * other.lookSpeed);
        transform.rotation = newRotation;
    }
}
