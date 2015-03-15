using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using System.Threading;
using DSProtocol;
using DSStructure.PlayerStructure;
using DSStructure.SpatiotemporalStructure;
using DSStructure.SpatiotemporalStructure.SpaceClass;
using System.IO;
using System.Collections.Concurrent;

namespace DSServer
{
    public class DSServer : ApplicationBase
    {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public WorldGraph mainWorldGraph;
        public ConcurrentDictionary<Guid, Answer> answerConcurrentDictionary { get; protected set; }
        public ConcurrentDictionary<string, Guid> uniqueIDGuidConcurrentDictionary { get; protected set; }
        public Dictionary<string, Scene> sceneDictionary { get; protected set; }
        public DSDatabase database;

        private bool broadcastActive = false;

        public bool AddAnswer(Guid _guid,Answer _answer)
        {
            return answerConcurrentDictionary.TryAdd(_guid, _answer);
        }

        public void UpdateAnswer(Guid _guid,Tuple<string,object>[] updateData)
        {
            if(answerConcurrentDictionary.ContainsKey(_guid))
                answerConcurrentDictionary[_guid].Update(updateData);
        }

        public bool RemoveAnswer(Guid _guid,out Answer _answer)
        {
            _answer = null;
            if (answerConcurrentDictionary.ContainsKey(_guid))
                return answerConcurrentDictionary.TryRemove(_guid, out _answer);
            else
                return false;
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new DSPeer(initRequest.Protocol,initRequest.PhotonPeer,this);
        }

        protected override void Setup()
        {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] =
                Path.Combine(this.ApplicationPath,"log");

            string path = Path.Combine(this.BinaryPath,"log4net.config");
            var file = new FileInfo(path);
            if(file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }
            Log.Info("Server Setup successiful!.......");
            answerConcurrentDictionary = new ConcurrentDictionary<Guid, Answer>();
            uniqueIDGuidConcurrentDictionary = new ConcurrentDictionary<string, Guid>();
            sceneDictionary = new Dictionary<string, Scene>();

            InitialWorld();
            database = new DSDatabase("IP","user","password","database");
            if(database.Connect())
                Log.Info("Database Connect successiful!.......");

            broadcastActive = true;
        }

        protected override void TearDown()
        {
            broadcastActive = false;
            database.Dispose(); 
        }

        public void BroadcastToContainerList(List<Container> receivers, BroadcastType broadcastType, Tuple<byte, object>[] dataPackage)
        {
            if(broadcastActive)
            {
                var parameter = new Dictionary<byte, object>();
                foreach (Tuple<byte, object> dataItem in dataPackage)
                    parameter.Add(dataItem.Item1, dataItem.Item2);
                var eventData = new EventData((byte)broadcastType, parameter);
                foreach (Container container in receivers)
                {
                    foreach (Soul soul in container.soulList)
                    {
                        soul.sourceAnswer.sourcePeer.SendEvent(eventData, new SendParameters());
                    }
                }
            }
        }

        private void InitialWorld()
        {
            string eternityName = "第一永恆"; 
            int eternityNumber = 1;
            mainWorldGraph = new WorldGraph("DS_v0.decipher", "DS", eternityNumber, eternityName,100);

            World _world = new World("mainWorld", "主世界", eternityNumber, eternityName, 100, mainWorldGraph);
            mainWorldGraph.AddWorld("mainWorld", _world);

            TreeUniverse _treeUniverse = new TreeUniverse("testTree", "測試用樹型", eternityNumber, eternityName, 100, _world);
            _world.AddTreeUniverse("testTree",_treeUniverse);


            string terminationName = "第一終焉";
            int terminationNumber = 1;

            Universe _universe = new Universe("testUniverse", "測試宇宙", terminationNumber, terminationName, 100, _treeUniverse);
            _treeUniverse.AddUniverse("testUniverse", _universe);

            EtherField _etherField = new EtherField("testEtherField", "測試乙太域", terminationNumber, terminationName, 100, _universe);
            _universe.AddEtherField("testEtherField",_etherField);


            string novaNmae = "第一新星";
            int novaNumber = 1;

            Supercluster _supercluster = new Supercluster("testSupercluster", "測試超星系群", novaNumber, novaNmae, 100, _etherField);
            _etherField.AddSupercluster("testSupercluster",_supercluster);

            Cluster _cluster = new Cluster("testCluster", "測試星系群", novaNumber, novaNmae, 100, _supercluster);
            _supercluster.AddCluster("testCluster",_cluster);


            string eraName = "第一紀元";
            int eraNumber = 1;

            Galaxy _galaxy = new Galaxy("testGalaxy", "測試星系", eraNumber, eraName, 100, _cluster);
            _cluster.AddGalaxy("testGalaxy",_galaxy);

            Cloud _cloud = new Cloud("testCloud", "測試星雲", eraNumber, eraName, 100, _galaxy);
            _galaxy.AddCloud("testCloud",_cloud);

            Star _star = new Star("testStar", "測試恆星", eraNumber, eraName, 100, _cloud);
            _cloud.AddStar("testStar", _star);


            string extinctionName = "第一滅絕";
            int extinctionNumber = 1;

            Planet _planet = new Planet("testPlanet", "測試行星", extinctionNumber, extinctionName, 100, _star);
            _star.AddPlanet("testPlanet",_planet);

            Field _field = new Field("testField", "測試領域", extinctionNumber, extinctionName, 100, _planet);
            _planet.AddField("testField",_field);

            Continent _continent = new Continent("testContinent", "測試大陸", extinctionNumber, extinctionName, 100, _field);
            _field.AddContinent("testContinent",_continent);


            string historyName = "第一歷史";
            int historyNumber = 1;

            Country _country = new Country("testCountry", "測試國家", historyNumber, historyName, 100, _continent);
            _continent.AddCountry("testCountry",_country);

            Area _area = new Area("testArea", "測試區域", historyNumber, historyName, 100, _country);
            _country.AddArea("testArea",_area);

            Block _block = new Block("testBlock", "測試區塊", historyNumber,historyName, 100, _area);
            _area.AddBlock("testBlock",_block);

            Scene _scene = new Scene("testScene", "測試場景", historyNumber,historyName, 50,_block);
            _block.AddScene("testScene",_scene);

            sceneDictionary.Add(_scene.uniqueID,_scene);
        }
    }
}
