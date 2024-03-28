using Bars.B4;

namespace Bars.Gkh.Domain
{
    using System;

    /// <summary>
    /// Интерфейс сервиса для получения текущей версии сборки
    /// </summary>
    public interface ISystemVersionService
    {
        /// <summary>
        /// Метод возвращает информацию о версии приложения
        /// </summary>
        /// <returns>Информация о версии</returns>
        IDataResult<VersionInfo> GetVersionInfo();
    }

    /// <summary>
    /// Класс информации о версии сборки
    /// </summary>
    public class VersionInfo
    {
        /// <summary>
        /// Версия приложения.
        /// <remarks>Формат версии {major}.{minor}.{hotfix}</remarks>
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// Номер сборки
        /// </summary>
        public int BuildNumber { get; set; }

		/// <summary>
        /// Название ветки
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Название региона
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Дата сборки
        /// </summary>
        public DateTime BuildDate { get; set; }
    }
}