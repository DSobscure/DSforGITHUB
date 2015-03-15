using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSStructure.PlayerStructure;

namespace DSStructure.SpatiotemporalStructure.SpaceClass
{
    public class Supercluster : Nova
    {
        public Supercluster(string _uniqueID, string _name, int novaNumber, string novaName, int _limit, EtherField _sourceEtherField)
            : base(novaNumber, novaName)
        {
            name = _name;
            uniqueID = _uniqueID;
            sourceEtherField = _sourceEtherField;
            _containerNumberLimit = _limit;
            clusterDictionary = new Dictionary<string, Cluster>();
        }
        public override int containerNumber
        {
            get { return clusterDictionary.Values.Sum(x=>x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public EtherField sourceEtherField { get; protected set; }
        public Dictionary<string, Cluster> clusterDictionary { get; protected set; }

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
                    case "NovaName":
                        returnData[i] = new Tuple<string, object>("NovaName", NovaName);
                        break;
                    case "NovaNumber":
                        returnData[i] = new Tuple<string, object>("NovaNumber", NovaNumber);
                        break;
                    case "LifePeriod":
                        returnData[i] = new Tuple<string, object>("LifePeriod", LifePeriod);
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
                    case "NovaName":
                        NovaName = (string)_data.Item2;
                        break;
                    case "NovaNumber":
                        NovaNumber = (int)_data.Item2;
                        break;
                    case "LifePeriod":
                        LifePeriod = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in clusterDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddCluster(string _ID,Cluster _cluster)
        {
            if (containerNumberLimit - containerNumber >= _cluster.containerNumberLimit)
            {
                clusterDictionary.Add(_ID, _cluster);
                return true;
            }
            else
                return false;
        }
        public void UpdateCluster(string _ID, Tuple<string, object>[] updateData)
        {
            clusterDictionary[_ID].Update(updateData);
        }
        public bool RemoveCluster(string _ID)
        {
            return clusterDictionary.Remove(_ID);
        }
    }

    public class Cluster : Nova
    {
        public Cluster(string _uniqueID, string _name, int novaNumber, string novaName, int _limit, Supercluster _sourceSupercluster)
            : base(novaNumber, novaName)
        {
            name = _name;
            uniqueID = _uniqueID;
            sourceSupercluster = _sourceSupercluster;
            _containerNumberLimit = _limit;
            galaxyDictionary = new Dictionary<string, Galaxy>();
        }
        public override int containerNumber
        {
            get { return galaxyDictionary.Values.Sum(x=>x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Supercluster sourceSupercluster { get; protected set; }
        public Dictionary<string, Galaxy> galaxyDictionary { get; protected set; }

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
                    case "NovaName":
                        returnData[i] = new Tuple<string, object>("NovaName", NovaName);
                        break;
                    case "NovaNumber":
                        returnData[i] = new Tuple<string, object>("NovaNumber", NovaNumber);
                        break;
                    case "LifePeriod":
                        returnData[i] = new Tuple<string, object>("LifePeriod", LifePeriod);
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
                    case "NovaName":
                        NovaName = (string)_data.Item2;
                        break;
                    case "NovaNumber":
                        NovaNumber = (int)_data.Item2;
                        break;
                    case "LifePeriod":
                        LifePeriod = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in galaxyDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddGalaxy(string _ID,Galaxy _galaxy)
        {
            if (containerNumberLimit - containerNumber >= _galaxy.containerNumberLimit)
            {
                galaxyDictionary.Add(_ID, _galaxy);
                return true;
            }
            else
                return false;
        }
        public void UpdateGalaxy(string _ID, Tuple<string, object>[] updateData)
        {
            galaxyDictionary[_ID].Update(updateData);
        }
        public bool RemoveGalaxy(string _ID)
        {
            return galaxyDictionary.Remove(_ID);
        }
    }
}
