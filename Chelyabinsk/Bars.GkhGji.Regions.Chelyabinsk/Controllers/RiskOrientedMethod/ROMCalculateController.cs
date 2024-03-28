namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Regions.Chelyabinsk.Enums;
    using Bars.GkhGji.Enums;

    public class ROMCalculateController : BaseController
    {
        private IFileManager _fileManager;
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;

        public ROMCalculateController(IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public IDomainService<ROMCategory> ROMCategoryDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public ActionResult Calculate(BaseParams baseParams, Int64 romCategory)
        {
            decimal result = 0;
            ROMCategory entity = ROMCategoryDomain.Get(romCategory);
            Bars.GkhGji.Enums.RiskCategory category = entity.RiskCategory;
           
            try
            {

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator.Inspector != null)
                    entity.Inspector = thisOperator.Inspector;
                else
                    return JsFailure("Расчет категорий риска доступен только сотрудникам ГЖИ");

            }
            catch (Exception e)
            {
                return JsFailure("Не удалось создать задачу");
            }

            decimal areaTotal = entity.MkdAreaTotal;
            int VpCount = entity.Vp;
            int VnCount = entity.Vn;
            int VprCount = entity.Vpr;
            int R = entity.MonthCount;
            if (areaTotal * R > 0)
            {
                // result = (5 * VpCount + VnCount + 2 * VprCount) * 24 / (areaTotal * R);
                result = (5 * VpCount + VnCount + 0.5m * VprCount) / areaTotal;
                entity.Result = result.RoundDecimal(2);
    
                if (entity.SeverityGroup == SeverityGroup.AGroup)
                {
                    if (entity.ProbabilityGroup == ProbabilityGroup.Group1)
                    {
                        entity.RiskCategory = RiskCategory.High;
                    }
                    else
                    {
                        entity.RiskCategory = RiskCategory.Average;
                    }
                }
                else
                {
                    entity.SeverityGroup = SeverityGroup.BGroup;
                    if (entity.ProbabilityGroup == ProbabilityGroup.Group1)
                    {
                        entity.RiskCategory = RiskCategory.Moderate;
                    }
                    else
                    {
                        entity.ProbabilityGroup = ProbabilityGroup.Group2;
                        entity.RiskCategory = RiskCategory.Low;
                    }

                }
   
                
                
                ROMCategoryDomain.Update(entity);
                category = entity.RiskCategory;
            }
            string str = romCategory.ToString();

            var data = new { result = result, category = category };
            return JsSuccess(data);
        }

    }
}
