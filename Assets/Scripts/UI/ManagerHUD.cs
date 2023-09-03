using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerHUD : MonoBehaviour
{
    public GameObject weaponInfoPrefab; // llamo este objeto como la clase del canvas

    private void Start()
    {
        EventManager.current.NewGunEvent.AddListener(CreateWeponInfo);
    }

    public void CreateWeponInfo()
    {
        Instantiate(weaponInfoPrefab, transform);
    }
}
