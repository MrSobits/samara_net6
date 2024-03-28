namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.PriorityParams;
    using Entities;
    using Enums;

    public class QualityPriorityParamViewModel : BaseViewModel<QualityPriorityParam>
    {
        public override IDataResult List(IDomainService<QualityPriorityParam> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var code = baseParams.Params.GetAs<string>("code");

            var existRecs =
                domainService.GetAll()
                    .Where(x => x.Code == code)
                    .ToDictionary(x => x.Value);

            var enumType =
                Container.ResolveAll<IQualitPriorityParam>().Where(x => x.Id == code).Select(x => x.EnumType).First();

            var result = new List<object>();

            foreach (TypePresence type in Enum.GetValues(enumType))
            {
                var value = (int)type;

                if (existRecs.ContainsKey(value))
                {
                    var qualitParam = existRecs[value];
                    result.Add(new
                    {
                        qualitParam.Id,
                        EnumDisplay = GetEnumDisplay(enumType, type),
                        qualitParam.Code,
                        qualitParam.Value,
                        qualitParam.Point
                    });
                }
                else
                {
                    result.Add(new
                    {
                        EnumDisplay = GetEnumDisplay(enumType, type),
                        Code = code,
                        Value = type,
                        Point = 0
                    });
                }
            }

            return new ListDataResult(result.AsQueryable().Order(loadParam).Paging(loadParam).ToList(), result.Count);
        }


        public override IDataResult Get(IDomainService<QualityPriorityParam> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            if (obj != null)
            {
                var enumType =
                    Container.ResolveAll<IQualitPriorityParam>()
                             .Where(x => x.Id == obj.Code)
                             .Select(x => x.EnumType)
                             .First();

                return enumType == null
                       ? new BaseDataResult()
                       : new BaseDataResult(
                             new { obj.Id, obj.Point, obj.Code, EnumDisplay = GetEnumDisplay(enumType, obj.Point), });
            }

            return new BaseDataResult();
        }



        private string GetEnumDisplay(Type enumType, object value)
        {
            var name = Enum.GetName(enumType, value);
            var enumMember = enumType.GetMember(name).FirstOrDefault();

            if (enumMember != null)
            {
                var attributes = enumMember.GetCustomAttributes(true);
                return
                    attributes.OfType<CustomValueAttribute>()
                              .FirstOrDefault(x => x.Key == "Display")
                              .Return(x => x.Value)
                              .As<string>()
                              .Or(enumMember.Name);
            }

            return "";
        }
    }
}