﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager de todo los callbacks para el Networking de Photon
/// </summary>
public class NetworkManager : Photon.PunBehaviour, IPunObservable {

    public static NetworkManager Instance = null;

    public GameController controller;
    public GameObject playerInstancePrefab;
    public PlayerInstancesManager playerInstanceManager;
    public string gameVersion = "0.1";
    public byte maxPlayers = 10;

    [HideInInspector] public bool isConnecting;
    [HideInInspector] public bool connected;

    private void Awake()
    {
        isConnecting = false;
        connected = false;
        Instance = this;
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void ConnectToServer()
    {
        isConnecting = true;

        if (PhotonNetwork.connected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = this.maxPlayers }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Conectado a habitación. ");
        isConnecting = false;
        connected = true;

        string[] newPlayer = StoreMyPlayerData();

        photonView.RPC("NewPlayerJoined", PhotonTargets.Others, newPlayer);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log(otherPlayer.UserId);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    public string[] StoreMyPlayerData()
    {
        return new string[]
        {
            controller.playerManager.playerName,
            controller.playerManager.gender,
            controller.playerManager.playerLevel.ToString(),
            controller.playerManager.currentHealth.ToString(),
            controller.playerManager.currentVisibility.ToString(),
            controller.playerRoomNavigation.currentPosition.ToString(),
            photonView.viewID.ToString()
        };
    }

    public PlayerInstance CreatePlayerInstance(string[] playerData)
    {
        PlayerInstance newPlayer = Instantiate(playerInstancePrefab).GetComponent<PlayerInstance>();

        newPlayer.playerName = playerData[0];
        newPlayer.playerGender = playerData[1];
        Int32.TryParse(playerData[2], out newPlayer.playerLevel);
        Int32.TryParse(playerData[3], out newPlayer.currentHealth);
        Int32.TryParse(playerData[4], out newPlayer.currentVisibility);
        newPlayer.currentRoom = RoomsChecker.RoomObjectFromVector(
            RoomsChecker.RoomPositionFromText(playerData[5])
            );
        Int32.TryParse(playerData[6], out newPlayer.playerUserID);

        playerInstanceManager.playerInstancesOnScene.Add(newPlayer.playerName, newPlayer);

        return newPlayer;
    }

    [PunRPC]
    public void InstantiateAlreadyExistingPlayers(string[] playerData)
    {
        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerData[0]))
        {
            return;
        }

        Debug.Log("Player entered");
        PlayerInstance oldPlayer = CreatePlayerInstance(playerData);

        if (oldPlayer.currentRoom != null)
        {
            if (oldPlayer.currentRoom == controller.playerRoomNavigation.currentRoom)
            {
                Debug.Log("Jugador en la habitación");
                controller.playerRoomNavigation.currentRoom.AddPlayerInRoom(oldPlayer);
                controller.playerRoomNavigation.ShowPlayersInRoom();
            }
        }
    }

    [PunRPC]
    public void NewPlayerJoined(string[] playerData)
    {
        Debug.Log("Player entered");
        PlayerInstance newPlayer = CreatePlayerInstance(playerData);

        if (newPlayer.currentRoom != null)
        {
            newPlayer.currentRoom.PlayerEnteredRoom(newPlayer, controller);
        }

        photonView.RPC("InstantiateAlreadyExistingPlayers", PhotonTargets.Others, StoreMyPlayerData());

    }


    //NetworkPlayer
    //photonview().ownerID
    public void PlayerDisconnected(string playerID)
    {
        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerID))
        {
            if (playerInstanceManager.playerInstancesOnScene[playerID].currentRoom == 
                controller.playerRoomNavigation.currentRoom)
            {
                controller.LogStringWithoutReturn(playerID + " se ha desvanecido frente a tus ojos.");
            }

            Destroy(playerInstanceManager.playerInstancesOnScene[playerID]);
            playerInstanceManager.playerInstancesOnScene.Remove(playerID);
        }
    }

    #region Exploration Players

    public void MyPlayerChangedRooms(string playerID, Vector3 newPosition)
    {
        photonView.RPC("PlayerChangedRoom", PhotonTargets.Others, playerID, newPosition.ToString());
    }

    [PunRPC]
    public void PlayerChangedRoom(string playerID, string newRoomPosition)
    {
        Debug.Log(playerID + " Cambió habitaciones hacia " + newRoomPosition);

        if (RoomsChecker.roomsDictionary.ContainsKey(
            RoomsChecker.RoomPositionFromText(newRoomPosition)
            ))
        {
            if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerID))
            {
                playerInstanceManager.playerInstancesOnScene[playerID].currentRoom =
                    RoomsChecker.RoomObjectFromVector(
                    RoomsChecker.RoomPositionFromText(newRoomPosition)
                    );

                controller.playerRoomNavigation.currentRoom.PlayerLeftRoom(
                    playerInstanceManager.playerInstancesOnScene[playerID], controller);

                controller.playerRoomNavigation.currentRoom.PlayerEnteredRoom(
                    playerInstanceManager.playerInstancesOnScene[playerID],
                    controller
                    );
            }
        }
    }

    #endregion

}
