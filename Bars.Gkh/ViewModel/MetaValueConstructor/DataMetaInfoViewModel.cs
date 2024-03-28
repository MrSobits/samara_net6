namespace Bars.Gkh.ViewModel.MetaValueConstructor
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Представление для <see cref="DataMetaInfo"/>
    /// </summary>
    public class DataMetaInfoViewModel : BaseViewModel<DataMetaInfo>
    {
        /// <summary>
        /// Интерфейс поставщика работы с регистрациями источников данных
        /// </summary>
        public IDataFillerProvider DataFillerProvider { get; set; }

        /// <summary>Получить объект</summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult Get(IDomainService<DataMetaInfo> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var entity = domainService.Get(id);
            if (entity == null)
            {
                return new BaseDataResult();
            }

            return
                new BaseDataResult(
                    new
                    {
                        entity.Id,
                        entity.Parent,
                        entity.Name,
                        entity.Code,
                        entity.Weight,
                        entity.Formula,
                        entity.Group,
                        entity.Level,
                        DataFillerName = this.DataFillerProvider.GetDataFillerInfo().FirstOrDefault(x => x.Code == entity.DataFillerName),
                        entity.DataValueType,
                        entity.Decimals,
                        entity.MaxLength,
                        entity.MinLength,
                        entity.Required
                    });
        }
    }
}