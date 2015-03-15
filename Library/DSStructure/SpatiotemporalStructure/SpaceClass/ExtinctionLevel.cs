using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSStructure.PlayerStructure;

namespace DSStructure.SpatiotemporalStructure.SpaceClass
{
    public class Planet : Extinction
    {
        public Planet(string _uniqueID, string _name, int extinctionNumber, string extinctionName, int _limit, Star _sourceStar)
            : base(extinctionNumber, extinctionName)
        {
            name = _name;
            uniqueID = _uniqueID;
            sourceStar = _sourceStar;
            _containerNumberLimit = _limit;
            fieldDictionary = new Dictionary<string, Field>();
        }
        public override int containerNumber
        {
            get { return fieldDictionary.Values.Sum(x => x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Star sourceStar { get; protected set; }
        public Dictionary<string, Field> fieldDictionary { get; protected set; }

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
                    case "ExtinctionName":
                        returnData[i] = new Tuple<string, object>("ExtinctionName", ExtinctionName);
                        break;
                    case "ExtinctionNumber":
                        returnData[i] = new Tuple<string, object>("ExtinctionNumber", ExtinctionNumber);
                        break;
                    case "ExtinctionReason":
                        returnData[i] = new Tuple<string, object>("ExtinctionReason", ExtinctionReason);
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
                    case "ExtinctionName":
                        ExtinctionName = (string)_data.Item2;
                        break;
                    case "ExtinctionNumber":
                        ExtinctionNumber = (int)_data.Item2;
                        break;
                    case "ExtinctionReason":
                        ExtinctionReason = (string)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in fieldDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddField(string _ID, Field _field)
        {
            if (containerNumberLimit - containerNumber >= _field.containerNumberLimit)
            {
                fieldDictionary.Add(_ID, _field);
                return true;
            }
            else
                return false;
        }
        public void UpdateField(string _ID, Tuple<string, object>[] updateData)
        {
            fieldDictionary[_ID].Update(updateData);
        }
        public bool RemoveField(string _ID)
        {
            return fieldDictionary.Remove(_ID);
        }

        

        
    }

    public class Field : Extinction
    {
        public Field(string _uniqueID, string _name, int extinctionNumber, string extinctionName, int _limit, Planet _sourcePlanet)
            : base(extinctionNumber, extinctionName)
        {
            name = _name;
            uniqueID = _uniqueID;
            _containerNumberLimit = _limit;
            sourcePlanet = _sourcePlanet;
            continentDictionary = new Dictionary<string, Continent>();
        }
        public override int containerNumber
        {
            get { return continentDictionary.Values.Sum(x => x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Planet sourcePlanet { get; protected set; }
        public Dictionary<string, Continent> continentDictionary { get; protected set; }

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
                    case "ExtinctionName":
                        returnData[i] = new Tuple<string, object>("ExtinctionName", ExtinctionName);
                        break;
                    case "ExtinctionNumber":
                        returnData[i] = new Tuple<string, object>("ExtinctionNumber", ExtinctionNumber);
                        break;
                    case "ExtinctionReason":
                        returnData[i] = new Tuple<string, object>("ExtinctionReason", ExtinctionReason);
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
                    case "ExtinctionName":
                        ExtinctionName = (string)_data.Item2;
                        break;
                    case "ExtinctionNumber":
                        ExtinctionNumber = (int)_data.Item2;
                        break;
                    case "ExtinctionReason":
                        ExtinctionReason = (string)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in continentDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddContinent(string _ID, Continent _continent)
        {
            if (containerNumberLimit - containerNumber >= _continent.containerNumberLimit)
            {
                continentDictionary.Add(_ID, _continent);
                return true;
            }
            else
                return false;
        }
        public void UpdateContinent(string _ID, Tuple<string, object>[] updateData)
        {
            continentDictionary[_ID].Update(updateData);
        }
        public bool RemoveContinent(string _ID)
        {
            return continentDictionary.Remove(_ID);
        }
    }

    public class Continent : Extinction
    {
        public Continent(string _uniqueID, string _name, int extinctionNumber, string extinctionName, int _limit, Field _sourceField)
            : base(extinctionNumber, extinctionName)
        {
            name = _name;
            uniqueID = _uniqueID;
            sourceField = _sourceField;
            _containerNumberLimit = _limit;
            countryDictionary = new Dictionary<string, Country>();
        }
        public override int containerNumber
        {
            get { return countryDictionary.Values.Sum(x=>x.containerNumber); }
        }
        public override int containerNumberLimit
        {
            get { return _containerNumberLimit; }
        }
        public Field sourceField { get; protected set; }
        public Dictionary<string, Country> countryDictionary { get; protected set; }

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
                    case "ExtinctionName":
                        returnData[i] = new Tuple<string, object>("ExtinctionName", ExtinctionName);
                        break;
                    case "ExtinctionNumber":
                        returnData[i] = new Tuple<string, object>("ExtinctionNumber", ExtinctionNumber);
                        break;
                    case "ExtinctionReason":
                        returnData[i] = new Tuple<string, object>("ExtinctionReason", ExtinctionReason);
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
                    case "ExtinctionName":
                        ExtinctionName = (string)_data.Item2;
                        break;
                    case "ExtinctionNumber":
                        ExtinctionNumber = (int)_data.Item2;
                        break;
                    case "ExtinctionReason":
                        ExtinctionReason = (string)_data.Item2;
                        break;
                }
            }
        }
        public override List<Container> BroadcastList()
        {
            List<Container> containerList = new List<Container>();
            foreach (GeneralSpace target in countryDictionary.Values)
                containerList.AddRange(target.BroadcastList());
            return containerList;
        }

        public bool AddCountry(string _ID, Country _country)
        {
            if (containerNumberLimit - containerNumber >= _country.containerNumberLimit)
            {
                countryDictionary.Add(_ID, _country);
                return true;
            }
            else
                return false;
            
        }
        public void UpdateCountry(string _ID, Tuple<string, object>[] updateData)
        {
            countryDictionary[_ID].Update(updateData);
        }
        public bool RemoveCountry(string _ID)
        {
            return countryDictionary.Remove(_ID);
        }        
    }
}
