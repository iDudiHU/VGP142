using UnityEngine;

public class CreatePrefabOnEvent : MonoBehaviour
{
    public GameObject prefabToCreate;

    public void CreatePrefab()
    {
        GameObject newPrefab = Instantiate(prefabToCreate, transform.position, transform.rotation);
        // Modify the new Prefab as desired
    }
}
