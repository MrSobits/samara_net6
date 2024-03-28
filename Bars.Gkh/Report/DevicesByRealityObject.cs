namespace Bars.Gkh.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.PassportProvider;
    using Bars.Gkh.Properties;
    using Castle.Windsor;

    public class DevicesByRealityObject : BasePrintForm
    {
        #region Свойства
        public IWindsorContainer Container { get; set; }

        private long[] municipailityIds;
        private long typeDeviceId;

        private Dictionary<long, CellCodesProxy> deviceByInfo = new Dictionary<long, CellCodesProxy>()
            {
                {17, new CellCodesProxy()
                         {
                             NameDevice = "Счетчик ХВС", 
                             FormCodeType = "Form_3_2_CW", 
                             CellCodeType = "1:3",
                             FormCodeDevCollectContr = "Form_3_2CW_2",
                             CellCodeDevCollectContr = "1:3"
                         }},
                {18, new CellCodesProxy()
                         {
                             NameDevice = "Счетчик ГВС", 
                             FormCodeType = "Form_3_2", 
                             CellCodeType = "1:3",
                             FormCodeDevCollectContr = "Form_3_2_2",
                             CellCodeDevCollectContr = "1:3",
                             FormCodeAssembContr = "Form_3_2_2",
                             CellCodeAssembContr = "2:3"
                         }},
                {20, new CellCodesProxy()
                         {
                             NameDevice = "Счетчик газоснабжение", 
                             FormCodeType = "Form_3_4", 
                             CellCodeType = "1:3",
                             FormCodeDevCollectContr = "Form_3_4_2",
                             CellCodeDevCollectContr = "1:3"
                         }},
                {21, new CellCodesProxy()
                         {
                             NameDevice = "Счетчик электроэнергии", 
                             FormCodeType = "Form_3_3", 
                             CellCodeType = "1:3",
                             FormCodeDevCollectContr = "Form_3_3_2",
                             CellCodeDevCollectContr = "1:3"
                         }},
                {22, new CellCodesProxy()
                         {
                             NameDevice = "Счетчик тепловой энергии (отопления)", 
                             FormCodeType = "Form_3_1", 
                             CellCodeType = "1:3",
                             FormCodeDevCollectContr = "Form_3_1_2",
                             CellCodeDevCollectContr = "1:3",
                             FormCodeAssembContr = "Form_3_1_2",
                             CellCodeAssembContr = "2:3"
                         }}
            };

        public DevicesByRealityObject()
            : base(new ReportTemplateBinary(Resources.DevicesByRealityObject))
        {
        }
        public override string Name
        {
            get
            {
                return "Отчет по приборам";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по приборам";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Раскрытие информации о деятельности УК ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.DevicesByRealityObject";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhTp.DevicesByRealityObject";
            }
        }
        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipality", string.Empty);
            municipailityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var typeDevice = baseParams.Params.GetAs("typedevice", string.Empty);
            typeDeviceId = !string.IsNullOrEmpty(typeDevice) ? typeDevice.ToInt() : -1;
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var device = deviceByInfo[typeDeviceId];
            if (typeDeviceId == 18 || typeDeviceId == 22)
            {
                var column = reportParams.ComplexReportParams.ДобавитьСекцию("колонка");
                column.ДобавитьСтроку();
                column["installedControlDev"] = "$installedControlDev$";
                column["nameService"] = device.NameDevice;
            }

            var serviceTehPassportValue = Container.Resolve<IDomainService<TehPassportValue>>();
            var serviceManOrgContractRealityObject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var serviceTehPassport = Container.Resolve<IDomainService<TehPassport>>();
            var serviceRealityObject = Container.Resolve<IDomainService<RealityObject>>();
            var passport = Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт");
            var serviceRealityObjectMeteringDevice = Container.Resolve<IDomainService<RealityObjectMeteringDevice>>();

            var realObj = serviceRealityObject.GetAll()
                         .WhereIf(this.municipailityIds.Length > 0, x => municipailityIds.Contains(x.Municipality.Id))
                         .Where(x => x.TypeHouse == TypeHouse.ManyApartments || x.TypeHouse == TypeHouse.SocialBehavior) 
                         .Where(x => x.ConditionHouse == ConditionHouse.Serviceable)
                         .Select(x => new
                         {
                             roId = x.Id,
                             Mu = x.Municipality.Name,
                             RealAddress = x.Address,
                             typeHouse = x.TypeHouse
                         });

            var realObjIdList = realObj.Select(x => x.roId).Distinct();

            var realObjDict = realObj.AsEnumerable()
                .OrderBy(x => x.Mu)
                .ThenBy(x => x.RealAddress)
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            #region Управляющая Организация
            var infoManagOrgList = serviceManOrgContractRealityObject.GetAll()
                               .Where(x => realObjIdList.Contains(x.RealityObject.Id)
                                   && (!x.ManOrgContract.StartDate.HasValue
                                       || x.ManOrgContract.StartDate <= DateTime.Now.Date)
                                   && (!x.ManOrgContract.EndDate.HasValue
                                       || x.ManOrgContract.EndDate >= DateTime.Now.Date)
                                       && x.ManOrgContract.ManagingOrganization.Contragent != null)
                                       .OrderByDescending(x => x.ManOrgContract.TypeContractManOrgRealObj)
                               .Select(x => new
                               {
                                   realityId = x.RealityObject.Id,
                                   managOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                                   managOrgType = x.ManOrgContract.TypeContractManOrgRealObj,
                                   dateStartManOrg = x.ManOrgContract.StartDate ?? DateTime.MinValue,
                                   dateEndManOrg = x.ManOrgContract.EndDate ?? DateTime.MinValue,
                               })
                               .ToList();

            var infoManagOrg = infoManagOrgList.AsEnumerable()
                         .GroupBy(x => x.realityId)
                      .ToDictionary(x => x.Key, x =>
                          {
                              var listName = new List<string>();
                              if (x.Count() > 1)
                              {
                                  foreach (var item in x.Where(z => z.dateStartManOrg != DateTime.MinValue))
                                  {
                                      string str;
                                      if (item.managOrgType == TypeContractManOrg.ManagingOrgJskTsj
                                          && x.Count() > 1)
                                      {
                                          str = "(" + item.managOrgName + ")";
                                      }
                                      else
                                      {
                                          str = item.managOrgName;
                                      }

                                      if (item.managOrgType != TypeContractManOrg.ManagingOrgJskTsj)
                                      {
                                          str = item.managOrgName;
                                      }

                                      listName.Add(str);
                                  }
                              }
                              else
                              {
                                  return x.Select(y => y.managOrgName).FirstOrDefault();
                              }
                              var namesManOrg = listName.IsNotEmpty() ? listName.Aggregate((curr, next) => curr + ", " + next) : string.Empty;
                              return namesManOrg;
                          });
            #endregion

            var tehPassport = serviceTehPassport.GetAll()
                    .WhereIf(this.municipailityIds.Length > 0, x => municipailityIds.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => (x.RealityObject.TypeHouse == TypeHouse.ManyApartments || x.RealityObject.TypeHouse == TypeHouse.SocialBehavior) 
                        && x.RealityObject.ConditionHouse == ConditionHouse.Serviceable)
                    .Select(x => new
                    {
                        idPassport = x.Id,
                        roId = x.RealityObject.Id
                    });

            var dictTechPassport = tehPassport.AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.idPassport).FirstOrDefault());

            var tehPasIdQuery = tehPassport.Select(x => x.idPassport).Distinct();
   
            var listTehPassportValue =
                serviceTehPassportValue.GetAll()
                                       .Where(x => tehPasIdQuery.Contains(x.TehPassport.Id)
                                               && ((x.FormCode == device.FormCodeType && x.CellCode == device.CellCodeType) // Вид
                                               || (x.FormCode == device.FormCodeDevCollectContr && x.CellCode == device.CellCodeDevCollectContr) // ПУ иУУ
                                               || (x.FormCode == device.FormCodeAssembContr && x.CellCode == device.CellCodeAssembContr)))
                                       .Select(x => new { tehPassportId = x.TehPassport.Id, x.FormCode, x.CellCode, x.Value })
                                       .ToList();

            var dictTehPassportValue = listTehPassportValue.GroupBy(x => x.tehPassportId)
                                   .ToDictionary(
                                       x => x.Key,
                                       x => x.GroupBy(y => y.FormCode)
                                            .ToDictionary(
                                                y => y.Key,
                                                y => y.ToDictionary(
                                                        z => z.CellCode,
                                                        z => passport.GetTextForCellValue(y.Key, z.CellCode, z.Value))));

            var dictService = serviceRealityObjectMeteringDevice.GetAll()
                .Where(x => realObjIdList.Contains(x.RealityObject.Id))
                .Where(x => x.MeteringDevice.Name == device.NameDevice)
                .Select(x => new
                                 {
                                     x.RealityObject.Id,
                                     x.MeteringDevice.AccuracyClass,
                                     dateReg = x.DateRegistration ?? DateTime.MinValue
                                 })
                                 .AsEnumerable()
                                 .GroupBy(x => x.Id)
                                 .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("секция");
            reportParams.SimpleReportParams["nameService"] = device.NameDevice;
            foreach (var ro in realObjDict)
            {
                section.ДобавитьСтроку();
                section["municipality"] = ro.Value.Mu;
                section["address"] = ro.Value.RealAddress;
                section["typeHouse"] = ro.Value.typeHouse.GetEnumMeta().Display;

                if (infoManagOrg.ContainsKey(ro.Key))
                {
                    section["managOrg"] = infoManagOrg[ro.Key];
                }

                if (dictTechPassport.ContainsKey(ro.Key) && dictTehPassportValue.ContainsKey(dictTechPassport[ro.Key]))
                {
                    var dictTpVal = dictTehPassportValue[dictTechPassport[ro.Key]];
                    if (dictTpVal.ContainsKey(device.FormCodeType) && dictTpVal[device.FormCodeType].ContainsKey(device.CellCodeType))
                    {
                        section["typeService"] = dictTpVal[device.FormCodeType][device.CellCodeType];
                    }

                    if (dictTpVal.ContainsKey(device.FormCodeDevCollectContr)
                        && dictTpVal[device.FormCodeDevCollectContr].ContainsKey(device.CellCodeDevCollectContr))
                    {
                        var dictCellDev = dictTpVal[device.FormCodeDevCollectContr][device.CellCodeDevCollectContr];
                        switch (dictCellDev)
                        {
                            case "Да":
                                section["installedDevice"] = 1;
                                break;
                            case "Нет":
                                section["installedDevice"] = 0;
                                break;
                        }
                    }

                    if ((typeDeviceId == 18 || typeDeviceId == 22) 
                        && dictTpVal.ContainsKey(device.FormCodeAssembContr)
                        && dictTpVal[device.FormCodeAssembContr].ContainsKey(device.CellCodeAssembContr))
                    {
                        var dictCellDev = dictTpVal[device.FormCodeAssembContr][device.CellCodeAssembContr];
                        switch (dictCellDev)
                        {
                            case "Да":
                                section["installedControlDev"] = 1;
                                break;
                            case "Нет":
                                section["installedControlDev"] = 0;
                                break;
                        }
                    }
                }

                if (dictService.ContainsKey(ro.Key))
                {
                    if (dictService[ro.Key].dateReg != DateTime.MinValue)
                    {
                        section["date"] = dictService[ro.Key].dateReg;
                    }

                    section["classPrec"] = dictService[ro.Key].AccuracyClass;
                }
            }
        }

        public class CellCodesProxy
        {
            public string NameDevice;

            // код формы и ячейки вида прибора
            public string FormCodeType;
            public string CellCodeType;

            //код формы и ячейки "Установлен прибор коллект.учета"
            public string FormCodeDevCollectContr;
            public string CellCodeDevCollectContr;

            //код формы и ячейки "Устанволен узел управления"
            public string FormCodeAssembContr;
            public string CellCodeAssembContr;
        }
    }
}