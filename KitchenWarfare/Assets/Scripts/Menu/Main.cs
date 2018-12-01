//////////////////////////////
///<summary>
///Author: HyunJin, Seol
///Date Created: 11/30/2018
///Describe what it does here
///This class is for main menu buttons UI
///</summary>
///<params>
///None
///</params>
//////////////////////////////
/// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    public Button Start;
    public Button Settings;
    public Button EXIT;
    public string Name;
    public GameObject loading;
    public GameObject setting;

    //////////////////////////////
    ///<summary> 
    ///Void type method
    ///this method activates three buttons whenever users click them; Start, Setting, Exit
    ///</summary>
    ///<params>
    ///None
    ///</params>
    //////////////////////////////
    public void Awake()
    {
        Start.onClick.AddListener(NewGame);
        Settings.onClick.AddListener(Set);
        EXIT.onClick.AddListener(ExitGame);
    }

    //////////////////////////////
    ///<summary> 
    ///Void type method
    ///this method activates the Start button.
    ///if users click this button, it will load Loading UI
    ///</summary>
    ///<params>
    ///None
    ///</params>
    //////////////////////////////
    public void NewGame()
    {
        //SceneManager.LoadScene(Name);
        loading.SetActive(true);
    }

    //////////////////////////////
    ///<summary> 
    ///Void type method
    ///this method activates the Setting button.
    ///if users click this button, it will load Setting UI
    ///</summary>
    ///<params>
    ///None
    ///</params>
    //////////////////////////////
    public void Set()
    {
        setting.SetActive(true);
    }

    //////////////////////////////
    ///<summary> 
    ///Void type method
    ///this method activates the Exit button.
    ///if users click this button, it will exit the program
    ///</summary>
    ///<params>
    ///None
    ///</params>
    //////////////////////////////
    public void ExitGame()
    {
        Application.Quit();
    }
}
