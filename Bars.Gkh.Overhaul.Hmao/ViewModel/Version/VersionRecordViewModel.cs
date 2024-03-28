namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Представление записи 3 этапа
    /// </summary>
    public class VersionRecordViewModel : BaseViewModel<VersionRecord>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен сервис <see cref="VersionRecord" /></param>
        /// <param name="baseParams">Базовое параметры</param>
        /// <returns>Список</returns>
        public override IDataResult List(IDomainService<VersionRecord> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var stage3Service = this.Container.Resolve<IStage3Service>();

            using (this.Container.Using(stage3Service))
            {
                var data = stage3Service.ListWithStructElements(baseParams);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}