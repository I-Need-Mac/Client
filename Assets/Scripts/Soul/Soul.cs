using System;
using System.Collections;
using System.Collections.Generic;

public class Soul
{
    private SoulData soulData;

    private void SetSoul(string id)
    {
        soulData = new SoulData();

        Dictionary<string, object> table = CSVReader.Read("MainCategorySoul")[id];
        soulData.SetSoulId(int.Parse(id));
        soulData.SetSoulName(Convert.ToString(table["SoulNameText"]));
        soulData.SetSoulExplain(Convert.ToString(table["SoulExplainText"]));
        soulData.SetSoulImage(Convert.ToString(table["SoulImagePath"]));

        try
        {
            List<SoulEffect> list = new List<SoulEffect>();
            foreach (string str in (table["SoulEffectEnum"] as List<string>))
            {
                if (Enum.TryParse(str, true, out SoulEffect soulEffect))
                {
                    list.Add(soulEffect);
                }
            }
            soulData.SetSoulEffect(list);
        }
        catch
        {
            try
            {
                List<SoulEffect> list2 = new List<SoulEffect>();
                if (Enum.TryParse(table["SoulEffectEnum"].ToString(), true, out SoulEffect soulEffect))
                {
                    list2.Add(soulEffect);
                }
                soulData.SetSoulEffect(list2);
            }
            catch
            {
                soulData.SetSoulEffect(new List<SoulEffect>());
            }
        }

        try
        {
            List<float> list = new List<float>();
            foreach (string str in (table["SoulEffectParam"] as List<string>))
            {
                if (float.TryParse(str, out float param))
                {
                    list.Add(param);
                }
            }
            soulData.SetEffectParams(list);
        }
        catch
        {
            if (float.TryParse(table["SoulEffectParam"].ToString(), out float param))
            {
                soulData.SetEffectParams(new List<float>() { param, });
            }
            else
            {
                soulData.SetEffectParams(new List<float>());
            }
        }

        soulData.SetCategorySoul(Convert.ToString(table["SoulMainCategory"]));
        soulData.SetColGroup(Convert.ToInt32(table["SoulColumnGroup"]));
        soulData.SetOrderInCol(Convert.ToInt32(table["SoulOrderInColumn"]));
    }
}
