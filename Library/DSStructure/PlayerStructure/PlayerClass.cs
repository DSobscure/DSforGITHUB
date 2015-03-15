using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSProtocol;
using DSStructure.SpatiotemporalStructure.SpaceClass;
using DSServer;

namespace DSStructure.PlayerStructure
{
    public class Answer
    {
        public Answer(Guid _guid, string _answerUniqueID, string _answerName, string _answerAccount, int _soulLimit, DSPeer _sourcePeer)
        {
            guid = _guid;
            answerUniqueID = _answerUniqueID;
            answerName = _answerName;
            answerAccount = _answerAccount;
            soulLimit = _soulLimit;
            sourcePeer = _sourcePeer;
            soulList = new List<Soul>();
        }

        public Guid guid { get; protected set; }
        public string answerUniqueID { get; protected set; }
        public string answerName { get; protected set; }
        public string answerAccount { get; protected set; }
        public List<Soul> soulList { get; protected set; }
        public int soulLimit { get; protected set; }
        public DSPeer sourcePeer { get; protected set; }

        public Tuple<string, object>[] Get(string[] requestData)
        {
            int length = requestData.Length;
            Tuple<string, object>[] returnData = new Tuple<string, object>[length];
            for (int i = 0; i < length; i++)
            {
                switch (requestData[i])
                {
                    case "guid":
                        returnData[i] = new Tuple<string, object>("guid", guid);
                        break;
                    case "answerUniqueID":
                        returnData[i] = new Tuple<string, object>("answerUniqueID", answerUniqueID);
                        break;
                    case "answerName":
                        returnData[i] = new Tuple<string, object>("answerName", answerName);
                        break;
                    case "answerAccount":
                        returnData[i] = new Tuple<string, object>("answerAccount", answerAccount);
                        break;
                    case "soulLimit":
                        returnData[i] = new Tuple<string, object>("soulLimit", soulLimit);
                        break;
                }
            }
            return returnData;
        }
        public void Update(Tuple<string, object>[] updateData)
        {
            foreach (Tuple<string, object> _data in updateData)
            {
                switch (_data.Item1)
                {
                    case "guid":
                        guid = (Guid)_data.Item2;
                        break;
                    case "answerUniqueID":
                        answerUniqueID = (string)_data.Item2;
                        break;
                    case "answerName":
                        answerName = (string)_data.Item2;
                        break;
                    case "answerAccount":
                        answerAccount = (string)_data.Item2;
                        break;
                    case "soulLimit":
                        soulLimit = (int)_data.Item2;
                        break;
                }
            }
        }

        public bool AddSoul(Soul soul)
        {
            if (soulList.Count < soulLimit)
            {
                soulList.Add(soul);
                return true;
            }
            else
                return false;
        }
        public void UpdateSoul(Guid soulUniqueID, Tuple<string, object>[] updateData)
        {
            soulList.Find(x=>x.soulUniqueID==soulUniqueID).Update(updateData);
        }
        public int RemoveSoul(Guid soulUniqueID)
        {
            return soulList.RemoveAll (x=>x.soulUniqueID==soulUniqueID);
        }   
    }

    public class Soul
    {
        public Soul(Guid _soulUniqueID,string _soulName,int _containerLimit,Answer _sourceAnswer,bool _active)
        {
            soulUniqueID = _soulUniqueID;
            soulName = _soulName;
            containerLimit = _containerLimit;
            loginTime = DateTime.Now;
            containerList = new List<Container>();
            sourceAnswer = _sourceAnswer;
            active = _active;
        }

        public Guid soulUniqueID { get; protected set; }
        public string soulName { get; protected set; }
        public int containerLimit { get; protected set; }
        public DateTime loginTime { get; protected set; }
        public bool active { get; protected set; }
        public Answer sourceAnswer { get; protected set; }
        public List<Container> containerList { get; protected set; }

        public Tuple<string, object>[] Get(string[] requestData)
        {
            int length = requestData.Length;
            Tuple<string, object>[] returnData = new Tuple<string, object>[length];
            for (int i = 0; i < length; i++)
            {
                switch (requestData[i])
                {
                    case "soulUniqueID":
                        returnData[i] = new Tuple<string, object>("soulUniqueID", soulUniqueID);
                        break;
                    case "soulName":
                        returnData[i] = new Tuple<string, object>("soulName", soulName);
                        break;
                    case "containerLimit":
                        returnData[i] = new Tuple<string, object>("containerLimit", containerLimit);
                        break;
                    case "loginTime":
                        returnData[i] = new Tuple<string, object>("loginTime", loginTime);
                        break;
                    case "active":
                        returnData[i] = new Tuple<string, object>("active", active);
                        break;
                }
            }
            return returnData;
        }
        public void Update(Tuple<string, object>[] updateData)
        {
            foreach (Tuple<string, object> _data in updateData)
            {
                switch (_data.Item1)
                {
                    case "soulUniqueID":
                        soulUniqueID = (Guid)_data.Item2;
                        break;
                    case "soulName":
                        soulName = (string)_data.Item2;
                        break;
                    case "containerLimit":
                        containerLimit = (int)_data.Item2;
                        break;
                    case "loginTime":
                        loginTime = (DateTime)_data.Item2;
                        break;
                    case "active":
                        active = (bool)_data.Item2;
                        break;
                }
            }
        }

