namespace Bars.Gkh.Overhaul.Tat.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class PropertyOwnerProtocolsLogMap : AuditLogMap<PropertyOwnerProtocols>
    {
        public PropertyOwnerProtocolsLogMap()
        {
            Name("Протоколы и решения собственников");

            Description(x => x.RealityObject.Address);

            MapProperty(v => v.RealityObject.Address, "Address", "Адрес");
            MapProperty(v => v.DocumentNumber, "DocumentNumber", "Номер документа");
            MapProperty(v => v.DocumentDate, "DocumentDate", "Дата", v => v.Return(d => d.Value.ToString("dd.MM.yyyy")));
            MapProperty(v => v.Description, "Description", "Описание");
            MapProperty(v => v.DocumentFile.FullName, "DocumentFileFullName", "Полное имя файла");
            MapProperty(v => v.NumberOfVotes, "NumberOfVotes", "Количество голосов (кв.м.)", v => v.Return(n => n.Value.ToInvariantString("F")));
            MapProperty(v => v.TotalNumberOfVotes, "TotalNumberOfVotes", "Общее количество голосов (кв.м.)", v => v.Return(n => n.Value.ToInvariantString("F")));
            MapProperty(v => v.PercentOfParticipating, "PercentOfParticipating", "Доля принявших участие (%)", v => v.Return(n => n.Value.ToInvariantString("F")));
            MapProperty(v => v.TypeProtocol, "TypeProtocol", "Тип протокола", v => v.GetEnumMeta().Display);
        }
    }
}
