namespace Bars.GkhDi.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Обязательные работы, обеспечивающие надлежащее содержание МКД
    /// </summary>
    public class WorkRequiredExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "WORKREQUIRED";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<WorkUslugaProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        this.GetWorkClassification(x.Name)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код работы/услуги",
                "Классификатор видов работ (услуг)"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "WORKUSLUGA"
            };
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => this.GetAllFieldIds();

        private string GetWorkClassification(string name)
        {
            switch (name)
            {
                case "Пользование лифтов":
                case "Содержание лифтов": 
                    return "1.2.9";
                case "Вывоз ЖБО":
                    return "1.3.4.2";
                case "Обслуживание мусоропроводов":
                    return "1.2.1";
                case "Уборка внутридомовых мест общего пользования":
                    return "1.3.1";
                case "Дератизация":
                    return "1.3.1.5";
                case "Уборка придомовой территории":
                    return "1.3.2";
                case "Вывод ТБО":
                    return "1.3.4";
                case "Текущий ремонт и содержание внутридомовых инженерных сетей электроснабжения":
                case "Техническое обслуживание и ремонт систем коллективного приема телесигнала":
                    return "1.2.7";
                case "Текущий ремонт жилого здания и благоустройство территории":
                    return "1.3.6";
                case "Текущий ремонт и содержание внутридомовых инжерных сетей центрального отопления":
                    return "1.2.6";
                case "Текущий ремонт и содержание внутридомовых инженерных сетей водоснабжения и водоотведения":
                    return "1.2.5";
                case "Текущий ремонт и содержание внутридомовых инженерных сетей газоснабжения":
                case "Обслуживание систем видеонаблюдения":
                    return "1.2.8";
                case "Техническое обслуживание системы контроля и управления доступом":
                    return "1.2.9";
                case "Обслживание, ремонт технических средств и систем пожаротушенияи дымоудаления":
                case "Техничекое обслуживание и ремонт вентиляционного оборудования и систем вентиляции":
                    return "1.2.2 ";
                case "Утилизация твердых бытовых отходов":
                    return "1.3.4";
                case "Техническое обслуживание автоматизированной противопожарной защиты и станции пожаротушения":
                    return "1.3.5";
                case "Консьерж":
                case "Охрана":
                case "Обслуживание домофона":
                    return "1.3";
                case "Мытье окон":
                    return "1.3.1.3";
            }

            return null;
        }
    }
}