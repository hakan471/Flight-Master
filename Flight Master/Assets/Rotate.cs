using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float angel = 5;
    void Update()
    {
         angel+=1;
         transform.rotation = Quaternion.Euler(90, angel, 0);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("1123");
    //        Destroy(gameObject);
    //    }
    //}
}
