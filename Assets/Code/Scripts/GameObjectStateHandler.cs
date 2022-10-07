using System.Collections;
using UnityEngine;
using DG.Tweening;

public class GameObjectStateHandler : MonoBehaviour
{
    [SerializeField] private GameObject objectToHandle;

    [SerializeField] private bool activateFirst;
    [SerializeField] private bool isSwitchNeeded;
    [SerializeField] private bool isSwitchNeededForRepetition;

    [SerializeField] private float repeatTime;
    [SerializeField] private float activationTime = 0;
    [SerializeField] private float deactivationTime = 0;

    [SerializeField] private bool destroyObject;

    void Start()
    {
        if (isSwitchNeededForRepetition)
        {
            if (activateFirst)
            {
                SwitchMechanism.OnSwitchDown += ActivateThenDeactivate;
                SwitchMechanism.OnSwitchUp += DeactivateThenActivate;
            }
            else
            {
                SwitchMechanism.OnSwitchDown += DeactivateThenActivate;
                SwitchMechanism.OnSwitchUp += ActivateThenDeactivate;
            }
        }
        else
        {
            if (destroyObject)
                SwitchMechanism.OnSwitchDown += Destroy;
            else
                SwitchMechanism.OnSwitchDown += Deactivate;
            SwitchMechanism.OnSwitchUp += Activate;
        }
        
        if (!isSwitchNeeded)
        {
            if (activateFirst)
                StartCoroutine(ActivateThenDeactivateRepeater());
            else
                StartCoroutine(DeactivateThenActivateRepeater());
        }
    }

    private void Activate()
    {
        StartCoroutine(ActivateCoroutine());
    }
    IEnumerator ActivateCoroutine()
    {
        yield return new WaitForSeconds(activationTime);
        objectToHandle.SetActive(true);
    }
    private void Deactivate()
    {
        StartCoroutine(DeactivateCoroutine());
    }
    IEnumerator DeactivateCoroutine()
    {
        yield return new WaitForSeconds(deactivationTime);
        objectToHandle.SetActive(false);
    }


    private void ActivateThenDeactivate()
    {
        StartCoroutine(ActivateThenDeactivateRepeater());
    }
    private void DeactivateThenActivate()
    {
        StartCoroutine(DeactivateThenActivateRepeater());
    }
    IEnumerator ActivateThenDeactivateRepeater()
    {
        StartCoroutine(ActivateCoroutine());
        yield return new WaitForSeconds(repeatTime);
        StartCoroutine(DeactivateCoroutine());
        yield return new WaitForSeconds(repeatTime);
        StartCoroutine(ActivateThenDeactivateRepeater());
    }
    IEnumerator DeactivateThenActivateRepeater()
    {
        StartCoroutine(DeactivateCoroutine());
        yield return new WaitForSeconds(repeatTime);
        StartCoroutine(ActivateCoroutine());
        yield return new WaitForSeconds(repeatTime);
        StartCoroutine(DeactivateThenActivateRepeater());
    }


    private void Destroy()
    {
        Destroy(objectToHandle);

        SwitchMechanism.OnSwitchDown -= Destroy;
        SwitchMechanism.OnSwitchDown -= Deactivate;
        SwitchMechanism.OnSwitchUp -= Activate;
    }
}
