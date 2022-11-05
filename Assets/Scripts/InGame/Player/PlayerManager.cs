using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private string testCharacterId; //테스트용 필드

    private PlayerData playerData; //플레이어의 데이터를 가지는 객체
    private int maxHp; //최대 체력

    private void Awake()
    {
        playerData = player.playerData;
        SetPlayerStatus(testCharacterId);
    }

    //캐릭터에 스탯 부여
    private void SetPlayerStatus(string characterId)
    {
        Dictionary<string, object> findRow = FindCharacter(characterId);
        if(findRow == null)
        {
            //없는 캐릭터일 경우 에러메시지 송출
            Debug.LogError("존재하지 않는 캐릭터입니다");
            return;
        }

        playerData.SetCharacterName(findRow["Character_Name"].ToString());
        playerData.SetHp(Convert.ToInt32(findRow["HP"]));
        maxHp = playerData.hp;
        playerData.SetAttack(Convert.ToInt32(findRow["Attack"]));
        playerData.SetCriRatio(Convert.ToInt32(findRow["CriRatio"]));
        playerData.SetCriDamage(Convert.ToInt32(findRow["CriDamage"]));
        playerData.SetCoolDown(Convert.ToInt32(findRow["CoolDown"]));
        playerData.SetHpRegen(Convert.ToInt32(findRow["HPRegen"]));
        playerData.SetShield(Convert.ToInt32(findRow["Shield"]));
        playerData.SetProjectileAdd(Convert.ToInt32(findRow["ProjectileAdd"]));
        playerData.SetMoveSpeed(Convert.ToInt32(findRow["MoveSpeed"]));
        playerData.SetGetItemRange(Convert.ToInt32(findRow["GetItemRange"]));

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
