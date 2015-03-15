using UnityEngine;
using DSStructureForClient;

public class ContainerGameObject
{
    public Container container { get; protected set; }
    public GameObject gameObject { get; protected set; }

    public ContainerGameObject(Container _container,GameObject _gameObject)
    {
        container = _container;
        gameObject = _gameObject;
    }

    public void Update(Tuple<string, object>[] updateContainerData, Vector3 position, Vector3 eulerAngles)
    {
        container.Update(updateContainerData);
        gameObject.transform.position = position;
        //gameObject.transform.eulerAngles = eulerAngles;
    }
}
