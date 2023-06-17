
public class MonsterSponeData
{
    public int sponeTime { get; private set; }
    public int sponeMobId { get; private set; }
    public int sponeMobAmount { get; private set; }
    public SpawnMobLocation spawnMobLocation { get; private set; }
    public string sponeMobObject { get; private set; }

    public void SetSponeTime(int sponeTime) { this.sponeTime = sponeTime; }
    public void SetSponeMobID(int sponeMobId) { this.sponeMobId = sponeMobId; }
    public void SetSponeMobAmount(int sponeMobAmount) { this.sponeMobAmount = sponeMobAmount; }
    public void SetSponeMobLocation(SpawnMobLocation sponeMobLocation) { this.spawnMobLocation = sponeMobLocation; }
    public void SetSponeMobObject(string sponeMobObject) { this.sponeMobObject = sponeMobObject; }
}