using UnityEngine;
using System.Collections;
using System;
using DSStructureForClient;

public class SoulCoreUI : MonoBehaviour {

    private string selectedSoulUniqueID = "";
    private SceneState state;
    private string soulName = "";
    private string containerName = "";
    private string selectedContainerUniqueID = "";
    private enum SceneState
    {
        AnswerCore,
        CreateSoul,
        SoulCore,
        CreateContainer
    }

    IEnumerator Start()
    {
        PhotonGlobal.PS.GetSoulUniqueIDListEvent += doGetSoulUniqueIDListEvent;
        PhotonGlobal.PS.GetSoulInfoEvent += doGetSoulInfoEvent;
        PhotonGlobal.PS.GetSoulUniqueIDList();
        state = SceneState.AnswerCore;
        PhotonGlobal.PS.GetContainerUniqueIDListEvent += doGetContainerUniqueIDListEvent;
        PhotonGlobal.PS.GetContainerInfoEvent += doGetContainerInfoEvent;
        yield return null;
	}

    void OnGUI()
    {
        try
        {
            switch(state)
            {
                case SceneState.AnswerCore:
                    AnswerCoreGUI();
                    break;
                case SceneState.CreateSoul:
                    CreateSoulGUI();
                    break;
                case SceneState.SoulCore:
                    SoulCoreGUI();
                    break;
                case SceneState.CreateContainer:
                    CreateContainerGUI();
                    break;
            }
        }
        catch (Exception EX)
        {
            Debug.Log(EX.Message);
        }
    }

    private void CreateContainerGUI()
    {
        GUI.Label(new Rect(30, 40, 100, 20), "名稱:");
        containerName = GUI.TextField(new Rect(30, 60, 100, 20), containerName, 20);
        if (containerName!="" && GUI.Button(new Rect(30, 80, 80, 20), "創建"))
        {
            PhotonGlobal.PS.CreateContainer(selectedSoulUniqueID, containerName, "testScene", 0, 0, 0, 0, 1, 0);
            PhotonGlobal.PS.GetContainerUniqueIDList(selectedSoulUniqueID);
            state = SceneState.SoulCore;
        }
    }

    private void SoulCoreGUI()
    {
        Soul soul = AnswerGlobal.answer.soulList.Find(x => x.soulUniqueID == selectedSoulUniqueID);
        AnswerGlobal.mainSoul = soul;
        GUI.Label(new Rect(30, 40, 200, 20), "名稱:" + soul.soulName);
        GUI.Label(new Rect(30, 60, 100, 20), "角色上限:" + soul.containerLimit);
        GUI.Label(new Rect(30, 80, 200, 20), "所選角色:" + containerName);
        for (int i = 0; i < soul.containerList.Count; i++)
        {
            if(GUI.Button(new Rect(30, 100 + 20 * i, 140, 20), soul.containerList[i].containerName))
            {
                AnswerGlobal.mainContainer = soul.containerList[i];
                containerName = soul.containerList[i].containerName;
            }
        }
        if (AnswerGlobal.mainContainer != null)
        {
            if (GUI.Button(new Rect(30, 300, 150, 20), "進入測試場景"))
            {
                PhotonGlobal.PS.GetSoulUniqueIDListEvent -= doGetSoulUniqueIDListEvent;
                PhotonGlobal.PS.GetSoulInfoEvent -= doGetSoulInfoEvent;
                PhotonGlobal.PS.GetContainerUniqueIDListEvent -= doGetContainerUniqueIDListEvent;
                PhotonGlobal.PS.GetContainerInfoEvent -= doGetContainerInfoEvent;
                Application.LoadLevel("testScene");
            }
        }      
        if (soul.containerList.Count < soul.containerLimit)
            if (GUI.Button(new Rect(30, 220, 100, 20), "創建角色"))
            {
                state = SceneState.CreateContainer;
            }
    }

