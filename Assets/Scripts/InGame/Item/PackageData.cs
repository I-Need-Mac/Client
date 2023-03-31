using System.Collections;
using System.Collections.Generic;

public class PackageData
{
    public int packageId { get; private set; }
    public string packageSource { get; private set; }
    public bool isMultiple { get; private set; }
    public int itemId { get; private set; }
    public int itemAmount { get; private set; }
    public int itemProbability { get; private set; }

    public void SetPackageId(int packageId) { this.packageId = packageId; }
    public void SetPackageSource(string packageSource) { this.packageSource = packageSource; }
    public void SetIsMultiple(bool isMultiple) { this.isMultiple = isMultiple; }
    public void SetItemId(int itemId) { this.itemId = itemId; }
    public void SetItemAmount(int itemAmount) { this.itemAmount = itemAmount; }
    public void SetItemProbabilitie(int itemProbability) { this.itemProbability = itemProbability; }
}
