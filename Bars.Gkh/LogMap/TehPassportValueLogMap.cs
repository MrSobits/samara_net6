namespace Bars.Gkh.LogMap
{
    using System;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.PassportProvider;
    using Bars.Gkh.Entities;

    public class TehPassportValueLogMap : AuditLogMap<TehPassportValue>
    {
        public TehPassportValueLogMap()
        {
            Name("Значение тех. паспорта");

            Description(x =>
            {
                var passport = ApplicationContext.Current.Container.ResolveAll<IPassportProvider>().FirstOrDefault(y => y.Name == "Техпаспорт" && y.TypeDataSource == "xml");
                if (passport == null)
                {
                    throw new Exception("Не найден провайдер технического паспорта");
                }

                var name = passport.GetLabelForFormElement(x.FormCode, x.CellCode);

                return string.Format("{0}, {1}", x.TehPassport.RealityObject.Address, name);
            });

            MapProperty(x => x.Value, "RoomNum", "Значение");
        }
    }
}
