using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public GameObject testCombat;
    public Vector3 testCombatPos = new Vector3(0, 2.7f, 8.59f);
    [SerializeField]
    private bool isTestCombatDone = false;

    [Space]
    public GameObject combatantData;
    [SerializeField]
    private bool isThereCombatantData;

    [Space]
    public GameObject playerObject;
    public Vector3 playerPosition;
    bool Bool = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("I've been called again");
		if (!isTestCombatDone)
		{
            Transform pos = this.transform;
            pos.position = testCombatPos;
            GameObject obj = Instantiate(testCombat, pos);
            obj.transform.parent = null;
		}
		if (!isThereCombatantData)
		{
            GameObject obj = Instantiate(combatantData, this.transform);
            obj.transform.parent = null;
            isThereCombatantData = true;
		}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null && Bool)
		{
            playerObject.transform.position = playerPosition;
            Bool = false;
        }
		else if (playerObject == null)
		{
            Bool = true;
		}
		else
		{
            playerPosition = playerObject.transform.position;
		}
    }

    public bool IsTestCombatDone { get => isTestCombatDone; set => isTestCombatDone = value; }
}
