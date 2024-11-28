using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New invetory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Holder = new List<InventorySlot>();
    public void AddItem (ItemObject _item, int _amount){
        bool hasItem = false;
        for(int i = 0; i < Holder.Count; i++){
            if(Holder[i].item == _item){
                Holder[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if(!hasItem){
            Holder.Add(new InventorySlot(_item, _amount));
        }
    }
}

[System.Serializable]
public class InventorySlot{
    public ItemObject item; //item stored in slot
    public int amount;
    public InventorySlot(ItemObject _item, int _amount){
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int val){
        amount += val;
    }
}
