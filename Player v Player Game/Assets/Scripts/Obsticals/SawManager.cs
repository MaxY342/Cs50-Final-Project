using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SawManager : MonoBehaviour
{
    public GameObject sawManager;
    public float rotationSpeed = -180f; // degrees per second
    private List<Transform> saws = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        InitializeSaws();
    }

    // Update is called once per frame
    void Update()
    {
        SpinSaws();
    }

    private void InitializeSaws()
    {
        foreach (Transform saw in sawManager.transform)
        {
            saws.Add(saw);
        }
    }

    private void SpinSaws()
    {
        foreach (Transform saw in saws)
        {
            saw.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}
