using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterCombat : MonoBehaviour
{
	public GameObject combatantData;
	public SaveData data;

	private void Start()
	{
		StartCoroutine(TouchScreenCompatability());
	}

	private void Update()
	{
		if (combatantData == null)
		{
			combatantData = GameObject.FindGameObjectWithTag("CombatantData");
		}

		if (data == null)
		{
			data = GameObject.FindGameObjectWithTag("Data").GetComponent<SaveData>();
		}
	}


	private void OnTriggerEnter(Collider other)
    {
		DontDestroyOnLoad(combatantData);
		DontDestroyOnLoad(data.gameObject);
        SceneManager.LoadScene("Combat");
    }

	IEnumerator TouchScreenCompatability() //Adding touch to move would be a non-core feature, so just a skip for phones.
	{
        //yield return new WaitForSecondsRealtime(30);
        //SceneManager.LoadScene("Combat", LoadSceneMode.Additive); No longer summons combat after 30 seconds
		yield break; // Exits the coroutine 
	}
    
}
