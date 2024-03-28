namespace Bars.GkhGji.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Модель представления для <see cref="ControlType"/>
    /// </summary>
    public class ControlTypeViewModel : BaseViewModel<ControlType>
    {
        /// <summary>
        /// Получить список для фильтра
        /// </summary>
        public IDataResult ListWithoutPaging(IDomainService<ControlType> domain)
        {
            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .ToList();

            return new BaseDataResult(data);
        }
    }
}