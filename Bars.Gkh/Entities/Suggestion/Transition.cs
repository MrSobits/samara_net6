namespace Bars.Gkh.Entities.Suggestion
{
    using System;
    using B4;

    using Bars.Gkh.Entities;

    using Enums;

    /// <summary>
    /// Переход обращения граждан с одного состояния обработки в другое
    /// </summary>
    public class Transition : BaseImportableEntity
    {
        #region .ctor
        /// <summary>
        /// Создание нового экземпляра <see cref="Transition"/>.
        /// </summary>
        [Obsolete("Используется только при создании объекта из BaseParams и Nhibernate")]
        public Transition()
        {
        }

        /// <summary>
        /// Создание нового экземпляра <see cref="Transition"/>.
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="executionDeadline">Срок исполнения в днях</param>
        public Transition(string name, int executionDeadline)
        {
            Name = name;
            ExecutionDeadline = executionDeadline;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsFirst { get; protected set; }

        /// <summary>
        /// Рубрика, в рамках которой действует данное правило перехода.
        /// </summary>
        public virtual Rubric Rubric { get; protected set; }

        /// <summary>
        /// С какого исполнителя меняем
        /// </summary>
        public virtual ExecutorType InitialExecutorType { get; protected set; }

        /// <summary>
        /// На какого исполнителя меняем
        /// </summary>
        public virtual ExecutorType TargetExecutorType { get; protected set; }

        /// <summary>
        /// Наименование перехода.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Срок исполнения в днях.
        /// </summary>
        public virtual int ExecutionDeadline { get; protected set; }

        /// <summary>
        /// Шаблон письма-уведомления.
        /// </summary>
        public virtual string EmailTemplate { get; protected set; }

        /// <summary>
        /// Email, на который отправляется уведомление при достижении срока исполнения. 
        /// </summary>
        public virtual string ExecutorEmail { get; protected set; }

        /// <summary>
        /// Тема письма.
        /// </summary>
        public virtual string EmailSubject { get; protected set; }

        /// <summary>
        /// Валидация перехода
        /// </summary>
        /// <returns></returns>
        public virtual IDataResult Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return BaseDataResult.Error("Наименование перехода должно быть заполнено.");
            }

            if (ExecutionDeadline <= 0)
            {
                return BaseDataResult.Error(ErrorMsg("Срок исполнения должен быть > 0."));
            }

            if (InitialExecutorType == 0)
            {
                return BaseDataResult.Error(ErrorMsg("Исходная тип исполнителя должен быть определен."));
            }

            if (InitialExecutorType == TargetExecutorType)
            {
                return BaseDataResult.Error(ErrorMsg("Исходный и целевой тип исполнителя должны отличаться."));
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual string ErrorMsg(string message)
        {
            return string.Format("Переход {0}: {1}", Name, message);
        }
    }
}