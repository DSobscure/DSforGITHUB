using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using ExitGames.Logging;
using DSProtocol;
using DSStructure.PlayerStructure;
using DSStructure.SpatiotemporalStructure.SpaceClass;
using DoorOfSoul;

namespace DSServer
{
    public class DSPeer : PeerBase
    {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        public Guid guid { get; set; }
        private DSServer _server;
        public Answer answer{ get; set;}
        public string uniqueID { get; set; }

        public DSPeer(IRpcProtocol rpcprotocol,IPhotonPeer nativePeer,DSServer serverApplication) : base(rpcprotocol,nativePeer)
        {
            guid = Guid.NewGuid();
            _server = serverApplication;
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            Answer answer;
            Guid _g;
            Container _container;
            if (_server.answerConcurrentDictionary.ContainsKey(guid))
            {
                _server.RemoveAnswer(guid, out answer);
                foreach(Soul soul in answer.soulList)
                {
                    foreach(Container container in soul.containerList)
                    {
                        container.location.RemoveContainer(container.containerUniqueID,out _container);
                        _server.BroadcastToContainerList
                            (
                                receivers: _container.location.BroadcastList(),
                                broadcastType: BroadcastType.Offline,
                                dataPackage: new Tuple<byte,object>[]
                                            {
                                                new Tuple<byte,object>((byte)OfflineBroadcastItem.SceneUniqueID,_container.location.uniqueID),
                                                new Tuple<byte,object>((byte)OfflineBroadcastItem.ContainerUniqueID,_container.containerUniqueID.ToString())
                                            }
                            );
                    }
                }
                if(_server.uniqueIDGuidConcurrentDictionary.ContainsKey(uniqueID))
                    _server.uniqueIDGuidConcurrentDictionary.TryRemove(uniqueID, out _g);
            }
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch(operationRequest.OperationCode)
            {
                #region Login
                case (byte)OperationType.Login:
                    {
                        LoginTask(operationRequest);
                    }
                    break;
                #endregion

                #region GetAnswerKey
                case (byte)OperationType.GetAnswerKey:
                    {
                        GetAnswerKeyTask(operationRequest);
                    }
                    break;
                #endregion

                #region GetSoulUniqueIDList
                case (byte)OperationType.GetSoulUniqueIDList:
                    {
                        GetSoulUniqueIDListTask(operationRequest);
                    }
                    break;
                #endregion

                #region CreateSoul
                case (byte)OperationType.CreateSoul:
                    {
                        CreateSoulTask(operationRequest);
                    }
                    break;
                #endregion

                #region GetSoulInfo
                case (byte)OperationType.GetSoulInfo:
                    {
                        GetSoulInfoTask(operationRequest);
                    }
                    break;
                #endregion

                #region GetContainerUniqueIDList
                case (byte)OperationType.GetContainerUniqueIDList:
                    {
                        GetContainerUniqueIDListTask(operationRequest);
                    }
                    break;
                #endregion

                #region GetContainerInfo
                case (byte)OperationType.GetContainerInfo:
                    {
                        GetContainerInfoTask(operationRequest);
                    }
                    break;
                #endregion

                #region CreateContainer
                case (byte)OperationType.CreateContainer:
                    {
                        CreateContainerTask(operationRequest);
                    }
                    break;
                #endregion

                #region Online
                case (byte)OperationType.Online:
                    {
                        OnlineTask(operationRequest);
                    }
                    break;
                #endregion

                #region ProjectToScene
                case (byte)OperationType.ProjectToScene:
                    {
                        ProjectToSceneTask(operationRequest);
                    }
                    break;
                #endregion

                #region GetSceneRealTimeInfo
                case (byte)OperationType.GetSceneRealTimeInfo:
                    {
                        GetSceneRealTimeInfoTask(operationRequest);
                    }
                    break;
                #endregion

                #region GetContainerRealTimeInfo
                case (byte)OperationType.GetContainerRealTimeInfo:
                    {
                        GetContainerRealTimeInfoTask(operationRequest);
                    }
                    break;
                #endregion

                #region UpdateContainerRealTimeInfo
                case (byte)OperationType.UpdateContainerRealTimeInfo:
                    {
                        UpdateContainerRealTimeInfoTask(operationRequest);
                    }
                    break;
                #endregion

                #region ContainerMove
                case (byte)OperationType.ContainerMove:
                    {
                        ContainerMoveTask(operationRequest);
                    }
                    break;
                #endregion
            }
        }

