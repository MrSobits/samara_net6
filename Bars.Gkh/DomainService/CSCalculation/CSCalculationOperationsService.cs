using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Utils;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using Bars.Gkh.Enums;
using NCalc;
using Bars.B4.Utils;
using Bars.B4.DataAccess;

namespace Bars.Gkh.DomainService
{
    public class CSCalculationOperationsService : ICSCalculationOperationsService
    { 

        #region Properties      
        public IDomainService<CSCalculation> CSCalculationDomain { get; set; }

        public IDomainService<CSCalculationRow> CSCalculationRowDomain { get; set; }

        public IDomainService<TarifNormative> TarifNormativeDomain { get; set; }

        public IDomainService<MOCoefficient> MOCoefficientDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        public IDomainService<Room> RoomDomain { get; set; }

        #endregion

        #region Public methods     

        public IDataResult GetListRoom(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var roId = loadParams.Filter.GetAs("RO", 0L);
            if (roId <= 0)
            {
                return new ListDataResult();
            }

            return RoomDomain.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    x.RoomNum,
                    x.CadastralNumber
                })
                .ToListDataResult(loadParams);
        }

        public IDataResult AddTarifs(BaseParams baseParams)
        {
            try
            {
                var municipalityIds = baseParams.Params.GetAs<List<long>>("municipalityIds");
                var tfName = baseParams.Params.GetAs<string>("tfName");
                var categoryId = baseParams.Params.GetAs<long>("categoryId");
                var dfDateFrom = baseParams.Params.GetAs<DateTime>("dfDateFrom");
                var dfDateTo = baseParams.Params.GetAs<DateTime?>("dfDateTo");
                var tfCode = baseParams.Params.GetAs<string>("tfCode");
                var tfValue = baseParams.Params.GetAs<decimal>("tfValue");
                var tfUnitMeasure = baseParams.Params.GetAs<string>("tfUnitMeasure");

                try
                {
                    foreach (var municipalityId in municipalityIds)
                    {
                        var newObj = new TarifNormative
                        {
                            CategoryCSMKD = categoryId>0? new CategoryCSMKD {Id = categoryId }:null,
                            DateFrom = dfDateFrom,
                            DateTo = dfDateTo,
                            Code = tfCode,
                            Name = tfName,
                            Municipality = new Municipality {Id = municipalityId},
                            UnitMeasure = tfUnitMeasure,
                            Value = tfValue
                        };

                        TarifNormativeDomain.Save(newObj);
                    }

                    return new BaseDataResult();
                }
                finally
                {
                }
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult AddCoefficient(BaseParams baseParams)
        {
            try
            {
                var municipalityIds = baseParams.Params.GetAs<List<long>>("municipalityIds");
                var tfName = baseParams.Params.GetAs<string>("tfName");
                var categoryId = baseParams.Params.GetAs<long>("categoryId");
                var dfDateFrom = baseParams.Params.GetAs<DateTime>("dfDateFrom");
                var dfDateTo = baseParams.Params.GetAs<DateTime?>("dfDateTo");
                var tfCode = baseParams.Params.GetAs<string>("tfCode");
                var tfValue = baseParams.Params.GetAs<decimal>("tfValue");
                var tfUnitMeasure = baseParams.Params.GetAs<string>("tfUnitMeasure");

                try
                {
                    foreach (var municipalityId in municipalityIds)
                    {
                        var newObj = new MOCoefficient
                        {
                            CategoryCSMKD = categoryId > 0 ? new CategoryCSMKD { Id = categoryId } : null,
                            DateFrom = dfDateFrom,
                            DateTo = dfDateTo,
                            Code = tfCode,
                            Name = tfName,
                            Municipality = new Municipality { Id = municipalityId },
                            UnitMeasure = tfUnitMeasure,
                            Value = tfValue
                        };

                        MOCoefficientDomain.Save(newObj);
                    }

                    return new BaseDataResult();
                }
                finally
                {
                }
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult AddCategoryes(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {                
                var serviceCategory = this.Container.Resolve<IDomainService<CategoryCSMKD>>();
                var serviceROCategory = this.Container.Resolve<IDomainService<RealityObjectCategoryMKD>>();

                List<long> typelist = new List<long>();

                try
                {
                    var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds") ?? new long[0];

                    if (objectIds.Length < 1)
                        return new BaseDataResult(false, "Необходимо выбрать категории");

                    //получаем существующие типы
                    typelist.AddRange(serviceROCategory.GetAll()
                         .Where(x => x.RealityObject.Id == realityObjectId)
                         .Select(x => x.CategoryCSMKD.TypeCategoryCS.Id).Distinct().ToList()
                        );
                    List<long> duplicateTypesCategory = new List<long>();

                    serviceCategory.GetAll()
                        .Where(x => objectIds.Contains(x.Id))
                        .ToList()
                        .ForEach(rec =>
                        {
                            if (typelist.Contains(rec.TypeCategoryCS.Id))
                            {
                                duplicateTypesCategory.Add(rec.Id);
                            }
                            else
                            {
                                typelist.Add(rec.TypeCategoryCS.Id);
                            }
                        });

                    var listObjects =
                        serviceROCategory.GetAll()
                            .Where(x => x.RealityObject.Id == realityObjectId)
                            .Select(x => x.CategoryCSMKD.Id)
                            .Where(x => objectIds.Contains(x))
                            .Distinct()
                            .ToList();                   

                    foreach (var id in objectIds)
                    {
                        if (listObjects.Contains(id))
                            continue;

                        if(duplicateTypesCategory.Contains(id))
                            continue;



                        var newRecord = new RealityObjectCategoryMKD
                        {
                            RealityObject = new RealityObject {Id = realityObjectId},
                            CategoryCSMKD = serviceCategory.Load(id)
                        };

                        serviceROCategory.Save(newRecord);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException exc)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = exc.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(serviceROCategory);
                    this.Container.Release(serviceCategory);
                }
            }
        }


        public object CalculateCS(BaseParams baseParams)
        {

            var taskId = baseParams.Params.GetAs("taskId", 0L); // это идентификатор Disposal, энтити наследуется от DocumentGji
            if (taskId > 0)
            {
                var dict = new Dictionary<string, object>();
                var csCalc = CSCalculationDomain.Get(taskId);
                var csCalcRows = CSCalculationRowDomain.GetAll()
                    .Where(x=> x.CSCalculation.Id == taskId).ToList();

                var expr = new Expression(csCalc.CSFormula.Formula) { Parameters = dict };

                foreach (CSCalculationRow r in csCalcRows)
                {
                    dict[r.Code] = r.Value;
                }

                decimal result;

                try
                {
                    result = expr.Evaluate().To<decimal>();
                }
                catch (Exception e)
                {
                    throw new Exception("Не удалось расчитать выражение. Необходимо проверить корректность формулы.");
                }

                var data = new
                {

                    result,
                  
                };

                return data;
            }
            else
            {
                return null;
            }
        }

    

        #endregion

        

    }
}
