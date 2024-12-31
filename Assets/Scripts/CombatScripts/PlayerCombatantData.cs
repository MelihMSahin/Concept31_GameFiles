using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombatantData : MonoBehaviour
{
    public GameObject allyPrefab;

	private void Awake()
	{
        //Moved DontDestroyOnLoad to scene transition triggers
        //DontDestroyOnLoad(gameObject);
    }

	void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(allyPrefab, gameObject.transform);
        }
    }
}
