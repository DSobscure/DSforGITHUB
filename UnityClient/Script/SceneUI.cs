using UnityEngine;
using System.Collections;
using DSStructureForClient;
using System.Collections.Generic;
using DoorOfSoul;
using DSProtocol;

public class SceneUI : MonoBehaviour {

    public GameObject containerPrefab;
    public ContainerGameObject myContainer;
    public Dictionary<string, ContainerGameObject> containerDictionary;
    public string sceneUniqueID;
    public string sceneName;
    private int updateCounter = 0;
    public Camera mainCamera;

    IEnumerator Start()
    {
        PhotonGlobal.PS.ProjectToSceneEvent += doProjectToSceneEvent;
        PhotonGlobal.PS.GetSceneInfoEvent += doGetSceneInfoEvent;
        PhotonGlobal.PS.GetContainerInfoEvent += doGetContainerInfoEvent;
        PhotonGlobal.PS.UpdateContainerEvent += doUpdateContainerEvent;
        PhotonGlobal.PS.OfflineEvent += doOfflineEvent;
        PhotonGlobal.PS.ContainerMoveEvent += doContainerMoveEvent;

        Soul soul = AnswerGlobal.mainSoul;
        PhotonGlobal.PS.Online(soul.soulUniqueID, soul.soulName, soul.containerLimit);

        Container container = AnswerGlobal.mainContainer;
        PhotonGlobal.PS.ProjectToScene
            (soul.soulUniqueID,container.containerUniqueID, container.containerName, container.soulLimit, container.locationUniqueID,
            container.positionX, container.positionY, container.positionZ, container.angle, container.status);
        containerDictionary = new Dictionary<string, ContainerGameObject>();
        yield return null;
    }