        public bool Login(string UniqueID,out string answerUniqueID)
        {
            string[] requestItem = new string[1];
            requestItem[0] = "answerUniqueID";
            string[] returnData = _server.database.GetDataByUniqueID(UniqueID,requestItem,"user");
            answerUniqueID = NotImplement.STRING;
            if(returnData!=null)
                answerUniqueID = returnData[0];
            if (answerUniqueID == NotImplement.STRING)
            {
                return false;
            }

            requestItem = new string[3];
            requestItem[0] = "answerName";
            requestItem[1] = "answerAccount";
            requestItem[2] = "soulLimit";

            returnData = _server.database.GetDataByUniqueID(answerUniqueID,requestItem,"answer");

            answer = new Answer(guid, answerUniqueID, returnData[0], returnData[1], int.Parse(returnData[2]), this);

            uniqueID = UniqueID;
            _server.uniqueIDGuidConcurrentDictionary.TryAdd(UniqueID, guid);
            _server.AddAnswer(guid,answer);
            return true;
        }

        private void LoginTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count < 2)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "Login Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string account = (string)operationRequest.Parameters[(byte)LoginParameterItem.Account];
                string password = (string)operationRequest.Parameters[(byte)LoginParameterItem.Password];

                string uniqueID;
                if (_server.database.LoginCheck(account, password, out uniqueID))
                {
                    if (!_server.uniqueIDGuidConcurrentDictionary.ContainsKey(uniqueID))
                    {
                        
                        string answerUniqueID;
                        if (Login(uniqueID, out answerUniqueID))
                        {
                            Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)LoginResponseItem.UniqueID,uniqueID},
                                            {(byte)LoginResponseItem.AnswerUniqueID,answerUniqueID},
                                            {(byte)LoginResponseItem.AnswerName,answer.answerName},
                                            {(byte)LoginResponseItem.AnswerAccount,answer.answerAccount},
                                            {(byte)LoginResponseItem.SoulLimit,answer.soulLimit},
                                        };

