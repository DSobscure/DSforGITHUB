using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using DSProtocol;
using DSStructureForClient;
using DoorOfSoul;

public partial class PhotonService : IPhotonPeerListener
{
    public PhotonPeer peer { get; protected set;}
    public bool ServerConnected { get; protected set; }
    public string DebugMessage { get; protected set; }

    public PhotonService()
    {
        peer = null;
        ServerConnected = false;
        DebugMessage = "";
    }

    public void Connect(string IP, int Port, string ServerNmae)
    {
        try
        {
            string ServerAddress = IP + ":" + Port.ToString();
            this.peer = new PhotonPeer(this, ConnectionProtocol.Udp);
            if (!this.peer.Connect(ServerAddress, ServerNmae))
            {
                ConnectEvent(false);
            }
        }
        catch (Exception EX)
        {
            ConnectEvent(false);
            throw EX;
        }
    }

    public void Disconnect()
    {
        try
        {
            if (peer != null)
                this.peer.Disconnect();
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }

    public void Service()
    {
        try
        {
            if (this.peer != null)
                this.peer.Service();
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }


    public void DebugReturn(DebugLevel level, string message)
    {
        this.DebugMessage = message;
    }

    public void OnEvent(EventData eventData)
    {
        switch (eventData.Code)
        {
            #region ProjectContainer
            case (byte)BroadcastType.ProjectContainer:
                {
                    ProjectContainerTask(eventData);
                }
                break;
            #endregion UpdateContainerRealTimeInfo

            #region UpdateContainerRealTimeInfo
            case (byte)BroadcastType.UpdateContainerRealTimeInfo:
                {
                    UpdateContainerRealTimeInfoTask(eventData);
                }
                break;
            #endregion

            #region Offline
            case (byte)BroadcastType.Offline:
                {
                    OfflineTask(eventData);
                }
                break;
            #endregion

            #region MoveContainer
            case (byte)BroadcastType.ContainerMove:
                {
                    ContainerMoveTask(eventData);
                }
                break;
            #endregion
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        switch(operationResponse.OperationCode)
        {
            #region Login
            case (byte)OperationType.Login:
                {
                    LoginTask(operationResponse);
                }
                break;
            #endregion

            #region GetAnswerKey
            case (byte)OperationType.GetAnswerKey:
                {
                    GetAnswerKeyTask(operationResponse);
                }
                break;
            #endregion

            #region GetSoulUniqueIDList
            case (byte)OperationType.GetSoulUniqueIDList:
                {
                    GetSoulUniqueIDListTask(operationResponse);
                }
                break;
            #endregion

            #region CreateSoul
            case (byte)OperationType.CreateSoul:
                {
                    CreateSoulTask(operationResponse);
                }
                break;
            #endregion

            #region GetSoulInfo
            case (byte)OperationType.GetSoulInfo:
                {
                    GetSoulInfoTask(operationResponse);
                }
                break;
            #endregion

            #region GetContainerUniqueIDList
            case (byte)OperationType.GetContainerUniqueIDList:
                {
                    GetContainerUniqueIDListTask(operationResponse);
                }
                break;
            #endregion

            #region GetContainerInfo
            case (byte)OperationType.GetContainerInfo:
                {
                    GetContainerInfoTask(operationResponse);
                }
                break;
            #endregion

            #region CreateContainer
            case (byte)OperationType.CreateContainer:
                {
                    CreateContainerTask(operationResponse);
                }
                break;
            #endregion

            #region Online
            case (byte)OperationType.Online:
                {
                    OnlineTask(operationResponse);
                }
                break;
            #endregion

            #region ProjectToScene
            case (byte)OperationType.ProjectToScene:
                {
                    ProjectToSceneTask(operationResponse);
                }
                break;
            #endregion

            #region GetSceneRealTimeInfo
            case (byte)OperationType.GetSceneRealTimeInfo:
                {
                    GetSceneRealTimeInfoTask(operationResponse);
                }
                break;
            #endregion

            #region GetContainerRealTimeInfo
            case (byte)OperationType.GetContainerRealTimeInfo:
                {
                    GetContainerRealTimeInfoTask(operationResponse);
                }
                break;
            #endregion
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                this.peer.EstablishEncryption();
                break;
            case StatusCode.Disconnect:
                this.peer = null;
                this.ServerConnected = false;
                ConnectEvent(false);
                break;
            case StatusCode.EncryptionEstablished:
                this.ServerConnected = true;
                ConnectEvent(true);
                break;
        }
    }

    //OperationResponse Task

    private void LoginTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string responseAccount = Convert.ToString(operationResponse.Parameters[(byte)LoginResponseItem.AnswerAccount]);
            string responseUniqueID = Convert.ToString(operationResponse.Parameters[(byte)LoginResponseItem.UniqueID]);
            string responseAnswerUniqueID = Convert.ToString(operationResponse.Parameters[(byte)LoginResponseItem.AnswerUniqueID]);
            string responseAnswerName = Convert.ToString(operationResponse.Parameters[(byte)LoginResponseItem.AnswerName]);
            int responseSoulLimit = Convert.ToInt32(operationResponse.Parameters[(byte)LoginResponseItem.SoulLimit]);

            LoginEvent(true, "", responseUniqueID, responseAnswerUniqueID, responseAccount, responseAnswerName, responseSoulLimit);
        }
        else if (operationResponse.ReturnCode == (short)ErrorType.NoExist)
        {
            string responseUniqueID = Convert.ToString(operationResponse.Parameters[(byte)GetAnswerKeyItem.UniqueID]);
            string responseAccount = Convert.ToString(operationResponse.Parameters[(byte)GetAnswerKeyItem.AnswerAccount]);

            GetAnswerKeyEvent(responseUniqueID, responseAccount);
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
            LoginEvent(false, operationResponse.DebugMessage, NotImplement.STRING, NotImplement.STRING, NotImplement.STRING, NotImplement.STRING, NotImplement.INT); // send error message to loginEvent
        }
    }
    private void GetAnswerKeyTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string responseAnswerUniqueID = Convert.ToString(operationResponse.Parameters[(byte)GetAnswerKeyItem.AnswerUniqueID]);
            string responseAnswerName = Convert.ToString(operationResponse.Parameters[(byte)GetAnswerKeyItem.AnswerName]);
            string responseAnswerAccount = Convert.ToString(operationResponse.Parameters[(byte)GetAnswerKeyItem.AnswerAccount]);
            int responseSoulLimit = Convert.ToInt32(operationResponse.Parameters[(byte)GetAnswerKeyItem.SoulLimit]);
            LoginEvent(true, "", AnswerGlobal.UniqueID, responseAnswerUniqueID, responseAnswerAccount, responseAnswerName, responseSoulLimit);
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }
    private void GetSoulUniqueIDListTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string soulUniqueIDList = (string)operationResponse.Parameters[(byte)GetSoulUniqueIDListItem.SoulUniqueIDList];
            GetSoulUniqueIDListEvent(soulUniqueIDList);
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }
    private void CreateSoulTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            GetSoulUniqueIDList();
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }
    private void GetSoulInfoTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string soulName = Convert.ToString(operationResponse.Parameters[(byte)GetSoulInfoItem.SoulName]);
            int containerLimit = Convert.ToInt32(operationResponse.Parameters[(byte)GetSoulInfoItem.ContainerLimit]);
            string soulUniqueID = Convert.ToString(operationResponse.Parameters[(byte)GetSoulInfoItem.SoulUniqueID]);
            GetSoulInfoEvent(soulUniqueID,soulName,containerLimit);
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }
    private void GetContainerUniqueIDListTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string soulUniqueID = (string)operationResponse.Parameters[(byte)GetContainerUniqueIDListItem.SoulUniqueID];
            string containerUniqueIDList = (string)operationResponse.Parameters[(byte)GetContainerUniqueIDListItem.ContainerUniqueIDList];
            GetContainerUniqueIDListEvent(soulUniqueID,containerUniqueIDList);
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }
    private void GetContainerInfoTask(OperationResponse operationResponse)
    {
        if(operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string containerUniqueID = Convert.ToString(operationResponse.Parameters[(byte)GetContainerInfoItem.ContainerUniqueID]);
            string containerName = Convert.ToString(operationResponse.Parameters[(byte)GetContainerInfoItem.ContainerName]);
            string locationUniqueID = Convert.ToString(operationResponse.Parameters[(byte)GetContainerInfoItem.LocationUniqueID]);
            float positionX = Convert.ToSingle(operationResponse.Parameters[(byte)GetContainerInfoItem.PositionX]);
            float positionY = Convert.ToSingle(operationResponse.Parameters[(byte)GetContainerInfoItem.PositionY]);
            float positionZ = Convert.ToSingle(operationResponse.Parameters[(byte)GetContainerInfoItem.PositionZ]);
            float angle = Convert.ToSingle(operationResponse.Parameters[(byte)GetContainerInfoItem.Angle]);
            int soulLimit = Convert.ToInt32(operationResponse.Parameters[(byte)GetContainerInfoItem.SoulLimit]);
            int status = Convert.ToInt32(operationResponse.Parameters[(byte)GetContainerInfoItem.Status]);
            GetContainerInfoEvent
                (
                    containerUniqueID: containerUniqueID,
                    containerName:     containerName,
                    locationUniqueID:  locationUniqueID,
                    positionX:         positionX,
                    positionY:         positionY,
                    positionZ:         positionZ,
                    angle:             angle,
                    soulLimit:         soulLimit,
                    status:            status
                );
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }
    private void CreateContainerTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string soulUniqueID = (string)operationResponse.Parameters[(byte)CreateContainerItem.SoulUniqueID];
            GetContainerUniqueIDList(soulUniqueID);
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }
    private void OnlineTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string soulUniqueID = (string)operationResponse.Parameters[(byte)OnlineItem.SoulUniqueID];
            AnswerGlobal.answer.UpdateSoul(soulUniqueID, new Tuple<string, object>[] { new Tuple<string, object>("active", true) });
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }
    private void ProjectToSceneTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string containerUniqueID = (string)operationResponse.Parameters[(byte)ProjectToSceneItem.ContainerUniqueID];
            string sceneUniqueID = (string)operationResponse.Parameters[(byte)ProjectToSceneItem.SceneUniqueID];
            GetSceneRealTimeInfo(sceneUniqueID);
            ProjectToSceneEvent(AnswerGlobal.mainContainer,sceneUniqueID);
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }
    private void GetSceneRealTimeInfoTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string sceneUniqueID = (string)operationResponse.Parameters[(byte)GetSceneRealTimeInfoItem.SceneUniqueID];
            string sceneName = (string)operationResponse.Parameters[(byte)GetSceneRealTimeInfoItem.SceneName];
            string[] containerUniqueIDList = ((string)operationResponse.Parameters[(byte)GetSceneRealTimeInfoItem.ContainerUniqueIDList]).Split('\n');
            GetSceneInfoEvent(sceneUniqueID, sceneName, containerUniqueIDList);
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }
    private void GetContainerRealTimeInfoTask(OperationResponse operationResponse)
    {
        if (operationResponse.ReturnCode == (short)ErrorType.NoError)
        {
            string containerUniqueID = Convert.ToString(operationResponse.Parameters[(byte)GetContainerRealTimeInfoItem.ContainerUniqueID]);
            string containerName = Convert.ToString(operationResponse.Parameters[(byte)GetContainerRealTimeInfoItem.ContainerName]);
            string locationUniqueID = Convert.ToString(operationResponse.Parameters[(byte)GetContainerRealTimeInfoItem.LocationUniqueID]);
            float positionX = Convert.ToSingle(operationResponse.Parameters[(byte)GetContainerRealTimeInfoItem.PositionX]);
            float positionY = Convert.ToSingle(operationResponse.Parameters[(byte)GetContainerRealTimeInfoItem.PositionY]);
            float positionZ = Convert.ToSingle(operationResponse.Parameters[(byte)GetContainerRealTimeInfoItem.PositionZ]);
            float angle = Convert.ToSingle(operationResponse.Parameters[(byte)GetContainerRealTimeInfoItem.Angle]);
            int soulLimit = Convert.ToInt32(operationResponse.Parameters[(byte)GetContainerRealTimeInfoItem.SoulLimit]);
            int status = Convert.ToInt32(operationResponse.Parameters[(byte)GetContainerRealTimeInfoItem.Status]);
            GetContainerInfoEvent
                (
                    containerUniqueID: containerUniqueID,
                    containerName: containerName,
                    locationUniqueID: locationUniqueID,
                    positionX: positionX,
                    positionY: positionY,
                    positionZ: positionZ,
                    angle: angle,
                    soulLimit: soulLimit,
                    status: status
                );
        }
        else
        {
            DebugReturn(0, operationResponse.DebugMessage);
        }
    }

    //Event Task
    private void ProjectContainerTask(EventData eventData)
    {
        string sceneUniqueID = (string)eventData.Parameters[(byte)ProjectContainerBroadcastItem.SceneUniqueID];
        if (sceneUniqueID == AnswerGlobal.mainContainer.locationUniqueID)
        {
            string containerUniqueID = (string)eventData.Parameters[(byte)ProjectContainerBroadcastItem.ContainerUniqueID];
            string containerName = (string)eventData.Parameters[(byte)ProjectContainerBroadcastItem.ContainerName];
            float positionX = Convert.ToSingle(eventData.Parameters[(byte)ProjectContainerBroadcastItem.PositionX]);
            float positionY = Convert.ToSingle(eventData.Parameters[(byte)ProjectContainerBroadcastItem.PositionY]);
            float positionZ = Convert.ToSingle(eventData.Parameters[(byte)ProjectContainerBroadcastItem.PositionZ]);
            float angle = Convert.ToSingle(eventData.Parameters[(byte)ProjectContainerBroadcastItem.Angle]);
            int soulLimit = Convert.ToInt32(eventData.Parameters[(byte)ProjectContainerBroadcastItem.SoulLimit]);
            int status = Convert.ToInt32(eventData.Parameters[(byte)ProjectContainerBroadcastItem.Status]);
            Container container = new Container(containerUniqueID);
            container.Update
                (
                    new Tuple<string, object>[] 
                                {
                                    new Tuple<string,object>("containerName",containerName),
                                    new Tuple<string,object>("locationUniqueID",sceneUniqueID),
                                    new Tuple<string,object>("positionX",positionX),
                                    new Tuple<string,object>("positionY",positionY),
                                    new Tuple<string,object>("positionZ",positionZ),
                                    new Tuple<string,object>("angle",angle),
                                    new Tuple<string,object>("soulLimit",soulLimit),
                                    new Tuple<string,object>("status",status)
                                }
                );
            ProjectToSceneEvent(container, sceneUniqueID);
        }
    }
    private void UpdateContainerRealTimeInfoTask(EventData eventData)
    {
        string sceneUniqueID = (string)eventData.Parameters[(byte)UpdateContainerRealTimeInfoBroadcastItem.SceneUniqueID];
        if(sceneUniqueID == AnswerGlobal.mainContainer.locationUniqueID)
        {
            string containerUniqueID = (string)eventData.Parameters[(byte)UpdateContainerRealTimeInfoBroadcastItem.ContainerUniqueID];
            float positionX = Convert.ToSingle(eventData.Parameters[(byte)UpdateContainerRealTimeInfoBroadcastItem.PositionX]);
            float positionY = Convert.ToSingle(eventData.Parameters[(byte)UpdateContainerRealTimeInfoBroadcastItem.PositionY]);
            float positionZ = Convert.ToSingle(eventData.Parameters[(byte)UpdateContainerRealTimeInfoBroadcastItem.PositionZ]);
            float angle = Convert.ToSingle(eventData.Parameters[(byte)UpdateContainerRealTimeInfoBroadcastItem.Angle]);
            UpdateContainerEvent(sceneUniqueID, containerUniqueID, positionX, positionY, positionZ, angle);
        }
    }
    private void OfflineTask(EventData eventData)
    {
        string sceneUniqueID = (string)eventData.Parameters[(byte)OfflineBroadcastItem.SceneUniqueID];
        if(sceneUniqueID == AnswerGlobal.mainContainer.locationUniqueID)
        {
            string containerUniqueID = (string)eventData.Parameters[(byte)OfflineBroadcastItem.ContainerUniqueID];
            OfflineEvent(sceneUniqueID,containerUniqueID);
        }
    }
    private void ContainerMoveTask(EventData eventData)
    {
        string sceneUniqueID = (string)eventData.Parameters[(byte)ContainerMoveBroadcastItem.SceneUniqueID];
        if (sceneUniqueID == AnswerGlobal.mainContainer.locationUniqueID)
        {
            string containerUniqueID = (string)eventData.Parameters[(byte)ContainerMoveBroadcastItem.ContainerUniqueID];
            MoveType moveType = (MoveType)eventData.Parameters[(byte)ContainerMoveBroadcastItem.MoveType];
            MoveDirection direction = (MoveDirection)eventData.Parameters[(byte)ContainerMoveBroadcastItem.Direction];
            Phase7.Level level = (Phase7.Level)eventData.Parameters[(byte)ContainerMoveBroadcastItem.Level];
            ContainerMoveEvent(sceneUniqueID,containerUniqueID,moveType,direction,level);
        }
    }

    //內部函數區塊   主動行為
    public void Login(string account, string password)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)LoginParameterItem.Account, account },   
                             { (byte)LoginParameterItem.Password, password }
                        };

            this.peer.OpCustom((byte)OperationType.Login, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }

    }
    public void GetAnswerKey(string answerName,string answerAccount,int soulLimit)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)GetAnswerKeyItem.UniqueID, AnswerGlobal.UniqueID },
                             { (byte)GetAnswerKeyItem.AnswerName, answerName },   
                             { (byte)GetAnswerKeyItem.AnswerAccount, answerAccount },
                             { (byte)GetAnswerKeyItem.SoulLimit, soulLimit }
                        };

            this.peer.OpCustom((byte)OperationType.GetAnswerKey, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void GetSoulUniqueIDList()
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)GetSoulUniqueIDListItem.AnswerUniqueID, AnswerGlobal.AnswerUniqueID },
                        };

            this.peer.OpCustom((byte)OperationType.GetSoulUniqueIDList, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void CreateSoul(string soulName,int containerLimit)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)CreateSoulItem.AnswerUniqueID,AnswerGlobal.AnswerUniqueID},
                             { (byte)CreateSoulItem.SoulName,soulName},
                             { (byte)CreateSoulItem.ContainerLimit,containerLimit}
                        };

            this.peer.OpCustom((byte)OperationType.CreateSoul, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void GetSoulInfo(string soulUniqueID)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)GetSoulInfoItem.SoulUniqueID,soulUniqueID},
                        };

            this.peer.OpCustom((byte)OperationType.GetSoulInfo, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void GetContainerUniqueIDList(string soulUniqueID)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)GetContainerUniqueIDListItem.SoulUniqueID, soulUniqueID },
                        };

            this.peer.OpCustom((byte)OperationType.GetContainerUniqueIDList, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void GetContainerInfo(string containerUniqueID)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)GetContainerInfoItem.ContainerUniqueID,containerUniqueID},
                        };

            this.peer.OpCustom((byte)OperationType.GetContainerInfo, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void CreateContainer
        (string soulUniqueID,string containerName,string locationUniqueID,
        float positionX,float positionY,float positionZ,float angle,
        int soulLimit,int status)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             {(byte)CreateContainerItem.SoulUniqueID,soulUniqueID},
                             {(byte)CreateContainerItem.ContainerName,containerName},
                             {(byte)CreateContainerItem.LocationUniqueID,locationUniqueID},
                             {(byte)CreateContainerItem.PositionX,positionX},
                             {(byte)CreateContainerItem.PositionY,positionY},
                             {(byte)CreateContainerItem.PositionZ,positionZ},
                             {(byte)CreateContainerItem.Angle,angle},
                             {(byte)CreateContainerItem.SoulLimit,soulLimit},
                             {(byte)CreateContainerItem.Status,status}
                        };

            this.peer.OpCustom((byte)OperationType.CreateContainer, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void Online(string soulUniqueID,string soulName,int containerLimit)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             {(byte)OnlineItem.SoulUniqueID,soulUniqueID},
                             {(byte)OnlineItem.SoulName,soulName},
                             {(byte)OnlineItem.ContainerLimit,containerLimit}
                        };

            this.peer.OpCustom((byte)OperationType.Online, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void ProjectToScene
        (string soulUniqueID,string containerUniqueID,string containerName,int soulLimit,string sceneUniqueID,
        float positionX,float positionY,float positionZ,float angle,int status)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             {(byte)ProjectToSceneItem.SoulUniqueID,soulUniqueID},
                             {(byte)ProjectToSceneItem.ContainerUniqueID,containerUniqueID},
                             {(byte)ProjectToSceneItem.ContainerName,containerName},
                             {(byte)ProjectToSceneItem.SoulLimit,soulLimit},
                             {(byte)ProjectToSceneItem.SceneUniqueID,sceneUniqueID},
                             {(byte)ProjectToSceneItem.PositionX,positionX},
                             {(byte)ProjectToSceneItem.PositionY,positionY},
                             {(byte)ProjectToSceneItem.PositionZ,positionZ},
                             {(byte)ProjectToSceneItem.Angle,angle},
                             {(byte)ProjectToSceneItem.Status,status}
                        };

            this.peer.OpCustom((byte)OperationType.ProjectToScene, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void GetSceneRealTimeInfo(string sceneUniqueID)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)GetSceneRealTimeInfoItem.SceneUniqueID,sceneUniqueID},
                        };

            this.peer.OpCustom((byte)OperationType.GetSceneRealTimeInfo, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void GetContainerRealTimeInfo(string sceneUniqueID,string containerUniqueID)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)GetContainerRealTimeInfoItem.LocationUniqueID,sceneUniqueID},
                             { (byte)GetContainerRealTimeInfoItem.ContainerUniqueID,containerUniqueID}
                        };

            this.peer.OpCustom((byte)OperationType.GetContainerRealTimeInfo, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void UpdateContainerRealTimeInfo(string sceneUniqueID,string containerUniqueID,float positionX,float positionY,float positionZ,float angle)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)UpdateContainerRealTimeInfoItem.SceneUniqueID,sceneUniqueID},
                             { (byte)UpdateContainerRealTimeInfoItem.ContainerUniqueID,containerUniqueID},
                             { (byte)UpdateContainerRealTimeInfoItem.PositionX,positionX},
                             { (byte)UpdateContainerRealTimeInfoItem.PositionY,positionY},
                             { (byte)UpdateContainerRealTimeInfoItem.PositionZ,positionZ},
                             { (byte)UpdateContainerRealTimeInfoItem.Angle,angle}
                        };

            this.peer.OpCustom((byte)OperationType.UpdateContainerRealTimeInfo, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
    public void MoveContainer(string sceneUniqueID, string containerUniqueID, MoveType moveType, MoveDirection direction, Phase7.Level level)
    {
        try
        {
            var parameter = new Dictionary<byte, object> { 
                             { (byte)ContainerMoveItem.SceneUniqueID,sceneUniqueID},
                             { (byte)ContainerMoveItem.ContainerUniqueID,containerUniqueID},
                             { (byte)ContainerMoveItem.MoveType,moveType},
                             { (byte)ContainerMoveItem.Direction,direction},
                             { (byte)ContainerMoveItem.Level,level},
                        };
            this.peer.OpCustom((byte)OperationType.ContainerMove, parameter, true, 0, true);
        }
        catch (Exception EX)
        {
            throw EX;
        }
    }
}
