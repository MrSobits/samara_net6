namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сотрудники
    /// </summary>
    public class SotrudExportableEntity : BaseExportableEntity<ContragentContact>
    {
        /// <inheritdoc />
        public override string Code => "SOTRUD";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Omjk;

        private long leaderId;
        private long accountantId;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            this.leaderId = this.SelectParams.GetAsId("LeaderPositionId");
            this.accountantId = this.SelectParams.GetAsId("AccountantPositionId");

            return this.GetFiltred(x => x.Contragent)
                .Select(x => new
                {
                    x.Id,
                    ContragentId = x.Contragent.ExportId,
                    x.Surname,
                    x.Name,
                    x.Patronymic,
                    PositionName = x.Position.Name,
                    x.Email,
                    x.Snils,
                    PositionId = (long?) x.Position.Id,
                    x.Phone
                })
                .AsEnumerable()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.ContragentId.ToStr(), // 2. Контрагент
                        (x.Surname?.Split()[0]).Cut(50), // 3. Фамилия
                        x.Name.Cut(50), // 4. Имя
                        x.Patronymic.Cut(50), // 5. Отчество
                        x.PositionName.Cut(50), // 6. Должность
                        x.Email.Cut(50), // 7. Адрес электронной почты
                        x.Snils.Cut(11), // 8. СНИЛС
                        this.GetPositionCode(x.PositionId), // 9. Признак руководителя/бухгалтера
                        x.Phone.Cut(50) // 10. Телефон
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => new List<int> { 0, 1 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Контрагент",
                "Фамилия",
                "Имя",
                "Отчество",
                "Должность",
                "Адрес электронной почты",
                "СНИЛС",
                "Признак руководителя/бухгалтера",
                "Телефон"
            };
        }

        private string GetPositionCode(long? positionId)
        {
            // 1-руководитель
            // 2-бухгалтер
            // 3-руководитель и бухгалтер
            // 4-все остальные сотрудники
            if (positionId == this.leaderId)
            {
                return "1";
            }

            if (positionId == this.accountantId)
            {
                return "2";
            }

            return "4";
        }
    }
}