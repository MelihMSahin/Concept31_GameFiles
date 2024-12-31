using UnityEngine;
using UnityEngine.SceneManagement;

public class resetGame : MonoBehaviour
{
    public void ResetGame()
	{
		SceneManager.LoadScene("Out-Of-Combat");
	}
}
