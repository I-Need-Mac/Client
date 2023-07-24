using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : ActiveSkill
{
    private const int CHANGE_ID = 0;
    private const string ANIMATOR_PATH = "";
    private const string SKELETONDATA_ASSET_PATH = "";

    public Possession(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        Player player = shooter.GetComponent<Player>();
        Dictionary<string, Dictionary<string, object>> table = CSVReader.Read("CharacterTable");

        if (!skillData.isEffect)
        {
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        do
        {
            //변신
            yield return duration;
            //원래대로
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0.0f);

        
    }
}
