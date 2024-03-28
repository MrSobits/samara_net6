namespace Bars.GkhGji.NumberRule
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class BaseStatementNumberRuleTat : IBaseStatementNumberRule
    {
        public IWindsorContainer Container { get; set; }

        public void SetNumber(IEntity inspection)
        {
            var insp = inspection as BaseStatement;

            if(insp == null)
                return;

            insp.InspectionYear = DateTime.Now.Year;

            insp.InspectionNum = Container.Resolve<IDomainService<InspectionGji>>().GetAll()
                .Where(x => x.InspectionYear == insp.InspectionYear && x.Id != insp.Id && x.TypeBase == TypeBase.CitizenStatement)
                .Select(x => x.InspectionNum).Max().ToInt() + 1;

            // теперь получаем номер формата 12-0001 (12 - год, 0001 - номер в 4х значном виде)
            insp.InspectionNumber = string.Format("{0}-{1}",
                insp.InspectionYear.Value.ToString(CultureInfo.InvariantCulture).Substring(2),
                insp.InspectionNum.Value.ToString("D4", CultureInfo.InvariantCulture));
        }
    }
}