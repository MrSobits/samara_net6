namespace Bars.GkhGji.Regions.Tomsk.Utils
{
    using System.Text;

    public static class ArrayExtensions
    {
        public static string Encode(this byte[] array)
        {
            if (array == null)
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(array);
        }
    }
}
