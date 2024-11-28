using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment object", menuName = "Inventory System/Items/Equipment")]

public class EquipmentObject : ItemObject
{

    int AttackBonus;
    int DefenceBonus;
    public void awake(){
        type = ItemType.Equipment;

    }
}
