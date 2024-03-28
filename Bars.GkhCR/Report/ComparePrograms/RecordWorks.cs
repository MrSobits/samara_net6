namespace Bars.GkhCr.Report.ComparePrograms
{
    using Bars.B4.Utils;

    internal class RecordWorks
    {
        public static readonly RecordWorks Empty = new RecordWorks();

        public RecordWorks()
        {
        }

        public RecordWorks(decimal volume, decimal sum)
        {
            Volume = volume;
            Sum = sum;
        }

        public RecordWorks(object volume, object sum)
        {
            Volume = volume.ToDecimal();
            Sum = sum.ToDecimal();
        }

        public decimal Volume { get; set; }

        public decimal Sum { get; set; }

        public static RecordWorks operator +(RecordWorks recOne, RecordWorks recTwo)
        {
            return new RecordWorks(recOne.Volume + recTwo.Volume, recOne.Sum + recTwo.Sum);
        }

        public static RecordWorks operator -(RecordWorks запись1, RecordWorks запись2)
        {
            return new RecordWorks(запись1.Volume - запись2.Volume, запись1.Sum - запись2.Sum);
        }

        public bool IsZero()
        {
            return Volume == 0 && Sum == 0;
        }

        public RecordWorks Clone()
        {
            return new RecordWorks(Volume, Sum);
        }
    }
}
