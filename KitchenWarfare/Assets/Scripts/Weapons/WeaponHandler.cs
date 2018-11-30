using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    Animator animator;

    [Serializable]
    public class UserSettings
    {
        public Transform rightHand;
        public Transform pistolUnequipSpot;
        public Transform rifleUnequipSpot;
    }
    [SerializeField]
    public UserSettings userSettings;

    [Serializable]
    public class Animations
    {
        public string weaponTypeInt = "WeaponType";
        public string reloadingBool = "isReloading";
        public string aimingBool = "Aiming";
    }
    [SerializeField]
    public Animations animations;

    public Weapon currentWeapon;
    public List<Weapon> weaponsList = new List<Weapon>();
    public int maxWeapon = 2;
    bool aim;
    bool reload;
    int weaponType;
    bool settingWeapon;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (currentWeapon)
        {
            currentWeapon.SetEquip(true);
            currentWeapon.SetOwner(this);
            AddWeapon(currentWeapon);
            currentWeapon.ownerAiming = aim;

            if(currentWeapon.ammunition.magAmmo <= 0)
            {
                Reload();
            }

            if (reload)
            {
                if (settingWeapon)
                {
                    reload = false;
                }
            }
        }

        if (weaponsList.Count > 0)
        {
            for (int i = 0; i< weaponsList.Count; i++)
            {
                if (weaponsList[i] != currentWeapon)
                {
                    weaponsList[i].SetEquip(false);
                    weaponsList[i].SetOwner(this);
                }
            }
        }

        Animate();
	}

    //Sets animations
    private void Animate()
    {
        if (!animator)
        {
            return;
        }

        animator.SetBool(animations.aimingBool, aim);
        animator.SetBool(animations.reloadingBool, reload);
        animator.SetInteger(animations.weaponTypeInt, weaponType);

        if (!currentWeapon)
        {
            weaponType = 0;
            return;
        }

        switch (currentWeapon.weaponsType)
        {
            case Weapon.WeaponType.Pistol:
                weaponType = 1;
                break;
            case Weapon.WeaponType.Rifle:
                weaponType = 2;
                break;
        }
    }

    //Adds weapons to weapons list
    private void AddWeapon(Weapon weapon)
    {
        if (weaponsList.Contains(weapon))
        {
            return;
        }

        weaponsList.Add(weapon);
    }

    //Trigger pulled checker
    public void FingerOnTrigger(bool pulling)
    {
        if (!currentWeapon)
        {
            return;
        }

        currentWeapon.Trigger(pulling && aim && !reload);
    }

    //Reloads weapon
    public void Reload()
    {
        if (reload || !currentWeapon)
        {
            return;
        }

        if (currentWeapon.ammunition.carryingAmmo <= 0 || currentWeapon.ammunition.magAmmo == currentWeapon.ammunition.maxMagAmmo)
        {
            return;
        }

        reload = true;
        StartCoroutine(StopeReload());
    }

    private IEnumerator StopeReload()
    {
        yield return new WaitForSeconds(currentWeapon.wepSettings.reloadSpeed);
        currentWeapon.LoadMagazine();
        reload = false;
    }

    //Sets aim bool
    public void Aim(bool aiming)
    {
        aim = aiming;
    }

    //Drops weapon
    public void DropWeapon()
    {
        if (!currentWeapon)
        {
            return;
        }

        currentWeapon.SetEquip(false);
        currentWeapon.SetOwner(null);
        weaponsList.Remove(currentWeapon);
        currentWeapon = null;
    }

    //Switches weapon
    public void SwitchWeapon()
    {
        if (settingWeapon || weaponsList.Count == 0)
        {
            return;
        }

        if (currentWeapon)
        {
            int currentIndex = weaponsList.IndexOf(currentWeapon);
            int nextIndex = (currentIndex + 1) % weaponsList.Count;

            currentWeapon = weaponsList[nextIndex];
        }
        else
        {
            currentWeapon = weaponsList[0];
        }

        settingWeapon = true;
        StartCoroutine(StopSettingWeapon());
    }

    //Stop weapon swaping
    private IEnumerator StopSettingWeapon()
    {
        yield return new WaitForSeconds(0.7f);
        settingWeapon = false;
    }

    //Animates weapon
    private void OnAnimatorIK(int layerIndex)
    {
        if (!animator)
        {
            return;
        }
        
        if (currentWeapon && currentWeapon.userSettings.leftHandTarget 
            && weaponType == 2 && !reload && !settingWeapon)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            Transform target = currentWeapon.userSettings.leftHandTarget;
            Vector3 targetPos = target.position;
            Quaternion targetRotation = target.rotation;
            animator.SetIKPosition(AvatarIKGoal.LeftHand, targetPos);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, targetRotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }
    }
}