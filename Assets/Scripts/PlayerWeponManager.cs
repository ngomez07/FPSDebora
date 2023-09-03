using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeponManager : MonoBehaviour
{
    public List<WeaponController> startingWeapons = new List<WeaponController>();

    public Transform weaponParentSocket;
    public Transform defaultWeaponPosition;
    public Transform aimingPosition;

    public int ActiveWeaponIndex { get; private set; }

    private WeaponController[] weaponSlots = new WeaponController[2];

    // Start is called before the first frame update
    void Start()
    {
        ActiveWeaponIndex = -1;

        foreach (WeaponController startingWeapon in startingWeapons)
        {
            AddWeapon(startingWeapon);
        }
        SwitchWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            StartCoroutine(SwitchWeapon()); //Acuerdate que ahora es un IENumerator
        }
    }

    private IEnumerator SwitchWeapon() // puedo esperar unos segundos para ejecutarla de nuevo
    {
        int tempIndex = (ActiveWeaponIndex+1)%weaponSlots.Length;

        if (weaponSlots[tempIndex] == null)
        {
            yield return null; // hay que cambiar esto
        }

        foreach (WeaponController weapon in weaponSlots)
        {
            if (weapon != null)
            {
                //weapon.gameObject.SetActive(false);
                weapon.Hide();
            }
        }

        yield return new WaitForSeconds(0.5f); //es lo que dura la animacion de esconderse

        foreach (WeaponController weapon in weaponSlots)
        {
            if (weapon != null)
            {
                weapon.gameObject.SetActive(false);
                //weapon.Hide();
            }
        }

        weaponSlots[tempIndex].gameObject.SetActive(true);
        ActiveWeaponIndex = tempIndex;
        EventManager.current.NewGunEvent.Invoke();
    }

    private void AddWeapon(WeaponController p_weaponPrefab){
        weaponParentSocket.position = defaultWeaponPosition.position;

        //Añadir un arma pero no mostrarla
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                WeaponController weaponClone = Instantiate(p_weaponPrefab, weaponParentSocket);
                weaponClone.owner = gameObject;
                weaponClone.gameObject.SetActive(false);

                weaponSlots[i] = weaponClone;
                return;
            }

        }
    }
}
