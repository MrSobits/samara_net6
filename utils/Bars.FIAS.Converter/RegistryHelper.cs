namespace Bars.FIAS.Converter
{
    using System;
    using System.Text;

    using Microsoft.Win32;

    public class RegistryHelper
    {
        private readonly string fiasPath = "BarsGroup\\FiasConverter";
        private readonly string connectionStringKeyName = "ConnectionString";

        public string GetConnectionstring()
        {
            try
            {
                var connectionString = this.GetKey();
                return Encoding.ASCII.GetString(this.Salt(Convert.FromBase64String(connectionString)));
            }
            catch
            {
                return null;
            }
        }

        public bool SaveConnectionString(string connectionString)
        {
            try
            {
                var base64 = Convert.ToBase64String(this.Salt(Encoding.ASCII.GetBytes(connectionString)));
                this.SaveKey(base64);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SaveKey(string keyValue)
        {
            using (var hkcu = Registry.CurrentUser)
            {
                var hkSoftware = hkcu.CreateSubKey("SOFTWARE");

                var hkFias = hkSoftware.CreateSubKey(this.fiasPath);

                hkFias.SetValue(this.connectionStringKeyName, keyValue, RegistryValueKind.String);
            }

        }

        private string GetKey()
        {
            using (var hkcu = Registry.CurrentUser)
            {
                var hkSoftware = hkcu.OpenSubKey("SOFTWARE");

                var hkFias = hkSoftware.OpenSubKey(this.fiasPath);

                return hkFias.GetValue(this.connectionStringKeyName, string.Empty).ToString();
            }
        }

        private byte[] Salt(byte[] data)
        {
            byte[] result = data;
            byte[] salt = {192, 168, 141, 3};

            for (int i = 0; i < data.Length; i++)
            {
                result[i] ^= salt[(i + 7) % salt.Length];
            }

            return result;
        }
    }
}