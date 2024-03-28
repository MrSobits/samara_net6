namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Протоколы общего собрания собственников
    /// </summary>
    public class ProtocolossExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PROTOCOLOSS";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<ProtocolossProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.RealityObjectId.ToStr(),
                        this.Contragent.ExportId.ToStr(),
                        this.GetDate(x.DocumentDate),
                        x.DocumentNumber,
                        this.GetDate(x.StartDate),
                        x.VotingForm.ToStr(),
                        this.GetDate(x.EndDate),
                        x.DecisionPlace,
                        x.MeetingPlace,
                        this.GetDateTime(x.MeetingDateTime),
                        this.GetDateTime(x.VoteStartDateTime),
                        this.GetDateTime(x.VoteEndDateTime),
                        x.ReceptionProcedure,
                        x.ReviewProcedure,
                        x.IsAnnualMeeting.ToStr(),
                        x.IsCompetencyMeeting.ToStr(),
                        x.Status.ToStr(),
                        x.ChangeReason
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
                case 2:
                case 3:
                case 4:
                case 6:
                    return row.Cells[cell.Key].IsEmpty();
                case 7:
                case 8:
                    return (row.Cells[6] == "1" || row.Cells[6] == "4") && row.Cells[cell.Key].IsEmpty(); //  форма проведения=(1.Заочное голосование, 4.Очно - заочное голосование)
                case 9:
                case 10:
                    return (row.Cells[6] == "2" || row.Cells[6] == "4") && row.Cells[cell.Key].IsEmpty(); // форма проведения=(2.Очное, 4.Очно - заочное)
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    return row.Cells[6] == "3" && row.Cells[cell.Key].IsEmpty(); // 3-Заочное голосование с использованием системы
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Уникальный идентификатор дома",
                "Владелец протокола",
                "Дата составления протокола",
                "Номер протокола",
                "Дата вступления в силу",
                "Форма проведения голосования",
                "Дата окончания приема решений",
                "Место приема решений",
                "Место проведения собрания",
                "Дата и время проведения собрания",
                "Дата и время начала проведения голосования",
                "Дата и время окончания проведения голосования",
                "Порядок приема оформленных в письменной форме решений собственников",
                "Порядок ознакомления с информацией и материалами, которые будут представлены на данном собрании",
                "Ежегодное собрание",
                "Правомочность собрания",
                "Статус",
                "Основание изменения протокола"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(ContragentExportableEntity),
                typeof(HouseExportableEntity));
        }
    }
}