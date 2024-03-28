namespace Bars.GkhGji.Controllers.Dict
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.ViewModel.Dict;

    /// <summary>
    /// Контроллер для <see cref="ControlType"/>
    /// </summary>
    public class ControlTypeController : B4.Alt.DataController<ControlType>
    {
        #region Dependency Injection
        private readonly IDomainService<ControlType> controlTypeDomain;
        
        public ControlTypeController(IDomainService<ControlType> controlTypeDomain)
        {
            this.controlTypeDomain = controlTypeDomain;
        }
        #endregion
        
        /// <summary>
        /// Получить список для фильтра
        /// </summary>
        /// <returns></returns>
        public ActionResult ListWithoutPaging()
        {
            return new JsonNetResult((this.ViewModel as ControlTypeViewModel).ListWithoutPaging(this.controlTypeDomain));
        }
    }
}