    private void CreateSoulGUI()
    {
        GUI.Label(new Rect(30, 40, 100, 20), "名稱:");
        soulName = GUI.TextField(new Rect(110, 100, 100, 20), soulName, 20);
        if (soulName!="" && GUI.Button(new Rect(100, 10, 80, 20), "創建"))
        {
            PhotonGlobal.PS.CreateSoul(soulName, 1);
            PhotonGlobal.PS.GetSoulUniqueIDList();
            state = SceneState.AnswerCore;
        }
    }

    private void AnswerCoreGUI()
    {
        for (int i = 0; i < AnswerGlobal.answer.soulList.Count; i++)
        {
            if (GUI.Button(new Rect(130, 40 + i * 30, 200, 20), AnswerGlobal.answer.soulList[i].soulName))
            {
                selectedSoulUniqueID = AnswerGlobal.answer.soulList[i].soulUniqueID;
                PhotonGlobal.PS.GetContainerUniqueIDList(selectedSoulUniqueID);
                state = SceneState.SoulCore;
            }
        }
        if (AnswerGlobal.answer.soulList.Count < AnswerGlobal.answer.soulLimit)
            if (GUI.Button(new Rect(100, 10, 80, 20), "創建"))
            {
                state = SceneState.CreateSoul;
            }
    }

    void doGetSoulUniqueIDListEvent(string _soulUniqueIDList)
    {
        AnswerGlobal.soulUniqueIDList = _soulUniqueIDList.Split(',');
        for (int i = 0; i < AnswerGlobal.soulUniqueIDList.Length; i++)
            PhotonGlobal.PS.GetSoulInfo(AnswerGlobal.soulUniqueIDList[i]);
    }

    void doGetSoulInfoEvent(string soulUniqueID,string soulName, int containerLimit)
    {
        int index = AnswerGlobal.answer.soulList.FindIndex(x => x.soulUniqueID == soulUniqueID);
        if (index == -1)
        {
            AnswerGlobal.answer.AddSoul(new Soul(soulUniqueID,soulName,containerLimit,AnswerGlobal.answer));
        }
        else
        {
            Soul soul=AnswerGlobal.answer.soulList[index];
            soul.Update
                (
                    new Tuple<string, object>[] 
                    { 
                        new Tuple<string, object>("soulName", soulName), 
                        new Tuple<string, object>("containerLimit",containerLimit) 
                    }
                );
        }
    }

    void doGetContainerUniqueIDListEvent(string soulUniqueID, string containerUniqueIDList)
    {
        Soul soul = AnswerGlobal.answer.soulList.Find(x=>x.soulUniqueID==soulUniqueID);
        string[] containerUniqueIDs = containerUniqueIDList.Split(',');
        foreach (string containerUniqueID in containerUniqueIDs)
        {
            if(!soul.containerList.Exists(x=>x.containerUniqueID==containerUniqueID))
            {
                soul.AddContainer(new Container(containerUniqueID));
            }
        }
        for (int i = 0; i < containerUniqueIDs.Length; i++)
            PhotonGlobal.PS.GetContainerInfo(containerUniqueIDs[i]);
    }

    void doGetContainerInfoEvent
        (string containerUniqueID, string containerName, string locationUniqueID,
        float positionX, float positionY, float positionZ, float angle,
        int soulLimit, int status)
    {
        Container container = null;
        for(int i=0;i<AnswerGlobal.answer.soulList.Count;i++)
        {
            container = AnswerGlobal.answer.soulList[i].containerList.Find(x=>x.containerUniqueID==containerUniqueID);
            if (container != null)
                break;
        }
        if(container is Container)
        {
            Tuple<string, object>[] dataTuple = new Tuple<string, object>[] 
                {
                    new Tuple<string,object>("containerName",containerName),
                    new Tuple<string,object>("locationUniqueID",locationUniqueID),
                    new Tuple<string,object>("positionX",positionX),
                    new Tuple<string,object>("positionY",positionY),
                    new Tuple<string,object>("positionZ",positionZ),
                    new Tuple<string,object>("angle",angle),
                    new Tuple<string,object>("soulLimit",soulLimit),
                    new Tuple<string,object>("status",status)
                };
            container.Update(dataTuple);
        }
    }
}
