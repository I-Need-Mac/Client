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
		var data = Resources.Load(file) as TextAsset;

		if (data == null)
        {
			Debug.LogErrorFormat("[Read] {0} 파일을 찾을 수 없습니다.", file);
			return null;
        }

		string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

		if (lines.Length <= 3)
		{
			Debug.LogErrorFormat("[Read] 해당 파일의 내용이 부족합니다.");
			return null;
		}

		var list = new List<Dictionary<string, object>>();

		string[] header = Regex.Split(lines[0], SPLIT_RE);
		string[] dataType = Regex.Split(lines[2], SPLIT_RE);

		for (int i = 3; i < lines.Length; i++)
		{
			string[] values = Regex.Split(lines[i], SPLIT_RE);
			
			if (values.Length == 0 || string.IsNullOrEmpty(values[0]))
			{
				continue;
			}

			var entry = new Dictionary<string, object>();

			for (int j = 0; j < header.Length && j < values.Length; j++)
			{
				string _value = values[j].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
				object finalValue = _value;
				int n;
				float f;

				if (dataType[j].Equals("Int_Arr"))
				{
					string[] arrValue = Regex.Split(_value, ",");

					if (arrValue.Length > 0)
					{
						int[] iArr = new int[arrValue.Length];

						for (int k = 0; k < arrValue.Length; k++)
						{
							iArr[k] = int.Parse(arrValue[k]);
						}

						finalValue = iArr;
					}
				}
				if (dataType[j].Equals("Int") && int.TryParse(_value, out n))
				{
					finalValue = n;
				}
				else if (dataType[j].Equals("Float") && float.TryParse(_value, out f))
				{
					finalValue = f;
				}
				
				entry[header[j]] = finalValue;
			}

			list.Add(entry);
		}
		return list;
	}

	public static Dictionary<string, object> FindRead(string file, string header, object value)
    {
		var data = Resources.Load(file) as TextAsset;

		if (data == null)
		{
			Debug.LogErrorFormat("[FindRead] {0} 파일을 찾을 수 없습니다.", file);
			return null;
		}

		string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

		if (lines.Length <= 3)
		{
			Debug.LogErrorFormat("[FindRead] 해당 파일의 내용이 부족합니다.");
			return null;
		}

		string[] _header = Regex.Split(lines[0], SPLIT_RE);
		string[] dataType = Regex.Split(lines[2], SPLIT_RE);
		bool isFound = false;

		for (int i = 3; i < lines.Length; i++)
		{
			string[] values = Regex.Split(lines[i], SPLIT_RE);

			if (values.Length == 0 || string.IsNullOrEmpty(values[0]))
			{
				continue;
			}

			var entry = new Dictionary<string, object>();

			for (int j = 0; j < _header.Length && j < values.Length; j++)
			{
				string _value = values[j].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
				object finalValue = _value;
				int n;
				float f;

				if (dataType[j].Equals("Int_Arr"))
                {
					string[] arrValue = Regex.Split(_value, ",");

					if (arrValue.Length > 0)
                    {
						int[] iArr = new int[arrValue.Length];

						for (int k = 0; k < arrValue.Length; k++)
						{
							iArr[k] = int.Parse(arrValue[k]);
						}

						finalValue = iArr;
					}
                }
				else if (dataType[j].Equals("Int") && int.TryParse(_value, out n))
				{
					finalValue = n;
				}
				else if (dataType[j].Equals("Float") && float.TryParse(_value, out f))
				{
					finalValue = f;
				}

				if (_header[j] == header && finalValue.ToString() == value.ToString())
				{
					isFound = true;
				}

				entry[_header[j]] = finalValue;
			}

			if (isFound)
            {
				return entry;
            }
		}

		return null;
	}
}