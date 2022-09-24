using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveFall : MonoBehaviour
{
    private Touch touch;
    public float speed;
    public Text moneyTxt;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Camera playerTwoCamera;
    void Update()
    {
        transform.position = new Vector3(
            transform.position.x + joystick.Horizontal * speed * Time.deltaTime,
            transform.position.y,
            transform.position.z + /*joystick.Vertical **/ speed * Time.deltaTime
        );
        //----------------------------------------------------------------------

        transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -120, 120),
                transform.position.y,
                transform.position.z
        );


        //-----------------------s-----------------------------------------------

        //bug olup karakter aþþaðý düþmeye devam ederse diye
        if (transform.position.y < -2)
        {
            PlayerPrefs.SetInt("money", System.Convert.ToInt32(moneyTxt.text));
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameController.endFlagMissles = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //yere veya herhangi bir çatýya çarptýðýnda video izlet 30 saniye istemezse 3 saniyelik video.222
        if(other.gameObject.tag.Contains("x") || other.gameObject.tag.Contains("1xFinish"))
        {
             PlayerPrefs.SetInt("money", System.Convert.ToInt32(moneyTxt.text));
             SceneManager.LoadScene(SceneManager.GetActiveScene().name);
             GameController.endFlagMissles = false;
        }
    }
}
