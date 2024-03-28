namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class UploadingAtDataForFund : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }
        
        public UploadingAtDataForFund()
            : base(new ReportTemplateBinary(Properties.Resources.UploadingAtDataForFund))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.UploadingAtDataForFund";
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return "Выгрузка сведений по аварийности для Фонда"; }
        }

        /// <summary>
        /// Группа
        /// </summary>
        public override string GroupName
        {
            get { return "Формы для Фонда"; }
        }

        /// <summary>
        /// Представление с пользователскими параметрами
        /// </summary>
        public override string ParamsController
        {
            get { return "B4.controller.report.UploadingAtDataForFund"; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Выгрузка сведений по аварийности для Фонда"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
        }

        public override string ReportGenerator { get; set; }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceEmergencyObjects = Container.Resolve<IDomainService<EmergencyObject>>().GetAll();
            var serviceRealityObjectApartInfo = Container.Resolve<IDomainService<RealityObjectApartInfo>>().GetAll();
            var serviceEmerObjResettlementProgram = Container.Resolve<IDomainService<EmerObjResettlementProgram>>().GetAll();
            
            var EmergencyObjectData = serviceEmergencyObjects
                .Select(x => new
                                 {
                                     MoId = x.RealityObject.Municipality.Id,
                                     MoName = x.RealityObject.Municipality.Name,
                                     MoFedNumber = x.RealityObject.Municipality.FederalNumber,
                                     RoId = x.RealityObject.Id,
                                     x.RealityObject.Address,
                                     x.RealityObject.FederalNum,
                                     x.RealityObject.NumberApartments,
                                     x.DocumentName,
                                     x.DocumentNumber,
                                     x.DocumentDate,
                                     x.DemolitionDate,
                                     x.RealityObject.NumberLiving,
                                     x.RealityObject.AreaMkd,
                                     x.ResettlementFlatArea,
                                     x.ResettlementFlatAmount
                                 })
                .ToList()
                .GroupBy(x => new { x.MoId, x.MoName })
                .ToDictionary(x => x.Key, x => 
                    x.GroupBy(y => y.RoId)
                    .ToDictionary(y => y.Key, y => y.First()));

            var RoIds = serviceEmergencyObjects.Select(x => x.RealityObject.Id);


            var ROApartInfosData = serviceRealityObjectApartInfo
                .Where(x => RoIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    x.Privatized,
                    x.AreaTotal,
                    RoId = x.RealityObject.Id
                })
                .ToList()
                .GroupBy(z => z.RoId)
                .ToDictionary(z => z.Key, z =>
                    {
                        var NoPrivatizedTotalArea = z.Where(q => q.Privatized == YesNoNotSet.No).Sum(q => q.AreaTotal);

                        var NoPrivatizedCount = z.Count(q => q.Privatized == YesNoNotSet.No);

                        var PrivatizedTotalArea = z.Where(q => q.Privatized == YesNoNotSet.Yes).Sum(q => q.AreaTotal);

                        return new { NoPrivatizedTotalArea, NoPrivatizedCount, PrivatizedTotalArea };
                    });

            var EmerObjResetProgramsData = serviceEmerObjResettlementProgram
                .Where(x => RoIds.Contains(x.EmergencyObject.RealityObject.Id))
                .GroupBy(y => y.EmergencyObject.RealityObject.Id)
                .Select(x => new { x.Key, CountResidents = x.Sum(z => z.CountResidents)})
                .ToDictionary(y => y.Key, y => y.CountResidents);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var emergencyObject in EmergencyObjectData.OrderBy(x => x.Key.MoName))
            {
                foreach (var value in emergencyObject.Value.Values)
                {
                    var realtyObjectId = value.RoId;

                    section.ДобавитьСтроку();

                    section["IdMo"] = value.MoFedNumber;
                    section["MoName"] = value.MoName;
                    section["IdMKD"] = value.FederalNum;
                    section["Adress"] = value.Address;
                    section["DocNumber"] = !value.DocumentName.IsEmpty() && !value.DocumentNumber.IsEmpty() ? value.DocumentName + " № " + value.DocumentNumber : string.Empty;
                    section["DocDate"] = value.DocumentDate.HasValue ? value.DocumentDate.Value.ToShortDateString() : string.Empty;
                    section["FinishDate"] = value.DemolitionDate.HasValue ? value.DemolitionDate.Value.ToShortDateString() : string.Empty;
                    section["NumberLiving"] = value.NumberLiving.HasValue ? value.NumberLiving.Value.ToStr() : string.Empty;
                    section["AreaMKD"] = value.AreaMkd;
                    section["AreaRoomsMo"] = ROApartInfosData.ContainsKey(realtyObjectId) ? ROApartInfosData[realtyObjectId].NoPrivatizedTotalArea.ToStr() : string.Empty;
                    section["NumberRoomsMo"] = ROApartInfosData.ContainsKey(realtyObjectId) ? ROApartInfosData[realtyObjectId].NoPrivatizedCount.ToStr() : string.Empty;
                    section["AreaRoomsPrivate"] = ROApartInfosData.ContainsKey(realtyObjectId) ? ROApartInfosData[realtyObjectId].PrivatizedTotalArea.ToStr() : string.Empty;
                    section["DocProps"] = value.DocumentNumber;
                    section["NumberLivingRelocation"] = EmerObjResetProgramsData.ContainsKey(realtyObjectId) ? EmerObjResetProgramsData[realtyObjectId].ToStr() : string.Empty;
                    section["AreaResett"] = value.ResettlementFlatArea;
                    section["NumberReset"] = value.ResettlementFlatAmount;
                    section["TotalLivingArea"] = value.AreaMkd;
                    section["TotalLivingNumber"] = value.NumberApartments;
                }
            }
        }
    }
}