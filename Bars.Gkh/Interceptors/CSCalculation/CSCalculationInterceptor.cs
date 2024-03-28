namespace Bars.Gkh.Interceptors.Dict
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Entities.Suggestion;

    public class CSCalculationInterceptor : EmptyDomainInterceptor<CSCalculation>
    {
        /// <summary>
        /// Домен-сервис <see cref="CSCalculationRow"/>
        /// </summary>
        public IDomainService<CSCalculationRow> CSCalculationRowDomain { get; set; }

        public IDomainService<TarifNormative> TarifNormativeDomain { get; set; }
        public IDomainService<MOCoefficient> MOCoefficientDomain { get; set; }
        public IDomainService<Municipality> MunicipalityDomain { get; set; }
        public IDomainService<RealityObjectCategoryMKD> RealityObjectCategoryMKDDomain { get; set; }

        public IDomainService<RealityObjectBuildingFeature> RealityObjectBuildingFeatureDomain { get; set; }
        public override IDataResult AfterCreateAction(IDomainService<CSCalculation> service, CSCalculation entity)
        {
            string descr = entity.CSFormula.Formula;
            foreach (var formulaParam in entity.CSFormula.FormulaParameters)
            {
                DateTime calcDate = entity.CalcDate.HasValue ? entity.CalcDate.Value : DateTime.Now;
                descr = descr.Replace(formulaParam.Code, $"[{formulaParam.DisplayName}]");
                decimal value = 0;
                value = GetParamValue(formulaParam.Code, entity);
                //ебанутым нет покоя, решено что МО пустым быть не может, у нормативов которые действуют на всю обрасть указывать МО воронежская область
                var voronezhObl = MunicipalityDomain.GetAll().FirstOrDefault(x => x.Name == "Воронежская область");

                //Получаем коэффициенты(если есть)
                if (value == 0)
                {
                    var isCoeff = MOCoefficientDomain.GetAll().FirstOrDefault(x => x.Code == formulaParam.Code);
                    if (isCoeff != null)
                    {
                        value = GetCoefficientValue(formulaParam.Code, entity, voronezhObl);
                    }
                }

                //получаем категории дома
                var categoryList = RealityObjectCategoryMKDDomain.GetAll()
                    .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                    .Select(x => x.CategoryCSMKD.Id).ToList();

                //Ищем тарифы нормативы с категориями
                if (categoryList.Count > 0 && value == 0)
                {
                    //ищем параметры c МО
                    TarifNormative tarifNormativeByCategory = null;
                     tarifNormativeByCategory = TarifNormativeDomain.GetAll()
                        .Where(x => x.CategoryCSMKD != null && categoryList.Contains(x.CategoryCSMKD.Id))
                        .Where(x => x.Municipality == entity.RealityObject.MoSettlement)
                        .Where(x => x.Code == formulaParam.Code)
                     .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                        .OrderByDescending(x => x.Id).FirstOrDefault();
                    if (tarifNormativeByCategory == null)
                    tarifNormativeByCategory = TarifNormativeDomain.GetAll()
                      .Where(x => x.CategoryCSMKD != null && categoryList.Contains(x.CategoryCSMKD.Id))
                      .Where(x => x.Municipality == entity.RealityObject.Municipality)
                      .Where(x => x.Code == formulaParam.Code)
                   .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                      .OrderByDescending(x => x.Id).FirstOrDefault();

                    if (tarifNormativeByCategory != null)
                    {
                        value = tarifNormativeByCategory.Value;
                    }
                    else// пробуем с МО и без категорий
                    {
                        tarifNormativeByCategory = TarifNormativeDomain.GetAll()
                        .Where(x => x.CategoryCSMKD == null)
                        .Where(x => x.Municipality == entity.RealityObject.MoSettlement)
                        .Where(x => x.Code == formulaParam.Code)
                     .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                        .OrderByDescending(x => x.Id).FirstOrDefault();

                        if (tarifNormativeByCategory == null)
                            tarifNormativeByCategory = TarifNormativeDomain.GetAll()
                       .Where(x => x.CategoryCSMKD == null)
                       .Where(x => x.Municipality == entity.RealityObject.Municipality)
                       .Where(x => x.Code == formulaParam.Code)
                    .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                       .OrderByDescending(x => x.Id).FirstOrDefault();

                        if (tarifNormativeByCategory != null)
                        {
                            value = tarifNormativeByCategory.Value;
                        }
                    }
                    if (value == 0) // все еще 0, ищем без МО но с категориями
                    {
                        tarifNormativeByCategory = TarifNormativeDomain.GetAll()
                        .Where(x => x.CategoryCSMKD != null && categoryList.Contains(x.CategoryCSMKD.Id))
                        .Where(x => x.Municipality == voronezhObl)
                        .Where(x => x.Code == formulaParam.Code)
                     .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                        .OrderByDescending(x => x.Id).FirstOrDefault();
                        if (tarifNormativeByCategory != null)
                        {
                            value = tarifNormativeByCategory.Value;
                        }
                        else//тогда без МО и категорий
                        {
                            tarifNormativeByCategory = TarifNormativeDomain.GetAll()
                          .Where(x => x.Municipality == voronezhObl)
                          .Where(x => x.Code == formulaParam.Code)
                          .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                          .OrderByDescending(x => x.Id).FirstOrDefault();
                            if (tarifNormativeByCategory != null)
                            {
                                value = tarifNormativeByCategory.Value;
                            }
                        }    
                    }
                }
                else if (value == 0)
                {
                    //ищем параметры c МО
                    TarifNormative tarifNormative = null;
                    tarifNormative = TarifNormativeDomain.GetAll()
                       .Where(x => x.Municipality == entity.RealityObject.MoSettlement)
                       .Where(x => x.Code == formulaParam.Code)
                    .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                       .OrderByDescending(x => x.Id).FirstOrDefault();

                    if (tarifNormative == null)
                    {
                        tarifNormative = TarifNormativeDomain.GetAll()
                          .Where(x => x.Municipality == entity.RealityObject.Municipality)
                          .Where(x => x.Code == formulaParam.Code)
                       .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                          .OrderByDescending(x => x.Id).FirstOrDefault();
                    }       

                    if (tarifNormative != null)
                    {
                        value = tarifNormative.Value;
                    }
                    else //без МО
                    {
                        var tarifNormativeMO = TarifNormativeDomain.GetAll()
                        .Where(x => x.Municipality == voronezhObl)
                       .Where(x => x.Code == formulaParam.Code)
                       .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                       .OrderByDescending(x => x.Id).FirstOrDefault();
                        if (tarifNormativeMO != null)
                        {
                            value = tarifNormativeMO.Value;
                        }
                    }

                }              
                
                CSCalculationRow newrow = new CSCalculationRow
                {
                    CSCalculation = new CSCalculation { Id = entity.Id },
                    Code = formulaParam.Code,
                    Name = formulaParam.DisplayName,
                    DisplayValue = formulaParam.DisplayName,
                    Value = value
                };
                CSCalculationRowDomain.Save(newrow);

            }
            entity.Description = descr;

                return Success();
        }

        private decimal GetCoefficientValue(string paramName, CSCalculation calc, Municipality obl)
        {
            DateTime calcDate = calc.CalcDate.HasValue ? calc.CalcDate.Value : DateTime.Now;
            var realitySettelment = calc.RealityObject.MoSettlement;
            if (realitySettelment != null)
            {
                var coeff = MOCoefficientDomain.GetAll()
                    .Where(x => x.Municipality == realitySettelment)
                    .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                    .Where(x => x.Code == paramName).OrderByDescending(x => x.Id).FirstOrDefault();
                if (coeff != null)
                {
                    return coeff.Value;
                }
            }
            else
            {
                var coeff = MOCoefficientDomain.GetAll()
                     .Where(x => x.Municipality == obl)
                     .Where(x => x.DateFrom <= calcDate && (!x.DateTo.HasValue || x.DateTo.Value > calcDate))
                     .Where(x => x.Code == paramName).OrderByDescending(x => x.Id).FirstOrDefault();
                if (coeff != null)
                {
                    return coeff.Value;
                }
            }    
            return 0;
        }

            private decimal GetParamValue(string paramName, CSCalculation calc)
        {
            decimal value = 0;
            switch (paramName)
            {
                case "Spom":
                    {

                        if (calc.Room != null)
                        {
                            value = calc.Room.Area;
                        }
                    }
                    break;

                case "Smkdtotal":
                    {

                        if (calc.RealityObject != null)
                        {
                            value = calc.RealityObject.AreaMkd.HasValue? calc.RealityObject.AreaMkd.Value:0 ;
                        }
                    }
                    break;

                case "Smkdlvnlv":
                    {

                        if (calc.RealityObject != null)
                        {
                            value = calc.RealityObject.AreaLivingNotLivingMkd.HasValue ? calc.RealityObject.AreaLivingNotLivingMkd.Value : 0;
                        }
                    }
                    break;

                case "Smkdlvn":
                    {

                        if (calc.RealityObject != null)
                        {
                            value = calc.RealityObject.AreaLiving.HasValue ? calc.RealityObject.AreaLiving.Value : 0;
                        }
                    }
                    break;

                case "Smkdntlv":
                    {

                        if (calc.RealityObject != null)
                        {
                            value = calc.RealityObject.AreaNotLivingPremises.HasValue ? calc.RealityObject.AreaNotLivingPremises.Value : 0;
                        }
                    }
                    break;

                case "Smkdlvow":
                    {

                        if (calc.RealityObject != null)
                        {
                            value = calc.RealityObject.AreaLivingOwned.HasValue ? calc.RealityObject.AreaLivingOwned.Value : 0;
                        }
                    }
                    break;

                case "Smkdcomm":
                    {

                        if (calc.RealityObject != null)
                        {
                            value = calc.RealityObject.AreaCommonUsage.HasValue ? calc.RealityObject.AreaCommonUsage.Value : 0;
                        }
                    }
                    break;

                case "MKDFloors":
                    {

                        if (calc.RealityObject != null)
                        {
                            value = calc.RealityObject.Floors.HasValue ? calc.RealityObject.Floors.Value : 1;
                        }
                    }
                    break;

                case "Spomcomprop":
                    {

                        if (calc.Room != null)
                        {
                            value = calc.Room.CommunalArea.HasValue ? calc.Room.CommunalArea.Value : 0;
                        }
                    }
                    break;

                case "MKDMaxFloors":
                    {

                        if (calc.RealityObject != null)
                        {
                            value = calc.RealityObject.MaximumFloors.HasValue ? calc.RealityObject.MaximumFloors.Value : 1;
                        }
                    }
                    break;

                case "MKDType":
                    {

                        if (calc.RealityObject != null)
                        {
                            value = calc.RealityObject.TypeHouse == Enums.TypeHouse.Individual? 20:10;
                        }
                    }
                    break;

            }
            return value;
        }

        public override IDataResult AfterUpdateAction(IDomainService<CSCalculation> service, CSCalculation entity)
        {
            var row = CSCalculationRowDomain.GetAll().FirstOrDefault(x => x.CSCalculation.Id == entity.Id);
            if (row == null)
            {
                foreach (var formulaParam in entity.CSFormula.FormulaParameters)
                {
                    CSCalculationRow newrow = new CSCalculationRow
                    {
                        CSCalculation = new CSCalculation { Id = entity.Id },
                        Code = formulaParam.Code,
                        Name = formulaParam.DisplayName,
                        DisplayValue = formulaParam.DisplayName,
                        Value = 0
                    };
                    CSCalculationRowDomain.Save(newrow);

                }
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<CSCalculation> service, CSCalculation entity)
        {
            var serviceCSCalculationRow = Container.Resolve<IDomainService<CSCalculationRow>>();
            try
            {
                var reportRow = serviceCSCalculationRow.GetAll()
               .Where(x => x.CSCalculation.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in reportRow)
                {
                    serviceCSCalculationRow.Delete(id);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }

        }
    }
}
