using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using DSStructure.PlayerStructure;

namespace DSStructure.SpatiotemporalStructure.SpaceClass
{
    public class Country : History
    {
        public Country(string _uniqueID, string _name, int historyNumber, string historyName, int _limit, Continent _sourceContinent)
            : base(historyNumber, historyName)
        {
            name = _name;
            uniqueID = _uniqueID;
            areaDictionary = new Dictionary<string, Area>();
            sourceContinent = _sourceContinent;
            _containerNumberLimit = _limit;
        }
        public override int containerNumber
        {
            get { return areaDictionary.Values.Sum(x=>x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Continent sourceContinent { get; protected set; }
        public Dictionary<string, Area> areaDictionary { get; protected set; }

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
                    case "HistoryName":
                        returnData[i] = new Tuple<string, object>("HistoryName", HistoryName);
                        break;
                    case "HistoryNumber":
                        returnData[i] = new Tuple<string, object>("HistoryNumber", HistoryNumber);
                        break;
                    case "StartDate":
                        returnData[i] = new Tuple<string, object>("StartDate", StartDate);
                        break;
                    case "EndDate":
                        returnData[i] = new Tuple<string, object>("EndDate", EndDate);
                        break;
                    case "Duration":
                        returnData[i] = new Tuple<string, object>("Duration", Duration);
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
                    case "HistoryName":
                        HistoryName = (string)_data.Item2;
                        break;
                    case "HistoryNumber":
                        HistoryNumber = (int)_data.Item2;
                        break;
                    case "StartDate":
                        StartDate = (int)_data.Item2;
                        break;
                    case "EndDate":
                        EndDate = (int)_data.Item2;
                        break;
                    case "Duration":
                        Duration = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in areaDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddArea(string _ID, Area _area)
        {
            if (containerNumberLimit - containerNumber >= _area.containerNumberLimit)
            {
                areaDictionary.Add(_ID, _area);
                return true;
            }
            else
                return false;
        }
        public void UpdateArea(string _ID, Tuple<string, object>[] updateData)
        {
            areaDictionary[_ID].Update(updateData);
        }
        public bool RemoveArea(string _ID)
        {
            return areaDictionary.Remove(_ID);
        }
    }

    public class Area : History
    {
        public Area(string _uniqueID, string _name, int historyNumber, string historyName, int _limit, Country _sourceCountry)
            : base(historyNumber, historyName)
        {
            name = _name;
            uniqueID = _uniqueID;
            sourceCountry = _sourceCountry;
            _containerNumberLimit = _limit;
            blockDictionary = new Dictionary<string, Block>();
        }
        public override int containerNumber
        {
            get { return blockDictionary.Values.Sum(x => x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Country sourceCountry { get; protected set; }
        public Dictionary<string, Block> blockDictionary { get; protected set; }

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
                    case "HistoryName":
                        returnData[i] = new Tuple<string, object>("HistoryName", HistoryName);
                        break;
                    case "HistoryNumber":
                        returnData[i] = new Tuple<string, object>("HistoryNumber", HistoryNumber);
                        break;
                    case "StartDate":
                        returnData[i] = new Tuple<string, object>("StartDate", StartDate);
                        break;
                    case "EndDate":
                        returnData[i] = new Tuple<string, object>("EndDate", EndDate);
                        break;
                    case "Duration":
                        returnData[i] = new Tuple<string, object>("Duration", Duration);
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
                    case "HistoryName":
                        HistoryName = (string)_data.Item2;
                        break;
                    case "HistoryNumber":
                        HistoryNumber = (int)_data.Item2;
                        break;
                    case "StartDate":
                        StartDate = (int)_data.Item2;
                        break;
                    case "EndDate":
                        EndDate = (int)_data.Item2;
                        break;
                    case "Duration":
                        Duration = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in blockDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddBlock(string _ID, Block _block)
        {
            if (containerNumberLimit - containerNumber >= _block.containerNumberLimit)
            {
                blockDictionary.Add(_ID, _block);
                return true;
            }
            else
                return false;
        }
        public void UpdateBlock(string _ID, Tuple<string, object>[] updateData)
        {
            blockDictionary[_ID].Update(updateData);
        }
        public bool RemoveBlock(string _ID)
        {
            return blockDictionary.Remove(_ID);
        }
    }

    public class Block : History
    {
        public Block(string _uniqueID, string _name, int historyNumber, string historyName, int _limit, Area _sourceArea)
            : base(historyNumber, historyName)
        {
            name = _name;
            uniqueID = _uniqueID;
            soureceArea = _sourceArea;
            _containerNumberLimit = _limit;
            sceneDictionary = new Dictionary<string, Scene>();
        }
        public override int containerNumber 
        { 
            get { return sceneDictionary.Values.Sum(x=>x.containerNumber); } 
        }
        public override int containerNumberLimit 
        { 
            get { return _containerNumberLimit; } 
        }
        public Area soureceArea { get; protected set; }
        public Dictionary<string, Scene> sceneDictionary { get; protected set; }

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
                    case "HistoryName":
                        returnData[i] = new Tuple<string, object>("HistoryName", HistoryName);
                        break;
                    case "HistoryNumber":
                        returnData[i] = new Tuple<string, object>("HistoryNumber", HistoryNumber);
                        break;
                    case "StartDate":
                        returnData[i] = new Tuple<string, object>("StartDate", StartDate);
                        break;
                    case "EndDate":
                        returnData[i] = new Tuple<string, object>("EndDate", EndDate);
                        break;
                    case "Duration":
                        returnData[i] = new Tuple<string, object>("Duration", Duration);
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
                    case "HistoryName":
                        HistoryName = (string)_data.Item2;
                        break;
                    case "HistoryNumber":
                        HistoryNumber = (int)_data.Item2;
                        break;
                    case "StartDate":
                        StartDate = (int)_data.Item2;
                        break;
                    case "EndDate":
                        EndDate = (int)_data.Item2;
                        break;
                    case "Duration":
                        Duration = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in sceneDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddScene(string _ID, Scene _scene)
        {
            if (containerNumberLimit - containerNumber >= _scene.containerNumberLimit)
            {
                sceneDictionary.Add(_ID, _scene);
                return true;
            }
            else
                return false;
        }
        public void UpdateScene(string _ID, Tuple<string, object>[] updateData)
        {
            sceneDictionary[_ID].Update(updateData);
        }
        public bool RemoveScene(string _ID)
        {
            return sceneDictionary.Remove(_ID);
        }
    }

    public class Scene : History
    {
        public Scene(string _uniqueID, string _name, int historyNumber, string historyName, int _limit, Block _sourceBlock)
            : base(historyNumber, historyName)
        {
            name = _name;
            uniqueID = _uniqueID;
            sourceBlock = _sourceBlock;
            _containerNumberLimit = _limit;
            containerConcurrentDictionary = new ConcurrentDictionary<Guid, Container>();
        }
        public override int containerNumber 
        { 
            get { return containerConcurrentDictionary.Count; } 
        }
        public override int containerNumberLimit 
        { 
            get { return _containerNumberLimit; } 
        }
        public Block sourceBlock { get; protected set; }
        public ConcurrentDictionary<Guid, Container> containerConcurrentDictionary { get; protected set; }

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
                    case "HistoryName":
                        returnData[i] = new Tuple<string, object>("HistoryName", HistoryName);
                        break;
                    case "HistoryNumber":
                        returnData[i] = new Tuple<string, object>("HistoryNumber", HistoryNumber);
                        break;
                    case "StartDate":
                        returnData[i] = new Tuple<string, object>("StartDate", StartDate);
                        break;
                    case "EndDate":
                        returnData[i] = new Tuple<string, object>("EndDate", EndDate);
                        break;
                    case "Duration":
                        returnData[i] = new Tuple<string, object>("Duration", Duration);
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
                    case "HistoryName":
                        HistoryName = (string)_data.Item2;
                        break;
                    case "HistoryNumber":
                        HistoryNumber = (int)_data.Item2;
                        break;
                    case "StartDate":
                        StartDate = (int)_data.Item2;
                        break;
                    case "EndDate":
                        EndDate = (int)_data.Item2;
                        break;
                    case "Duration":
                        Duration = (int)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            return containerConcurrentDictionary.Values.ToList();
        }

        public bool AddContainer(Guid _ID, Container _container)
        {
            if (containerNumber < containerNumberLimit)
            {
                return containerConcurrentDictionary.TryAdd(_ID, _container);
            }
            else
                return false;
        }
        public void UpdateContainer(Guid _ID, Tuple<string, object>[] updateData)
        {
            containerConcurrentDictionary[_ID].Update(updateData);
        }
        public bool RemoveContainer(Guid _ID, out Container _container)
        {
            _container = null;
            if (containerConcurrentDictionary.ContainsKey(_ID))
                return containerConcurrentDictionary.TryRemove(_ID, out _container);
            else
                return false;
        }
    }
}
