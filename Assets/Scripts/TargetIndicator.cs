
using TMPro;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public CubeCollect target;
    public TMP_Text cubeDistance;
    public float rotationSpeed = 90;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (target == null)
        {
            target = FindObjectOfType<CubeCollect>();
        }
        if (target != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position),
         rotationSpeed * Time.deltaTime);
          
            cubeDistance.text = "Cube Distance " + Mathf.RoundToInt(Vector3.Distance(target.transform.position, transform.position)).ToString();

        }

    }
}
