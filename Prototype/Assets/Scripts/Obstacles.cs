using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    public static void RemoveObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");

        foreach (GameObject obs in obstacles)
        {
            obs.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public static IEnumerator ShowObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");

        foreach (GameObject obs in obstacles)
        {
            obs.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.25f);
            obs.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
