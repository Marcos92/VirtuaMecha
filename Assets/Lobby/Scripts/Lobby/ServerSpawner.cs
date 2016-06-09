using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerSpawner : NetworkBehaviour {

	[Command]
    public void CmdFire(GameObject toSpawn)
    {
        NetworkServer.Spawn(toSpawn);
    }
}
