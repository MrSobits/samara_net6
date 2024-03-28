namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ExceptionExtensions
    {
        public static IList<Exception> GetInnerExceptions(this Exception ex, int deepLevel = 10)
        {
            var result = new List<Exception>(deepLevel) { ex };

            var exception = ex;
            for (int i = 0; i < deepLevel; i++)
            {
                var innerException = exception.InnerException;
                if (innerException == null)
                {
                    break;
                }
                exception = innerException;
                result.Add(innerException);
            }

            return result;
        }

        /// <summary>
        /// Получить иерархичный объект ошибки для JSON
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="startMessage"></param>
        /// <returns></returns>
        public static ErrorAggregator GetIerarchyErrorObject(this Exception ex, string startMessage = "")
        {
            var innerExceptions = ex.GetInnerExceptions();
            var error = new ErrorAggregator(startMessage + ex.Message);
            var errorPointer = error;
            
            for (int i = 1; i < innerExceptions.Count; i++)
            {
                errorPointer.InnerException = new ErrorAggregator(innerExceptions[i].Message);
                errorPointer = errorPointer.InnerException;
            }

            return error;
        }
    }
}