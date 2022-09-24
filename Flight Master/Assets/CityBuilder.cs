using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBuilder : MonoBehaviour
{
    [SerializeField] List<GameObject> cityProducts;
    [Header("LIMITS")]
    [SerializeField] private float xMaxCityBuilder;

    //private void Awake()
    //{
    //    System.Random random = new System.Random();
    //    for (int i = 0; i < 1000; i+=25)
    //    {
    //        Instantiate(cityProducts[0], new Vector3(xMaxCityBuilder, 0, i), Quaternion.identity);
    //        Instantiate(cityProducts[0], new Vector3(-xMaxCityBuilder, 0, i), Quaternion.identity);

    //        Instantiate(cityProducts[random.Next(1, cityProducts.Count)], new Vector3(UnityEngine.Random.Range(-xMaxCityBuilder, xMaxCityBuilder), 0, i+200), Quaternion.identity);
    //    }
    //}
}
