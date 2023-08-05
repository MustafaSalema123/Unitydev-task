using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircleLineManager : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject linePrefab;

    private bool isDrawingLine;
    private List<Vector2> linePositions = new List<Vector2>();
    private LineRenderer currentLineRenderer;
    private List<GameObject> circles = new List<GameObject>();

    public GameObject restartButtonG;
    private void Start()
    {

        restartButtonG.gameObject.SetActive(false);
        SpawnRandomCircles();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrawingLine = true;
            linePositions.Clear();
            linePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            CreateLine();
        }

        if (Input.GetMouseButton(0) && isDrawingLine)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePosition, linePositions[linePositions.Count - 1]) > 0.1f)
            {
                linePositions.Add(mousePosition);
                UpdateLine();
            }


        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawingLine = false;
            DestroyCirclesInLine();
            Destroy(currentLineRenderer.gameObject);
        }

        if(circles.Count <= 0) 
        {
            restartButtonG.gameObject.SetActive(true);
        //RestartButton();
        }
    }

    void CreateLine()
    {
        GameObject lineGO = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        currentLineRenderer = lineGO.GetComponent<LineRenderer>();
        currentLineRenderer.positionCount = 1;
        currentLineRenderer.SetPosition(0, linePositions[0]);
    }

    void UpdateLine()
    {
        currentLineRenderer.positionCount = linePositions.Count;
        for (int i = 0; i < linePositions.Count; i++)
        {
            currentLineRenderer.SetPosition(i, linePositions[i]);
        }
    }

    void DestroyCirclesInLine()
    {
        List<GameObject> circlesToDestroy = new List<GameObject>();

        foreach (GameObject circle in circles)
        {
            if (IsCircleIntersectingLine(circle))
            {
                circlesToDestroy.Add(circle);
            }
        }

        foreach (GameObject circle in circlesToDestroy)
        {
            circles.Remove(circle);
            Destroy(circle);
        }
    }

    bool IsCircleIntersectingLine(GameObject circle)
    {


        for (int i = 1; i < linePositions.Count; i++)
        {
            Vector2 startPoint = linePositions[i - 1];
            Vector2 endPoint = linePositions[i];

            RaycastHit2D hit = Physics2D.Linecast(startPoint, endPoint, LayerMask.GetMask("Default"));

            if (hit.collider != null && hit.collider.gameObject == circle)
            {
                
                return true;
            }
        }

        return false;
    }



    public void SpawnRandomCircles()
    {
        
        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(-8f, 8f);
            float y = Random.Range(-3.5f, 5.4f);
            GameObject circle = Instantiate(circlePrefab, new Vector3(x, y, 0.04f), Quaternion.identity);
            circles.Add(circle);
        }
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}



