namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Дом
    /// </summary>
    public class HouseExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "HOUSE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.All ^
            FormatDataExportProviderFlags.RegOpWaste ^
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<HouseProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.City,
                        x.Settlement,
                        x.Street,
                        x.House,
                        x.Building,
                        x.Housing,
                        x.Letter,
                        x.HouseGuid,
                        x.OktmoCode,
                        x.TimeZone,
                        x.IsNumberExists.ToStr(),
                        x.CadastralHouseNumber,
                        x.EgrpNumber,
                        x.TypeHouse.ToStr(),
                        x.ManyHouseWithOneAddress.ToStr(),
                        this.GetDecimal(x.AreaMkd),
                        x.ConditionHouse.ToStr(),
                        this.GetDate(x.CommissioningYear),
                        x.MaximumFloors.ToStr(),
                        x.MinimumFloors.ToStr(),
                        x.UndergroundFloorCount.ToStr(),
                        x.IsCulturalHeritage.ToStr(),
                        this.GetDecimal(x.AreaCommonUsage),
                        this.GetDecimal(x.HeatingArea),
                        this.GetDate(x.BuildYear),
                        x.PersonalAccountCount.ToStr(),
                        this.GetDecimal(x.AreaLiving),
                        x.AccountFormationVariant.ToStr(),
                        x.TypeManagement.ToStr(),
                        x.LifeCycleStage,
                        this.GetDate(x.ReconstructionYear),
                        x.ContragentId.ToStr(),
                        x.ComfortСategory.ToStr(),
                        this.GetDate(x.DestroyDate),
                        x.NoInstallationPu.ToStr(),
                        x.ReasonAbsencePu.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 8:
                case 9:
                case 10:
                case 11:
                case 14:
                case 35:
                    return row.Cells[cell.Key].IsEmpty();
                case 16:
                case 17:
                case 18:
                case 19:
                case 21:
                case 22:
                    return row.Cells[14] == "1" && row.Cells[cell.Key].IsEmpty();
                case 12:
                case 13:
                    return row.Cells[11] == "1" && row.Cells[12].IsEmpty() && row.Cells[13].IsEmpty();
                case 36:
                    return row.Cells[35] == "1" && (row.Cells[14] == "2" || row.Cells[14] == "3") && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код дома в системе отправителя",
                "Город/район",
                "Населенный пункт",
                "Улица",
                "Номер дома",
                "Строение (секция)",
                "Корпус",
                "Литера",
                "Код дома ФИАС",
                "Код ОКТМО",
                "Часовая зона",
                "Наличие сведений о кадастровом номере или условном номере дома в ЕГРП/ГКН",
                "Кадастровый номер",
                "Условный номер ЕГРП",
                "Тип дома",
                "Несколько домов с одинаковым адресом",
                "Общая площадь дома",
                "Состояние дома",
                "Год ввода в эксплуатацию",
                "Количество этажей (максимальное кол-во этажей в доме)",
                "Минимальная этажность (минимальное кол-во этажей в доме)",
                "Количество подземных этажей",
                "Наличие у дома статуса объекта культурного наследия",
                "Площадь мест общего пользования",
                "Полезная (отапливаемая площадь)",
                "Год постройки (указывается 1 число года, например 01.01.1900)",
                "Количество лицевых счетов в доме",
                "Общая площадь жилых помещений по паспорту помещения",
                "Способ формирования фонда капитального ремонта",
                "Способ управления домом",
                "Стадия жизненного цикла",
                "Год проведения реконструкции дома",
                "Код контрагента, с которым заключен договор на управление домом",
                "Категория благоустроенности",
                "Дата прекращения существования объекта",
                "Отсутствует установленный ПУ",
                "Причина отсутствия ПУ"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(UoExportableEntity));
        }
    }
}