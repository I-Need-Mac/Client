using System.Collections;
using System.Collections.Generic;

public class AchievementData
{
    public AchievementData()
    {
        this.monsterKillCount = new Dictionary<int, int>();
        this.stagePlayCount = new Dictionary<int, int>();
        this.isStageClear = new Dictionary<int, bool>();
        this.stageClearCount = new Dictionary<int, int>();
        this.skillLevelMaxCount = new Dictionary<int, int>();
        this.acquireSkillCount = new Dictionary<int, int>();
    }

    #region Function


    #endregion

    #region Data
    public Dictionary<int, int> monsterKillCount { get; private set; }                       //몬스터별 킬 횟수
    public int levelUpCount { get; private set; }                                            //레벨업 횟수
    public int statueDestroyCount { get; private set; }                                      //석상 파괴 개수

    public Dictionary<int, int> stagePlayCount { get; private set; }                         //스테이지별 플레이 횟수
    public int stagePlayTotalCount                                                           //총 스테이지 플레이 횟수
    {
        get
        {
            int total = 0;
            foreach (int count in stagePlayCount.Values)
            {
                total += count;
            }
            return total;
        }
        private set
        {

        }
    }

    public Dictionary<int, bool> isStageClear { get; private set; }                          //스테이지별 클리어 여부
    public Dictionary<int, int> stageClearCount { get; private set; }                        //스테이지별 클리어 횟수
    public int stageClearTotalCount                                                          //총 스테이지 클리어 횟수
    {
        get
        {
            int total = 0;
            foreach (int count in stageClearCount.Values)
            {
                total += count;
            }
            return total;
        }
        private set
        {

        }
    }
    
    public int useKeyCount { get; private set; }                                             //열쇠 사용 횟수
    public int acquireKeyCount { get; private set; }                                         //획득한 열쇠 개수
    public int useBoxCount { get; private set; }                                             //박스 사용 횟수
    public int acquireBoxCount { get; private set; }                                         //획득한 박스 개수

    public Dictionary<int, int> skillLevelMaxCount { get; private set; }                     //스킬별 만렙 달성 횟수
    public Dictionary<int, int> acquireSkillCount { get; private set; }                      //스킬별 획득 횟수

    //특정 레벨 이전 사망 체크 생각해봐야함
    #endregion
}
