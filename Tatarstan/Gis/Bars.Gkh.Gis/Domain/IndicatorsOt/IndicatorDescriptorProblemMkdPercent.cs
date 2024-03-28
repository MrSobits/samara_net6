namespace Bars.Gkh.Gis.Domain.IndicatorsOt
{
    public class IndicatorDescriptorProblemMkdPercent : IIndicatorDescriptor
    {
        public int Id
        {
            get
            {
                return 30895;
            }
        }

        public string Title
        {
            get
            {
                return "% проблемных МКД к общему кол-ву МКД";
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
                return 744;
            }
        }
    }
}
