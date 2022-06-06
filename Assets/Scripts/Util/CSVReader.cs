using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	static char[] TRIM_CHARS = { '\"' };

	public static List<Dictionary<string, object>> Read(string file)
	{
		TextAsset data = Resources.Load(file) as TextAsset;

		if (data == null)
        {
			Debug.LogErrorFormat("[Read] {0} 파일을 찾을 수 없습니다.", file);
			return null;
        }

		var lines = Regex.Split(data.text, LINE_SPLIT_RE);

		if (lines.Length <= 3)
		{
			Debug.LogErrorFormat("[Read] 해당 파일의 내용이 부족합니다.");
			return null;
		}

		var list = new List<Dictionary<string, object>>();

		var header = Regex.Split(lines[0], SPLIT_RE);
		for (var i = 4; i < lines.Length; i++)
		{
			var values = Regex.Split(lines[i], SPLIT_RE);
			
			if (values.Length == 0 || values[0] == "")
			{
				continue;
			}

			var entry = new Dictionary<string, object>();

			for (var j = 0; j < header.Length && j < values.Length; j++)
			{
				string value = values[j].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
				object finalvalue = value;
				int n;
				float f;

				if (int.TryParse(value, out n))
				{
					finalvalue = n;
				}
				else if (float.TryParse(value, out f))
				{
					finalvalue = f;
				}

				entry[header[j]] = finalvalue;
			}

			list.Add(entry);
		}
		return list;
	}
}