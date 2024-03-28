namespace Bars.Gkh.DomainService.MetaValueConstructor
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с мета-информацией конструкторов
    /// </summary>
    public class DataMetaInfoService : IDataMetaInfoService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Интерфейс поставщика работы с регистрациями источников данных
        /// </summary>
        public IDataFillerProvider DataFillerProvider { get; set; }

        /// <summary>
        /// Поставщик сессий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="DataMetaInfo"/>
        /// </summary>
        public IDomainService<DataMetaInfo> DataMetaInfoDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="MetaConstructorGroup"/>
        /// </summary>
        public IDomainService<MetaConstructorGroup> MetaConstructorGroupDomain { get; set; }

        /// <inheritdoc/>
        public IDataResult GetTree(BaseParams baseParams)
        {
            var rootMetaInfoId = baseParams.Params.GetAsId();
            var metaInfo = this.DataMetaInfoDomain.Get(rootMetaInfoId);

            if (metaInfo == null)
            {
                return BaseDataResult.Error("Не найдено описание по указанному идентификатору");
            }

            var metaList = this.DataMetaInfoDomain.GetAll().Where(x => x.Group.Id == metaInfo.Group.Id).ToList();

            return new BaseDataResult(new DataMetaTreeGenerator().GetMetaTree(metaList, metaInfo));
        }

        /// <inheritdoc/>
        public IDataResult GetRootElements(BaseParams baseParams)
        {
            var groupId = baseParams.Params.GetAsId("groupId");

            if (groupId <= 0)
            {
                return BaseDataResult.Error("Не удалось определить тип конструктора");
            }

            var metaList = this.DataMetaInfoDomain.GetAll().Where(x => x.Group.Id == groupId && x.Level == 0).ToList();

            return new BaseDataResult(new DataMetaTreeGenerator().GetMetaTree(metaList));
        }

        /// <inheritdoc/>
        public IDataResult GetDataFillers(BaseParams baseParams)
        {
            return new BaseDataResult(this.DataFillerProvider.GetTree(baseParams));
        }

        /// <inheritdoc/>
        public IDataResult CopyConstructor(BaseParams baseParams)
        {
            var groupFromId = baseParams.Params.GetAsId("groupFromId");
            var groupToId = baseParams.Params.GetAsId("groupToId");

            var groups = this.MetaConstructorGroupDomain.GetAll().WhereContains(x => x.Id, new[] { groupToId, groupFromId }).ToDictionary(x => x.Id);

            var groupFrom = groups.Get(groupFromId);
            var groupTo = groups.Get(groupToId);

            if (groupFrom.IsNull())
            {
                return BaseDataResult.Error("Не найдена группа, из которой производится копирование");
            }

            if (groupTo.IsNull())
            {
                return BaseDataResult.Error("Не найдена группа, в которую производится копирование");
            }

            if (groupFrom.Id == groupTo.Id)
            {
                return BaseDataResult.Error("Необходимо выбрать период, отличный от текущего");
            }

            if (groupFrom.ConstructorType != groupTo.ConstructorType)
            {
                return BaseDataResult.Error("Ошибка, объекты относятся к разным типам конструкторов");
            }

            if (this.DataMetaInfoDomain.GetAll().Any(x => x.Group.Id == groupToId))
            {
                return BaseDataResult.Error("Целевой конструктор не пустой");
            }

            var metaToCopy = this.DataMetaInfoDomain.GetAll().Where(x => x.Group.Id == groupFromId).AsEnumerable().OrderBy(x => x.Level).ToList();
            var metaFromToDict = new Dictionary<DataMetaInfo, DataMetaInfo>();

            foreach (var dataMetaInfo in metaToCopy)
            {
                var meta = new DataMetaInfo
                {
                    Parent = dataMetaInfo.Parent.IsNotNull() ? metaFromToDict.Get(dataMetaInfo.Parent) : null,
                    Group = groupTo,

                    Code = dataMetaInfo.Code,
                    DataFillerName = dataMetaInfo.DataFillerName,
                    DataValueType = dataMetaInfo.DataValueType,
                    Decimals = dataMetaInfo.Decimals,
                    Formula = dataMetaInfo.Formula,
                    Level = dataMetaInfo.Level,
                    MaxLength = dataMetaInfo.MaxLength,
                    MinLength = dataMetaInfo.MinLength,
                    Name = dataMetaInfo.Name,
                    Required = dataMetaInfo.Required,
                    Weight = dataMetaInfo.Weight
                };
                metaFromToDict.Add(dataMetaInfo, meta);
            }

            var currentSession = this.SessionProvider.GetCurrentSession();
            this.Container.InTransaction(() => metaFromToDict.Values.ForEach(x => currentSession.Save(x)));

            return new BaseDataResult();
        }
    }
}