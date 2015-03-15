using DSStructureForClient;
using DSProtocol;
using DoorOfSoul;

public partial class PhotonService
{
    public delegate void ConnectEventHandler(bool ConnectStatus);
    public event ConnectEventHandler ConnectEvent;

    public delegate void LoginEventHandler(bool LoginStatus, string DebugMessage, string UniqueID, string answerUniqueID, string Account, string answerName, int soulLimit);
    public event LoginEventHandler LoginEvent;

    public delegate void GetAnswerKeyEventHandler(string uniqueID, string account);
    public event GetAnswerKeyEventHandler GetAnswerKeyEvent;

    public delegate void GetSoulUniqueIDListEventHandler(string soulUniqueIDList);
    public event GetSoulUniqueIDListEventHandler GetSoulUniqueIDListEvent;

    public delegate void GetSoulInfoEventHandler(string soulUniqueID, string soulName, int containerLimit);
    public event GetSoulInfoEventHandler GetSoulInfoEvent;

    public delegate void GetContainerUniqueIDListEventHandler(string soulUniqueID, string containerUniqueIDList);
    public event GetContainerUniqueIDListEventHandler GetContainerUniqueIDListEvent;

    public delegate void GetContainerInfoEventHandler
        (string containerUniqueID, string containerName, string locationUniqueID,
        float positionX, float positionY, float positionZ, float angle,
        int soulLimit, int status);
    public event GetContainerInfoEventHandler GetContainerInfoEvent;

    public delegate void ProjectToSceneEventHandler(Container container, string sceneUniqueID);
    public event ProjectToSceneEventHandler ProjectToSceneEvent;

    public delegate void GetSceneInfoEventHandler(string sceneUniqueID, string sceneName, string[] containerUniqueIDList);
    public event GetSceneInfoEventHandler GetSceneInfoEvent;

    public delegate void UpdateContainerEventHandler(string sceneUniqueID, string containerUniqueID, float positionX, float positionY, float positionZ, float angle);
    public event UpdateContainerEventHandler UpdateContainerEvent;

    public delegate void OfflineEventHandler(string sceneUniqueID, string containerUniqueID);
    public event OfflineEventHandler OfflineEvent;

    public delegate void ContainerMoveEventHandler(string sceneUniqueID, string containerUniqueID, MoveType moveType, MoveDirection direction, Phase7.Level level);
    public event ContainerMoveEventHandler ContainerMoveEvent;
}
