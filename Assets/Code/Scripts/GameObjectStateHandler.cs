using UnityEngine;

public class GameObjectStateHandler : MonoBehaviour
{
    [SerializeField] private GameObject objectToHandle;
    [SerializeField] private bool destroyObject;

    void Start()
    {
        if (destroyObject)
            SwitchMechanism.OnSwitchDown += Destroy;
        else
            SwitchMechanism.OnSwitchDown += Deactivate;
        SwitchMechanism.OnSwitchUp += Activate;
    }

    private void Activate()
    {
        objectToHandle.SetActive(true);
    }
    private void Deactivate()
    {
        objectToHandle.SetActive(false);
    }

    private void Destroy()
    {
        Destroy(objectToHandle);

        SwitchMechanism.OnSwitchDown -= Destroy;
        SwitchMechanism.OnSwitchDown -= Deactivate;
        SwitchMechanism.OnSwitchUp -= Activate;
    }
}
