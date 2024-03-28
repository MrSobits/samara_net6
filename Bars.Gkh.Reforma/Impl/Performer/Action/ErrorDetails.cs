namespace Bars.Gkh.Reforma.Impl.Performer.Action
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Детали ошибки
    /// </summary>
    public class ErrorDetails
    {
        #region Constructors and Destructors

        /// <summary>
        /// Конструктор
        /// </summary>
        public ErrorDetails()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="e">Исключение</param>
        public ErrorDetails(Exception e)
        {
            Name = e.GetType().Name;
            Description = e.Message;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fault">Исключение, возникшее в ходе обращения к сервису Реформы</param>
        public ErrorDetails(FaultException fault)
        {
            try
            {
                var det = fault.CreateMessageFault().GetReaderAtDetailContents();
                do
                {
                    var value = det.ReadString();
                    switch (det.Name)
                    {
                        case "code":
                            this.Code = value;
                            break;
                        case "name":
                            this.Name = value;
                            break;
                        case "description":
                            this.Description = value;
                            break;
                    }
                }
                while (det.Read());
            }
            catch
            {
                try
                {
                    Name = fault.GetType().Name;
                    Code = fault.Code != null ? fault.Code.Name : string.Empty;
                    Description = fault.Message;
                }
                catch
                {
                    Description = "Неизвестная ошибка";
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Код ошибки
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Подробное описание ошибки
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Название ошибки
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}