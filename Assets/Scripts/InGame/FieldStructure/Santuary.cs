using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santuary : FieldStructure
{
    private const string SANTUARY_CIRCLE_PATH = "Prefabs/InGame/FieldStructure/FieldStructure_SantuaryCircle";

    [SerializeField] float speed = 5.0f;

    private bool isActive;
    private float diffCount;
    private int needKillCount;
    private int currentKillCount;

    private SantuaryCircle santuaryCircle;

    protected override void Awake()
    {
        base.Awake();

        isActive = false;
        needKillCount = int.Parse(this.fieldStructureData.gimmickParam[0]);

        santuaryCircle = Instantiate(ResourcesManager.Load<SantuaryCircle>(SANTUARY_CIRCLE_PATH), transform);
    }

    private void Update()
    {
        if (!isActive)
        {
            StartCoroutine(Activation());
        }

        //top.transform.localScale = Vector2.one * (diffCount / needKillCount);
        top.transform.localScale = Vector2.Lerp(top.transform.localScale, Vector2.one * (diffCount / needKillCount), Time.deltaTime * speed);
    }

    private IEnumerator Activation()
    {
        isActive = true;
        WaitForFixedUpdate tick = new WaitForFixedUpdate();
        currentKillCount = GameManager.Instance.killCount;
        //while (GameManager.Instance.killCount - currentKillCount < needKillCount)
        //{
        //    yield return tick;
        //}
        do
        {
            diffCount = GameManager.Instance.killCount - currentKillCount;
            yield return tick;
        } while (diffCount < needKillCount);

        yield return santuaryCircle.Activation();

        isActive = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
