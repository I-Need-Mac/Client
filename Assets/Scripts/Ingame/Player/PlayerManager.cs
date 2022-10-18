using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class PlayerManager : MonoBehaviour
{
    [Header("--- Script ---")]
    [SerializeField] private Player player;
    [SerializeField] private GameManager GAME;
    private PlayerData playerData;

    [Header("--- Image ---")]
    [SerializeField] private Image image_characterIcon;
    [SerializeField] private Image image_hpFill;
    [SerializeField] private Image image_ultimateIcon;
    [SerializeField] private Image[] image_skillIcons;

    [Header("--- Text ---")]
    [SerializeField] private TMP_Text text_nickname;
    [SerializeField] private TMP_Text text_money;

    private int maxHp;

    private void Awake()
    {
        playerData = player.playerData;
        SetPlayerStat();

        Init();
    }

    private void Init()
    {
        player.Init();

        maxHp = 0;
    }

    //플레이어 위치 변경
    public void SetPlayerPos(Vector2 newPos)
    {
        player.transform.position = newPos;
    }

    private void SetPlayerStat()
    {
        string name = "abc";
        int temp = 0;
        int hp = 100;
        int ulti = 0;
        int skill = 0;
        maxHp = hp;

        playerData.SetCharacterName(name);
        playerData.SetHp(hp);
        playerData.SetAtk(temp);
        playerData.SetCriRatio(temp);
        playerData.SetCriDamage(temp);
        playerData.SetCoolDown(temp);
        playerData.SetHpRegen(temp);
        playerData.SetShield(temp);
        playerData.SetProjectileAdd(temp);
        playerData.SetMoveSpeed(temp);
        playerData.SetGetItemRange(temp);
        playerData.SetUltimateID(ulti);
        playerData.SetskillID(skill);

        maxHp = hp;

        if (playerData.ultimateID > 0)
        {
            GetSkillData(playerData.ultimateID, ref playerData.ultimateData);
        }
        if (playerData.skillID > 0)
        {
            GetSkillData(playerData.skillID, ref playerData.skillData);
        }

        text_nickname.text = name;
        text_money.text = temp.ToString();
        //image_characterIcon.sprite = ;

        SetHpBar();
    }

    private void GetSkillData(int skillID, ref SkillData skillData)
    {
        Debug.LogFormat("[GetSkillData] Find {0} SkillTable", skillID);
        //Dictionary<string, object> skillTableInfo = CSVReader.FindRead("SkillTable", "SkillID", skillID);

        //if (skillTableInfo != null)
        //{
        //    try
        //    {
        //        skillData.skillID = (int)skillTableInfo["SkillID"];
        //        skillData.name = (string)skillTableInfo["Name"];
        //        skillData.desc = (string)skillTableInfo["Desc"];
        //        skillData.icon = (string)skillTableInfo["Icon"];
        //        skillData.coolTime = (int)skillTableInfo["Cooltime"];
        //        skillData.skillCut = (bool)skillTableInfo["Skill_Cut"];
        //        skillData.cutDire = (string)skillTableInfo["Cut_dire"];
        //        skillData.skillImg = (string)skillTableInfo["SkillImage"];
        //        skillData.atkDis = (int)skillTableInfo["AttackDistance"];
        //        skillData.isEffect = (bool)skillTableInfo["IsEffect"];
        //        skillData.isUltimate = (bool)skillTableInfo["IsUltimate"];
        //        skillData.damage = (int)skillTableInfo["Damage"];
        //        skillData.speed = (int)skillTableInfo["Speed"];
        //        skillData.isSplash = (bool)skillTableInfo["IsSplash"];
        //        skillData.isPenetrate = (bool)skillTableInfo["IsPenetrate"];
        //        skillData.splashRange = (int)skillTableInfo["SplashRange"];
        //        skillData.projectileSizeMulti = (int)skillTableInfo["ProjectileSizeMulti"];
        //        skillData.skillEffectParam = (int)skillTableInfo["SkillEffectParam"];
        //        skillData.skillEffect = EnumUtil<SkillEffect>.ParseString(skillTableInfo["SkillEffect"].ToString());
        //        skillData.skillTarget = EnumUtil<SkillTarget>.ParseString(skillTableInfo["SkillTarget"].ToString());
        //        skillData.projectileType = EnumUtil<ProjectileType>.ParseString(skillTableInfo["ProjectileType"].ToString());
        //    }
        //    catch (InvalidCastException invalidCastEx)
        //    {
        //        Debug.LogError("[GetSkillData] Data Type Error");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError("[GetSkillData] " + ex.Message);
        //    }
        //}
        //else
        //{
        //    Debug.LogError("[GetSkillData] skillTableInfo is null.");
        //}
    }

    #region UI
    public void SetHpBar()
    {
        if (maxHp != 0)
        {
            image_hpFill.fillAmount = playerData.hp / maxHp;
        }
    }

    public void SetText_Money()
    {

    }

    public void SetImage_UltimateIcon()
    {

    }

    public void SetImage_SkillIcon()
    {

    }
    #endregion
}
