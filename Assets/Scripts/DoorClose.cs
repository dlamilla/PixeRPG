using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClose : MonoBehaviour
{
    [SerializeField] private float timeClosed;
    [SerializeField] private GameObject door;

    private void OnEnable()
    {
        StartCoroutine(DoorClosed());
    }

    private IEnumerator DoorClosed()
    {
        yield return new WaitForSeconds(timeClosed);
        door.SetActive(true);
    }

    private void OnDisable()
    {
        door.SetActive(false);
    }
}
