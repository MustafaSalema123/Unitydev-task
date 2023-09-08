
using UnityEngine;

public class SpawnCube : MonoBehaviour
{

    


    public Transform[] spawntransforms;

    public GameObject cubePrefabs;
    public int currentSpawnPosition;

    private void Awake()
    {
        SpiderController.OnTriggerCube += SpawnCubePrefabs;
    }
    private void OnDestroy()
    {
        SpiderController.OnTriggerCube -= SpawnCubePrefabs;
    }
    private void Start()
    {
        ShuffleArray(spawntransforms);

        SpawnCubePrefabs();
    }

   
    private void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = Random.Range(0, n);
            n--;
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    private void Update()
    {

        if (currentSpawnPosition >= spawntransforms.Length)
        {
            currentSpawnPosition = 0;
        }
       
    }
    void SpawnCubePrefabs()
    {
        Instantiate(cubePrefabs, spawntransforms[currentSpawnPosition].position, Quaternion.identity);
        currentSpawnPosition++;
    }
}
