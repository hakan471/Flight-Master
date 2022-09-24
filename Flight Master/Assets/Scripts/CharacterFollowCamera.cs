using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFollowCamera : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        //Vector3 desiredPosition = transform.position + new Vector3(0, Move.speedCharacterAndCamera,0);
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.125f);
        //transform.position = smoothedPosition;
    }

}
