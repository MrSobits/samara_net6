namespace Bars.GkhGji.LogMap.Resolution
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class ResolutionLogMap : AuditLogMap<Resolution>
    {
        public ResolutionLogMap()
        {
            this.Name("Постановление ГЖИ");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.DocumentNumSsp, "DocumentNumSsp", "Документ-основание");
            this.MapProperty(x => x.DeliveryDate, "DeliveryDate", "Дата вручения");
            this.MapProperty(x => x.GisUin, "GisUin", "Код ГИС - уникальный идентификатор начисления");
            this.MapProperty(x => x.AbandonReason, "AbandonReason", "Основание аннулирования");
            this.MapProperty(x => x.TypeInitiativeOrg, "AbandonReason", "Кем вынесено", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.SectorNumber, "SectorNumber", "Кем вынесено");
            this.MapProperty(x => x.FineMunicipality.Name, "FineMunicipality", "МО получателя штрафа");
            this.MapProperty(x => x.Official.Fio, "Official", "Должностное лицо");
            this.MapProperty(x => x.Municipality.Name, "Municipality", "Местонахождение");
            this.MapProperty(x => x.Sanction.Name, "Sanction", "Вид санкции");
            this.MapProperty(x => x.Paided, "Paided", "Штраф оплачен", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.PenaltyAmount, "PenaltyAmount", "Сумма штрафа");
            this.MapProperty(x => x.DateTransferSsp, "DateTransferSsp", "Дата передачи в ССП");
            this.MapProperty(x => x.DocumentNumSsp, "DocumentNumSsp", "Номер документа, передача в ССП");
            this.MapProperty(x => x.TypeTerminationBasement, "TypeTerminationBasement", "Основание прекращения", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.RulingNumber, "RulingNumber", "Номер документа");
            this.MapProperty(x => x.Executant.Name, "Executant", "Тип исполнителя");
            this.MapProperty(x => x.DateWriteOut, "DateWriteOut", "Выписка из ЕГРЮЛ");
            this.MapProperty(x => x.Contragent.Name, "Contragent", "Контрагент");
            this.MapProperty(x => x.PhysicalPerson, "PhysicalPerson", "Физическое лицо");
            this.MapProperty(x => x.PhysicalPersonInfo, "PhysicalPersonInfo", "Реквизиты физ. лица");
        }
    }
}