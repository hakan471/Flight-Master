using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkilsShop : MonoBehaviour
{
	#region Singlton:Shop

	public static SkilsShop Instance;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	#endregion

	[System.Serializable]
	public class SkilssShopItem
	{
		public int id;
		public string name;
		public Sprite Image;
		public int Price;
		public bool IsPurchased = false;
	}

	public List<SkilssShopItem> ShopItemsList;
	[SerializeField] Animator NoCoinsAnim;

	[SerializeField] GameObject ItemTemplate;
	GameObject g;
	[SerializeField] Transform ShopScrollView;
	[SerializeField] GameObject ShopPanel;
	Button buyBtn;

	void Start()
	{
		//PlayerPrefs.DeleteAll();
		foreach (var item in ShopItemsList)
		{
			if (PlayerPrefs.GetInt("IsPurchasedSkils" + item.id, 0) == 1)
			{
				ProfileSkils.Instance.AddAvatar(item.Image);
				item.IsPurchased = true;
			}
		}
		int len = ShopItemsList.Count;
		for (int i = 0; i < len; i++)
		{
			g = Instantiate(ItemTemplate, ShopScrollView);
			g.transform.GetChild(0).GetComponent<Image>().sprite = ShopItemsList[i].Image;
			g.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = ShopItemsList[i].Price.ToString();
			buyBtn = g.transform.GetChild(2).GetComponent<Button>();
			if (ShopItemsList[i].IsPurchased)
			{
				DisableBuyButton();
			}
			buyBtn.AddEventListener(i, OnShopItemBtnClicked);
		}
	}

	void OnShopItemBtnClicked(int itemIndex)
	{
		if (Game.Instance.HasEnoughCoins(ShopItemsList[itemIndex].Price))
		{
			Game.Instance.UseCoins(ShopItemsList[itemIndex].Price);
			//purchase Item
			ShopItemsList[itemIndex].IsPurchased = true;
			PlayerPrefs.SetInt("IsPurchasedSkils" + ShopItemsList[itemIndex].id, 1);

			Debug.Log("IsPurchasedSkils" + ShopItemsList[itemIndex].id);
			//disable the button
			buyBtn = ShopScrollView.GetChild(itemIndex).GetChild(2).GetComponent<Button>();
			DisableBuyButton();
			//change UI text: coins
			Game.Instance.UpdateAllCoinsUIText();

			//add avatar
			ProfileSkils.Instance.AddAvatar(ShopItemsList[itemIndex].Image);
		}
		else
		{
			NoCoinsAnim.SetTrigger("NoCoins");
			Debug.Log("You don't have enough coins!!");
		}
	}

	void DisableBuyButton()
	{
		buyBtn.interactable = false;
		buyBtn.transform.GetChild(0).GetComponent<Text>().text = "PURCHASED";
	}
	/*-------------2--------Open & Close shop--------------------------*/
	public void OpenShop()
	{
		ShopPanel.SetActive(true);
	}

	public void CloseShop()
	{
		ShopPanel.SetActive(false);
	}

}
