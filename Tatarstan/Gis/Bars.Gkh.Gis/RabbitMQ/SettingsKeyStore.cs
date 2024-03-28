namespace Bars.Gkh.Gis.RabbitMQ
{
    /// <summary>
    /// Класс для настроек RabbitMQ
    /// </summary>
    public class SettingsKeyStore
    {
        /// <summary>Ip</summary>
        public static string Ip = "rabbitIp";
        
        /// <summary>Порт</summary>
        public static string Port = "rabbitPort";
        
        /// <summary>Логин</summary>
        public static string Login = "rabbitLogin";
        
        /// <summary>Пароль</summary>
        public static string Password = "rabbitPassword";
        
        /// <summary>Виртуальный хост</summary>
        public static string VirtualHost = "rabbitVirtualHost";
        
        /// <summary>Работать через RabbitMQ?</summary>
        public static string Enable = "rabbitEnable";
    }
}
