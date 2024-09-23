using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;

namespace Lobby
{
    public class NetworkDiscovered : MonoBehaviour
    {
        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

        public NetworkDiscovery networkDiscovery;
        public NetworkManager networkManager;

        public void StarttheDiscovery()
        {
            if (networkDiscovery != null)
            {
                networkDiscovery.OnServerFound.AddListener(OnDiscoveredServer);
            }
            else
            {
                Debug.LogError("NetworkDiscovery is not assigned.");
            }

            // Start the discovery process when the script starts
            StartDiscoveryOrHost();
        }

        void StartDiscoveryOrHost()
        {
            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();

            // Set a timeout for discovery to allow time for servers to be found
            Invoke(nameof(CheckDiscoveryResults), 5f); // Adjust timeout as needed
        }

        void CheckDiscoveryResults()
        {
            if (discoveredServers.Count > 0)
            {
                // Join the first discovered server
                ConnectToFirstServer();
            }
            else
            {
                // No servers found, start hosting
                StartHosting();
            }
        }

        void ConnectToFirstServer()
        {
            // Get the first server from the discovered servers
            if (discoveredServers.Count > 0)
            {
                var firstServer = discoveredServers.Values.GetEnumerator();
                if (firstServer.MoveNext())
                {
                    var serverResponse = firstServer.Current;
                    networkDiscovery.StopDiscovery();
                    NetworkManager.singleton.StartClient(serverResponse.uri);
                }
            }
        }

        void StartHosting()
        {
            networkDiscovery.StopDiscovery();
            networkManager.StartHost();
            networkDiscovery.AdvertiseServer();
        }

        void OnDiscoveredServer(ServerResponse info)
        {
            discoveredServers[info.serverId] = info;
        }
    }
}
