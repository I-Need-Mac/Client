using System.Collections;
using System.Collections.Generic;

public class GroupData
{
    public int groupId { get; private set; }
    public string groupSource { get; private set; }
    public bool isMultiple { get; private set; }
    public string packageId { get; private set; }
    public int packageProbability { get; private set; }

    public void SetGroupId(int groupId) { this.groupId = groupId; }
    public void SetGroupSource(string groupSource) { this.groupSource = groupSource; }
    public void SetIsMultiple(bool isMultiple) { this.isMultiple = isMultiple; }
    public void SetPackageId(string packageId) { this.packageId = packageId; }
    public void SetPackageProbability(int packageProbability) { this.packageProbability = packageProbability; }
}
