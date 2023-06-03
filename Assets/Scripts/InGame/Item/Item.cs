using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField] public int itemId { get; private set; }
    [SerializeField] private float itemSpeed = 1f;

    protected Player player;
    protected Transform target;
    protected ItemData itemData;
    protected Collider2D itemCollider;

    private void Awake()
    {
        gameObject.tag = "Item";
        itemData = new ItemData();
    }

    private void OnEnable()
    {
        ItemSetting(itemId.ToString());
    }

    protected virtual IEnumerator Move()
    {
        while (true)
        {
            transform.Translate((target.position - transform.position).normalized * Time.deltaTime * itemSpeed);
            yield return null;
        }
    }

    private void ItemSetting(string itemId)
    {
        Dictionary<string, Dictionary<string, object>> itemTable = CSVReader.Read("ItemTable");
        if (itemTable.ContainsKey(itemId))
        {
            Dictionary<string, object> table = itemTable[itemId];
            itemData.SetItemName(Convert.ToString(table["ItemName"]));
            itemData.SetItemImage(Convert.ToString(table["ItemImage"]));
            //itemData.SetItemType((ItemConstant)Enum.Parse(typeof(ItemConstant), Convert.ToString(table["ItemType"])));
            //Enum.TryParse(table["ItemType"].ToString(), true, out ItemConstant result);
            itemData.SetItemTypeParam(Convert.ToInt32(table["ItemTypeParam"]));
            //itemData.SetImagePath(Convert.ToString(table["ImagePath"]));
        }
    }
}
