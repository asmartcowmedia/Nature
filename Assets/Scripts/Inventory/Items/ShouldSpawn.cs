using Unity.Collections;
using UnityEngine;

public class ShouldSpawn : MonoBehaviour
{
    [SerializeField] private ItemsCollected collected;
    
    [SerializeField][ReadOnly] private bool
        machete,
        infectedMachete,
        headlamp,
        map,
        infectedMap;

    private void Update()
    {
        if (machete && collected.macheteCollected)
            Destroy(gameObject);
        if (infectedMachete && collected.infectedMacheteCollected)
            Destroy(gameObject);
        if (headlamp && collected.headlampCollected)
            Destroy(gameObject);
        if (map && collected.mapCollected)
            Destroy(gameObject);
        if (infectedMap && collected.infectedMapCollected)
            Destroy(gameObject);
    }
}