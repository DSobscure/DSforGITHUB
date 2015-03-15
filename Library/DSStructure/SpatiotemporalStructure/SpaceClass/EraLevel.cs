using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSStructure.PlayerStructure;

namespace DSStructure.SpatiotemporalStructure.SpaceClass
{
    public class Galaxy : Era
    {
        public Galaxy(string _uniqueID, string _name, int eraNumber, string eraName, int _limit, Cluster _sourceCluster)
            : base(eraNumber, eraName)
        {
            uniqueID = _uniqueID;
            name = _name;
            _containerNumberLimit = _limit;
            sourceCluster = _sourceCluster;
            cloudDictionary = new Dictionary<string, Cloud>();
        }
        public override int containerNumber
        {
            get { return cloudDictionary.Values.Sum(x=>x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Cluster sourceCluster { get; protected set; }
        public Dictionary<string, Cloud> cloudDictionary { get; protected set; }

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
                    case "EraName":
                        returnData[i] = new Tuple<string, object>("EraName", EraName);
                        break;
                    case "EraNumber":
                        returnData[i] = new Tuple<string, object>("EraNumber", EraNumber);
                        break;
                    case "EraPioneer":
                        returnData[i] = new Tuple<string, object>("EraPioneer", EraPioneer);
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
                    case "EraName":
                        EraName = (string)_data.Item2;
                        break;
                    case "EraNumber":
                        EraNumber = (int)_data.Item2;
                        break;
                    case "EraPioneer":
                        EraPioneer = (string)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in cloudDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddCloud(string _ID,Cloud _cloud)
        {
            if (containerNumberLimit - containerNumber >= _cloud.containerNumberLimit)
            {
                cloudDictionary.Add(_ID, _cloud);
                return true;
            }
            else
                return false;
        }
        public void UpdateCloud(string _ID, Tuple<string, object>[] updateData)
        {
            cloudDictionary[_ID].Update(updateData);
        }
        public bool RemoveCloud(string _ID)
        {
            return cloudDictionary.Remove(_ID);
        }
    }

    public class Cloud : Era
    {
        public Cloud(string _uniqueID, string _name, int eraNumber, string eraName, int _limit, Galaxy _sourceGalaxy)
            : base(eraNumber, eraName)
        {
            name = _name;
            uniqueID = _uniqueID;
            sourceGalaxy = _sourceGalaxy;
            _containerNumberLimit = _limit;
            starDictionary = new Dictionary<string, Star>();
        }
        public override int containerNumber
        {
            get { return starDictionary.Values.Sum(x => x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Galaxy sourceGalaxy { get; protected set; }
        public Dictionary<string, Star> starDictionary { get; protected set; }

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
                    case "EraName":
                        returnData[i] = new Tuple<string, object>("EraName", EraName);
                        break;
                    case "EraNumber":
                        returnData[i] = new Tuple<string, object>("EraNumber", EraNumber);
                        break;
                    case "EraPioneer":
                        returnData[i] = new Tuple<string, object>("EraPioneer", EraPioneer);
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
                    case "EraName":
                        EraName = (string)_data.Item2;
                        break;
                    case "EraNumber":
                        EraNumber = (int)_data.Item2;
                        break;
                    case "EraPioneer":
                        EraPioneer = (string)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in starDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddStar(string _ID, Star _star)
        {
            if (containerNumberLimit - containerNumber >= _star.containerNumberLimit)
            {
                starDictionary.Add(_ID, _star);
                return true;
            }
            else
                return false;
            
        }
        public void UpdateStar(string _ID, Tuple<string, object>[] updateData)
        {
            starDictionary[_ID].Update(updateData);
        }
        public bool RemoveStar(string _ID)
        {
            return starDictionary.Remove(_ID);
        }
    }

    public class Star : Era
    {
        public Star(string _uniqueID, string _name, int eraNumber, string eraName, int _limit, Cloud _sourceCloud)
            : base(eraNumber, eraName)
        {
            name = _name;
            uniqueID = _uniqueID;
            sourceCloud = _sourceCloud;
            _containerNumberLimit = _limit;
            planetDictionary = new Dictionary<string, Planet>();
        }
        public override int containerNumber
        {
            get { return planetDictionary.Values.Sum(x=>x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Cloud sourceCloud { get; protected set; }
        public Dictionary<string, Planet> planetDictionary { get; protected set; }

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
                    case "EraName":
                        returnData[i] = new Tuple<string, object>("EraName", EraName);
                        break;
                    case "EraNumber":
                        returnData[i] = new Tuple<string, object>("EraNumber", EraNumber);
                        break;
                    case "EraPioneer":
                        returnData[i] = new Tuple<string, object>("EraPioneer", EraPioneer);
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
                    case "EraName":
                        EraName = (string)_data.Item2;
                        break;
                    case "EraNumber":
                        EraNumber = (int)_data.Item2;
                        break;
                    case "EraPioneer":
                        EraPioneer = (string)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in planetDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddPlanet(string _ID, Planet _planet)
        {
            if (containerNumberLimit - containerNumber >= _planet.containerNumberLimit)
            {
                planetDictionary.Add(_ID, _planet);
                return true;
            }
            else
                return false;
            
        }
        public void UpdatePlanet(string _ID, Tuple<string, object>[] updateData)
        {
            planetDictionary[_ID].Update(updateData);
        }
        public bool RemovePlanet(string _ID)
        {
            return planetDictionary.Remove(_ID);
        }    
    }
}
