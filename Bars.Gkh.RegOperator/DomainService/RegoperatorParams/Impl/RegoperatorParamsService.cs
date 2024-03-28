namespace Bars.Gkh.RegOperator.DomainService.RegoperatorParams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Analytics.Utils;
    using B4.Modules.States;
    using B4.Utils;
    using Bars.B4.IoC;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using FastMember;
    using Gkh.Utils;

    public class RegoperatorParamsService : IRegoperatorParamsService
    {
        private Dictionary<string, string> _paramCache; 

        public RegoperatorParamsService()
        {
            _paramCache = new Dictionary<string, string>();
        }

        public IWindsorContainer Container { get; set; }

        public IDomainService<RegoperatorParam> RegoperatorParamDomain { get; set; }

        public IDataResult SaveParams(BaseParams baseParams)
        {
            var regopparams = baseParams.Params.GetAs<DynamicDictionary>("regopparams");

            if (regopparams == null)
            {
                return new BaseDataResult(false, "Ошибка получения параметров");
            }

            var paramsDict = RegoperatorParamDomain.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.First());

            foreach (var regopparam in regopparams)
            {
                var paramValue = regopparam.Value.ToStr();

                if (paramsDict.ContainsKey(regopparam.Key))
                {
                    var paramToUpdate = paramsDict[regopparam.Key];

                    if (paramToUpdate.Value != paramValue)
                    {
                        paramToUpdate.Value = paramValue;

                        RegoperatorParamDomain.Update(paramToUpdate);

                        paramsDict[regopparam.Key] = paramToUpdate;
                    }
                }
                else
                {
                    var newParam = new RegoperatorParam { Key = regopparam.Key, Value = paramValue };

                    RegoperatorParamDomain.Save(newParam);

                    paramsDict[regopparam.Key] = newParam;
                }
            }

            return new BaseDataResult();
        }

        public IDataResult GetParams()
        {
            var stateRepo = Container.ResolveRepository<State>();

            var result = RegoperatorParamDomain.GetAll()
                .Select(x => new { x.Key, x.Value })
                .AsEnumerable()
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key, x =>
                {
                    object res;
                    int intValue;
                    string value = x.First().Value;

                    if (int.TryParse(value, out intValue)) { res = intValue; }
                    else { res = value; }

                    if (x.Key == "RepaymentTime" && value.IsNotEmpty())
                    {
                        return DateTime.Parse(value).ToShortTimeString();
                    }

                    //данный костыль служит для отображения списка выбранных статусов
                    if (x.Key == "PersonalAccountStates")
                    {
                        var ids = x.First().Value.ToLongArray();
                        res = stateRepo.GetAll()
                            .Where(s => ids.Contains(s.Id))
                            .Select(s => new
                            {
                                s.Id,
                                s.Name
                            })
                            .ToList()
                            .GroupBy(s => 1)
                            .Select(s => new
                            {
                                Id = s.AggregateWithSeparator(c => c.Id, ", "),
                                Name = s.AggregateWithSeparator(c => c.Name, ", ")
                            })
                            .FirstOrDefault();
                    }

                    return res;
                });

            return new BaseDataResult(result);
        }

        public string GetParamByKey(string key)
        {
            string cachedValue;
            if (!_paramCache.TryGetValue(key ?? "", out cachedValue))
            {
                cachedValue = RegoperatorParamDomain.GetAll().Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault();
                _paramCache[key ?? ""] = cachedValue;
            }

            return cachedValue;
        }

        public T ReadParam<T>() where T : class
        {
            var accessor = TypeAccessor.Create(typeof (T));
            var props = accessor.GetMembers().ToDictionary(x => x.Name, x => x.Type);
            var propKeys = props.Keys;


            var parameters = RegoperatorParamDomain.GetAll().Where(x => propKeys.Contains(x.Key)).ToList();

            var result = accessor.CreateNew();
            foreach (var param in parameters)
            {
                accessor[result, param.Key] = ConvertHelper.ConvertTo(param.Value, props[param.Key]);
            }

            return result as T;
        }

        public IDataResult ValidateSave(BaseParams baseParams)
        {
            return ValidateSaveTypeAccountNumber(baseParams);
        }

        private IDataResult ValidateSaveTypeAccountNumber(BaseParams baseParams)
        {
            var regopparams = baseParams.Params.GetAs<DynamicDictionary>("regopparams");

            var newType = regopparams.GetAs<TypeAccountNumber>("TypeAccountNumber");
            var paramFromClient = regopparams.Keys.Any(x => x == "TypeAccountNumber");
            var persistentType = GetParamByKey("TypeAccountNumber").ToEnum<TypeAccountNumber>();

            //первое условие - костыль чтоб при первом сохранении не ругался
            if (persistentType == 0 || persistentType == newType || !paramFromClient)
            {
                return new BaseDataResult();
            }

            var accountDomain = Container.ResolveDomain<BasePersonalAccount>();

            using (Container.Using(accountDomain))
            {
                if (accountDomain.GetAll().Any())
                {
                    return new BaseDataResult(false,
                        "Невозможно сменить способ генерации лицевых счетов, так как существуют сформированные лицевые счета");
                }
            }

            return new BaseDataResult(new {ChangedTypeAccount = true});
        }
    }
}