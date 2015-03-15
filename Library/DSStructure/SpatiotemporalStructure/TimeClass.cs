using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSProtocol;
using System.Collections.Concurrent;
using DSStructure.PlayerStructure;

namespace DSStructure.SpatiotemporalStructure
{
    public abstract class GeneralSpace
    {
        public string uniqueID { get; protected set; }
        public string name { get; protected set; }
        public abstract int containerNumber { get; }
        protected int _containerNumberLimit;
        public abstract int containerNumberLimit { get;}

        public abstract Tuple<string, object>[] Get(string[] requestData);
        public abstract void Update(Tuple<string, object>[] updateData);
        public abstract List<Container> BroadcastList();
    }

    public abstract class Eternity : GeneralSpace
    {
        protected Eternity(int _number, string _name)
        {
            EternityNumber = _number;
            EternityName = _name;
        }
        public int EternityNumber { get; protected set; }
        public string EternityName { get; protected set; }
        public TimeClassEnumeration TimeLevel
        {
            get { return TimeClassEnumeration.Eternity; }
        }
    }

    public abstract class Termination : GeneralSpace
    {
        protected Termination(int _number, string _name)
        {
            TerminationNumber = _number;
            TerminationName = _name;
            TerminationTime = NotImplement.INT;
        }
        public int TerminationNumber { get; protected set; }
        public string TerminationName { get; protected set; }
        public int TerminationTime { get; protected set; }
        public TimeClassEnumeration TimeLevel
        {
            get { return TimeClassEnumeration.Termination; }
        }
    }

    public abstract class Nova : GeneralSpace
    {
        protected Nova(int _number, string _name)
        {
            NovaNumber = _number;
            NovaName = _name;
            LifePeriod = NotImplement.INT;
        }
        public int NovaNumber { get; protected set; }
        public string NovaName { get; protected set; }
        public int LifePeriod { get; protected set; }
        public TimeClassEnumeration TimeLevel
        {
            get
            {
                return TimeClassEnumeration.Nova;
            }
        }
    }

    public abstract class Era : GeneralSpace
    {
        protected Era(int _number, string _name)
        {
            EraNumber = _number;
            EraName = _name;
            EraPioneer = NotImplement.STRING;
        }
        public int EraNumber { get; protected set; }
        public string EraName { get; protected set; }
        public string EraPioneer { get; protected set; }
        public TimeClassEnumeration TimeLevel
        {
            get
            {
                return TimeClassEnumeration.Era;
            }
        }
    }

    public abstract class Extinction : GeneralSpace
    {
        protected Extinction(int _number, string _name)
        {
            ExtinctionNumber = _number;
            ExtinctionName = _name;
            ExtinctionReason = NotImplement.STRING;
        }
        public int ExtinctionNumber { get; protected set; }
        public string ExtinctionName { get; protected set; }
        public string ExtinctionReason { get; protected set; }
        public TimeClassEnumeration TimeLevel
        {
            get
            {
                return TimeClassEnumeration.Extinction;
            }
        }
    }

    public abstract class History : GeneralSpace
    {
        protected History(int _number, string _name)
        {
            HistoryName = _name;
            HistoryNumber = _number;
            StartDate = NotImplement.INT;
            EndDate = NotImplement.INT;
        }
        public int HistoryNumber { get; protected set; }
        public string HistoryName { get; protected set; }
        public int StartDate { get; protected set; }
        public int EndDate { get; protected set; }
        public int Duration { get { return EndDate - StartDate; } protected set { EndDate = StartDate + value; } }
        public TimeClassEnumeration TimeLevel
        {
            get
            {
                return TimeClassEnumeration.History;
            }
        }
    }
}
