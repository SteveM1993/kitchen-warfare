using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME_MANAGER;

    private UserInput player { get { return FindObjectOfType<UserInput>(); } set { player = value; } }
    private WeaponHandler weaponHandler { get { return player.GetComponent<WeaponHandler>(); } set { weaponHandler = value; } }
    private PlayerUI playerUI { get { return FindObjectOfType<PlayerUI>(); } set { playerUI = value; } }

    //Singleton game manager
    private void Awake()
    {
        if (GAME_MANAGER == null)
        {
            GAME_MANAGER = this;
        }
        else if (GAME_MANAGER != this)
        {
            Destroy(gameObject);
        }
    }

    //Per frame
    private void Update()
    {
        UpdateUI();
    }

    //Updates the UI
    private void UpdateUI()
    {
        if (player)
        {
            if (playerUI)
            {
                if (weaponHandler)
                {
                    if (playerUI.ammoCount)
                    {
                        if (weaponHandler.currentWeapon == null)
                        {
                            playerUI.ammoCount.text = "Unarmed";
                        }
                        else
                        {
                            playerUI.ammoCount.text = weaponHandler.currentWeapon.ammunition.magAmmo + "/" + weaponHandler.currentWeapon.ammunition.carryingAmmo;
                        }
                    }
                }
            }
        }
    }
}
