using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSProtocol;

namespace DSStructureForClient
{
    public class Answer
    {
        public Answer(string _answerUniqueID, string _answerName, string _answerAccount, int _soulLimit)
        {
            answerUniqueID = _answerUniqueID;
            answerName = _answerName;
            answerAccount = _answerAccount;
            soulLimit = _soulLimit;
            soulList = new List<Soul>();
        }

        public string answerUniqueID { get; protected set; }
        public string answerName { get; protected set; }
        public string answerAccount { get; protected set; }
        public List<Soul> soulList { get; protected set; }
        public int soulLimit { get; protected set; }
        public string soulUniqueIDList { get; protected set; }

        public Tuple<string, object>[] Get(string[] requestData)
        {
            int length = requestData.Length;
            Tuple<string, object>[] returnData = new Tuple<string, object>[length];
            for (int i = 0; i < length; i++)
            {
                switch (requestData[i])
                {
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
        public void UpdateSoul(string soulUniqueID, Tuple<string, object>[] updateData)
        {
            soulList.Find(x => x.soulUniqueID == soulUniqueID).Update(updateData);
        }
        public int RemoveSoul(string soulUniqueID)
        {
            return soulList.RemoveAll(x => x.soulUniqueID == soulUniqueID);
        }
    }

    public class Soul
    {
        public Soul(string _soulUniqueID, string _soulName, int _containerLimit, Answer _sourceAnswer)
        {
            soulUniqueID = _soulUniqueID;
            soulName = _soulName;
            containerLimit = _containerLimit;
            loginTime = DateTime.Now;
            containerList = new List<Container>();
            sourceAnswer = _sourceAnswer;
        }

        public string soulUniqueID { get; protected set; }
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
                        soulUniqueID = (string)_data.Item2;
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
        public void UpdateContainer(string containerID, Tuple<string, object>[] updateData)
        {
            containerList.Find(x => x.containerUniqueID == containerID).Update(updateData);
        }
        public int RemoveContainer(string containerID)
        {
            return containerList.RemoveAll(x => x.containerUniqueID == containerID);
        }
    }

    public class Container
    {
        public Container(string _containerUniqueID)
        {
            containerName = NotImplement.STRING;
            containerUniqueID = _containerUniqueID;
            soulLimit = NotImplement.INT;
            positionX = NotImplement.FLOAT;
            positionY = NotImplement.FLOAT;
            positionZ = NotImplement.FLOAT;
            angle = NotImplement.FLOAT;
            soulList = new List<Soul>();
            status = NotImplement.INT;
        }
        public string containerUniqueID { get; protected set; }
        public string containerName { get; protected set; }
        public string locationUniqueID { get; protected set; }
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
                    case "locationUniqueID":
                        returnData[i] = new Tuple<string, object>("locationUniqueID", locationUniqueID);
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
                        containerUniqueID = (string)_data.Item2;
                        break;
                    case "locationUniqueID":
                        locationUniqueID = (string)_data.Item2;
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
        public void UpdateSoul(string soulUniqueID, Tuple<string, object>[] updateData)
        {
            soulList.Find(x => x.soulUniqueID == soulUniqueID).Update(updateData);
        }
        public int RemoveSoul(string soulUniqueID)
        {
            return soulList.RemoveAll(x => x.soulUniqueID == soulUniqueID);
        }
    }
}
