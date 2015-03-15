namespace DSProtocol
{
    public enum ParameterItem
    {
        UniqueID,
        Name,
        ContainerNumber,
        ContainerLimit
    }
    public enum LoginParameterItem
    {
        Account,
        Password
    }
    public enum LoginResponseItem
    {
        UniqueID,
        AnswerUniqueID,
        AnswerName,
        AnswerAccount,
        SoulLimit,
    }
    public enum GetAnswerKeyItem
    {
        UniqueID,
        AnswerUniqueID,
        AnswerName,
        AnswerAccount,
        SoulLimit
    }
    public enum GetSoulUniqueIDListItem
    {
        AnswerUniqueID,
        SoulUniqueIDList
    }
    public enum GetSoulInfoItem
    {
        SoulUniqueID,
        SoulName,
        ContainerLimit,
        LoginTime
    }
    public enum CreateSoulItem
    {
        AnswerUniqueID,
        SoulName,
        ContainerLimit
    }
    public enum GetContainerUniqueIDListItem
    {
        SoulUniqueID,
        ContainerUniqueIDList
    }
    public enum GetContainerInfoItem
    {
        ContainerUniqueID,
        ContainerName,
        LocationUniqueID,
        PositionX,
        PositionY,
        PositionZ,
        Angle,
        SoulLimit,
        Status
    }
    public enum CreateContainerItem
    {
        SoulUniqueID,
        ContainerName,
        LocationUniqueID,
        PositionX,
        PositionY,
        PositionZ,
        Angle,
        SoulLimit,
        Status
    }
    public enum OnlineItem
    {
        SoulUniqueID,
        SoulName,
        ContainerLimit,
    }
    public enum ProjectToSceneItem
    {
        SoulUniqueID,
        ContainerUniqueID,
        ContainerName,
        SoulLimit,
        SceneUniqueID,
        PositionX,
        PositionY,
        PositionZ,
        Angle,
        Status
    }
    public enum GetSceneRealTimeInfoItem
    {
        SceneUniqueID,
        SceneName,
        ContainerUniqueIDList
    }
    public enum GetContainerRealTimeInfoItem
    {
        ContainerUniqueID,
        ContainerName,
        LocationUniqueID,
        PositionX,
        PositionY,
        PositionZ,
        Angle,
        SoulLimit,
        Status
    }
    public enum UpdateContainerRealTimeInfoItem
    {
        SceneUniqueID,
        ContainerUniqueID,
        PositionX,
        PositionY,
        PositionZ,
        Angle
    }
    public enum ContainerMoveItem
    {
        SceneUniqueID,
        ContainerUniqueID,
        MoveType,
        Direction,
        Level
    }
}
