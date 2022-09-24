using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProfileSkils : MonoBehaviour
{
	#region Singlton:ProfileSkils

	public static ProfileSkils Instance;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	#endregion

	public class Avatar2
	{
		public Sprite Image;
	}

	public List<Avatar2> AvatarsList2;

	[SerializeField] GameObject AvatarUITemplate2;
	[SerializeField] Transform AvatarsScrollView2;

	GameObject g;
	int newSelectedIndex, previousSelectedIndex;

	[SerializeField] Color ActiveAvatarColor;
	[SerializeField] Color DefaultAvatarColor;

	[SerializeField] Image CurrentAvatar;


	void Start()
	{

		GetAvailableAvatars();
		newSelectedIndex = previousSelectedIndex = 0;
	}

	void GetAvailableAvatars()
	{
		for (int i = 0; i < SkilsShop.Instance.ShopItemsList.Count; i++)
		{
			if (SkilsShop.Instance.ShopItemsList[i].IsPurchased)
			{
				//add all purchased avatars to AvatarsList
				AddAvatar(SkilsShop.Instance.ShopItemsList[i].Image);
			}
		}
		SelectAvatar(newSelectedIndex);
	}

	public void AddAvatar(Sprite img)
	{
		if (AvatarsList2 == null)
			AvatarsList2 = new List<Avatar2>();

		Avatar2 av = new Avatar2() { Image = img };
		//add av to AvatarsList
		AvatarsList2.Add(av);

		//add avatar in the UI scroll view
		g = Instantiate(AvatarUITemplate2, AvatarsScrollView2);
		g.transform.GetChild(0).GetComponent<Image>().sprite = av.Image;
		
		//add click event
		g.transform.GetComponent<Button>().AddEventListener(AvatarsList2.Count - 1, OnAvatarClick);
	}

	void OnAvatarClick(int AvatarIndex)
	{
		SelectAvatar(AvatarIndex);
	}

	void SelectAvatar(int AvatarIndex)
	{
        if (AvatarsScrollView2.childCount != 0)
        {
			previousSelectedIndex = newSelectedIndex;
			newSelectedIndex = AvatarIndex;
			AvatarsScrollView2.GetChild(previousSelectedIndex).GetComponent<Image>().color = DefaultAvatarColor;
			AvatarsScrollView2.GetChild(newSelectedIndex).GetComponent<Image>().color = ActiveAvatarColor;

			//Change Avatar
			CurrentAvatar.sprite = AvatarsList2[newSelectedIndex].Image;
			foreach (var item in SkilsShop.Instance.ShopItemsList)
			{
				if (item.Image == CurrentAvatar.sprite)
				{
					PlayerPrefs.SetInt("selectedSkilsId", item.id); // get selected player id
				}
			}
		}
	}
}
