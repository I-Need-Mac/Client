using BFM;
using System;
using System.Collections.Generic;
using UnityEngine;

//싱글톤 사용
public class PlayerManager : SingletonBehaviour<PlayerManager>
{
    //private Player player;

    public PlayerData playerData { get; private set; } = new PlayerData(); //플레이어의 데이터를 가지는 객체

    protected override void Awake()
    {
        //player = transform.parent.GetComponent<Player>();
        //DebugManager.Instance.PrintDebug(GameManager.Instance.GetPlayerId());
        PlayerSetting(FindCharacter(Convert.ToString(GameManager.Instance.GetPlayerId())));
    }

    private void Start()
    {
    }

    //캐릭터에 스탯 부여
    private void PlayerSetting(Dictionary<string, object> characterData)
    {
        if (characterData == null)
        {
            //없는 캐릭터일 경우 에러메시지 송출
            Debug.LogError("존재하지 않는 캐릭터입니다");
            return;
        }

        playerData.SetCharacterName(Convert.ToString(characterData["CharacterName"]));
        playerData.SetHp(Convert.ToInt32(characterData["HP"]));
        playerData.SetAttack(Convert.ToInt32(characterData["Attack"]));
        playerData.SetCriRatio(Convert.ToInt32(characterData["CriRatio"]));
        playerData.SetCriDamage(Convert.ToInt32(characterData["CriDamage"]));
        playerData.SetCoolDown(Convert.ToInt32(characterData["CoolDown"]));
        playerData.SetHpRegen(Convert.ToInt32(characterData["HPRegen"]));
        playerData.SetShield(Convert.ToInt32(characterData["Shield"]));
        playerData.SetProjectileAdd(Convert.ToInt32(characterData["ProjectileAdd"]));
        playerData.SetMoveSpeed(Convert.ToInt32(characterData["MoveSpeed"]));
        playerData.SetGetItemRange(Convert.ToInt32(characterData["GetItemRange"]));

        //Debug.Log("캐릭터 이름: " + playerData.characterName);
        //Debug.Log("캐릭터 체력: " + playerData.hp);
        //Debug.Log("캐릭터 공격력: " + playerData.attack);
        //Debug.Log("캐릭터 크리티컬 확률: " + playerData.criRatio);
        //Debug.Log("캐릭터 크리티컬 데미지: " + playerData.criDamage);
        //Debug.Log("캐릭터 쿨타임 감소량: " + playerData.coolDown);
        //Debug.Log("캐릭터 체젠량: " + playerData.hpRegen);
        //Debug.Log("캐릭터 쉴드개수: " + playerData.shield);
        //Debug.Log("캐릭터 투사체증가량: " + playerData.projectileAdd);
        //Debug.Log("캐릭터 이동속도: " + playerData.moveSpeed);
        //Debug.Log("캐릭터 아이템획득범위: " + playerData.getItemRange);

    }

    //캐릭터 id와 일치하는 행(Dictionary)을 리턴
    private Dictionary<string, object> FindCharacter(string characterId)
    {
        Dictionary<string, Dictionary<string, object>> playerStatusTable = CSVReader.Read("CharacterTable");

        //foreach -> ContainsKey로 변경
        //가독성을 위함, 유의미한 성능차이 없음
        if (playerStatusTable.ContainsKey(characterId))
        {
            return playerStatusTable[characterId];
        }
        //찾는 캐릭터가 없을 경우 null 리턴
        return null;
    }

}
