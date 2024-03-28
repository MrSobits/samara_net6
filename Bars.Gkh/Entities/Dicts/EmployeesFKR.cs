using Bars.B4.DataAccess;

namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Справочник сотрудников ФКР
    /// </summary>
    public class EmployeesFKR : BaseEntity
    {

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string Position { get; set; }

        /// <summary>
        /// Отдел
        /// </summary>
        public virtual string Departament { get; set; }


    }
}
