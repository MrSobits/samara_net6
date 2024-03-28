namespace Bars.Gkh.Gis.DomainService.RealEstate.Impl
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using B4.DataAccess;
    using B4.Utils;
    using Entities.Register.HouseRegister;
    using Enum;
    using System;
    using System.Linq;
    using Entities.RealEstate.GisRealEstateType;
    using Enums;

    public class RealEstateTypeCommonParamService : IRealEstateTypeCommonParamService
    {
        
        protected IRepository<GisRealEstateTypeCommonParam> RealEstateTypeCommonParamRepository;
        protected IRepository<HouseRegister> HouseRegisterRepository;

        public RealEstateTypeCommonParamService(
            IRepository<GisRealEstateTypeCommonParam> realEstateTypeCommonParamRepository,
            IRepository<HouseRegister> houseRegisterRepository
            )
        {
            RealEstateTypeCommonParamRepository = realEstateTypeCommonParamRepository;
            HouseRegisterRepository = houseRegisterRepository;
        }

        /// <summary>
        /// Получить список домов по типу 
        /// </summary>
        /// <param name="realEstateType">тип</param>
        /// <returns>список домов</returns>
        public IQueryable<HouseRegister> GetHouseRegistersByRealEstateType(GisRealEstateType realEstateType)
        {
            //список параметров по типу
            var commonParamsCodes = RealEstateTypeCommonParamRepository
                .GetAll()
                .Where(x => x.RealEstateType == realEstateType)
                .ToList();

            //группировка параметров для одинаковых значения соед через - или, для разных  - и
            var commonParamsDict = commonParamsCodes
                .GroupBy(x => x.CommonParamCode)
                .ToDictionary(x => x.Key, x => x);

            var listOrLamda = new List<Expression<Func<HouseRegister, bool>>>();
            foreach (var kvp in commonParamsDict)
            {   
                //список лямд для условия ИЛИ
                var expForOrList = kvp.Value.Select(GetHouseParameterValueByCommonParam).ToList();
                
                var baseExp = expForOrList.First();
                foreach (var exp in expForOrList)
                {
                    var parameter = Expression.Parameter(typeof (HouseRegister));
                    var invokedExpr = Expression.Invoke(baseExp, parameter);
                    var invokedExp = Expression.Invoke(exp, parameter);
                    var mergedExpression = Expression.OrElse(invokedExp, invokedExpr);
                    baseExp = Expression.Lambda<Func<HouseRegister, bool>>(mergedExpression, parameter);
                }

                listOrLamda.Add(baseExp);
            }

            var baseAndExp = listOrLamda.FirstOrDefault();
            foreach (var exp in listOrLamda)
            {
                var parameter = Expression.Parameter(typeof(HouseRegister));
                if (baseAndExp == null) continue;
                var invokedExpr = Expression.Invoke(baseAndExp, parameter);
                var invokedExp = Expression.Invoke(exp, parameter);
                var mergedExpression = Expression.AndAlso(invokedExp, invokedExpr);
                baseAndExp = Expression.Lambda<Func<HouseRegister, bool>>(mergedExpression, parameter);
            }            

            //выборка домов - далее добавление доп условий по параметрам            
            return HouseRegisterRepository.GetAll()
                .Where(x => x.FiasAddress != null)
                .WhereIf(baseAndExp != null, baseAndExp);
        }

        //todo refactor!!!
        /// <summary>
        /// Получить дополнительное ограничение для выборки домов
        /// </summary>
        /// <param name="commonParam">параметр</param>
        /// <returns>выражение доп. оганичения</returns>
        private Expression<Func<HouseRegister, bool>> GetHouseParameterValueByCommonParam(
            GisRealEstateTypeCommonParam commonParam)
        {
            //тип параметра
            var typeCommonParam =
                Enum.GetValues(typeof (TypeCommonParamsCodes))
                    .OfType<TypeCommonParamsCodes>()
                    .FirstOrDefault(x => x.GetEnumMeta().Display == commonParam.CommonParamCode);

            
            switch (typeCommonParam)
            {                
                case TypeCommonParamsCodes.AreaLivingNotLivingMkdGis:
                    {
                        decimal min, max, precisionValue;
                        decimal.TryParse(commonParam.PrecisionValue, out precisionValue);
                        decimal.TryParse(commonParam.Min, out min);
                        decimal.TryParse(commonParam.Max, out max);
                        Expression<Func<HouseRegister, bool>> exp;
                        if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                        {
                            exp =
                                houseReg =>
                                    houseReg.AreaLivingNotLivingMkd != null
                                    &&
                                    houseReg.AreaLivingNotLivingMkd >= min
                                    &&
                                    houseReg.AreaLivingNotLivingMkd <= max;
                        }
                        else
                        {
                            exp = houseReg =>
                                houseReg.AreaLivingNotLivingMkd != null
                                &&
                                houseReg.AreaLivingNotLivingMkd == precisionValue;
                        }
                        return exp;
                    }                
                case TypeCommonParamsCodes.AreaMkdGis:
                    {
                        decimal min, max, precisionValue;
                        decimal.TryParse(commonParam.PrecisionValue, out precisionValue);
                        decimal.TryParse(commonParam.Min, out min);
                        decimal.TryParse(commonParam.Max, out max);
                        Expression<Func<HouseRegister, bool>> exp;
                        if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                        {
                            exp = houseReg => houseReg.TotalSquare >= min && houseReg.TotalSquare <= max;
                        }
                        else
                        {
                            exp = houseReg => Math.Abs(houseReg.TotalSquare - precisionValue) < 0.0000000001M;
                        }
                        return exp;
                    }                
                case TypeCommonParamsCodes.AreaOwnedGis:
                    {
                        decimal min, max, precisionValue;
                        decimal.TryParse(commonParam.PrecisionValue, out precisionValue);
                        decimal.TryParse(commonParam.Min, out min);
                        decimal.TryParse(commonParam.Max, out max);
                        Expression<Func<HouseRegister, bool>> exp;
                        if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                        {
                            exp =
                                houseReg =>
                                    houseReg.AreaOwned != null
                                    &&
                                    houseReg.AreaOwned >= min
                                    &&
                                    houseReg.AreaOwned <= max;
                        }
                        else
                        {
                            exp =
                                houseReg =>
                                    houseReg.AreaOwned != null
                                    &&
                                    houseReg.AreaOwned == precisionValue;
                        }
                        return exp;
                    }

                case TypeCommonParamsCodes.BuildYearGis:
                {
                    int min, max, precisionValue;
                    int.TryParse(commonParam.PrecisionValue, out precisionValue);
                    int.TryParse(commonParam.Min, out min);
                    int.TryParse(commonParam.Max, out max);
                    Expression<Func<HouseRegister, bool>> exp;
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        exp = houseReg => houseReg.BuildDate.Year >= min && houseReg.BuildDate.Year <= max;
                    }
                    else
                    {
                        exp = houseReg => houseReg.BuildDate.Year == precisionValue;
                    }
                    return exp;
                }
                case TypeCommonParamsCodes.HeatingSystem:
                {
                    Expression<Func<HouseRegister, bool>> exp = 
                        houseReg => houseReg.HeatingSystem == (HeatingSystem)commonParam.PrecisionValue.ToInt();

                    return exp; 
                }
                case TypeCommonParamsCodes.MaximumFloorsGis:
                {
                    int min, max, precisionValue;
                    int.TryParse(commonParam.PrecisionValue, out precisionValue);
                    int.TryParse(commonParam.Min, out min);
                    int.TryParse(commonParam.Max, out max);
                    Expression<Func<HouseRegister, bool>> exp;
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        exp = houseReg => houseReg.MaximumFloors >= min && houseReg.MaximumFloors <= max;
                    }
                    else
                    {
                        exp = houseReg => houseReg.MaximumFloors == precisionValue;
                    }
                    return exp;                    
                }
                case TypeCommonParamsCodes.FloorsGis:
                {
                    int min, max, precisionValue;
                    int.TryParse(commonParam.PrecisionValue, out precisionValue);
                    int.TryParse(commonParam.Min, out min);
                    int.TryParse(commonParam.Max, out max);
                    Expression<Func<HouseRegister, bool>> exp;
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        exp = houseReg => houseReg.MinimumFloors >= min && houseReg.MinimumFloors <= max;
                    }
                    else
                    {
                        exp = houseReg => houseReg.MinimumFloors == precisionValue;
                    }
                    return exp;                    
                }                
                case TypeCommonParamsCodes.NumberApartments:
                {
                    int min, max, precisionValue;
                    int.TryParse(commonParam.PrecisionValue, out precisionValue);
                    int.TryParse(commonParam.Min, out min);
                    int.TryParse(commonParam.Max, out max);
                    Expression<Func<HouseRegister, bool>> exp;
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        exp =
                            houseReg =>
                                houseReg.NumberApartments != null
                                &&
                                houseReg.NumberApartments >= min
                                &&
                                houseReg.NumberApartments <= max;
                    }
                    else
                    {
                        exp = houseReg => houseReg.NumberApartments == precisionValue;
                    }
                    return exp;                     
                }
                case TypeCommonParamsCodes.NumberEntrances:
                {
                    int min, max, precisionValue;
                    int.TryParse(commonParam.PrecisionValue, out precisionValue);
                    int.TryParse(commonParam.Min, out min);
                    int.TryParse(commonParam.Max, out max);
                    Expression<Func<HouseRegister, bool>> exp;
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        exp = houseReg => houseReg.NumberEntrances >= min && houseReg.NumberEntrances <= max;
                    }
                    else
                    {
                        exp = houseReg => houseReg.NumberEntrances == precisionValue;
                    }
                    return exp;                     
                }
                case TypeCommonParamsCodes.NumberLiftsGis:
                {
                    int min, max, precisionValue;
                    int.TryParse(commonParam.PrecisionValue, out precisionValue);
                    int.TryParse(commonParam.Min, out min);
                    int.TryParse(commonParam.Max, out max);
                    Expression<Func<HouseRegister, bool>> exp;
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        exp = houseReg => houseReg.NumberLifts >= min && houseReg.NumberLifts <= max;
                    }
                    else
                    {
                        exp = houseReg => houseReg.NumberLifts == precisionValue;
                    }
                    return exp;                      
                }
                case TypeCommonParamsCodes.NumberLiving:
                {
                    int min, max, precisionValue;
                    int.TryParse(commonParam.PrecisionValue, out precisionValue);
                    int.TryParse(commonParam.Min, out min);
                    int.TryParse(commonParam.Max, out max);
                    Expression<Func<HouseRegister, bool>> exp;
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        exp = houseReg => houseReg.NumberLiving >= min && houseReg.NumberLiving <= max;
                    }
                    else
                    {
                        exp = houseReg => houseReg.NumberLiving == precisionValue;
                    }
                    return exp;                      
                }
                case TypeCommonParamsCodes.PhysicalWear:
                {
                    decimal min, max, precisionValue;
                    decimal.TryParse(commonParam.PrecisionValue, out precisionValue);
                    decimal.TryParse(commonParam.Min, out min);
                    decimal.TryParse(commonParam.Max, out max);
                    Expression<Func<HouseRegister, bool>> exp;
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        exp = houseReg => houseReg.PhysicalWear >= min && houseReg.PhysicalWear <= max;
                    }
                    else
                    {
                        exp = houseReg => Math.Abs(houseReg.PhysicalWear - precisionValue) < 0.0000000001M;
                    }
                    return exp;                      
                }
                case TypeCommonParamsCodes.PrivatizationDateFirstApartmentGis:
                {
                    DateTime min, max, precisionValue;

                    DateTime.TryParseExact(commonParam.PrecisionValue, "d", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out precisionValue);

                    DateTime.TryParseExact(commonParam.Min, "d", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out min);

                    DateTime.TryParseExact(commonParam.Max, "d", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out max);

                    Expression<Func<HouseRegister, bool>> exp;
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        exp = houseReg => houseReg.PrivatizationDate >= min && houseReg.PrivatizationDate <= max;
                    }
                    else
                    {
                        exp = houseReg => houseReg.PrivatizationDate == precisionValue;
                    }
                    return exp;
                }
                case TypeCommonParamsCodes.RoofingMaterial:
                {
                    Expression<Func<HouseRegister, bool>> exp = 
                        houseReg => houseReg.RoofingMaterial.Id == commonParam.PrecisionValue.ToInt();

                    return exp;                  
                }                    
                case TypeCommonParamsCodes.TypeHouse:
                {
                    Expression<Func<HouseRegister, bool>> exp =
                        houseReg => houseReg.TypeHouse == (TypeHouse)commonParam.PrecisionValue.ToInt();

                    return exp;                     
                }
                case TypeCommonParamsCodes.TypeProject:
                {
                    Expression<Func<HouseRegister, bool>> exp =
                        houseReg => houseReg.TypeProject.Id == commonParam.PrecisionValue.ToInt();

                    return exp;                      
                }
                case TypeCommonParamsCodes.TypeRoof:
                {
                    Expression<Func<HouseRegister, bool>> exp =
                        houseReg => houseReg.TypeRoof == (TypeRoof)commonParam.PrecisionValue.ToInt();

                    return exp; 
                }
                case TypeCommonParamsCodes.WallMaterial:
                {
                    Expression<Func<HouseRegister, bool>> exp =
                        houseReg => houseReg.WallMaterial.Id == commonParam.PrecisionValue.ToInt();

                    return exp;
                }
                default:
                {
                    return x => true;
                }
            }
        }

        //todo эксперимент
        private Expression<Func<HouseRegister, bool>> GetMinMaxCompareExpressionInt(Func<HouseRegister, int> prop,
            GisRealEstateTypeCommonParam commonParam)
        {
            try
            {
                Expression<Func<HouseRegister, bool>> exp = x => true;

                if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                {
                    var min = Convert.ToInt32(commonParam.Min);
                    var max = Convert.ToInt32(commonParam.Max);
                    exp = houseReg => prop(houseReg) >= min && prop(houseReg) <= max;
                }
                else
                {
                    var precisionValue = Convert.ToInt32(commonParam.PrecisionValue);
                    exp = houseReg => prop(houseReg) == precisionValue;
                }

                return exp;
            }
            catch (Exception ex)
            {
                return x => true;
            }
        }

        //todo как то сравнить generic T с помошью операторов сравнения ==
        private Expression<Func<HouseRegister, bool>> GetMinMaxCompareExpression<T>(Func<HouseRegister,T> prop,
            GisRealEstateTypeCommonParam commonParam) where T : struct 
        {
            try
            {
                Expression<Func<HouseRegister, bool>> exp = x => true;

                if (typeof (T) == typeof (int))
                {
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        var min = Convert.ToInt32(commonParam.Min);
                        var max = Convert.ToInt32(commonParam.Max);
                        exp = houseReg => Convert.ToInt32(prop(houseReg)) >= min && Convert.ToInt32(prop(houseReg)) <= max;
                    }
                    else
                    {
                        var precisionValue = Convert.ToInt32(commonParam.PrecisionValue);
                        exp = houseReg => Convert.ToInt32(prop(houseReg)) == precisionValue;
                    }
                }

                if (typeof(T) == typeof(long))
                {
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        var min = Convert.ToInt64(commonParam.Min);
                        var max = Convert.ToInt64(commonParam.Max);
                        exp = houseReg => Convert.ToInt64(prop(houseReg)) >= min && Convert.ToInt64(prop(houseReg)) <= max;
                    }
                    else
                    {
                        var precisionValue = Convert.ToInt64(commonParam.PrecisionValue);
                        exp = houseReg => Convert.ToInt64(prop(houseReg)) == precisionValue;
                    }
                }

                if (typeof(T) == typeof(decimal))
                {
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        var min = Convert.ToDecimal(commonParam.Min);
                        var max = Convert.ToDecimal(commonParam.Max);
                        exp = houseReg => Convert.ToDecimal(prop(houseReg)) >= min && Convert.ToDecimal(prop(houseReg)) <= max;
                    }
                    else
                    {
                        var precisionValue = Convert.ToDecimal(commonParam.PrecisionValue);
                        exp = houseReg => Convert.ToDecimal(prop(houseReg)) == precisionValue;
                    }
                }

                if (typeof(T) == typeof(double))
                {
                    if (string.IsNullOrEmpty(commonParam.PrecisionValue))
                    {
                        var min = Convert.ToDouble(commonParam.Min);
                        var max = Convert.ToDouble(commonParam.Max);
                        exp = houseReg => Convert.ToDouble(prop(houseReg)) >= min && Convert.ToDouble(prop(houseReg))<= max;
                    }
                    else
                    {
                        var precisionValue = Convert.ToDouble(commonParam.PrecisionValue);
                        exp = houseReg => Math.Abs(Convert.ToDouble(prop(houseReg)) - precisionValue) < 0.000000001;
                    }
                }                               
                return exp;
            }
            catch (Exception ex)
            {
                return x => true;
            }
        }
    }    
}
