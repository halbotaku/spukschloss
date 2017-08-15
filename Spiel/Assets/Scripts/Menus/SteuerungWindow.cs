using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SteuerungWindow : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("pickUp"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