        public bool AddContainer(Container container)
        {
            if (containerList.Count < containerLimit)
            {
                containerList.Add(container);
                return true;
            }
            else
                return false;
        }
        public void UpdateContainer(Guid containerID, Tuple<string, object>[] updateData)
        {
            containerList.Find(x=>x.containerUniqueID==containerID).Update(updateData);
        }
        public int RemoveContainer(Guid containerID)
        {
            return containerList.RemoveAll(x => x.containerUniqueID == containerID);
        }       
    }

    public class Container
    {
        public Container(Guid _containerUniqueID, string _containerName,int _soulLimit,float _positionX,float _positionY,float _positionZ,float _angle,int _status,Scene _location)
        {
            containerName = _containerName;
            containerUniqueID = _containerUniqueID;
            soulLimit = _soulLimit;
            positionX = _positionX;
            positionY = _positionY;
            positionZ = _positionZ;
            angle = _angle;
            soulList = new List<Soul>();
            status = _status;
            location = _location;
        }
        public Guid containerUniqueID { get; protected set; }
        public string containerName { get; protected set; }
        public Scene location { get; protected set; }
        public float positionX { get; protected set; }
        public float positionY { get; protected set; }
        public float positionZ { get; protected set; }
        public float angle { get; protected set; }
        public int soulLimit { get; protected set; }
        public int status { get; protected set; }
        public List<Soul> soulList { get; protected set; }

        public Tuple<string, object>[] Get(string[] requestData)
        {
            int length = requestData.Length;
            Tuple<string, object>[] returnData = new Tuple<string, object>[length];
            for (int i = 0; i < length; i++)
            {
                switch (requestData[i])
                {
                    case "containerName":
                        returnData[i] = new Tuple<string, object>("containerName", containerName);
                        break;
                    case "containerUniqueID":
                        returnData[i] = new Tuple<string, object>("containerUniqueID", containerUniqueID);
                        break;
                    case "positionX":
                        returnData[i] = new Tuple<string, object>("positionX", positionX);
                        break;
                    case "positionY":
                        returnData[i] = new Tuple<string, object>("positionY", positionY);
                        break;
                    case "positionZ":
                        returnData[i] = new Tuple<string, object>("positionZ", positionZ);
                        break;
                    case "angle":
                        returnData[i] = new Tuple<string, object>("angle", angle);
                        break;
                    case "soulLimit":
                        returnData[i] = new Tuple<string, object>("soulLimit", soulLimit);
                        break;
                    case "status":
                        returnData[i] = new Tuple<string, object>("status", status);
                        break;
                }
            }
            return returnData;
        }
        public void Update(Tuple<string, object>[] updateData)
        {
            foreach (Tuple<string, object> _data in updateData)
            {
                switch (_data.Item1)
                {
                    case "containerName":
                        containerName = (string)_data.Item2;
                        break;
                    case "containerUniqueID":
                        containerUniqueID = (Guid)_data.Item2;
                        break;
                    case "location":
                        location = (Scene)_data.Item2;
                        break;
                    case "positionX":
                        positionX = (float)_data.Item2;
                        break;
                    case "positionY":
                        positionY = (float)_data.Item2;
                        break;
                    case "positionZ":
                        positionZ = (float)_data.Item2;
                        break;
                    case "angle":
                        angle = (float)_data.Item2;
                        break;
                    case "soulLimit":
                        soulLimit = (int)_data.Item2;
                        break;
                    case "status":
                        status = (int)_data.Item2;
                        break;
                }
            }
        }

        public bool AddSoul(Soul soul)
        {
            if (soulList.Count < soulLimit)
            {
                soulList.Add(soul);
                return true;
            }
            else
                return false;
        }
        public void UpdateSoul(Guid soulUniqueID, Tuple<string, object>[] updateData)
        {
            soulList.Find(x=>x.soulUniqueID==soulUniqueID).Update(updateData);
        }
        public int RemoveSoul(Guid soulUniqueID)
        {
            return soulList.RemoveAll(x => x.soulUniqueID == soulUniqueID);
        }
    }
}