    void Update()
    {
        if(updateCounter%120==0)
        {
            if(containerDictionary.ContainsKey(AnswerGlobal.mainContainer.containerUniqueID))
            {
                GameObject containerObject = containerDictionary[AnswerGlobal.mainContainer.containerUniqueID].gameObject;
                PhotonGlobal.PS.UpdateContainerRealTimeInfo
                        (
                            sceneUniqueID,
                            AnswerGlobal.mainContainer.containerUniqueID,
                            containerObject.transform.position.x,
                            containerObject.transform.position.y,
                            containerObject.transform.position.z,
                            containerObject.transform.eulerAngles.y
                        );
            }
        }
        updateCounter++;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            PhotonGlobal.PS.MoveContainer(sceneUniqueID, AnswerGlobal.mainContainer.containerUniqueID, MoveType.Rotate, MoveDirection.RotateRight, Phase7.Level.C4);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            PhotonGlobal.PS.MoveContainer(sceneUniqueID, AnswerGlobal.mainContainer.containerUniqueID, MoveType.Move, MoveDirection.MoveBackWard, Phase7.Level.C4);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            PhotonGlobal.PS.MoveContainer(sceneUniqueID, AnswerGlobal.mainContainer.containerUniqueID, MoveType.Rotate, MoveDirection.RotateLeft, Phase7.Level.C4);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            PhotonGlobal.PS.MoveContainer(sceneUniqueID, AnswerGlobal.mainContainer.containerUniqueID, MoveType.Move, MoveDirection.MoveFoward, Phase7.Level.C4);
        }
    }

    void doProjectToSceneEvent(Container _container, string _sceneUniqueID)
    {
        if(!containerDictionary.ContainsKey(_container.containerUniqueID))
        {
            containerDictionary.Add
            (
                _container.containerUniqueID,
                new ContainerGameObject(_container, Instantiate(containerPrefab, new Vector3(_container.positionX, _container.positionY, _container.positionZ), Quaternion.identity) as GameObject)
            );
            if (_container.containerUniqueID == AnswerGlobal.mainContainer.containerUniqueID)
            {
                myContainer = containerDictionary[_container.containerUniqueID];
                this.mainCamera.transform.parent = myContainer.gameObject.transform;
                this.mainCamera.transform.position = new Vector3(0f,6f,26f);
                this.mainCamera.transform.eulerAngles = new Vector3(10f,180f,0f);
            }
        }
    }
    void doGetSceneInfoEvent(string _sceneUniqueID, string _sceneName, string[] _containerUniqueIDList)
    {
        sceneUniqueID = _sceneUniqueID;
        sceneName = _sceneName;
        for (int i = 0; i < _containerUniqueIDList.Length - 1;i++ )
        {
            PhotonGlobal.PS.GetContainerRealTimeInfo(sceneUniqueID, _containerUniqueIDList[i]);
        }
    }
    void doGetContainerInfoEvent
        (string containerUniqueID, string containerName, string locationUniqueID,
        float positionX, float positionY, float positionZ, float angle,
        int soulLimit, int status)
    {
        ContainerGameObject containerGameObject = (containerDictionary.ContainsKey(containerUniqueID)) ? containerDictionary[containerUniqueID] : null;

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

        if (containerGameObject is ContainerGameObject)
        {
            Vector3 position = new Vector3(positionX,positionY,positionZ);
            Vector3 eulerAngles = new Vector3(0f, angle, 0f);
            containerGameObject.Update(dataTuple,position,eulerAngles);
        }
        else
        {
            Container container = new Container(containerUniqueID);
            container.Update(dataTuple);
            doProjectToSceneEvent(container, locationUniqueID);
        }
    }
    void doUpdateContainerEvent(string _sceneUniqueID, string _containerUniqueID, float _positionX, float _positionY, float _positionZ, float _angle)
    {
        if(sceneUniqueID == _sceneUniqueID)
        {
            if(containerDictionary.ContainsKey(_containerUniqueID))
            {
                containerDictionary[_containerUniqueID].Update
                    (
                        new Tuple<string, object>[] 
                        {
                            new Tuple<string,object>("positionX",_positionX),
                            new Tuple<string,object>("positionY",_positionY),
                            new Tuple<string,object>("positionZ",_positionZ),
                            new Tuple<string,object>("angle",_angle)
                        },
                        new Vector3(_positionX,_positionY,_positionZ),
                        new Vector3(0f,_angle,0f)
                    );
            }
        }
    }
    void doOfflineEvent(string _sceneUniqueID, string _containerUniqueID)
    {
        if (sceneUniqueID == _sceneUniqueID)
        {
            if (containerDictionary.ContainsKey(_containerUniqueID))
            {
                Destroy(containerDictionary[_containerUniqueID].gameObject);
                containerDictionary.Remove(_containerUniqueID);
            }
        }
    }
    void doContainerMoveEvent(string _sceneUniqueID, string _containerUniqueID, MoveType moveType, MoveDirection direction, Phase7.Level level)
    {
        if (sceneUniqueID == _sceneUniqueID)
        {
            if (containerDictionary.ContainsKey(_containerUniqueID))
            {
                switch(moveType)
                {
                    case MoveType.Move:
                        {
                            switch(direction)
                            {
                                case MoveDirection.MoveFoward:
                                    {
                                        containerDictionary[_containerUniqueID].gameObject.transform.Translate(0f,0f,-0.005f*(float)level);
                                    }
                                    break;
                                case MoveDirection.MoveBackWard:
                                    {
                                        containerDictionary[_containerUniqueID].gameObject.transform.Translate(0f, 0f, 0.005f * (float)level);
                                    }
                                    break;
                            }
                        }
                        break;
                    case MoveType.Rotate:
                        {
                            switch(direction)
                            {
                                case MoveDirection.RotateLeft:
                                    {
                                        containerDictionary[_containerUniqueID].gameObject.transform.Rotate(new Vector3(0f, -0.01f*(float)level, 0f));
                                    }
                                    break;
                                case MoveDirection.RotateRight:
                                    {
                                        containerDictionary[_containerUniqueID].gameObject.transform.Rotate(new Vector3(0f, 0.01f * (float)level, 0f));
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }
    }
}
