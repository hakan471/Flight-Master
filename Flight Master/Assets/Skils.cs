using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skils : MonoBehaviour
{
    [System.Serializable]
    public class SkilManager : SkilsShop.SkilssShopItem
    {
        public float skillReloadTime;
        public GameObject skilObje;
    }
    public List<SkilManager> skilManagers;
    public FixedJoystick joystick;
    [SerializeField] private Button blinkButton;
    [SerializeField] private Text skilButtonText;
    [SerializeField] private Slider blinkReloadSlider;
    public GameObject circle;
    float blinkTimer = 0;
    bool blinkFlag = false;
    GameObject activePlayer, selectedSkil;
    int selectedSkilsId, skilsCount;
    private void Awake()
    {
        selectedSkilsId = PlayerPrefs.GetInt("selectedSkilsId", -1);

        skilsCount = skilManagers.Count;
        for (int i = 0; i < skilsCount; i++)
        {
            if(selectedSkilsId == skilManagers[i].id)
            {
                skilButtonText.text = skilManagers[i].name;
                blinkReloadSlider.minValue = 0;
                blinkReloadSlider.value = 0;
                blinkReloadSlider.maxValue = skilManagers[i].skillReloadTime;
                selectedSkil = skilManagers[i].skilObje;
                break;
            }
        }
    }
    void Start()
    {
        activePlayer = GameController.selectedPlayer;
    }

    // Update is called once per frames
    void FixedUpdate()
    {
        if (blinkFlag)
        {
            blinkTimer += Time.deltaTime;
            blinkReloadSlider.value += Time.deltaTime;

            if (blinkTimer >= blinkReloadSlider.maxValue)
            {
                blinkFlag = false;
                blinkTimer = 0;
                blinkButton.enabled = true;
                blinkReloadSlider.value = 0;
            }
        }
    }
    public void SelectedSkillStart()
    {
        switch (selectedSkilsId)
        {
            case 0:
                blinkFlag = true;
                StartCoroutine(MegaBlast());
                blinkButton.enabled = false;
                break;
            case 1:
                Instantiate(selectedSkil, GameController.selectedPlayer.transform.position, Quaternion.identity);
                blinkFlag = true;
                float blinkValue = 500;
                GameController.selectedPlayer.transform.position = new Vector3(GameController.selectedPlayer.transform.position.x + joystick.Horizontal * blinkValue * Time.deltaTime,
                    GameController.selectedPlayer.transform.position.y + joystick.Vertical * blinkValue * Time.deltaTime,
                    GameController.selectedPlayer.transform.position.z
                    );
                blinkButton.enabled = false;
                break;
            default:
                break;
        }
    }

    IEnumerator MegaBlast()
    {
        float time=0.0f;
        GameObject circle2 = Instantiate(selectedSkil, activePlayer.transform.position, Quaternion.Euler(0,0,0));

        while (time<=blinkReloadSlider.maxValue/2)
        {
            circle2.transform.position = activePlayer.transform.position;
            time += 5 * Time.deltaTime;
            Debug.Log(time);
            circle2.transform.localScale = new Vector3(circle2.transform.localScale.x + 20 * Time.deltaTime, circle2.transform.localScale.y + 20 * Time.deltaTime, circle2.transform.localScale.z + 20 * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(circle2);
    }
}
