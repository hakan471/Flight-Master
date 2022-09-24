using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Tarodev;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerValues: Shop.ShopItem
    {
        public GameObject gameObj;
    }
    #region Variables
    public int selectedPlayerId;
    public static GameObject selectedPlayer;
    public List<PlayerValues> playerValues;
    //[SerializeField] public List<GameObject> players;
    [SerializeField] List<GameObject> clouds, interfaces, airplanes, coins, drones, cityProducts;
    [SerializeField] GameObject startButton, BlinkButton, playerTwoCamera, speedButton, turboButton, fuelButton, speedLvlImage, missle;
    private GameObject player, playerTwo;
    public static string playerName;
    [SerializeField] Camera cam;
    [SerializeField] Slider fuelSlider;
    [SerializeField] float speedCharacterAndCamera;
    [SerializeField] GameObject bomb;
    public Text speedTxt, fuelTxt, moneyTxt;
    public static float xMax, xMin, yMax, yMin;
    private int speedMoney, fuelMoney, money;
    public static bool endFlagMissles = false;
    #endregion
    private void Awake()
    {
        selectedPlayerId = PlayerPrefs.GetInt("selectedPlayerId", 0);
        //get selected player values
        for (int i = 0; i < playerValues.Count; i++)
        {
            if(selectedPlayerId == playerValues[i].id)
            {
                playerName = PlayerPrefs.GetString("playerName", "Balloon");
                PlayerPrefs.SetFloat(playerName + "speedX", PlayerPrefs.GetFloat(playerName + "speedX", playerValues[i].speedX)); //default control
                PlayerPrefs.SetFloat(playerName + "maxValue", PlayerPrefs.GetFloat(playerName + "maxValue", playerValues[i].fuel));

                player = playerValues[i].gameObj;
                player.SetActive(true);
                playerTwo = player.transform.GetChild(0).gameObject;
                break;
            }
        }
        selectedPlayer = player;
        //System.Random random = new System.Random();
        //int count = 0;
        //while (count < 250)
        //{
        //    count++;
        //    var position = new Vector3(UnityEngine.Random.Range(-350, 350), (UnityEngine.Random.Range(player.transform.position.y + 100, 14.3f * fuelSlider.maxValue)), UnityEngine.Random.Range(20, 1400)); //max gideceði yükseklik veriyor
        //    Instantiate(clouds[random.Next(clouds.Count)], position, Quaternion.Euler(0, 90, 0));
        //}
    }
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        fuelSlider.GetComponent<Slider>().enabled = false;
        //canvas
        speedTxt.text = PlayerPrefs.GetInt(playerName + "speedMoney", playerValues[selectedPlayerId].speedXStartPrice).ToString();
        fuelTxt.text = PlayerPrefs.GetInt(playerName + "fuelMoney", playerValues[selectedPlayerId].fuelStartPrice).ToString();
        moneyTxt.text = PlayerPrefs.GetInt("money", 20000).ToString();
        //selected player values variables
        speedMoney = PlayerPrefs.GetInt(playerName + "speedMoney", playerValues[selectedPlayerId].speedXStartPrice);
        fuelMoney = PlayerPrefs.GetInt(playerName + "fuelMoney", playerValues[selectedPlayerId].fuelStartPrice);
        money = PlayerPrefs.GetInt("money", 20000);
        //fuel values
        fuelSlider.minValue = 0;
        fuelSlider.maxValue = PlayerPrefs.GetFloat(playerName + "maxValue", playerValues[selectedPlayerId].fuel);
        fuelSlider.value = PlayerPrefs.GetFloat(playerName + "maxValue", playerValues[selectedPlayerId].fuel);
        //phone values to world point
        Vector3 xVector = cam.ScreenToWorldPoint(new Vector3(player.transform.position.x, player.transform.position.y, cam.transform.position.z - player.transform.position.z));
        yMax = xVector.y - 11;
        yMin = -(xVector.y) + 70;
        xMax = xVector.x-12;
        xMin = -xVector.x+12;
    }
    public void EndFonk()
    {
        player.GetComponent<Move>().enabled = false;
        playerTwo.SetActive(true);
        try
        {
            GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        }
        catch { }
        playerTwo.GetComponent<Rigidbody>().useGravity = true;
    }
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 1);
        #region speedMoneyControl
        if (speedMoney > money)
            speedButton.GetComponent<Button>().interactable = false;
        else
            speedButton.GetComponent<Button>().interactable = true;
        #endregion
        #region fuelMoneyControl
        if (fuelMoney > money)
            fuelButton.GetComponent<Button>().interactable = false;
        else
            fuelButton.GetComponent<Button>().interactable = true;
        #endregion
 
        if((fuelSlider.value == 0 && GameObject.FindGameObjectWithTag("MainCamera")) || endFlagMissles/* && herhangi bir engele çarpmýþsa */)
        {
            EndFonk();
        }
    }
    public void StartButton()
    {
        startButton.SetActive(false);
        BlinkButton.SetActive(true);
        for (int i = 0; i < interfaces.Count; i++)
        {
            interfaces[i].SetActive(false);
        }
        StartCoroutine(MoveEnum());
        //StartCoroutine(CloudCreator());
        //StartCoroutine(CloudCreatorTwo());
        StartCoroutine(CreateAirplane());
        StartCoroutine(FuelControl());
        StartCoroutine(CreateCoin());
        //StartCoroutine(CreateDrone()); tekrardan posizyonlarýný gözden geçir
        StartCoroutine(CreateMissles());
    }
    #region Fuel
    IEnumerator FuelControl()
    {
        float fuelDiscount = fuelSlider.maxValue + 2;
        while (fuelDiscount > 0)
        {
            fuelDiscount = fuelSlider.value--;
            yield return new WaitForSeconds(1);
        }
    }
    #endregion
    #region Missles
    IEnumerator CreateMissles()
    {
        System.Random random = new System.Random();
        while (true)
        {
            missle.GetComponent<FollowTarget>()._target = player.GetComponent<Target>();
            int rand = random.Next(0, 2);
            var position = new Vector3(rand == 1 ? xMin-30 : xMax+30, player.transform.position.y + random.Next(0, 100), player.transform.position.z - random.Next(-50, 50));
            GameObject prefmissle = Instantiate(missle, position, rand == 1 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0)); 

            prefmissle.GetComponent<FollowTarget>()._speed += (PlayerPrefs.GetFloat(playerName + "artisMissles", 0) * 100);

            Debug.Log("Missles time - " + (3 - PlayerPrefs.GetFloat(playerName + "artisMissles", 0))+"\n"+"Missle Speed"+ prefmissle.GetComponent<FollowTarget>()._speed);
            yield return new WaitForSeconds(3 - PlayerPrefs.GetFloat(playerName + "artisMissles", 0));
        }
    }
    #endregion 
    #region Drones
    IEnumerator CreateDrone()
    {
        System.Random random = new System.Random();
        while (true)
        {
            int rand = random.Next(0, 2);
            var position = new Vector3(rand == 1 ? -70f : 70f, player.transform.position.y + 100, player.transform.position.z);
            StartCoroutine(MoveDron(Instantiate(drones[random.Next(drones.Count)], position, rand == 1 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0))));
            yield return new WaitForSeconds(3);
        }
    }
    IEnumerator MoveDron(GameObject airplane)
    {
        GameObject _airplane = airplane;
        float value = _airplane.transform.position.x < 0 ? speedCharacterAndCamera : -speedCharacterAndCamera;
        while (true)
        {
            _airplane.transform.position = new Vector3(_airplane.transform.position.x + value * Time.deltaTime, _airplane.transform.position.y, _airplane.transform.position.z);
            yield return new WaitForSeconds(0.01f);
        }
    }
    #endregion
    #region Airplane
    IEnumerator CreateAirplane()
    {
        System.Random random = new System.Random();
        while (true)
        {
            int rand = random.Next(0, 2);
            var position = new Vector3(rand == 1 ? -350f : 300f, playerTwo.transform.position.y-200, playerTwo.transform.position.z + random.Next(25, 1400));
            StartCoroutine(MoveAirPlane(Instantiate(airplanes[random.Next(airplanes.Count)], position, rand == 1 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0))));
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator MoveAirPlane(GameObject airplane)
    {
        GameObject _airplane = airplane;
        float value = _airplane.transform.position.x < 0 ? 25f : -25f;
        while (true)
        {
            _airplane.transform.position = new Vector3(_airplane.transform.position.x + value * Time.deltaTime, _airplane.transform.position.y, _airplane.transform.position.z);
            yield return new WaitForSeconds(0.01f);
        }
    }
    #endregion
    #region Coin
    IEnumerator CreateCoin()
    {
        System.Random random = new System.Random();
        while (true)
        {
            int rand = random.Next(coins.Count);
            float x = UnityEngine.Random.Range(xMin, xMax);
            for (int i = 5; i < 30; i += 5)
            {
                var position = new Vector3(x, player.transform.position.y + 100 + i, player.transform.position.z);
                Instantiate(coins[rand], position, Quaternion.Euler(90, 0, 0));
            }
            yield return new WaitForSeconds(3f);
        }
    }
    #endregion
    #region CameraAndCharakterMove
    IEnumerator MoveEnum()
    {
        while (true)
        {
            Vector3 desiredPosition = player.transform.position + new Vector3(0, speedCharacterAndCamera * Time.deltaTime, 0);
            //Vector3 smoothedPosition = Vector3.Lerp(player.transform.position, desiredPosition, 0.125f);
            player.transform.position = desiredPosition;
            desiredPosition = cam.transform.position + new Vector3(0, speedCharacterAndCamera * Time.deltaTime, 0);
            yMin += speedCharacterAndCamera * Time.deltaTime;
            yMax += speedCharacterAndCamera * Time.deltaTime; // burada aralýðý korumak için deðerleri arttýrýyorum.
            //smoothedPosition = Vector3.Lerp(cam.transform.position, desiredPosition, 0.125f);
            Camera.main.transform.position = desiredPosition;
            yield return null;
        }
    }
    #endregion

    public void SpeedButton()
    {
        if (PlayerPrefs.GetFloat(playerName + "artisMissles", 0) <= 2.4f)
        {
            money -= speedMoney;
            speedMoney += 5;
            PlayerPrefs.SetInt("money", money);
            PlayerPrefs.SetInt(playerName + "speedMoney", speedMoney);
            moneyTxt.text = money.ToString();
            speedTxt.text = speedMoney.ToString();
            Move.speedX += 2f;

            PlayerPrefs.SetFloat(playerName + "speedX", Move.speedX);
            Debug.Log(Move.speedX);
            PlayerPrefs.SetFloat(playerName + "artisMissles", (float)((PlayerPrefs.GetFloat(playerName + "speedX") / 20) - 1));
        }
        else
        {
            PlayerPrefs.SetFloat(playerName + "artisMissles", 2.41f);
            
            //max player speed (speeX + 22 * 2)
        }
    }
    public void TurboButton()
    {
        //
    }
    public void FuelButton()
    {
        money -= fuelMoney;
        fuelMoney += 5;
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt(playerName + "fuelMoney", fuelMoney);
        moneyTxt.text = money.ToString();
        fuelTxt.text = fuelMoney.ToString();
        fuelSlider.maxValue += 5;
        fuelSlider.value += 5;
        PlayerPrefs.SetFloat(playerName + "maxValue", fuelSlider.maxValue);
    }

    public void ShopSceene()
    {
        SceneManager.LoadScene("scene");
    }
}
