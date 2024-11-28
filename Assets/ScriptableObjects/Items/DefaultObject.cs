using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Default object", menuName = "Inventory System/Items/Default")]


public class DefaultObject : ItemObject
{
    public void Awake(){
        type = ItemType.Default;
    }
}
