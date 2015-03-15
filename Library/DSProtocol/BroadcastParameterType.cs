namespace DSProtocol
{
    public enum OnlineBroadcastItem
    {
        SoulUniqueID
    }
    public enum ProjectContainerBroadcastItem
    {
        SceneUniqueID,
        ContainerUniqueID,
        ContainerName,
        PositionX,
        PositionY,
        PositionZ,
        Angle,
        SoulLimit,
        Status
    }
    public enum UpdateContainerRealTimeInfoBroadcastItem
    {
        SceneUniqueID,
        ContainerUniqueID,
        PositionX,
        PositionY,
        PositionZ,
        Angle
    }
    public enum OfflineBroadcastItem
    {
        SceneUniqueID,
        ContainerUniqueID
    }
    public enum ContainerMoveBroadcastItem
    {
        SceneUniqueID,
        ContainerUniqueID,
        MoveType,
        Direction,
        Level
    }
}
