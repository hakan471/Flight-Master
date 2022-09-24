using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCoins : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Gold") || other.gameObject.tag.Contains("Silver"))
        {
            Destroy(other.gameObject);  
        }
    }
}
