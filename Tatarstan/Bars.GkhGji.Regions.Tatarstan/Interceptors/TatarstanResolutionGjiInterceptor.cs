namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanResolutionGji;

    public class TatarstanResolutionGjiInterceptor : DocumentGjiInterceptor<TatarstanResolutionGji>
    {
       
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<TatarstanResolutionGji> service, TatarstanResolutionGji entity)
        {
            this.UpdateDocNumber(entity);
            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<TatarstanResolutionGji> service, TatarstanResolutionGji entity)
        {
            this.UpdateDocNumber(entity);
            return base.BeforeUpdateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<TatarstanResolutionGji> service, TatarstanResolutionGji entity)
        {
            var documentGjiChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var annexDomain = this.Container.Resolve<IDomainService<TatarstanProtocolGjiAnnex>>();
            var eyewitnessDomain = this.Container.Resolve<IDomainService<TatarstanDocumentWitness>>();
            var definitionDomain = this.Container.ResolveDomain<ResolutionDefinition>();
            var disputeDomain = this.Container.ResolveDomain<ResolutionDispute>();
            var payFineDomain = this.Container.ResolveDomain<ResolutionPayFine>();
            
            using (this.Container.Using(documentGjiChildrenDomain, annexDomain, eyewitnessDomain, 
                definitionDomain, disputeDomain, payFineDomain))
            {
                var refFuncs = new List<Func<long, string>>
                {
                    id => annexDomain.GetAll().Any(x => x.DocumentGji.Id == id) ? "Приложения" : null,
                    id => definitionDomain.GetAll().Any(x => x.Resolution.Id == id) ? "Определения" : null,
                    id => disputeDomain.GetAll().Any(x => x.Resolution.Id == id) ? "Оспаривания" : null,
                    id => payFineDomain.GetAll().Any(x => x.Resolution.Id == id) ? "Оплаты штрафов" : null,
                    id => eyewitnessDomain.GetAll().Any(x => x.DocumentGji.Id == id) ? "Сведения о свидетелях и потерпевших" : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                if (refs.Any())
                {
                    return this.Failure($"Существуют связанные записи в следующих таблицах: {string.Join(", ", refs)}");
                }

                var docChilds = documentGjiChildrenDomain.GetAll()
                    .Where(x => x.Children.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                docChilds.ForEach(x => documentGjiChildrenDomain.Delete(x));

            }

            return base.BeforeDeleteAction(service, entity);
        }

        private void UpdateDocNumber(TatarstanResolutionGji entity)
        {
            var slIndex = entity.DocumentSubNum.HasValue && (entity.DocumentNumber?.Contains("/") ?? false)
                ? entity.DocumentNumber.IndexOf('/') 
                : -1;
            var newDocNum = slIndex != -1
                    ? entity.DocumentNumber.Substring(0, slIndex)
                : entity.DocumentNumber;
            if (entity.DocumentSubNum.HasValue)
            {
                newDocNum += $"/{entity.DocumentSubNum}";
            }

            entity.DocumentNumber = newDocNum;
            if (entity.DocumentDate.HasValue)
            {
                entity.DocumentYear = entity.DocumentDate.Value.Year;
            }
        }
    }
}
