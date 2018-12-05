//////////////////////////////
///<summary>
///Author: HyunJin, Seol
///Date Created: December 4, 2018
///This class is for pause.
///If users want to quit or go back to main menu,
///they should press ESC on keyboard.
///If they press the ESC, the pause menu is opened. 
///user can click some buttons what they want on the pause menu.
///</summary>
//////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    //////////////////////////////
    ///<summary> 
    ///Void type method
    ///If users press ESC key, open the pause menu
    ///</summary>
    ///<returns>
    ///None
    ///</returns>
    //////////////////////////////
    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //////////////////////////////
    ///<summary> 
    ///Void Type method. 
    ///If users press the ESC button, stop playing the game.
    ///</summary>
    ///<returns>
    ///None
    ///</returns>
    //////////////////////////////
    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    //////////////////////////////
    ///<summary> 
    ///Void Type method. 
    ///if users click the resume button, resume the game.
    ///</summary>
    ///<returns>
    ///None
    ///</returns>
    //////////////////////////////
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    //////////////////////////////
    ///<summary> 
    ///Void Type method. 
    ///if users click the main menu button, go back to main menu.
    ///</summary>
    ///<returns>
    ///None
    ///</returns>
    //////////////////////////////
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    //////////////////////////////
    ///<summary> 
    //////Void Type method. 
    ///if users click the quit button, quit the game.
    ///</summary>
    ///<returns>
    ///None
    ///</returns>
    //////////////////////////////
    public void QuitGame()
    {
        Application.Quit();
    }
}
