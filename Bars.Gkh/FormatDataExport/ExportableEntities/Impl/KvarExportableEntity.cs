namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Лицевые счета
    /// </summary>
    public class KvarExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "KVAR";

        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<KvarProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.PersonalAccountNum,
                        x.RealityObjectId.ToStr(),
                        x.Surname,
                        x.FirstName,
                        x.SecondName,
                        this.GetDate(x.BirthDate),
                        this.GetDate(x.OpenDate),
                        this.GetDate(x.CloseDate),
                        x.CloseReasonType.ToStr(),
                        x.CloseReason.ToStr(),
                        x.ResidentCount.ToStr(),
                        x.TempInResidentCount.ToStr(),
                        x.TempOutResidentCount.ToStr(),
                        x.RoomCount.ToStr(),
                        this.GetDecimal(x.Area),
                        this.GetDecimal(x.LivingArea),
                        this.GetDecimal(x.HeatedArea),
                        this.GetDecimal(x.RentArea),
                        x.HasElecticStove.ToStr(),
                        x.HasGasStove.ToStr(),
                        x.HasGeyser.ToStr(),
                        x.HasFireStove.ToStr(),
                        x.GasHouseTypeCode.ToStr(),
                        x.WaterHouseTypeCode.ToStr(),
                        x.HotWaterHouseTypeCode.ToStr(),
                        x.SewerHouseTypeCode.ToStr(),
                        x.HasOpenHeatingSystem.ToStr(),
                        x.HousingDepartmentPlace,
                        x.PrincipalContragentId.ToStr(),
                        x.PersonalAccountType.ToStr(),
                        x.IndividualOwner.ToStr(),
                        x.ContragentId.ToStr(),
                        x.IsArendator.ToStr(),
                        x.IsPartial.ToStr(),
                        x.State.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код лицевого счета в системе отправителя",
                "Уникальный номер лицевого счета в системе отправителя",
                "Уникальный код дома",
                "Фамилия абонента",
                "Имя абонента",
                "Отчество абонента",
                "Дата рождения абонента",
                "Дата открытия ЛС",
                "Дата закрытия ЛС",
                "Причина закрытия ЛС",
                "Основание закрытия ЛС",
                "Количество проживающих",
                "Количество временно прибывших жильцов",
                "Количество временно убывших жильцов",
                "Количество комнат",
                "Общая площадь (площадь, применяемая для расчета большинства площадных услуг)",
                "Жилая площадь",
                "Отапливаемая площадь",
                "Площадь для найма",
                "Наличие эл. Плиты",
                "Наличие газовой плиты",
                "Наличие газовой колонки",
                "Наличие огневой плиты",
                "Код типа жилья по газоснабжению",
                "Код типа жилья по водоснабжению",
                "Код типа жилья по горячей воде",
                "Код типа жилья по канализации",
                "Наличие забора из открытой системы отопления",
                "Участок (ЖЭУ)",
                "Код контрагента, с которым у потребителя ЖКУ заключен договор на оказание услуг (принципал)",
                "Тип лицевого счета",
                "Плательщик – Физ.лицо",
                "Плательщик – Организация",
                "Плательщик является нанимателем",
                "ЛС на помещение(-я) разделены",
                "Статус лицевого счета"
            };
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 2:
                case 7:
                case 15:
                case 30:
                case 35:
                    return row.Cells[cell.Key].IsEmpty();
                case 8:
                case 9:
                    return row.Cells[35] == "2" && row.Cells[cell.Key].IsEmpty(); // Статус ЛС = 2-Закрыт
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(ContragentExportableEntity),
                typeof(IndExportableEntity),
                typeof(HouseExportableEntity),
                typeof(KvaraccomExportableEntity),
                typeof(KvarOpenReasonExportableEntity));
        }
    }
}
