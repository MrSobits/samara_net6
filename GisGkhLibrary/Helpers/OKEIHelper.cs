using GisGkhLibrary.Enums;
using System;

namespace GisGkhLibrary.Helpers
{
    internal static class OKEIHelper
    {
        internal static OKEI? GetOKEI(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            if (!Enum.TryParse(code, out OKEI result))
                throw new ApplicationException($"Не удалось найти ОКЕИ с идентификатором {code}");

            return result;
        }
    }
}
