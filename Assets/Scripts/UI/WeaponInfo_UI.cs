using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // importo la libreria de TextMesh

public class WeaponInfo_UI : MonoBehaviour
{
    // vamos a borrar todo lo que viene por defecto

    //mantenemos los mismos nombres de los textos para saber a que se refiere
    public TMP_Text currentBullets;
    public TMP_Text totalBullets;

    // la UI es la que escucha el evento
    private void OnEnable()
    {
        // cada que se ejecute el evento que el escucha, que es lo que tiene que correr
        EventManager.current.updateBulletsEvent.AddListener(UpdateBullets);
    }

    private void OnDisable()
    {
        EventManager.current.updateBulletsEvent.RemoveListener(UpdateBullets);
    }

    public void UpdateBullets(int newCurrentBullets, int newTotalBullets)
    {
        if (newCurrentBullets <= 0)
        {
            currentBullets.color = new Color(1, 0, 0); //pongo el color en una escada de 0 a 1 en rgb
        } else
        {
            currentBullets.color = Color.white;
        }

        currentBullets.text = newCurrentBullets.ToString();
        totalBullets.text = newTotalBullets.ToString();
    }
}
