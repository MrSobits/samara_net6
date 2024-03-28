namespace Bars.GisIntegration.Base.Enums
{
    using System;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.NsiCommon;

    /// <summary>
    /// Группа справочников
    /// </summary>
    public enum DictionaryGroup
    {
        /// <summary>
        /// Общесистемные
        /// </summary>
        [Display("Общесистемные")]
        Nsi = 10,

        /// <summary>
        /// Справочники ОЖФ
        /// </summary>
        [Display("Справочники ОЖФ")]
        NsiRao = 20
    }

    public static class DictionaryGroupExtensions
    {
        public static ListGroup ToGisGroup(this DictionaryGroup group)
        {
            ListGroup result;

            switch (group)
            {
                case DictionaryGroup.Nsi:
                    result = ListGroup.NSI;
                    break;
                case DictionaryGroup.NsiRao:
                    result = ListGroup.NSIRAO;
                    break;
                default:
                    throw new Exception("Неизвестная группа справочника");
            }

            return result;
        }

        public static DictionaryGroup ToDictionaryGroup(this ListGroup group)
        {
            DictionaryGroup result;

            switch (group)
            {
                case ListGroup.NSI:
                    result = DictionaryGroup.Nsi;
                    break;
                case ListGroup.NSIRAO:
                    result = DictionaryGroup.NsiRao;
                    break;
                default:
                    throw new Exception("Неизвестная группа справочника");
            }

            return result;
        }
    }
}