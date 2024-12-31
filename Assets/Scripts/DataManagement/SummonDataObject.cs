using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDataObject : MonoBehaviour
{
    public GameObject dataObject;

    // Start is called before the first frame update
    void Start()
    {
		if (GameObject.FindGameObjectWithTag("Data") == null)
		{
			GameObject obj = Instantiate(dataObject, this.transform);
			obj.transform.parent = null;
			Debug.Log("Summon object");
		}
	}
}
