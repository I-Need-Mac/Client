using System.Collections;
using UnityEngine;

public enum BEAM_TYPE
{
    WIDTH,
    VERTICAL,
    CIRCLE,
    CROSS,
}

public class DestructionStone : FieldStructure
{
    private const string BEAM_PATH = "Prefabs/InGame/FieldStructure/";

    [SerializeField] private BEAM_TYPE type = BEAM_TYPE.WIDTH;

    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = front.GetComponent<SpriteRenderer>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!front.enabled)
        {
            return;
        }

        if (collision.gameObject.layer == (int)LayerConstant.SKILL)
        {
            StartCoroutine(Activation());
            switch (type)
            {
                case BEAM_TYPE.WIDTH:
                    Beam width = GameObject.Instantiate(ResourcesManager.Load<Beam>(BEAM_PATH + "BeamWidth"), transform);
                    width.BeamInit(float.Parse(this.fieldStructureData.gimmickParam[0]), float.Parse(this.fieldStructureData.gimmickParam[1]));
                    break;
                case BEAM_TYPE.VERTICAL:
                    Beam vertical = GameObject.Instantiate(ResourcesManager.Load<Beam>(BEAM_PATH + "BeamVertical"), transform);
                    vertical.BeamInit(float.Parse(this.fieldStructureData.gimmickParam[0]), float.Parse(this.fieldStructureData.gimmickParam[1]));
                    break;
                case BEAM_TYPE.CIRCLE:
                    Beam circle = GameObject.Instantiate(ResourcesManager.Load<Beam>(BEAM_PATH + "BeamCircle"), transform);
                    circle.BeamInit(float.Parse(this.fieldStructureData.gimmickParam[0]), float.Parse(this.fieldStructureData.gimmickParam[0]));
                    break;
                case BEAM_TYPE.CROSS:
                    Beam crossWidth = GameObject.Instantiate(ResourcesManager.Load<Beam>(BEAM_PATH + "BeamWidth"), transform);
                    Beam crossVertical = GameObject.Instantiate(ResourcesManager.Load<Beam>(BEAM_PATH + "BeamVertical"), transform);
                    crossWidth.BeamInit(float.Parse(this.fieldStructureData.gimmickParam[0]), float.Parse(this.fieldStructureData.gimmickParam[1]));
                    crossVertical.BeamInit(float.Parse(this.fieldStructureData.gimmickParam[1]), float.Parse(this.fieldStructureData.gimmickParam[0]));
                    break;
                default:
                    DebugManager.Instance.PrintError("[DestructionStone]: 없는 Type 입니다.");
                    break;
            }
        }
    }

    private IEnumerator Activation()
    {
        front.enabled = false;
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(this.fieldStructureData.coolTime);
        spriteRenderer.enabled = true;
        front.enabled = true;
    }
}
