using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


    
public class Menu : MonoBehaviour
{  
    public enum PlaneType {
        Plane,
        Pyramid,
        Tree
    };
    [SerializeField] public GameObject Page1;
    [SerializeField] public GameObject Page2;
    public static PlaneType plane = PlaneType.Plane;
    public Transform PlanePreviews;
    public void Start() {
        string currScene = SceneManager.GetActiveScene().name;
        if(currScene == "StartScene") {
            PlanePreviews = GameObject.Find("PlanePreview").transform;
            ChoosePlane();
        }
    }

    public void StartGame() {
        SceneManager.LoadScene("Game");
        Debug.Log("The game has started.");
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("StartScene");
    }

    public void LoadPlaneSelect() {
        SceneManager.LoadScene("ChoosePlane");
    }

    public void ChoosePlane() {
        foreach(Transform plane in PlanePreviews) {
            plane.gameObject.SetActive(false);
        }
        PlanePreviews.Find(plane.ToString()).gameObject.SetActive(true);
    }

    public void Choose() {
        Menu.plane = Menu.PlaneType.Plane;
    }
    public void ChoosePyramid() {
        Menu.plane = Menu.PlaneType.Pyramid;
    }
    public void ChooseTree() {
        Menu.plane = Menu.PlaneType.Tree;
    }


    public void LoadInstructions() {
        SceneManager.LoadScene("Instruction");
    }

    public void NextInstructionPage() {
        Page1.SetActive(false);
        Page2.SetActive(true);
        GameObject.Find("NextButton").SetActive(false);
        GameObject.Find("ReturnButton").SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