                            OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter)
                            {
                                ReturnCode = (short)ErrorType.NoError,
                                DebugMessage = ""
                            };

                            SendOperationResponse(response, new SendParameters());
                        }
                        else
                        {
                            Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)GetAnswerKeyItem.UniqueID,uniqueID},
                                            {(byte)GetAnswerKeyItem.AnswerAccount,account}
                                        };

                            OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter)
                            {
                                ReturnCode = (short)ErrorType.NoExist,
                                DebugMessage = "尚未取得鑰匙!"
                            };
                            SendOperationResponse(response, new SendParameters());
                        }
                    }
                    else
                    {
                        OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                        {
                            ReturnCode = (short)ErrorType.InvalidOperation,
                            DebugMessage = "此帳號已經登入!"
                        };
                        SendOperationResponse(response, new SendParameters());
                    }
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.InvalidOperation,
                        DebugMessage = "帳號密碼錯誤!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void GetAnswerKeyTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count < 4)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "GetAnswerKey Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string uniqueID = (string)operationRequest.Parameters[(byte)GetAnswerKeyItem.UniqueID];
                string answerName = (string)operationRequest.Parameters[(byte)GetAnswerKeyItem.AnswerName];
                string answerAccount = (string)operationRequest.Parameters[(byte)GetAnswerKeyItem.AnswerAccount];
                int soulLimit = (int)operationRequest.Parameters[(byte)GetAnswerKeyItem.SoulLimit];
                string answerUniqueID = Guid.NewGuid().ToString();

                string[] insertItem = new string[] { "UniqueID", "answerName", "answerAccount", "soulLimit" };
                string[] insertValues = new string[] { answerUniqueID, answerName, answerAccount, soulLimit.ToString() };
                if (_server.database.InsertData(insertItem, insertValues, "answer") && _server.database.UpdateDataByUniqueID(uniqueID, new string[] { "answerUniqueID" }, new string[] { answerUniqueID }, "user"))
                {
                    Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)GetAnswerKeyItem.AnswerUniqueID,answerUniqueID},
                                            {(byte)GetAnswerKeyItem.AnswerName,answerName},
                                            {(byte)GetAnswerKeyItem.AnswerAccount,answerAccount},
                                            {(byte)GetAnswerKeyItem.SoulLimit,soulLimit}
                                        };
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter)
                    {
                        ReturnCode = (short)ErrorType.NoError,
                        DebugMessage = ""
                    };

                    SendOperationResponse(response, new SendParameters());
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.InvalidOperation,
                        DebugMessage = "無法發送!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void GetSoulUniqueIDListTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 1)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "GetSoulUniqueIDListTask Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string answerUniqueID = (string)operationRequest.Parameters[(byte)GetSoulUniqueIDListItem.AnswerUniqueID];
                string[] responseData = _server.database.GetDataByUniqueID(answerUniqueID, new string[] { "soulUniqueIDList" }, "answer");
                string responseSoulUniqueIDList = responseData[0];
                if (responseData != null && responseSoulUniqueIDList != NotImplement.STRING)
                {
                    Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)GetSoulUniqueIDListItem.SoulUniqueIDList,responseSoulUniqueIDList},
                                        };
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter)
                    {
                        ReturnCode = (short)ErrorType.NoError,
                        DebugMessage = ""
                    };

                    SendOperationResponse(response, new SendParameters());
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.NoExist,
                        DebugMessage = "沒有靈魂!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void CreateSoulTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 3)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "CreateSoul Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string answerUniqueID = (string)operationRequest.Parameters[(byte)CreateSoulItem.AnswerUniqueID];
                string soulName = (string)operationRequest.Parameters[(byte)CreateSoulItem.SoulName];
                int containerLimit = (int)operationRequest.Parameters[(byte)CreateSoulItem.ContainerLimit];
                string soulUniqueID = Guid.NewGuid().ToString();
                string[] responseData = _server.database.GetDataByUniqueID(answerUniqueID, new string[] { "soulUniqueIDList","soulLimit" }, "answer");
                string soulUniqueIDList = responseData[0];
                int soulLimit = int.Parse(responseData[1]);
                string[] soulUniqueIDs = soulUniqueIDList.Split(',');
                int soulNumber = (soulUniqueIDList == "") ? 0 : soulUniqueIDs.Length;
                if(soulNumber<soulLimit)
                {
                    if (_server.database.InsertData(new string[] { "UniqueID", "soulName", "containerLimit" }, new string[] { soulUniqueID, soulName, containerLimit.ToString() }, "soul"))
                    {
                        soulUniqueIDList=(soulNumber==0)?soulUniqueID:(soulUniqueIDList+","+soulUniqueID);
                        if(_server.database.UpdateDataByUniqueID(answerUniqueID,new string[]{"soulUniqueIDList"},new string[]{soulUniqueIDList},"answer"))
                        {
                            OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                            {
                                ReturnCode = (short)ErrorType.NoError,
                                DebugMessage = ""
                            };
                            SendOperationResponse(response, new SendParameters());
                        }
                        else
                        {
                            OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                            {
                                ReturnCode = (short)ErrorType.InvalidParameter,
                                DebugMessage = "創造靈魂錯誤!"
                            };
                            SendOperationResponse(response, new SendParameters());
                        }
                    }
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.InvalidOperation,
                        DebugMessage = "靈魂已達上限"
                    };
                    this.SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void GetSoulInfoTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 1)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "GetSoulInfo Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string soulUniqueID = (string)operationRequest.Parameters[(byte)GetSoulInfoItem.SoulUniqueID];
                string[] responseData = _server.database.GetDataByUniqueID(soulUniqueID,new string[]{"soulName","containerLimit"},"soul");
                if(responseData != null)
                {
                    Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)GetSoulInfoItem.SoulName,responseData[0]},
                                            {(byte)GetSoulInfoItem.ContainerLimit,responseData[1]},
                                            {(byte)GetSoulInfoItem.SoulUniqueID,soulUniqueID}
                                        };

                    OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter)
                    {
                        ReturnCode = (short)ErrorType.NoError,
                        DebugMessage = ""
                    };
                    SendOperationResponse(response, new SendParameters());
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.NoExist,
                        DebugMessage = "此靈魂不存在!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void GetContainerUniqueIDListTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 1)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "GetContainerUniqueIDListTask Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string soulUniqueID = (string)operationRequest.Parameters[(byte)GetContainerUniqueIDListItem.SoulUniqueID];
                string[] responseData = _server.database.GetDataByUniqueID(soulUniqueID, new string[] { "containerUniqueIDList" }, "soul");
                string responseContainerUniqueIDList = responseData[0];
                if (responseData != null && responseContainerUniqueIDList != NotImplement.STRING)
                {
                    Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)GetContainerUniqueIDListItem.SoulUniqueID,soulUniqueID},
                                            {(byte)GetContainerUniqueIDListItem.ContainerUniqueIDList,responseContainerUniqueIDList}
                                        };
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter)
                    {
                        ReturnCode = (short)ErrorType.NoError,
                        DebugMessage = ""
                    };

                    SendOperationResponse(response, new SendParameters());
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.NoExist,
                        DebugMessage = "沒有容器!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void GetContainerInfoTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 1)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "GetContainerInfo Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string containerUniqueID = (string)operationRequest.Parameters[(byte)GetContainerInfoItem.ContainerUniqueID];
                object[] responseData = _server.database.GetDataByUniqueID
                    (
                        containerUniqueID,
                        new string[] 
                        { 
                            "containerName","locationUniqueID",
                            "positionX","positionY","positionZ",
                            "angle","soulLimit","status"
                        },
                        new TypeCode[]
                        {
                            TypeCode.String,TypeCode.String,
                            TypeCode.Single,TypeCode.Single,TypeCode.Single,
                            TypeCode.Single,TypeCode.Int32,TypeCode.Int32
                        },
                        "container"
                    );
                if (responseData != null)
                {
                    Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)GetContainerInfoItem.ContainerUniqueID,containerUniqueID},
                                            {(byte)GetContainerInfoItem.ContainerName,responseData[0]},
                                            {(byte)GetContainerInfoItem.LocationUniqueID,responseData[1]},
                                            {(byte)GetContainerInfoItem.PositionX,responseData[2]},
                                            {(byte)GetContainerInfoItem.PositionY,responseData[3]},
                                            {(byte)GetContainerInfoItem.PositionZ,responseData[4]},
                                            {(byte)GetContainerInfoItem.Angle,responseData[5]},
                                            {(byte)GetContainerInfoItem.SoulLimit,responseData[6]},
                                            {(byte)GetContainerInfoItem.Status,responseData[7]}
                                        };
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter)
                    {
                        ReturnCode = (short)ErrorType.NoError,
                        DebugMessage = ""
                    };
                    SendOperationResponse(response, new SendParameters());
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.NoExist,
                        DebugMessage = "此容器不存在!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void CreateContainerTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 9)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "CreateSoul Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string soulUniqueID = (string)operationRequest.Parameters[(byte)CreateContainerItem.SoulUniqueID];
                string containerUniqueID = Guid.NewGuid().ToString();
                string containerName = (string)operationRequest.Parameters[(byte)CreateContainerItem.ContainerName];
                string locationUniqueID = (string)operationRequest.Parameters[(byte)CreateContainerItem.LocationUniqueID];
                float positionX = Convert.ToSingle(operationRequest.Parameters[(byte)CreateContainerItem.PositionX]);
                float positionY = Convert.ToSingle(operationRequest.Parameters[(byte)CreateContainerItem.PositionY]);
                float positionZ = Convert.ToSingle(operationRequest.Parameters[(byte)CreateContainerItem.PositionZ]);
                float angle = Convert.ToSingle(operationRequest.Parameters[(byte)CreateContainerItem.Angle]);
                int soulLimit = Convert.ToInt32(operationRequest.Parameters[(byte)CreateContainerItem.SoulLimit]);
                int status = Convert.ToInt32(operationRequest.Parameters[(byte)CreateContainerItem.Status]);

                string[] responseData = _server.database.GetDataByUniqueID(soulUniqueID, new string[] { "containerUniqueIDList", "containerLimit" }, "soul");
                string containerUniqueIDList = responseData[0];
                int containerLimit = int.Parse(responseData[1]);
                string[] containerUniqueIDs = containerUniqueIDList.Split(',');
                int containerNumber = (containerUniqueIDList == "") ? 0 : containerUniqueIDs.Length;
                if (containerNumber < containerLimit)
                {
                    if (
                        _server.database.InsertData
                        (
                        insertItem:new string[] { "UniqueID","containerName", "locationUniqueID", "positionX", "positionY", "positionZ", "angle" ,"soulLimit", "status" }, 
                        insertValue:new object[] { containerUniqueID,containerName,locationUniqueID,positionX,positionY,positionZ,angle,soulLimit,status }, 
                        table:"container"
                        )
                       )
                    {
                        containerUniqueIDList = (containerNumber == 0) ? containerUniqueID : (containerUniqueIDList + "," + containerUniqueID);
                        if (_server.database.UpdateDataByUniqueID(soulUniqueID, new string[] { "containerUniqueIDList" }, new string[] { containerUniqueIDList }, "soul"))
                        {
                            Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)CreateContainerItem.SoulUniqueID,soulUniqueID}
                                        };
                            OperationResponse response = new OperationResponse(operationRequest.OperationCode,parameter)
                            {
                                ReturnCode = (short)ErrorType.NoError,
                                DebugMessage = ""
                            };
                            SendOperationResponse(response, new SendParameters());
                        }
                        else
                        {
                            OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                            {
                                ReturnCode = (short)ErrorType.InvalidParameter,
                                DebugMessage = "創造容器錯誤!"
                            };
                            SendOperationResponse(response, new SendParameters());
                        }
                    }
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.InvalidOperation,
                        DebugMessage = "容器已達上限"
                    };
                    this.SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void OnlineTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 3)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "Online Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                Guid soulUniqueID = Guid.Parse((string)operationRequest.Parameters[(byte)OnlineItem.SoulUniqueID]);
                string soulName = (string)operationRequest.Parameters[(byte)OnlineItem.SoulName];
                int containerLimit = Convert.ToInt32(operationRequest.Parameters[(byte)OnlineItem.ContainerLimit]);

                if(answer.soulList.Count<answer.soulLimit && !answer.soulList.Exists(x=>x.soulUniqueID==soulUniqueID))
                {
                    if(answer.AddSoul(new Soul(soulUniqueID,soulName,containerLimit,answer,true)))
                    {
                        Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)OnlineItem.SoulUniqueID,soulUniqueID.ToString()}
                                        };
                        OperationResponse response = new OperationResponse(operationRequest.OperationCode,parameter)
                        {
                            ReturnCode = (short)ErrorType.NoError,
                            DebugMessage = ""
                        };
                        SendOperationResponse(response, new SendParameters());
                        //_server.BroadcastToContainerList();
                    }
                    else
                    {
                        OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                        {
                            ReturnCode = (short)ErrorType.InvalidParameter,
                            DebugMessage = "上線錯誤!"
                        };
                        SendOperationResponse(response, new SendParameters());
                    }
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.InvalidOperation,
                        DebugMessage = "錯誤  可能已達靈魂上限或已經上線"
                    };
                    this.SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void ProjectToSceneTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 10)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "ProjectToSceneTask Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                Guid soulUniqueID = Guid.Parse((string)operationRequest.Parameters[(byte)ProjectToSceneItem.SoulUniqueID]);
                Guid containerUniqueID = Guid.Parse((string)operationRequest.Parameters[(byte)ProjectToSceneItem.ContainerUniqueID]);
                string containerName = (string)operationRequest.Parameters[(byte)ProjectToSceneItem.ContainerName];
                int soulLimit = Convert.ToInt32(operationRequest.Parameters[(byte)ProjectToSceneItem.SoulLimit]);
                string sceneUniqueID = (string)operationRequest.Parameters[(byte)ProjectToSceneItem.SceneUniqueID];
                float positionX = Convert.ToSingle(operationRequest.Parameters[(byte)ProjectToSceneItem.PositionX]);
                float positionY = Convert.ToSingle(operationRequest.Parameters[(byte)ProjectToSceneItem.PositionY]);
                float positionZ = Convert.ToSingle(operationRequest.Parameters[(byte)ProjectToSceneItem.PositionZ]);
                float angle = Convert.ToSingle(operationRequest.Parameters[(byte)ProjectToSceneItem.Angle]);
                int status = Convert.ToInt32(operationRequest.Parameters[(byte)ProjectToSceneItem.Status]);

                Container container = new Container
                            (
                                _containerUniqueID: containerUniqueID,
                                _containerName: containerName,
                                _soulLimit: soulLimit,
                                _positionX: positionX,
                                _positionY: positionY,
                                _positionZ: positionZ,
                                _angle: angle,
                                _status: status,
                                _location: _server.sceneDictionary[sceneUniqueID]
                            );
                Soul soul = answer.soulList.Find(x => x.soulUniqueID == soulUniqueID);
                soul.AddContainer(container);
                container.AddSoul(soul);

                if (_server.sceneDictionary[sceneUniqueID].AddContainer(containerUniqueID, container))
                {
                    Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)ProjectToSceneItem.ContainerUniqueID,containerUniqueID.ToString()},
                                            {(byte)ProjectToSceneItem.SceneUniqueID,sceneUniqueID}
                                        };
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter)
                    {
                        ReturnCode = (short)ErrorType.NoError,
                        DebugMessage = ""
                    };
                    SendOperationResponse(response, new SendParameters());

                    _server.BroadcastToContainerList
                        (
                        receivers:        _server.sceneDictionary[sceneUniqueID].BroadcastList(),
                        broadcastType:    BroadcastType.ProjectContainer,
                        dataPackage:      new Tuple<byte,object>[] 
                            {
                                new Tuple<byte,object>((byte)ProjectContainerBroadcastItem.SceneUniqueID,sceneUniqueID),
                                new Tuple<byte,object>((byte)ProjectContainerBroadcastItem.ContainerUniqueID,containerUniqueID.ToString()),
                                new Tuple<byte,object>((byte)ProjectContainerBroadcastItem.ContainerName,containerName),
                                new Tuple<byte,object>((byte)ProjectContainerBroadcastItem.PositionX,positionX),
                                new Tuple<byte,object>((byte)ProjectContainerBroadcastItem.PositionY,positionY),
                                new Tuple<byte,object>((byte)ProjectContainerBroadcastItem.PositionZ,positionZ),
                                new Tuple<byte,object>((byte)ProjectContainerBroadcastItem.Angle,angle),
                                new Tuple<byte,object>((byte)ProjectContainerBroadcastItem.SoulLimit,soulLimit),
                                new Tuple<byte,object>((byte)ProjectContainerBroadcastItem.Status,status)
                            }
                        );
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.InvalidOperation,
                        DebugMessage = "投影錯誤!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void GetSceneRealTimeInfoTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 1)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "GetSceneInfo Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string sceneUniqueID = (string)operationRequest.Parameters[(byte)GetSceneRealTimeInfoItem.SceneUniqueID];
                if(_server.sceneDictionary.ContainsKey(sceneUniqueID))
                {
                    Scene scene = _server.sceneDictionary[sceneUniqueID];
                    StringBuilder containerUniqueIDList = new StringBuilder();
                    foreach (Container container in scene.containerConcurrentDictionary.Values)
                        containerUniqueIDList.AppendLine(container.containerUniqueID.ToString());
                    Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)GetSceneRealTimeInfoItem.SceneUniqueID,sceneUniqueID},
                                            {(byte)GetSceneRealTimeInfoItem.SceneName,scene.name},
                                            {(byte)GetSceneRealTimeInfoItem.ContainerUniqueIDList,containerUniqueIDList.ToString()}
                                        };
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter)
                    {
                        ReturnCode = (short)ErrorType.NoError,
                        DebugMessage = ""
                    };
                    SendOperationResponse(response, new SendParameters());
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.InvalidParameter,
                        DebugMessage = "場景不存在!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
                

            }
        }
        private void GetContainerRealTimeInfoTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 2)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "GetContainerRealTimeInfo Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string sceneUniqueID = (string)operationRequest.Parameters[(byte)GetContainerRealTimeInfoItem.LocationUniqueID];
                Guid containerUniqueID = Guid.Parse((string)operationRequest.Parameters[(byte)GetContainerRealTimeInfoItem.ContainerUniqueID]);
                if (_server.sceneDictionary.ContainsKey(sceneUniqueID)&&_server.sceneDictionary[sceneUniqueID].containerConcurrentDictionary.ContainsKey(containerUniqueID))
                {
                    Container container = _server.sceneDictionary[sceneUniqueID].containerConcurrentDictionary[containerUniqueID];
                    Dictionary<byte, object> parameter = new Dictionary<byte, object>
                                        {
                                            {(byte)GetContainerRealTimeInfoItem.ContainerUniqueID,container.containerUniqueID.ToString()},
                                            {(byte)GetContainerRealTimeInfoItem.ContainerName,container.containerName},
                                            {(byte)GetContainerRealTimeInfoItem.LocationUniqueID,sceneUniqueID},
                                            {(byte)GetContainerRealTimeInfoItem.PositionX,container.positionX},
                                            {(byte)GetContainerRealTimeInfoItem.PositionY,container.positionY},
                                            {(byte)GetContainerRealTimeInfoItem.PositionZ,container.positionZ},
                                            {(byte)GetContainerRealTimeInfoItem.Angle,container.angle},
                                            {(byte)GetContainerRealTimeInfoItem.SoulLimit,container.soulLimit},
                                            {(byte)GetContainerRealTimeInfoItem.Status,container.status}
                                        };
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode, parameter)
                    {
                        ReturnCode = (short)ErrorType.NoError,
                        DebugMessage = ""
                    };
                    SendOperationResponse(response, new SendParameters());
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.NoExist,
                        DebugMessage = "此容器不存在!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void UpdateContainerRealTimeInfoTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 6)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "UpdateContainerRealTimeInfo Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string sceneUniqueID = (string)operationRequest.Parameters[(byte)UpdateContainerRealTimeInfoItem.SceneUniqueID];
                Guid containerUniqueID = Guid.Parse((string)operationRequest.Parameters[(byte)UpdateContainerRealTimeInfoItem.ContainerUniqueID]);
                float positionX = Convert.ToSingle(operationRequest.Parameters[(byte)UpdateContainerRealTimeInfoItem.PositionX]);
                float positionY = Convert.ToSingle(operationRequest.Parameters[(byte)UpdateContainerRealTimeInfoItem.PositionY]);
                float positionZ = Convert.ToSingle(operationRequest.Parameters[(byte)UpdateContainerRealTimeInfoItem.PositionZ]);
                float angle = Convert.ToSingle(operationRequest.Parameters[(byte)UpdateContainerRealTimeInfoItem.Angle]);

                if(_server.sceneDictionary.ContainsKey(sceneUniqueID)&&_server.sceneDictionary[sceneUniqueID].containerConcurrentDictionary.ContainsKey(containerUniqueID))
                {
                    Container container = _server.sceneDictionary[sceneUniqueID].containerConcurrentDictionary[containerUniqueID];
                    container.Update(
                            new Tuple<string,object>[]
                            {
                                new Tuple<string,object>("positionX",positionX),
                                new Tuple<string,object>("positionY",positionY),
                                new Tuple<string,object>("positionZ",positionZ),
                                new Tuple<string,object>("angle",angle)
                            }
                        );
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.NoError,
                        DebugMessage = ""
                    };
                    SendOperationResponse(response, new SendParameters());

                    _server.BroadcastToContainerList
                        (
                        receivers: _server.sceneDictionary[sceneUniqueID].BroadcastList(),
                        broadcastType: BroadcastType.UpdateContainerRealTimeInfo,
                        dataPackage: new Tuple<byte, object>[] 
                            {
                                new Tuple<byte,object>((byte)UpdateContainerRealTimeInfoBroadcastItem.SceneUniqueID,sceneUniqueID),
                                new Tuple<byte,object>((byte)UpdateContainerRealTimeInfoBroadcastItem.ContainerUniqueID,containerUniqueID.ToString()),
                                new Tuple<byte,object>((byte)UpdateContainerRealTimeInfoBroadcastItem.PositionX,positionX),
                                new Tuple<byte,object>((byte)UpdateContainerRealTimeInfoBroadcastItem.PositionY,positionY),
                                new Tuple<byte,object>((byte)UpdateContainerRealTimeInfoBroadcastItem.PositionZ,positionZ),
                                new Tuple<byte,object>((byte)UpdateContainerRealTimeInfoBroadcastItem.Angle,angle),
                            }
                        );
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.InvalidOperation,
                        DebugMessage = "UpdateContainerRealTimeInfo 錯誤!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
            }
        }
        private void ContainerMoveTask(OperationRequest operationRequest)
        {
            if (operationRequest.Parameters.Count != 5)
            {
                OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                {
                    ReturnCode = (short)ErrorType.InvalidParameter,
                    DebugMessage = "ContainerMoveTask Parameter Error"
                };
                this.SendOperationResponse(response, new SendParameters());
            }
            else
            {
                string sceneUniqueID = (string)operationRequest.Parameters[(byte)ContainerMoveItem.SceneUniqueID];
                Guid containerUniqueID = Guid.Parse((string)operationRequest.Parameters[(byte)ContainerMoveItem.ContainerUniqueID]);
                MoveType moveType = (MoveType)operationRequest.Parameters[(byte)ContainerMoveItem.MoveType];
                MoveDirection moveDirection = (MoveDirection)operationRequest.Parameters[(byte)ContainerMoveItem.Direction];
                Phase7.Level level = (Phase7.Level)operationRequest.Parameters[(byte)ContainerMoveItem.Level];

                if (_server.sceneDictionary.ContainsKey(sceneUniqueID) && _server.sceneDictionary[sceneUniqueID].containerConcurrentDictionary.ContainsKey(containerUniqueID))
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.NoError,
                        DebugMessage = ""
                    };
                    SendOperationResponse(response, new SendParameters());

                    _server.BroadcastToContainerList
                        (
                        receivers: _server.sceneDictionary[sceneUniqueID].BroadcastList(),
                        broadcastType: BroadcastType.ContainerMove,
                        dataPackage: new Tuple<byte, object>[] 
                            {
                                new Tuple<byte,object>((byte)ContainerMoveBroadcastItem.SceneUniqueID,sceneUniqueID),
                                new Tuple<byte,object>((byte)ContainerMoveBroadcastItem.ContainerUniqueID,containerUniqueID.ToString()),
                                new Tuple<byte,object>((byte)ContainerMoveBroadcastItem.MoveType,moveType),
                                new Tuple<byte,object>((byte)ContainerMoveBroadcastItem.Direction,moveDirection),
                                new Tuple<byte,object>((byte)ContainerMoveBroadcastItem.Level,level),
                            }
                        );
                }
                else
                {
                    OperationResponse response = new OperationResponse(operationRequest.OperationCode)
                    {
                        ReturnCode = (short)ErrorType.InvalidOperation,
                        DebugMessage = "UpdateContainerRealTimeInfo 錯誤!"
                    };
                    SendOperationResponse(response, new SendParameters());
                }
            }
        }
    }
}
