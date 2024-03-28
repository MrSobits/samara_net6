namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    public class CheckMkdAndOwnersPremises : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }
        public IDomainService<Room> RoomDomain { get; set; }

        private long[] municipalityIds;
        private long[] mrIds;

        public CheckMkdAndOwnersPremises()
            : base(new ReportTemplateBinary(Properties.Resources.CheckMkdAndOwnersPremises))
        {
        }

        public override string Name
        {
            get { return "Проверка по МКД и собственникам помещений"; }
        }

        public override string Desciption
        {
            get { return "Проверка по МКД и собственникам помещений"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CheckMkdAndOwnersPremises"; }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhRegOp.CheckMkdAndOwnersPremises";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var mRIdsList = baseParams.Params.GetAs("mrIds", string.Empty);

            mrIds = !string.IsNullOrEmpty(mRIdsList) ? mRIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var realityObjectQuery = RealityObjectDomain.GetAll()
                .WhereIf(mrIds.Length > 0, x => mrIds.Contains(x.Municipality.Id))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.MoSettlement.Id));

            var realityObjectData = realityObjectQuery
                .Select(x => new
                {
                    RealObjId = x.Id,
                    ParentMuId = x.Municipality.Id,
                    ParentMuName = x.Municipality.Name,
                    MuName = x.MoSettlement.Name,
                    x.Address,
                    x.AreaLiving,
                    x.AreaMkd,
                    x.AreaLivingNotLivingMkd,
                    x.AreaLivingOwned
                })
                .OrderBy(x => x.ParentMuName)
                .ThenBy(x => x.MuName)
                .AsEnumerable()
                .GroupBy(x => x.ParentMuId)
                .ToDictionary(x => x.Key, x => x.ToList());

            var realObjIds = realityObjectQuery.Select(x => x.Id).ToList();

            var roomData = RoomDomain.GetAll()
                .Where(x => realObjIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    RoomId = x.Id, 
                    x.RealityObject.Id,
                    x.RoomNum,
                    x.Area
                })
                .ToList();
            
            var roomIds = roomData.Select(x => x.RoomId).ToList();

            var basePersonalAccount = BasePersonalAccountDomain.GetAll()
                .Where(x => roomIds.Contains(x.Room.Id))
                .Where(x => x.State.Code == "1")
                .Select(x => new
                {
                    x.Room.Id,
                    x.AreaShare,
                })
                .ToList();

            var changeStoryByRoomArea = EntityLogLightDomain.GetAll()
                .Where(x => x.ParameterName == "room_area")
                .Select(x => new
                {
                    x.EntityId,
                    x.DateActualChange,
                    x.PropertyValue
                })
                .OrderByDescending(x => x.DateActualChange)
                .ToList();

            var sectionRegion = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRegion");
            var sectionMu = sectionRegion.ДобавитьСекцию("sectionMU");
            var section = sectionMu.ДобавитьСекцию("section");

            var num = 1;

            foreach (var realityObjRegion in realityObjectData)
            {
                sectionRegion.ДобавитьСтроку();
                var parentId = realityObjRegion.Key;
                sectionRegion["MuName"] = realityObjRegion.Value.Where(x => x.ParentMuId == parentId).Select(x => x.ParentMuName).FirstOrDefault();
                var error = 0;
                foreach (var realityObj in realityObjRegion.Value)
                {
                    sectionMu.ДобавитьСтроку();

                    sectionMu["Num"] = num++;
                    sectionMu["MU"] = realityObj.MuName;
                    sectionMu["Address"] = realityObj.Address;
                    sectionMu["Area"] = realityObj.AreaMkd;
                    sectionMu["Leave"] = realityObj.AreaLiving;

                    sectionMu["LeaveCitizen"] = realityObj.AreaLivingOwned;

                    var realObjId = realityObj.RealObjId;
                    
                    decimal totalRoomsArea = 0;
                    foreach (var room in roomData.Where(x => x.Id == realObjId).OrderBy(x => x.RoomNum.ToInt()))
                    {
                        section.ДобавитьСтроку();

                        var roomId = room.RoomId;
                        var factDateArea = changeStoryByRoomArea.Where(x => x.EntityId == roomId).Select(x => x.PropertyValue).FirstOrDefault();

                        section["RoomNumber"] = room.RoomNum;
                        section["RoomArea"] = factDateArea;

                        var roomShareOwnership = basePersonalAccount.Where(x => x.Id == roomId).Select(x => x.AreaShare).Sum();
                        //парам 4
                        section["RoomShareOwnership"] = roomShareOwnership;

                        //парам 2
                        if (roomShareOwnership > 1)
                        {
                            error++;
                            section["Description2"] = "Внимание!!! Доля собственности по помещению превышает допустимых значений";
                        }
                        else if (roomShareOwnership <= 1)
                        {
                            section["Description2"] = string.Empty;
                        }
                        
                        totalRoomsArea += factDateArea.ToDecimal();

                    }

                    sectionMu["TotalArea"] = totalRoomsArea;
                    var areaLivingOwned = realityObj.AreaLivingOwned;
                    var needArea = areaLivingOwned - totalRoomsArea;

                    if (needArea < 0)
                    {
                        error++;
                        //парам 1
                        sectionMu["Description"] = "Внимание !!! Итого по общей площади квартир/помещений превышает значение Жилой площади по МКД";
                    }
                    else if (needArea >= 0 || needArea == null)
                    {
                        sectionMu["Description"] = string.Empty;
                    }
                }
                sectionRegion["Error"] = error;
            }

            reportParams.SimpleReportParams["Today"] = DateTime.Today.ToShortDateString();
            reportParams.SimpleReportParams["Region"] = "Район";
           
        }
    }
}
