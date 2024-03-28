using Bars.Gkh.Entities;
using System;

namespace Bars.Gkh.DomainService
{
    /// <summary>
    /// Базовый класс для получения значений по жилому дому из модуля Overhaul
    /// </summary>
    public abstract class BaseRealObjOverhaulDataObject
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        protected RealityObject realityObject;
        
        /// <summary>
        /// Номер документа ДПКР
        /// </summary>
        public virtual string DocumentNumber { get; }

        /// <summary>
        /// Дата документа ДПКР
        /// </summary>
        public virtual DateTime? DocumentDate { get; }
        
        /// <summary>
        /// Дата включения дома в ДПКР
        /// </summary>
        public virtual DateTime? PublishDate { get; }

        /// <summary>
        /// Дата исключения дома из ДПКР
        /// </summary>
        public virtual DateTime? UnpublishDate { get;}

        /// <summary>
        /// Дата возникновения обязанности по дому
        /// </summary>
        public virtual DateTime? ObligationDate { get; }

        /// <summary>
        /// Инициализация класса
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        public virtual void Init(RealityObject realityObject)
        {
            this.realityObject = realityObject;
        }
    }
}
