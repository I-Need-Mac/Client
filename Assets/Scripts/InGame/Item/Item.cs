using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemId = "101";
    [SerializeField] private float itemSpeed = 1f;

    private Player player;
    private Transform target;

    private ItemData itemData;

    protected Collider2D itemCollider;
    protected SpriteRenderer render;

    public bool isCollision { get; set; } = false;

    private void Awake()
    {
        gameObject.tag = "Item";
        render = GetComponent<SpriteRenderer>();
        itemData = new ItemData();
        ItemSetting(itemId);
        ImageSetting();
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ItemCollider"))
        {
            isCollision = true;
            target = collision.gameObject.transform.parent.Find("Character").transform;
        }

        if (collision.gameObject.tag.Equals("Player"))
        {
            isCollision = false;
            gameObject.SetActive(false);
            GameManager.Instance.player.GetExp(10);
            //DebugManager.Instance.PrintDebug("get exp: 10\ntotal exp: " + GameManager.Instance.player.exp);
        }
    }

    private void ImageSetting()
    {
        Sprite sprite = ItemResourceLoader.Load(itemData.imagePath);
        render.sprite = sprite;
    }

    private void ItemSetting(string itemId)
    {
        Dictionary<string, Dictionary<string, object>> itemTable = CSVReader.Read("ItemTable");
        if (itemTable.ContainsKey(itemId))
        {
            Dictionary<string, object> table = itemTable[itemId];
            itemData.SetItemName(Convert.ToString(table["ItemName"]));
            itemData.SetItemImage(Convert.ToString(table["ItemImage"]));
            itemData.SetItemType((ItemConstant)Enum.Parse(typeof(ItemConstant), Convert.ToString(table["ItemType"])));
            itemData.SetItemTypeParam(Convert.ToInt32(table["ItemTypeParam"]));
            itemData.SetImagePath(Convert.ToString(table["ImagePath"]));
        }
    }
}
