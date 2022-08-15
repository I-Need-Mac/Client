using System;

public class EnumUtil<T>
{
    public static T ParseString(string value)
    {
        return (T)Enum.Parse(typeof(T), value);
    }
}
