using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSStructure.PlayerStructure;

namespace DSStructure.SpatiotemporalStructure.SpaceClass
{
    public class Universe : Termination
    {
        public Universe(string _uniqueID, string _name, int terminationNumber, string terminationName, int _limit, TreeUniverse _sourceTreeUniverse)
            : base(terminationNumber, terminationName)
        {
            uniqueID = _uniqueID;
            name = _name;
            _containerNumberLimit = _limit;
            sourceTreeUniverse = _sourceTreeUniverse;
            etherFieldDictionary = new Dictionary<string, EtherField>();
        }
        public override int containerNumber
        {
            get { return etherFieldDictionary.Values.Sum(x => x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public TreeUniverse sourceTreeUniverse { get; protected set; }
        public Dictionary<string, EtherField> etherFieldDictionary { get; protected set; }

        public override Tuple<string, object>[] Get(string[] requestData)
        {
            int length = requestData.Length;
            Tuple<string, object>[] returnData = new Tuple<string, object>[length];
            for (int i = 0; i < length; i++)
            {
                switch (requestData[i])
                {
                    case "name":
                        returnData[i] = new Tuple<string, object>("name", name);
                        break;
                    case "uniqueID":
                        returnData[i] = new Tuple<string, object>("uniqueID", uniqueID);
                        break;
                    case "containerNumber":
                        returnData[i] = new Tuple<string, object>("containerNumber", containerNumber);
                        break;
                    case "containerNumberLimit":
                        returnData[i] = new Tuple<string, object>("containerNumberLimit", containerNumberLimit);
                        break;
                    case "TerminationName":
                        returnData[i] = new Tuple<string, object>("TerminationName", TerminationName);
                        break;
                    case "TerminationNumber":
                        returnData[i] = new Tuple<string, object>("TerminationNumber", TerminationNumber);
                        break;
                    case "TerminationTime":
                        returnData[i] = new Tuple<string, object>("TerminationTime", TerminationTime);
                        break;
                    case "TimeLevel":
                        returnData[i] = new Tuple<string, object>("TimeLevel", TimeLevel);
                        break;
                }
            }
            return returnData;
        }
        public override void Update(Tuple<string, object>[] updateData)
        {
            foreach (Tuple<string, object> _data in updateData)
            {
                switch (_data.Item1)
                {
                    case "name":
                        name = (string)_data.Item2;
                        break;
                    case "uniqueID":
                        uniqueID = (string)_data.Item2;
                        break;
                    case "TerminationName":
                        TerminationName = (string)_data.Item2;
                        break;
                    case "TerminationNumber":
                        TerminationNumber = (int)_data.Item2;
                        break;
                    case "TerminationTime":
                        TerminationTime = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in etherFieldDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddEtherField(string _ID,EtherField _etherField)
        {
            if (containerNumberLimit - containerNumber >= _etherField.containerNumberLimit)
            {
                etherFieldDictionary.Add(_ID, _etherField);
                return true;
            }
            else
                return false;
        }
        public void UpdateEtherField(string _ID, Tuple<string, object>[] updateData)
        {
            etherFieldDictionary[_ID].Update(updateData);
        }
        public bool RemoveEtherField(string _ID)
        {
            return etherFieldDictionary.Remove(_ID);
        }
    }

    public class EtherField : Termination
    {
        public EtherField(string _uniqueID, string _name, int terminationNumber, string terminationName, int _limit, Universe _sourceUniverse)
            : base(terminationNumber, terminationName)
        {
            name = _name;
            uniqueID = _uniqueID;
            _containerNumberLimit = _limit;
            sourceUniverse = _sourceUniverse;
            superclusterDictionary = new Dictionary<string, Supercluster>();
        }
        public override int containerNumber
        {
            get { return superclusterDictionary.Values.Sum(x => x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Universe sourceUniverse { get; protected set; }
        public Dictionary<string, Supercluster> superclusterDictionary { get; protected set; }

        public override Tuple<string, object>[] Get(string[] requestData)
        {
            int length = requestData.Length;
            Tuple<string, object>[] returnData = new Tuple<string, object>[length];
            for (int i = 0; i < length; i++)
            {
                switch (requestData[i])
                {
                    case "name":
                        returnData[i] = new Tuple<string, object>("name", name);
                        break;
                    case "uniqueID":
                        returnData[i] = new Tuple<string, object>("uniqueID", uniqueID);
                        break;
                    case "containerNumber":
                        returnData[i] = new Tuple<string, object>("containerNumber", containerNumber);
                        break;
                    case "containerNumberLimit":
                        returnData[i] = new Tuple<string, object>("containerNumberLimit", containerNumberLimit);
                        break;
                    case "TerminationName":
                        returnData[i] = new Tuple<string, object>("TerminationName", TerminationName);
                        break;
                    case "TerminationNumber":
                        returnData[i] = new Tuple<string, object>("TerminationNumber", TerminationNumber);
                        break;
                    case "TerminationTime":
                        returnData[i] = new Tuple<string, object>("TerminationTime", TerminationTime);
                        break;
                    case "TimeLevel":
                        returnData[i] = new Tuple<string, object>("TimeLevel", TimeLevel);
                        break;
                }
            }
            return returnData;
        }
        public override void Update(Tuple<string, object>[] updateData)
        {
            foreach (Tuple<string, object> _data in updateData)
            {
                switch (_data.Item1)
                {
                    case "name":
                        name = (string)_data.Item2;
                        break;
                    case "uniqueID":
                        uniqueID = (string)_data.Item2;
                        break;
                    case "TerminationName":
                        TerminationName = (string)_data.Item2;
                        break;
                    case "TerminationNumber":
                        TerminationNumber = (int)_data.Item2;
                        break;
                    case "TerminationTime":
                        TerminationTime = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in superclusterDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddSupercluster(string _ID,Supercluster _supercluster)
        {
            if (containerNumberLimit - containerNumber >= _supercluster.containerNumberLimit)
            {
                superclusterDictionary.Add(_ID, _supercluster);
                return true;
            }
            else
                return false;
        }
        public void UpdateSupercluster(string _ID, Tuple<string, object>[] updateData)
        {
            superclusterDictionary[_ID].Update(updateData);
        }
        public bool RemoveSupercluster(string _ID)
        {
            return superclusterDictionary.Remove(_ID);
        }
    }
}
