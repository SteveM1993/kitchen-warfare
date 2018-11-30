using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{

    CharacterMovement characterMove;
    WeaponHandler weaponHandler;

    [Serializable]
    public class InputSettings
    {
        public string verticalAxis = "Vertical";
        public string horizontalAxis = "Horizontal";
        public string jump = "Jump";
        public string reloadButton = "Reload";
        public string aimButton = "Fire2";
        public string fireButton = "Fire1";
        public string dropWeapon = "DropWeapon";
        public string switchWeapon = "SwitchWeapon";
    }
    [SerializeField]
    InputSettings input;

    [Serializable]
    public class OtherSettings
    {
        public float lookSpeed = 5.0f;
        public float lookDistance = 10.0f;
        public bool requireInputForTurn = true;
        public LayerMask aimDetectionLayers;
    }
    [SerializeField]
    public OtherSettings other;

    public bool debugAim;
    public Transform spine;
    private bool aiming;
    Camera mainCam;

	// Use this for initialization
	void Start ()
    {
        characterMove = GetComponent<CharacterMovement>();
        mainCam = Camera.main;
        weaponHandler = GetComponent<WeaponHandler>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CharacterMoveHandler();
        CameraLookHandler();
        WeaponManage();
    }

    private void LateUpdate()
    {
        if (weaponHandler)
        {
            if (weaponHandler.currentWeapon)
            {
                if (aiming)
                {
                    PositionSpine();
                }
            }
        }
    }

    //Character movement
    private void CharacterMoveHandler()
    {
        if (!characterMove)
        {
            return;
        }

        characterMove.Animate(Input.GetAxis(input.verticalAxis), Input.GetAxis(input.horizontalAxis));

        if (Input.GetButtonDown(input.jump))
        {
            characterMove.Jump();
        }
        
    }

    //Camera look handler
    private void CameraLookHandler()
    {
        if (!mainCam)
        {
            return;
        }
        
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

    //Weapon Handler
    private void WeaponManage()
    {
        if (!weaponHandler)
        {
            return;
        }

        aiming = Input.GetButton(input.aimButton) || debugAim;

        if (weaponHandler.currentWeapon)
        {
            weaponHandler.Aim(aiming);

            other.requireInputForTurn = !aiming;

            weaponHandler.FingerOnTrigger(Input.GetButton(input.fireButton));

            if (Input.GetButtonDown(input.reloadButton))
            {
                weaponHandler.Reload();
            }

            if (Input.GetButtonDown(input.dropWeapon))
            {
                weaponHandler.DropWeapon();
            }

            if (Input.GetButtonDown(input.switchWeapon))
            {
                weaponHandler.SwitchWeapon();
            }
        }
    }

    //Positions the spine
    private void PositionSpine()
    {
        if (!spine || !weaponHandler.currentWeapon || !mainCam)
        {
            return;
        }

        Transform mainCamT = mainCam.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 direction = mainCamT.forward;
        Ray ray = new Ray(mainCamPos, direction);
        spine.LookAt(ray.GetPoint(50));
        

        Vector3 eulerAngleOffset = weaponHandler.currentWeapon.userSettings.spineRotation;
        spine.Rotate(eulerAngleOffset);
    }

    //Character looks at forward point from camera
    private void CharacterLook()
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
