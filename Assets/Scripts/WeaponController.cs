using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// es un enumerador, aca puedo poner los tipos de disparo.
public enum ShotType
{
    Manual,
    Automatic
}

public class WeaponController : MonoBehaviour
{
    // boquilla referecias
    public Transform weponMuzzle;
    public Animator animator; // con esto puedo llamar al controlador

    [Header("General")]
    //public Transform cameraPlayerTransform; // cuando me la traiga no me va a dejar porque es un prefab!!! por eso***
    public LayerMask hittableLayers;
    public GameObject bulletHolePrefab;

    [Header("Shoot parameters")]
    public ShotType shotType; // se llama igual y se crea el tipo de variable
    public float fireRange = 200;
    // para retroceso usando codigo hago esto
    public float recoilForce = 4f;
    // velocidad entre disparos
    public float fireRate = 0.6f;
    public int maxAmmo = 8;

    [Header("Reload parameters")]
    public float reloadTime = 2f;

    public int currentAmmo {  get; private set; }//ponerle esto la deja publica pero no puedo modificarla cuando juego....

    private float lastTimeShoot = Mathf.NegativeInfinity;

    [Header("Sounds and visuals")]
    //visuales y sonidos referencias
    public GameObject flashEffect;

    // identificar al dueño del arma para evitar el collider
    public GameObject owner { set; get; } //set y get para no modificar mientras juego

    // como no puedo instanciala en el editor lo hago por codigo
    private Transform cameraPlayerTransform;

    // como puedo recargar mientas esta la animacion voy a crear una variable para evitar esto
    private bool isReloading;

    private void Awake()
    {
        currentAmmo = maxAmmo;
        // DE ACA VOY A LLAMAR EL EVENTO QUE CREE EN OTRO SCRIPT, asi llamo el update
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo, maxAmmo);
    }

    private void Start()
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update()
    {
        if (shotType == ShotType.Automatic && !isReloading)
        {
            if (Input.GetButton("Fire1"))
            {
                TryShoot();
            }
        }

        if (shotType == ShotType.Manual && !isReloading)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                TryShoot();
                Debug.Log("Disparo");
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            // para llamar un IEnumerator hay que
            StartCoroutine(Reload());
        }

        //lo regreso para que no se quede atras el arma
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5);
    }

    private bool TryShoot()
    {
        if (lastTimeShoot + fireRate < Time.time)
        {
            if (currentAmmo >= 1)
            {
                HandleShoot();
                currentAmmo--;
                EventManager.current.updateBulletsEvent.Invoke(currentAmmo, maxAmmo);
                return true;
            }
        }
        return false;
    }

    private void HandleShoot()
    {
        //creemos un objeto flash
        GameObject flashClone = Instantiate(flashEffect, weponMuzzle.position, Quaternion.Euler(weponMuzzle.forward), transform);
        Destroy(flashClone,1f);

        AddRecoil(); // creo la funcion para crear retroceso.

        RaycastHit[] hits; // declaro varios puntos de choque
        hits = Physics.RaycastAll(cameraPlayerTransform.position, cameraPlayerTransform.forward, fireRange); // choques del reycast

        foreach(RaycastHit hit in hits)
        {
            if (hit.collider.gameObject != owner) //cuando no hace collide con el player
            {
                GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleClone, 15f);
            }
        }

        lastTimeShoot = Time.time; // time retorna el tiempo que va el juego
    }

    private void AddRecoil()
    {
        //meotod para generar el recoil
        transform.Rotate(-recoilForce, 0f, 0f);
        transform.position = transform.position - transform.forward * (recoilForce / 50);
    }

    IEnumerator Reload() // IEnumerator, me permite esperar un tiempo en su funcion recargar
    {
        isReloading = true;
        if (animator) // este se hace porque el arma 1 todavia no tiene animaciones entonces se hace un if
        {
            animator.SetTrigger("Reloading"); //igualito al trigger en animator para que funcione
        }
        Debug.Log("Reload....");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo, maxAmmo);
        Debug.Log("Reloaded!");
        isReloading = false; // Con esta variable evito poder recargar mientras estoy recargando
    }

    public void Hide()
    {
        if (animator) // este se hace porque el arma 1 todavia no tiene animaciones entonces se hace un if
        {
            animator.SetTrigger("Hiding"); // es el nombre del trigger en el animator
        }
    }

}
