
using UnityEngine;

public class CubeCollect : MonoBehaviour
{
  
    void Update()
    {
        transform.Rotate(20 * Time.deltaTime, 20 * Time.deltaTime, 0);
    }


    public Vector3 GetSpawnPosition()
    {
        return transform.position;
    }

}
