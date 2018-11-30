using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    Weapon weapon;
	
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        weapon = (Weapon)target;
        EditorGUILayout.LabelField("Weapon Helpers");

        if (GUILayout.Button("Save Gun Equip Location"))
        {
            Transform weaponT = weapon.transform;
            Vector3 weaponPos = weaponT.localPosition;
            Vector3 weaponRotation = weaponT.localEulerAngles;
            weapon.wepSettings.equipPos = weaponPos;
            weapon.wepSettings.equipRotation = weaponRotation;
        }

        if (GUILayout.Button("Save Gun Unequip Location"))
        {
            Transform weaponT = weapon.transform;
            Vector3 weaponPos = weaponT.localPosition;
            Vector3 weaponRotation = weaponT.localEulerAngles;
            weapon.wepSettings.unequipPos = weaponPos;
            weapon.wepSettings.unequipRotation = weaponRotation;
        }

        EditorGUILayout.LabelField("Debug Positioning");

        if (GUILayout.Button("Move Gun To Equip Location"))
        {
            Transform weaponT = weapon.transform;
            weaponT.localPosition = weapon.wepSettings.equipPos;
            Quaternion eulerAngles = Quaternion.Euler(weapon.wepSettings.equipRotation);
            weaponT.localRotation = eulerAngles;
        }

        if (GUILayout.Button("Move Gun To Unequip Location"))
        {
            Transform weaponT = weapon.transform;
            weaponT.localPosition = weapon.wepSettings.unequipPos;
            Quaternion eulerAngles = Quaternion.Euler(weapon.wepSettings.unequipRotation);
            weaponT.localRotation = eulerAngles;
        }
    }
}
