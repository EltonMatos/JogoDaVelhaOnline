using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{

    public LayerMask UI;

    private void Start()
    {
        if (IsServer)
        {
            BoardManager.instance.AddPlayer(OwnerClientId);
        }           
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var pickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
               if(Physics.Raycast(pickRay, out var hitInfo, Mathf.Infinity, UI))
                {
                    var slot = hitInfo.transform.GetComponentInChildren<Spot>();
                    if(slot != null)
                    {
                        MakePlayServerRpc(slot.Linha, slot.Coluna);
                    }
                }
            }
        }
    }

    [ServerRpc]
    private void MakePlayServerRpc(int line, int column)
    {        
        BoardManager.instance.MakePlay(OwnerClientId, line, column);
    }

    //arrumar
    [ClientRpc]
    public void UpdateBoardClientRpc(int line, int column)
    {
        BoardManager.instance.MakePlay(OwnerClientId, line, column);
    }
}
