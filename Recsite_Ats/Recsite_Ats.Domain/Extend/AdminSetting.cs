namespace Recsite_Ats.Domain.Extend;
public class AdminSetting
{
    public enum Setting
    {
        Companies = 1,
        Jobs = 2,
        Contacts = 3,
        Candidates = 4
    }

    public enum ContactType
    {
        Owner = 1,
        Recruiter = 2,
    }

    public static bool IsValidateEnumName<TEnum>(string name) where TEnum : struct, Enum
    {
        return Enum.TryParse<TEnum>(name, out _) && Enum.IsDefined(typeof(TEnum), name);
    }

    public static TEnum ParseEnumName<TEnum>(string name) where TEnum : struct, Enum
    {
        if (Enum.TryParse<TEnum>(name, out var result) && Enum.IsDefined(typeof(TEnum), result))
        {
            return result;
        }

        throw new ArgumentException($"'{name}' is not a valid name in the enum '{typeof(TEnum).Name}'.");
    }
}

