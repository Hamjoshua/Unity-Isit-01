using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public Slider RotationYSlider;
    bool _cubeInitRotation = false;
    void Start()
    {

    }

    public void InitRotationOfSelectedCube()
    {
        RotationYSlider.gameObject.SetActive(true);
        RotationYSlider.value = GameManager.SingleInstance.SelectedCube.transform.eulerAngles.y;
    }

    public void HideSlider()
    {
        RotationYSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (GameManager.SingleInstance.SelectedCube != null)
        {
            RotationYSlider.value += (Input.mouseScrollDelta.y * 5);
            float yRotation = RotationYSlider.value;
            GameManager.SingleInstance.SelectedCube.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeCursorMode(Dropdown change)
    {
        GameManager.SingleInstance.ChangeCursorMode(change.value);
    }
}
