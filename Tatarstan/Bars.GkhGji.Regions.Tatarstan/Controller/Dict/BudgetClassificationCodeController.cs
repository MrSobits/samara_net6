namespace Bars.GkhGji.Regions.Tatarstan.Controller.Dict
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode;

    /// <summary>
    /// Контроллер для работы с КБК
    /// </summary>
    public class BudgetClassificationCodeController : B4.Alt.DataController<BudgetClassificationCode>
    {
        /// <summary>
        /// Получить доп информацию о КБК
        /// </summary>
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBudgetClassificationCodeService>();
            using (this.Container.Using(service))
            {
                var result = service.GetInfo(baseParams);
                return result.Success ? this.JsSuccess(result.Data) : this.JsFailure(result.Message);
            }
        }

        /// <summary>
        /// Сохранить МО для КБК
        /// </summary>
        public ActionResult SaveMunicipalities(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBudgetClassificationCodeService>();
            using (this.Container.Using(service))
            {
                var result = service.SaveMunicipalities(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
        }

        /// <inheritdoc />
        public override ActionResult Create(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBudgetClassificationCodeService>();
            using (this.Container.Using(service))
            {
                var result = service.SaveOrUpdate(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
        }

        /// <inheritdoc />
        public override ActionResult Update(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBudgetClassificationCodeService>();
            using (this.Container.Using(service))
            {
                var result = service.SaveOrUpdate(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
        }
    }
}
