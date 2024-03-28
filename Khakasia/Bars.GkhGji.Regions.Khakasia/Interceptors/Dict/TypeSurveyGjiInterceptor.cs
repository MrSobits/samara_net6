namespace Bars.GkhGji.Regions.Khakasia.Interceptors.Dict
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

    public class TypeSurveyGjiInterceptor : GkhGji.Interceptors.TypeSurveyGjiInterceptor
    {
        protected override List<Func<long, string>> GetDeleteValidators()
        {
            var validators = base.GetDeleteValidators();
            validators.Add(id => this.Container.ResolveDomain<TypeSurveyContragentType>().GetAll().Any(x => x.TypeSurveyGji.Id == id) ? "Типы контрагентов" : null);
            return validators;
        }
    }
}