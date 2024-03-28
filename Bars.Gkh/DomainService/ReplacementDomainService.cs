namespace Bars.Gkh.DomainService
{
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Данный класс предназначен для того чтобы заменять домен сервис 
    /// поскольку в регионах Сущности заменяются через subclass, то необходимо избежать того что ктото в коде сделает
    /// new Tbase(), затем Сохранит его через IDisposalService<Tbase> необходимо чтобы после такого действия Сохранение шло через сущность Tderived
    /// Который добавился в регионных модулях
    /// </summary>
    public class ReplacementDomainService<Tbase, Tderived> : BaseDomainService<Tbase>, IReplacementEntity<Tbase, Tderived>
        where Tbase : class, IEntity
        where Tderived : class, IEntity, Tbase, new()
    {

        public IDomainService<Tderived> NewDomain { get; set; }

        private PropertyInfo[] tBaseProperties = null;
        public PropertyInfo[] TBaseProperties
        {
            get
            {
                if (tBaseProperties != null) return tBaseProperties;

                tBaseProperties = typeof(Tbase).GetProperties();
                return tBaseProperties;
            }
        }

        public Tderived ToDerived(Tbase tBase)
        {
            var tDerived = new Tderived();

            foreach (PropertyInfo propBase in TBaseProperties)
            {
                PropertyInfo propDerived = typeof(Tderived).GetProperty(propBase.Name);
                propDerived.SetValue(tDerived, propBase.GetValue(tBase, null), null);
            }
            return tDerived;
        }

        protected override void SaveInternal(Tbase value)
        {
            var newValue = this.ToDerived(value);
            NewDomain.Save(newValue);

            // Поскольку метод Сохранения мог использоватся во внешнем коде как пакет транзакции то могли
            // На этот объект поставить ссылку. Следовательно чтобы неупал коммит внешней транзакции возвращаем Id сохраненного объекта 
            value.Id = newValue.Id;
        }

        /// <summary>
        /// тут удаление тоже всвязи с этим делаем
        /// </summary>
        protected override void DeleteInternal(object id)
        {
            NewDomain.Delete(id);
        }
    }
}
