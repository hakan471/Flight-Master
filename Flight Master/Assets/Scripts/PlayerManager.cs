using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    private GameObject character;
    private string playerName;
    private float playerSpeed = 1;
    private float playerFuel = 10;
    public PlayerManager(float _playerSpeed, float _playerFuel, string  _playerName, GameObject _character)
    {
        this.playerSpeed = _playerSpeed;
        this.playerFuel = _playerFuel;
        this.playerName = _playerName;
        this.character = _character;
    }
    public string GetPlayerName
    {
        get
        {
            return playerName;
        }
    }
    //class oluþturulduðuna karakterin defaul verilerini alacak, bu sayede returnde ilk o verileri döndürecek
    public float PlayerSpeed { 
        get
        {
            return PlayerPrefs.GetFloat("playerSpeed", playerSpeed);
        }
        set
        {
            PlayerPrefs.SetFloat("playerSpeed", value);
        }
    }
    public float GetPlayerFuel
    {
        get
        {
            return PlayerPrefs.GetFloat("playerFuel", playerFuel);
        }
        set
        {
            PlayerPrefs.SetFloat("playerFuel", value);
        }
    }
}
