﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class ImprovedHitBoxAssigner : UdonSharpBehaviour
{
    private VRCPlayerApi[] players = new VRCPlayerApi[80];
    public ImprovedHitBoxManager[] hitboxArray;
    //hitbox assignment stuff

    int[] InsertionSort(int[] inputArray)
    {
        for (int i = 0; i < inputArray.Length - 1; i++)
        {
            for (int j = i + 1; j > 0; j--)
            {
                if (inputArray[j - 1] > inputArray[j])
                {
                    int temp = inputArray[j - 1];
                    inputArray[j - 1] = inputArray[j];
                    inputArray[j] = temp;
                }
            }
        }
        return inputArray;
    }
    public ImprovedHitBoxManager getHitBoxByPlayerID(int playerID)
    {
        VRCPlayerApi requestedPlayer = VRCPlayerApi.GetPlayerById(playerID);
        if(Utilities.IsValid(requestedPlayer))
        {
            //find the index of the matching player in the players array
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == requestedPlayer)
                {
                    return hitboxArray[i];
                }
            }
        }
        else
        {
            Debug.Log("invalid player requested from hitbox assigner");
        }
        return null;
    }
    private void assignHitboxes()
    {
        VRCPlayerApi.GetPlayers(players);
        int[] playerIDs = new int[VRCPlayerApi.GetPlayerCount()];
        int playersCounter= 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (Utilities.IsValid(players[i]))
            {
                playerIDs[playersCounter] = players[i].playerId;
                playersCounter++;
            }
            
        }
        //sort the player ID's by number
        playerIDs = InsertionSort(playerIDs);
        int hitboxCounter = 0;
        for (int i = 0; i < playerIDs.Length; i++)
        {
            VRCPlayerApi player = VRCPlayerApi.GetPlayerById(playerIDs[i]);
            if (Utilities.IsValid(player))
            {
                hitboxArray[hitboxCounter].gameObject.SetActive(true);
                hitboxArray[hitboxCounter].SetAssignedPlayer(player);
                hitboxCounter++;
            }
        }
        for (int i = 0; i < hitboxArray.Length; i++)
        {
            if(!Utilities.IsValid(hitboxArray[i].assignedPlayer))
            {
                hitboxArray[i].gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        assignHitboxes();
    }
    private void OnPlayerJoined(VRCPlayerApi player)
    {
        assignHitboxes();
    }
    private void OnPlayerLeft(VRCPlayerApi player)
    {
        for (int i = 0; i < hitboxArray.Length; i++)
        {
            if (hitboxArray[i].assignedPlayer == player)
            {
                hitboxArray[i].gameObject.SetActive(false);
            }
        }
        //assignHitboxes();
    }
}
