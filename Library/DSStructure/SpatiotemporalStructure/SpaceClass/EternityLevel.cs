using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using DSStructure.PlayerStructure;

namespace DSStructure.SpatiotemporalStructure.SpaceClass
{
    public class WorldGraph : Eternity
    {
        public WorldGraph(string _uniqueID, string _name, int ethernityNumber, string eternityName, int _limit)
            : base(ethernityNumber, eternityName)
        {
            uniqueID = _uniqueID;
            name = _name;
            _containerNumberLimit = _limit;
            worldDictionary = new Dictionary<string, World>();
            pathDictionary = new Dictionary<string, WorldPath>();
        }
        public override int containerNumber
        {
            get { return worldDictionary.Values.Sum(x => x.containerNumber) + pathDictionary.Values.Sum(x => x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Dictionary<string, World> worldDictionary { get; protected set; }
        public Dictionary<string, WorldPath> pathDictionary { get; protected set; }

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
                    case "EternityName":
                        returnData[i] = new Tuple<string, object>("EternityName", EternityName);
                        break;
                    case "EternityNumber":
                        returnData[i] = new Tuple<string, object>("EternityNumber", EternityNumber);
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
                    case "uniqueID":
                        uniqueID = (string)_data.Item2;
                        break;
                    case "name":
                        name = (string)_data.Item2;
                        break;
                    case "EternityNumber":
                        EternityNumber = (int)_data.Item2;
                        break;
                    case "EternityName":
                        EternityName = (string)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in worldDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            foreach (GeneralSpace target in pathDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddWorld(string _ID,World _world)
        {
            if (containerNumberLimit - containerNumber >= _world.containerNumberLimit)
            {
                worldDictionary.Add(_ID, _world);
                return true;
            }
            else
                return false;
        }
        public bool AddPath(string _ID,WorldPath _path)
        {
            if (containerNumberLimit - containerNumber >= _path.containerNumberLimit)
            {
                pathDictionary.Add(_ID, _path);
                return true;
            }
            else
                return false;
            
        }
        public void UpdateWorld(string _ID, Tuple<string, object>[] updateData)
        {
            worldDictionary[_ID].Update(updateData);
        }
        public void UpdatePath(string _ID, Tuple<string, object>[] updateData)
        {
            pathDictionary[_ID].Update(updateData);
        }
        public bool RemoveWorld(string _ID)
        {
            return worldDictionary.Remove(_ID);
        }
        public bool RemovePath(string _ID)
        {
            return pathDictionary.Remove(_ID);
        }
    }

    public class World : Eternity
    {
        public World(string _uniqueID, string _name, int ethernityNumber, string eternityName, int _limit, WorldGraph _sourceWorldGraph)
            : base(ethernityNumber, eternityName)
        {
            uniqueID = _uniqueID;
            name = _name;
            _containerNumberLimit = _limit;
            sourceWorldGraph = _sourceWorldGraph;
            treeUniverseDictionary = new Dictionary<string, TreeUniverse>();
        }
        public override int containerNumber
        {
            get { return treeUniverseDictionary.Values.Sum(x => x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public WorldGraph sourceWorldGraph { get; protected set; }
        public Dictionary<string, TreeUniverse> treeUniverseDictionary { get; protected set; }

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
                    case "EternityName":
                        returnData[i] = new Tuple<string, object>("EternityName", EternityName);
                        break;
                    case "EternityNumber":
                        returnData[i] = new Tuple<string, object>("EternityNumber", EternityNumber);
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
                    case "EternityName":
                        EternityName = (string)_data.Item2;
                        break;
                    case "EternityNumber":
                        EternityNumber = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in treeUniverseDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddTreeUniverse(string _ID,TreeUniverse _treeUniverse)
        {
            if (containerNumberLimit - containerNumber >= _treeUniverse.containerNumberLimit)
            {
                treeUniverseDictionary.Add(_ID, _treeUniverse);
                return true;
            }
            else
                return false;
        }
        public void UpdateTreeUniverse(string _ID, Tuple<string, object>[] updateData)
        {
            treeUniverseDictionary[_ID].Update(updateData);
        }
        public bool RemoveTreeUniverse(string _ID)
        {
            return treeUniverseDictionary.Remove(_ID);
        }

        
    }

    public class WorldPath : Eternity
    {
        public WorldPath(string _uniqueID, string _name, int ethernityNumber, string eternityName, int _limit, World _connectedWorld1, World _connectedWorld2, WorldGraph _sourceWorldGraph)
            : base(ethernityNumber, eternityName)
        {
            uniqueID = _uniqueID;
            name = _name;
            _containerNumberLimit = _limit;
            connectedWorld1ID = _connectedWorld1.uniqueID;
            connectedWorld2ID = _connectedWorld2.uniqueID;
            sourceWorldGraph = _sourceWorldGraph;
            sceneCouncurrentDictionary = new ConcurrentDictionary<string, Scene>();
        }
        public override int containerNumber
        {
            get { return sceneCouncurrentDictionary.Count; }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public string connectedWorld1ID { get; protected set; }
        public string connectedWorld2ID { get; protected set; }
        public WorldGraph sourceWorldGraph { get; protected set; }
        public ConcurrentDictionary<string, Scene> sceneCouncurrentDictionary { get; protected set; }

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
                    case "EternityName":
                        returnData[i] = new Tuple<string, object>("EternityName", EternityName);
                        break;
                    case "EternityNumber":
                        returnData[i] = new Tuple<string, object>("EternityNumber", EternityNumber);
                        break;
                    case "TimeLevel":
                        returnData[i] = new Tuple<string, object>("TimeLevel", TimeLevel);
                        break;
                    case "connectedWorld1ID":
                        returnData[i] = new Tuple<string, object>("World1ID", connectedWorld1ID);
                        break;
                    case "connectedWorld2ID":
                        returnData[i] = new Tuple<string, object>("World1ID", connectedWorld2ID);
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
                    case "connectedWorld1ID":
                        connectedWorld1ID = (string)_data.Item2;
                        break;
                    case "connectedWorld2ID":
                        connectedWorld2ID = (string)_data.Item2;
                        break;
                    case "EternityName":
                        EternityName = (string)_data.Item2;
                        break;
                    case "EternityNumber":
                        EternityNumber = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in sceneCouncurrentDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddSoul(string _ID,Scene _scene)
        {
            if (containerNumberLimit - containerNumber >= 1)
            {
                return sceneCouncurrentDictionary.TryAdd(_ID, _scene);
            }
            else
                return false;
        }
        public void UpdateSoul(string _ID, Tuple<string, object>[] updateData)
        {
            sceneCouncurrentDictionary[_ID].Update(updateData);
        }
        public bool RemoveSoul(string _ID,out Scene _scene)
        {
            return sceneCouncurrentDictionary.TryRemove(_ID, out _scene);
        }

        
    }

    public class TreeUniverse : Eternity
    {
        public TreeUniverse(string _uniqueID, string _name, int ethernityNumber, string eternityName, int _limit, World _sourceWorld)
            : base(ethernityNumber, eternityName)
        {
            uniqueID = _uniqueID;
            name = _name;
            _containerNumberLimit = _limit;
            sourceWorld = _sourceWorld;
            universeDictionary = new Dictionary<string, Universe>();
        }
        public override int containerNumber
        {
            get { return universeDictionary.Values.Sum(x => x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public World sourceWorld { get; protected set; }
        public Dictionary<string, Universe> universeDictionary { get; protected set; }

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
                    case "EternityName":
                        returnData[i] = new Tuple<string, object>("EternityName", EternityName);
                        break;
                    case "EternityNumber":
                        returnData[i] = new Tuple<string, object>("EternityNumber", EternityNumber);
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
                    case "EternityName":
                        EternityName = (string)_data.Item2;
                        break;
                    case "EternityNumber":
                        EternityNumber = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in universeDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddUniverse(string _ID,Universe _universe)
        {
            if (containerNumberLimit - containerNumber >= _universe.containerNumberLimit)
            {
                universeDictionary.Add(_ID, _universe);
                return true;
            }
            else
                return false;
            
        }
        public void UpdateUniverse(string _ID, Tuple<string, object>[] updateData)
        {
            universeDictionary[_ID].Update(updateData);
        }
        public bool RemoveUniverse(string _ID)
        {
            return universeDictionary.Remove(_ID);
        }
    }
}
