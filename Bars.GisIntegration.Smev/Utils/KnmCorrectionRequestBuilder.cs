namespace Bars.GisIntegration.Smev.Utils
{
    using System;
    using System.Linq;

    using Bars.GisIntegration.Smev.SmevExchangeService.ERKNM;

    using NHibernate.Util;

    /// <summary>
    /// Построитель запроса для корректировки КНМ
    /// </summary>
    internal class KnmCorrectionRequestBuilder
    {
        private LetterToErknmType _requestObject;
        private readonly string _iGuid;

        public KnmCorrectionRequestBuilder(string iGuid)
        {
            this._iGuid = iGuid;
            this._requestObject = new LetterToErknmType
            {
                Item = new LetterToErknmTypeSet
                {
                    Items = new object[]{}
                }
            };
        }

        /// <summary>
        /// Добавление секции обновления (UpdateInspectionRequestType)
        /// </summary>
        /// <param name="items">Объекты, которые должны быть помещены внутрь тега UpdateInspectionRequestType</param>
        /// <returns></returns>
        public KnmCorrectionRequestBuilder AddUpdatingObject(params object[] items)
        {
            if (!this.checkItems(items))
            {
                return this;
            }
            
            var requestBodyItems = (this._requestObject.Item as LetterToErknmTypeSet).Items.ToList();

            requestBodyItems.AddRange
            (
                items
                    .Where(x => x != null)
                    .Select(x => this.getUpdateInspectionRequestTypeObject(x))
            );
            
            (this._requestObject.Item as LetterToErknmTypeSet).Items = requestBodyItems.ToArray();

            return this;
        }

        public LetterToErknmType GetRequestObject()
        {
            return this._requestObject;
        }

        private UpdateInspectionRequestType getUpdateInspectionRequestTypeObject(params object[] items)
        {
            return new UpdateInspectionRequestType
            {
                iGuid = this._iGuid,
                Items = items
            };
        }

        private bool checkItems(params object[] items)
        {
            if (items == null)
            {
                return false;
            }

            if (items.All(x => x == null))
            {
                return false;
            }

            return true;
        }
    }
}