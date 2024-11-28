using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{ //polymorphic time, declare stats in each part
    Consumable,
    Equipment,
    Tool,
    Default
}

public abstract class ItemObject : ScriptableObject //this is the base object with all the traits to be inherited
{
    public GameObject prefab;
    public ItemType type;
    public string description;

    
}
