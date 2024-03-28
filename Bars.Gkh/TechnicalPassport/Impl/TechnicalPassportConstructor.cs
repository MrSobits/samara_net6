namespace Bars.Gkh.TechnicalPassport.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Entities.TechnicalPassport;

    /// <summary>
    /// Конструктор технического паспорта 
    /// </summary>
    public class TechnicalPassportConstructor : ITechnicalPassportConstructor
    {
        public IDomainService<Form> FormDomain { get; set; }

        public ITechnicalPassportTransformer TechnicalPassportTransformer { get; set; }

        //todo: обернуть в общую транзакцию
        public IDataResult AddForm(BaseParams baseParams)
        {
            var forms = this.Save(this.FormDomain, baseParams);

            forms.ForEach(this.TechnicalPassportTransformer.CreateForm);

            return new BaseDataResult();
        }

        public IDataResult UpdateForm(BaseParams baseParams)
        {
            var forms = this.Update(this.FormDomain, baseParams);

            forms.ForEach(this.TechnicalPassportTransformer.UpdateForm);

            return new BaseDataResult();
        }

        public IDataResult RemoveForm(BaseParams baseParams)
        {
            var forms = this.Delete(this.FormDomain, baseParams);

            forms.ForEach(this.TechnicalPassportTransformer.DeleteForm);

            return new BaseDataResult();
        }

        public IDataResult AddEditor(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        public IDataResult UpdateEditor(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        public IDataResult RemoveEditor(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        public IDataResult AddAttribute(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        public IDataResult UpdateAttribute(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        public IDataResult RemoveAttribute(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        private List<T> Save<T>(IDomainService<T> domainService, BaseParams baseParams)
        {
            return domainService.Save(baseParams).Data as List<T>;
        }

        private List<T> Update<T>(IDomainService<T> domainService, BaseParams baseParams)
        {
            return domainService.Update(baseParams).Data as List<T>;
        }

        private List<T> Delete<T>(IDomainService<T> domainService, BaseParams baseParams)
        {
            return domainService.Delete(baseParams).Data as List<T>;
        }
    }
}
