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
    //protected SpriteRenderer render;

    public bool isCollision { get; set; }

    private void Awake()
    {
        gameObject.tag = "Item";
        //render = GetComponent<SpriteRenderer>();
        itemData = new ItemData();
        ItemSetting(itemId.ToString());
        //ImageSetting();
    }

    private void OnEnable()
    {
        isCollision = false;
    }

    private void FixedUpdate()
    {
        if (isCollision)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * itemSpeed);
    }

    //protected virtual void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!isCollision && collision.gameObject.tag.Equals("ItemCollider"))
    //    {
    //        isCollision = true;
    //        target = collision.transform.parent.Find("Character").transform;
    //    }

    //    if (collision.gameObject.tag.Equals("Player"))
    //    {
    //        isCollision = false;
    //        gameObject.SetActive(false);
    //        GameManager.Instance.player.GetExp(10);
    //        //DebugManager.Instance.PrintDebug("get exp: 10\ntotal exp: " + GameManager.Instance.player.exp);
    //    }
    //}

    //private void ImageSetting()
    //{
    //    render.sprite = ResourcesManager.Load<Sprite>(itemData.imagePath);
    //}

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
            itemData.SetImagePath(Convert.ToString(table["ImagePath"]));
        }
    }
}
