namespace Bars.GkhCr.Modules.ClaimWork.Permission
{
    using B4;
    using Bars.Gkh.Modules.ClaimWork.DomainService;

    public class BuildContractClaimWorkPermission : IClaimWorkPermission 
    {
        private class BuildContractPermission : PermissionMap
        {
            public BuildContractPermission()
            {
                Namespace("Clw.ClaimWork.BuildContract", "Основание ПИР - подрядчики, нарушившие условия договора");
                Permission("Clw.ClaimWork.BuildContract.View", "Просмотр");
                Permission("Clw.ClaimWork.BuildContract.Update", "Обновление");
                Permission("Clw.ClaimWork.BuildContract.Save", "Сохранение");

                Permission("GkhCr.BuilderViolator.ClaimWorkForm", "Начать претензионную работу");
                Permission("GkhCr.BuilderViolator.MakeNew", "Сформировать");
                Permission("GkhCr.BuilderViolator.Clear", "Очистить реестр");
            }
        }
        
        public PermissionMap GetPermissionMap()
        {
            return new BuildContractPermission();
        }
    }
}