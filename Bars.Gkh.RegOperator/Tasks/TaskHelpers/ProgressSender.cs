namespace Bars.Gkh.RegOperator.Tasks.TaskHelpers
{
    using System;

    /// <summary>
    /// Посылает уведомление при изменении прогресса выполнения над коллекцией
    /// </summary>
    internal class ProgressSender
    {
        private readonly int endValue;
        private readonly float partSize;
        private int previousValue;

        private readonly Action<int, string> SendProgress;

        /// <summary>
        /// Прогресс выполнения
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="countObjects">Количество элементов в коллекции</param>
        /// <param name="sendFunction">Callback функция</param>
        /// <param name="endValue">Конечное значени прогресса выполнения</param>
        /// <param name="startValue">Начальное значени прогресса выполнения</param>
        public ProgressSender(int countObjects, Action<int, string> sendFunction, int endValue = 100, int startValue = 0)
        {
            this.Progress = startValue;
            this.endValue = endValue;
            this.partSize = endValue / (float)countObjects;
            this.previousValue = startValue;
            this.SendProgress = sendFunction;
        }

        /// <summary>
        /// Увеличивает счетчик прогресса выполнения и посылает сообщение при изменении прогресса на целое число
        /// </summary>
        public void TrySend(string message = "")
        {
            this.Progress += this.partSize;
            var value = (int) Math.Round(this.Progress);
            if (value <= this.endValue && value > this.previousValue)
            {
                this.previousValue = value;
                this.SendProgress(value, message);
            }
        }

        /// <summary>
        /// Увеличивает счетчик прогресса выполнения и посылает сообщение
        /// </summary>
        public void Send(string message = "")
        {
            this.Progress += this.partSize;
            var value = (int) Math.Round(this.Progress);
            this.previousValue = value;
            this.SendProgress(value, message);
        }

        /// <summary>
        /// Сбрасывает счетчик прогресса выполнения
        /// </summary>
        public void Reset(int startValue = 0)
        {
            this.previousValue = startValue;
            this.Progress = startValue;
        }

        /// <summary>
        /// Посылает сообщение. Счетчик прогресса не изменяется
        /// </summary>
        public void ForceSend(int value, string message)
        {
            this.SendProgress(value, message);
        }
    }
}