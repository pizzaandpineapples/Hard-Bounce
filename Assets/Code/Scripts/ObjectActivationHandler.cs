using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ObjectActivationHandler : MonoBehaviour
{
    [SerializeField] private GameObject objectToHandle;

    [SerializeField] private bool activateFirst = false;
    [SerializeField] private bool isSwitchNeeded = true;
    [SerializeField] private bool isSwitchNeededForRepetition = false;
    //[SerializeField] private bool isRepetitionOffWhenSwitchIsRemoved = true;

    [SerializeField] private float repeatTime;
    [SerializeField] private float activationTime = 0;
    [SerializeField] private float deactivationTime = 0;

    [SerializeField] private bool destroyObject;

    void Start()
    {
        // Most cases will require a switch to operate an external mechanism.
        // Mechanism may be ONESHOT or REPEATING.
        // Default: Mechanism is initially OFF, Switch is required, ONESHOT. 
        // TODO: Switch ONESHOT repeating.

        // Switch is required.
        if (isSwitchNeeded)
        {
            if (isSwitchNeededForRepetition) // Mechanism is REPEATING.
            {
                if (activateFirst) // Mechanism is initially OFF.
                {
                    SwitchMechanism.OnSwitchDown += ActivateThenDeactivate;
                    SwitchMechanism.OnSwitchUp += Deactivate;
                }
                else // Mechanism is initially ON.
                {
                    SwitchMechanism.OnSwitchDown += DeactivateThenActivate;
                    SwitchMechanism.OnSwitchUp += Activate;
                }
            }
            else // Mechanism is ONESHOT
            {
                if (activateFirst) // Mechanism is initially OFF.
                {
                    SwitchMechanism.OnSwitchDown += Activate;
                    SwitchMechanism.OnSwitchUp += Deactivate;
                }
                else // Mechanism is initially ON.
                {
                    if (destroyObject)
                        SwitchMechanism.OnSwitchDown += Destroy;
                    else
                        SwitchMechanism.OnSwitchDown += Deactivate;
                    SwitchMechanism.OnSwitchUp += Activate;
                }
            }
        }
        // Switch is not required. Mechanism is REPEATING.
        else
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

        //if (isRepetitionOffWhenSwitchIsRemoved)
        //{
        //    StopAllCoroutines();
        //}
    }
    IEnumerator DeactivateThenActivateRepeater()
    {
        StartCoroutine(DeactivateCoroutine());
        yield return new WaitForSeconds(repeatTime);
        StartCoroutine(ActivateCoroutine());
        yield return new WaitForSeconds(repeatTime);
        StartCoroutine(DeactivateThenActivateRepeater());

        //if (isRepetitionOffWhenSwitchIsRemoved)
        //{
        //    StopAllCoroutines();
        //}
    }

    private void Destroy()
    {
        Destroy(objectToHandle);
    }

    private void OnDisable()
    {
        SwitchMechanism.OnSwitchDown -= ActivateThenDeactivate;
        SwitchMechanism.OnSwitchUp -= DeactivateThenActivate;
        SwitchMechanism.OnSwitchDown -= DeactivateThenActivate;
        SwitchMechanism.OnSwitchUp -= ActivateThenDeactivate;

        SwitchMechanism.OnSwitchDown -= Activate;
        SwitchMechanism.OnSwitchUp -= Deactivate;
        SwitchMechanism.OnSwitchDown -= Destroy;
        SwitchMechanism.OnSwitchDown -= Deactivate;
        SwitchMechanism.OnSwitchUp -= Activate;
    }
}
