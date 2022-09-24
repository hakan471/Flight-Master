using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	#region SIngleton:Game

	public static Game Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOn2Load(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [SerializeField] Text[] allCoinsUIText;
	public int Coins;

	void Start ()
	{
		Coins = PlayerPrefs.GetInt("money", 20000);
		UpdateAllCoinsUIText ();
	}

	public void UseCoins (int amount)
	{
		Coins -= amount;
		PlayerPrefs.SetInt("money", Coins);
	}

	public bool HasEnoughCoins (int amount)
	{
		return (Coins >= amount);
	}

	public void UpdateAllCoinsUIText ()
	{
		for (int i = 0; i < allCoinsUIText.Length; i++) {
			allCoinsUIText [i].text = Coins.ToString ();
		}
	}

	public void MainScreenOpen()
    {
		SceneManager.LoadScene("MainSceene");
    }
}
