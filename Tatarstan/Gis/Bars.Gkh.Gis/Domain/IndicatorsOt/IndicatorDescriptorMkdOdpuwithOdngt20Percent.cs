namespace Bars.Gkh.Gis.Domain.IndicatorsOt
{
    public class IndicatorDescriptorMkdOdpuwithOdngt20Percent : IIndicatorDescriptor
    {
        public int Id
        {
            get
            {
                return 30894;
            }
        }

        public string Title
        {
            get
            {
                return "Кол-во МКД с ОДПУ, где ОДН свыше 20%";
            }
        }

        public TypeFrequency FrequencyId
        {
            get
            {
                return TypeFrequency.Monthly;
            }
        }

        public int? GroupId
        {
            get
            {
                return null;
            }
        }

        public int MeasureId
        {
            get
            {
                return 796;
            }
        }
    }
}
