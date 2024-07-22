using UnityEngine;

public class NetworkManagerInitializer : MonoBehaviour
{
    public GameObject networkManagerPrefab;

    void Awake()
    {
        if (CustomNetworkManager.Instance == null)
        {
            Instantiate(networkManagerPrefab);
        }
    }
}
