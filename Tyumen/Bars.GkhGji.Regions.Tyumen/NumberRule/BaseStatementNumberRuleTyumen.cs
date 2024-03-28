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

namespace Bars.GkhGji.Regions.Tyumen.NumberRule
{
    public class BaseStatementNumberRuleTyumen : IBaseStatementNumberRule
    {
        public IWindsorContainer Container { get; set; }

        public void SetNumber(IEntity inspection)
        {
            var insp = inspection as BaseStatement;

            if (insp == null)
                return;

            insp.InspectionYear = DateTime.Now.Year;

            insp.InspectionNum = Container.Resolve<IDomainService<InspectionGji>>().GetAll()
                .Where(x => x.InspectionYear == insp.InspectionYear && x.Id != insp.Id && x.TypeBase == TypeBase.CitizenStatement)
                .Select(x => x.InspectionNum).Max().ToInt() + 1;

            // теперь получаем номер формата 12/2013 (12 - номер п/п, 2013 - год в 4х значном виде)
            insp.InspectionNumber = string.Format("{0}/{1}",
                insp.InspectionNum,
                insp.InspectionYear.Value.ToString(CultureInfo.InvariantCulture));
        }
    }
}