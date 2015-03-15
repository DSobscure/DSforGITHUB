namespace DSProtocol
{
    public enum OperationType
    {
        Login,
        GetAnswerKey,
        GetSoulUniqueIDList,
        GetSoulInfo,
        CreateSoul,
        GetContainerUniqueIDList,
        GetContainerInfo,
        CreateContainer,
        Online,
        ProjectToScene,
        GetSceneRealTimeInfo,
        GetContainerRealTimeInfo,
        UpdateContainerRealTimeInfo,
        ContainerMove
    }

    public enum BroadcastType
    {
        Online,
        ProjectContainer,
        UpdateContainerRealTimeInfo,
        Offline,
        ContainerMove
    }
}
