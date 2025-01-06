using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public Transform player;

    [Space]
    public GameObject combatantData;
    [SerializeField]
    private bool isThereCombatantData;

    [Space]
    public GameObject playerObject;
    public Vector3 playerPosition;
    bool isSceneChanged = false;

    // Start is called before the first frame update
    void Start()
    {
		if (!isThereCombatantData)
		{
            GameObject obj = Instantiate(combatantData, this.transform);
            isThereCombatantData = true;
		}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null && isSceneChanged)
		{
            playerObject.transform.position = playerPosition;
            isSceneChanged = false;
        }
		else if (playerObject == null)
		{
            isSceneChanged = true;
		}
		else
		{
            playerPosition = playerObject.transform.position;
		}
    }
}
