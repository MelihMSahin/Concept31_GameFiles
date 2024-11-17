using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterCombat : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(TouchScreenCompatability());
	}

	private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("Combat");
    }

    IEnumerator TouchScreenCompatability() //Adding touch to move would be a non-core feature, so just a skip for phones.
	{
        yield return new WaitForSecondsRealtime(30);
        SceneManager.LoadScene("Combat");
	}
}
