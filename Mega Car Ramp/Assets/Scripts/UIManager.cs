using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region USER_CLASS
    [System.Serializable]
    public class Panels
    {
        [SerializeField] internal GameObject Subscription;
        [SerializeField] internal GameObject MainMenu;
        [SerializeField] internal GameObject LevelSelection;
        [SerializeField] internal GameObject DailyRewards;
        [SerializeField] internal GameObject CarSelection;
        [SerializeField] internal GameObject Unlock_4_Cars;
        [SerializeField] internal GameObject Unlock_All_Cars;
        [SerializeField] internal GameObject Unlock_Buy_FullGame;
        [SerializeField] internal GameObject Settings;
    }

    #endregion

    #region REFERENCES

    [SerializeField] internal Panels Screens;
    private float x = 0;
    [SerializeField] float cameraRotateSpeed;
    [SerializeField] Transform vehicleRoot;

    #endregion

    private void Start()
    {
        //Screens.MainMenu.SetActive(true);
    }

    public void play_Button()
    {
        Screens.MainMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            x = Mathf.Lerp(x, Mathf.Clamp(Input.GetAxis("Mouse X"), -2, 2) * cameraRotateSpeed, Time.deltaTime * 5.0f);
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 50, 60);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 50, Time.deltaTime);
        }
        else
        {
            x = Mathf.Lerp(x, cameraRotateSpeed * 0.01f, Time.deltaTime * 5.0f);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime);
        }

        transform.RotateAround(vehicleRoot.position, Vector3.up, x);
    }

}
