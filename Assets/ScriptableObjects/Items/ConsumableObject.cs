using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Consumable object", menuName = "Inventory System/Items/Consumable")]

public class ConsumableObject : ItemObject
{

    public int RestoreHealth;
    public int RestoreSpecial;
    public void Awake(){
        type = ItemType.Consumable;



    }
}


