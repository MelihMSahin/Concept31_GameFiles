using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMaze : MonoBehaviour
{
    public GameObject mazeSpawner;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Maze") == null)
        {
            GameObject obj = Instantiate(mazeSpawner, this.transform);
            obj.transform.parent = null;
        }
    }
}
