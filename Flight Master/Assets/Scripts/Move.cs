using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private Touch touch;
    public static float speedX;
    public Text moneyTxt;
    public GameObject targetCoin;
    public static List<GameObject> coins = new List<GameObject>();
    public FixedJoystick joystick;
    int score;
    private GameController gameController;

    float horizontal;
    float vertical;
    Vector3 vec;
    float angle;
    void Start()
    {
        angle = -35f;
        rb = GetComponent<Rigidbody>();
        gameController = GameObject.FindGameObjectWithTag("gameController").GetComponent<GameController>();
        
        speedX = PlayerPrefs.GetFloat(GameController.playerName + "speedX", gameController.playerValues[gameController.selectedPlayerId].speedX);
        
        coins.Add(targetCoin);   
    }
    void FixedUpdate()
    {
        //Debug.Log(speedX);
        score = PlayerPrefs.GetInt("money", 0);
        CharacterControl();
        //s
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Gold") || other.gameObject.tag.Contains("Silver"))/* && other.gameObject.transform.position.y > transform.position.y*/)
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            Debug.Log(other.gameObject.name);
            score++;
            PlayerPrefs.SetInt("money", score);
            moneyTxt.text = score.ToString();
            StartCoroutine(CoinLerp(other.gameObject, coins[coins.Count-1]));
            coins.Add(other.gameObject);
        }
    }
    private void CharacterControl()
    {
        transform.position = new Vector3(transform.position.x + joystick.Horizontal * speedX * Time.deltaTime,
            transform.position.y + joystick.Vertical * speedX * Time.deltaTime,
            transform.position.z
            );

        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;
        vec = new Vector3(horizontal, 0, vertical);
        rb.velocity = vec * speedX * Time.deltaTime;

        rb.rotation = Quaternion.Euler(transform.name == "Player" ? 0 : -90, 0, rb.velocity.x * angle);

        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, GameController.xMin, GameController.xMax),
        Mathf.Clamp(transform.position.y, GameController.yMin, GameController.yMax),
        Mathf.Clamp(transform.position.z, 0,0)
        );
    }
   
    IEnumerator CoinLerp(GameObject coin, GameObject _targetCoin)
    {
        float _height = -0.5f;
        while (coin != null)
        {
            coin.transform.position = Vector3.Lerp(coin.transform.position, new Vector3(_targetCoin.transform.position.x, _targetCoin.transform.position.y + _height, _targetCoin.transform.position.z), 20 * Time.deltaTime);
            yield return coin;
        }
    }
}
