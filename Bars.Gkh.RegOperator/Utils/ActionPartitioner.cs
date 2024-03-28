namespace Bars.Gkh.RegOperator.Utils
{
    using System;

    public class ActionPartitioner
    {
        public static void ProcessByPortion(Action<int> action, int totalCount, int portion)
        {
            var done = 0;
            while (done < totalCount)
            {
                action(done);

                done += portion;
            }
        } 
    }
}