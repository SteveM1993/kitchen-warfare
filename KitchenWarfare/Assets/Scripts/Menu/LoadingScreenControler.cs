//////////////////////////////
///<summary>
///Author: HyunJin, Seol
///Date Created: 11/30/2018
///This class is for the loading screen.
///This class receives next scene and opens it.
/// while this class receives the next scene, this shows progress using loading bar.
///</summary>
//////////////////////////////
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenControler : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider slider;

    //////////////////////////////
    ///<summary> 
    ///Void type method
    ///this method takes the scene index number and calls IEnumerator
    ///</summary>
    ///<param name="sceneIndex">
    ///It needs an integer type parameter
    ///</param>
    //////////////////////////////
    public void LoadGame(int sceneIndex)
    {
        //SceneManager.LoadScene("SampleScene");
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    //////////////////////////////
    ///<summary> 
    ///IEnumerator
    ///This function loads the next scene and excutes it.
    ///</summary>
    ///<param name="sceneIndex">
    ///It needs an integer type parameter which is scene index number.
    ///</param>
    //////////////////////////////
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            yield return null;
        }
    }

}
