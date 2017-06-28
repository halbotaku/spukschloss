using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitHandler : MonoBehaviour {

    public bool isSpecialGuest = false;
    public GameObject portraits;

    public void Start()
    {
        if (isSpecialGuest)
        {
            portraits.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            portraits.transform.GetChild(0).gameObject.SetActive(true);
        }
    }


        public void destroy()
        {
            GameObject.Destroy(portraits);
        }
}