using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LoadCharacter : NetworkBehaviour
{
    Player player;
    NetworkLobbyInfo lobbyInfo;
    bool attached = false;
    string childName;

    void Awake()
    {
        player = GetComponent<Player>();
        lobbyInfo = GetComponent<NetworkLobbyInfo>();
    }

    void Start()
    {
        NetworkLobbyInfo infoTmp = gameObject.GetComponent<NetworkLobbyInfo>();

        childName = NetworkCharacterManager.instance.weapons[CharacterChoice.instance.LeftWeapon].name + lobbyInfo.lobbyIndex;
        if (hasAuthority)
        {
            //CmdLoad();
        }
    }


    [ClientCallback]
    void Update()
    {
        if (!attached)
        {
            GameObject filho = GameObject.Find(childName + "(Clone)");

            if (filho != null)
            {
                Debug.Log("so far so good!");

                for (int i = 0; i < 4; i++)
                {
                    if (i > 0)
                    {
                        switch(i)
                        {
                            case 1:
                                childName = NetworkCharacterManager.instance.weapons[CharacterChoice.instance.RightWeapon].name + lobbyInfo.lobbyIndex;
                                filho = GameObject.Find(childName + "(Clone)");
                                break;

                            case 2:
                                childName = NetworkCharacterManager.instance.weapons[CharacterChoice.instance.Offensive].name + lobbyInfo.lobbyIndex;
                                filho = GameObject.Find(childName + "(Clone)");
                                break;

                            case 3:
                                childName = NetworkCharacterManager.instance.weapons[CharacterChoice.instance.Defensive].name + lobbyInfo.lobbyIndex;
                                filho = GameObject.Find(childName + "(Clone)");
                                break;
                        }
                    }


                    filho.transform.parent = transform;
                    GetComponents<NetworkTransformChild>()[i].target = filho.transform;

                    if (isServer)
                    {
                        filho.transform.rotation = transform.rotation;
                        filho.transform.position = transform.position;
                    }
                    else
                    {
                        NetworkLobbyInfo infoTmp = gameObject.GetComponent<NetworkLobbyInfo>();
                        GameObject spawnTmp = NetworkCharacterManager.instance.Spawns[infoTmp.lobbyIndex];

                        transform.rotation = spawnTmp.transform.rotation;
                        transform.position = spawnTmp.transform.position;
                    }
                }

                attached = true;

                Transform placeHolder = transform.FindChild("Placeholder");
                if (placeHolder != null)
                {
                    placeHolder.parent = null;
                    //				transform.rotation = placeHolder.rotation;
                    //				transform.position = placeHolder.position;
                    //
                    Destroy(placeHolder.gameObject);
                }
            }
        }
    }



    //[ClientRpc]
    //public void RpcSetRotation(Quaternion rotation)
    //{
    //    transform.rotation = rotation;
    //    for (int i = 0; i < transform.childCount; i++)
    //    { // just in case;
    //        transform.GetChild(i).rotation = rotation;
    //    }
    //    Debug.Log("Called rotation set!");
    //}

    //[ClientRpc]
    //public void RpcSetPosition(Vector3 position)
    //{
    //    transform.position = position;
    //    Debug.Log("Called position set!");
    //}
    //[ClientRpc]
    //public void RpcSetChild(string s)
    //{
    //    //	ChildName = s;
    //}

    //[Command]
    //public void CmdLoad()
    //{
    //    //NetworkLobbyInfo infoTmp = gameObject.GetComponent<NetworkLobbyInfo>();
    //    GameObject spawnTmp = NetworkCharacterManager.instance.Spawns[lobbyInfo.lobbyIndex];

    //    transform.position = spawnTmp.transform.position;
    //    transform.rotation = spawnTmp.transform.rotation;

    //    GameObject character = (GameObject)Instantiate(NetworkCharacterManager.instance.Characters[infoTmp.characterIndex],
    //                               Vector3.zero, Quaternion.identity);
    //    //   spawnTmp.transform.position, spawnTmp.transform.rotation);
    //    //		character.name = PlayableCharacters.instance.Characters [infoTmp.characterIndex].name;

    //    character.transform.parent = gameObject.transform;
    //    //		character.transform.position = transform.position;
    //    //		character.transform.rotation = transform.rotation;

    //    //	RpcSetChild (character.name);
    //    //	RpcSetRotation(transform.rotation);
    //    //	RpcSetPosition (transform.position);


    //    //GetComponent<NetworkTransform> ().enabled = false;
    //    GetComponent<NetworkTransformChild>().target = character.transform;

    //    Debug.Log("Destroying placeholder at " + gameObject.name);
    //    Transform placeholder = transform.FindChild("Placeholder");
    //    placeholder.parent = null;
    //    Destroy(placeholder.gameObject);

    //    NetworkServer.Spawn(character);
    //}
}