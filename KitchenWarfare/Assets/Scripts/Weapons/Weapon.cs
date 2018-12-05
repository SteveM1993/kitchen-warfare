using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour
{
    Collider col;
    Rigidbody rigidBody;
    Animator animator;
    SoundController sound;

    public enum WeaponType
    {
        Pistol, Rifle
    }
    public WeaponType weaponsType;

    [Serializable]
    public class UserSettings
    {
        public Transform leftHandTarget;
        public Vector3 spineRotation;
    }
    [SerializeField]
    public UserSettings userSettings;

    [Serializable]
    public class WeaponSettings
    {
        [Header("-Ammo Options-")]
        public Transform ammoSpawn;
        public float damage = 5.0f;
        public float ammoSpread = 5.0f;
        public float fireRate = 0.2f;
        public LayerMask ammoLayers;
        public float range = 200.0f;

        [Header("-Effects-")]
        public GameObject muzzleFlash;
        public GameObject decal;
        public GameObject shell;
        public GameObject mag;

        [Header("-Other-")]
        public GameObject crosshair;
        public float reloadSpeed = 2.0f;
        public Transform shellEjectSpot;
        public float shellEjectSpeed = 7.5f;
        public Transform clipEjectPos;
        public GameObject magGO;

        [Header("-Positioning-")]
        public Vector3 equipPos;
        public Vector3 equipRotation;
        public Vector3 unequipPos;
        public Vector3 unequipRotation;

        [Header("-Animation-")]
        public bool useAnimation;
        public int fireAnimationLayer;
        public string fireAnimationName;
    }
    [SerializeField]
    public WeaponSettings wepSettings;

    [Serializable]
    public class Ammunition
    {
        public int carryingAmmo;
        public int magAmmo;
        public int maxMagAmmo;
    }
    [SerializeField]
    public Ammunition ammunition;

    [Serializable]
    public class SoundSettings
    {
        public AudioClip[] shotSounds;
        public AudioClip reloadSound;
        [Range(0, 3)] public float pitchMin = 1;
        [Range(0, 3)] public float pitchMax = 1.2f;
        public AudioSource audio;
    }
    [SerializeField]
    public SoundSettings soundSettings;

    public Ray shootRay { protected get; set; }
    public bool ownerAiming { get; set; }

    WeaponHandler owner;
    bool equipped;
    bool trigger;
    bool resettingCartrige;

	// Use this for initialization
	void Start ()
    {
        sound = GameObject.FindGameObjectWithTag("Sound Controller").GetComponent<SoundController>();
        col = GetComponent<Collider>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (wepSettings.crosshair != null)
        {
            wepSettings.crosshair = Instantiate(wepSettings.crosshair);
            CrosshairToggle(false);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (owner)
        {
            DisableEnableComponents(false);

            if (equipped)
            {
                if (owner.userSettings.rightHand)
                {
                    Equip();

                    if (trigger)
                    {
                        Fire(shootRay);
                    }

                    if (ownerAiming)
                    {
                        CrosshairToggle(true);
                    }
                    else
                    {
                        CrosshairToggle(false);
                    }
                }
            }
            else
            {
                Unequip(weaponsType);
                CrosshairToggle(false);
            }
        }
        else
        {
            DisableEnableComponents(true);
            transform.SetParent(null);
            ownerAiming = false;
        }
	}

    //Fires weapon
    private void Fire(Ray ray)
    {
        if (ammunition.magAmmo <= 0 || resettingCartrige || !wepSettings.ammoSpawn)
        {
            return;
        }

        RaycastHit hit;
        Transform aSpawn = wepSettings.ammoSpawn;
        Vector3 aSpawnPoint = aSpawn.position;
        Vector3 aDirection = ray.GetPoint(wepSettings.range) - aSpawnPoint;

        aDirection += (Vector3)UnityEngine.Random.insideUnitCircle * wepSettings.ammoSpread;

        if (Physics.Raycast(aSpawnPoint, aDirection, out hit, wepSettings.range, wepSettings.ammoLayers))
        {
            HitEffects(hit);
        }

        WeaponEffects();

        if (wepSettings.useAnimation)
        {
            animator.Play(wepSettings.fireAnimationName, wepSettings.fireAnimationLayer);
        }

        ammunition.magAmmo--;
        resettingCartrige = true;
        StartCoroutine(LoadNextRound());
    }

    //Loads next round
    private IEnumerator LoadNextRound()
    {
        yield return new WaitForSeconds(wepSettings.fireRate);
        resettingCartrige = false;
    }

    //Effects when something is hit
    private void HitEffects(RaycastHit hit)
    {
        if (hit.collider.gameObject.isStatic)
        {
            if (wepSettings.decal)
            {
                Vector3 hitPoint = hit.point;
                Quaternion lookRotation = Quaternion.LookRotation(hit.normal);
                GameObject decal = Instantiate(wepSettings.decal, hitPoint, lookRotation) as GameObject;
                Transform decalTransform = decal.transform;
                Transform hitTransform = hit.transform;
                decalTransform.SetParent(hitTransform);
                Destroy(decal, UnityEngine.Random.Range(30.0f, 45.0f));
            }
        }
    }

    //Weapon effects
    private void WeaponEffects()
    {
        if (wepSettings.muzzleFlash)
        {
            Vector3 roundSpawnPos = wepSettings.ammoSpawn.position;
            GameObject muzzleFlash = Instantiate(wepSettings.muzzleFlash, roundSpawnPos, Quaternion.identity) as GameObject;
            Transform muzzleT = muzzleFlash.transform;
            muzzleT.SetParent(wepSettings.ammoSpawn);
            Destroy(muzzleFlash, 1.0f);
        }

        if (wepSettings.shell)
        {
            if (wepSettings.shellEjectSpot)
            {
                Vector3 shellEjectPos = wepSettings.shellEjectSpot.position;
                Quaternion shellEjectRot = wepSettings.shellEjectSpot.rotation;
                GameObject shell = Instantiate(wepSettings.shell, shellEjectPos, shellEjectRot) as GameObject;

                if (shell.GetComponent<Rigidbody>())
                {
                    Rigidbody rigB = shell.GetComponent<Rigidbody>();
                    rigB.AddForce(wepSettings.shellEjectSpot.forward * wepSettings.shellEjectSpeed, ForceMode.Impulse);
                }

                Destroy(shell, UnityEngine.Random.Range(30.0f, 45.0f));
            }
        }

        if (sound == null)
        {
            return;
        }

        if (soundSettings.audio != null)
        {
            if (soundSettings.shotSounds.Length > 0)
            {
                sound.InstansiateClip(wepSettings.ammoSpawn.position, soundSettings.shotSounds[UnityEngine.Random.Range(0, soundSettings.shotSounds.Length)],
                    2, true, soundSettings.pitchMin, soundSettings.pitchMax);
            }
        }
    }
    
    //Toggle crosshair
    private void CrosshairToggle(bool enabled)
    {
        if (wepSettings.crosshair != null)
        {
            Vector3 camCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
            wepSettings.crosshair.transform.position = Camera.main.ScreenToWorldPoint(camCenter);
            wepSettings.crosshair.SetActive(enabled);
        }
    }

    //Disables/Enables components
    private void DisableEnableComponents(bool enabled)
    {
        if (!enabled)
        {
            rigidBody.isKinematic = true;
            col.enabled = false;
        }
        else
        {
            rigidBody.isKinematic = false;
            col.enabled = true;
        }
    }

    //Equips object
    private void Equip()
    {
        if (!owner)
        {
            return;
        }
        else if (!owner.userSettings.rightHand)
        {
            return;
        }

        transform.SetParent(owner.userSettings.rightHand);
        transform.localPosition = wepSettings.equipPos;
        Quaternion equipRotation = Quaternion.Euler(wepSettings.equipRotation);
        transform.localRotation = equipRotation;
    }

    //Unequip object and places it to a location
    private void Unequip(WeaponType wepType)
    {
        if (!owner)
        {
            return;
        }

        switch (wepType)
        {
            case WeaponType.Pistol:
                transform.SetParent(owner.userSettings.pistolUnequipSpot);
                break;
            case WeaponType.Rifle:
                transform.SetParent(owner.userSettings.rifleUnequipSpot);
                break;
        }

        transform.localPosition = wepSettings.unequipPos;
        Quaternion unequipRot = Quaternion.Euler(wepSettings.unequipRotation);
        transform.localRotation = unequipRot;
    }

    //Load magazine and calculate ammo
    public void LoadMagazine()
    {
        int ammoNeeded = ammunition.maxMagAmmo - ammunition.magAmmo;

        if (ammoNeeded >= ammunition.carryingAmmo)
        {
            ammunition.magAmmo = ammunition.carryingAmmo;
            ammunition.carryingAmmo = 0;
        }
        else
        {
            ammunition.carryingAmmo -= ammoNeeded;
            ammunition.magAmmo = ammunition.maxMagAmmo;
        }
    }

    //Set weapon equip
    public void SetEquip(bool equip)
    {
        equipped = equip;
    }

    //Triggers the weapon trigger
    public void Trigger(bool isPulling)
    {
        trigger = isPulling;
    }

    //Sets weapon owner
    public void SetOwner(WeaponHandler wpHandler)
    {
        owner = wpHandler;
    }
}