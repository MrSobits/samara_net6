using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Castle.Windsor;

namespace Bars.B4.Modules.FIAS
{
   
    public static class DomainFias
    {
        /// <summary>
        /// Данный метод проверяет заполнены ли нужные коды для Уровня
        /// и выдает сообщение если неправильно заполнены коды
        /// </summary>
        public static string ValidationCode(Fias value)
        {
            var result = "";

            switch (value.AOLevel)
            {
                case FiasLevelEnum.Region:
                    {
                        if( string.IsNullOrEmpty(value.CodeRegion) )
                            result += "код региона,";
                        
                    }
                    break;

                case FiasLevelEnum.AutonomusRegion:
                    {
                        if (string.IsNullOrEmpty(value.CodeRegion))
                            result += "код региона,";

                        if (string.IsNullOrEmpty(value.CodeAuto))
                            result += "код автономии,";
                    }
                    break;

                case FiasLevelEnum.Raion:
                    {
                        if (string.IsNullOrEmpty(value.CodeRegion))
                            result += "код региона,";

                        if (string.IsNullOrEmpty(value.CodeAuto))
                            result += "код автономии,";

                        if (string.IsNullOrEmpty(value.CodeArea))
                            result += "код района,";
                    }
                    break;

                case FiasLevelEnum.City:
                    {
                        if (string.IsNullOrEmpty(value.CodeRegion))
                            result += "код региона,";

                        if (string.IsNullOrEmpty(value.CodeAuto))
                            result += "код автономии,";

                        if (string.IsNullOrEmpty(value.CodeArea))
                            result += "код района,";

                        if (string.IsNullOrEmpty(value.CodeCity))
                            result += "код города,";
                    }
                    break;

                case FiasLevelEnum.Ctar:
                    {
                        if (string.IsNullOrEmpty(value.CodeRegion))
                            result += "код региона,";

                        if (string.IsNullOrEmpty(value.CodeAuto))
                            result += "код автономии,";

                        if (string.IsNullOrEmpty(value.CodeArea))
                            result += "код района,";

                        if (string.IsNullOrEmpty(value.CodeCity))
                            result += "код города,";

                        if (string.IsNullOrEmpty(value.CodeCtar))
                            result += "код внутригородской территории,";

                    }
                    break;

                case FiasLevelEnum.Place:
                    {
                        if (string.IsNullOrEmpty(value.CodeRegion))
                            result += "код региона,";

                        if (string.IsNullOrEmpty(value.CodeAuto))
                            result += "код автономии,";

                        if (string.IsNullOrEmpty(value.CodeArea))
                            result += "код района,";

                        if (string.IsNullOrEmpty(value.CodeCity))
                            result += "код города,";

                        if (string.IsNullOrEmpty(value.CodeCtar))
                            result += "код внутригородской территории,";

                        if (string.IsNullOrEmpty(value.CodePlace))
                            result += "код населенного пункта,";
                    }
                    break;

                case FiasLevelEnum.Street:
                    {
                        if (string.IsNullOrEmpty(value.CodeRegion))
                            result += "код региона,";

                        if (string.IsNullOrEmpty(value.CodeAuto))
                            result += "код автономии,";

                        if (string.IsNullOrEmpty(value.CodeArea))
                            result += "код района,";

                        if (string.IsNullOrEmpty(value.CodeCity))
                            result += "код города,";

                        if (string.IsNullOrEmpty(value.CodeCtar))
                            result += "код внутригородской территории,";

                        if (string.IsNullOrEmpty(value.CodePlace))
                            result += "код населенного пункта,";

                        if (string.IsNullOrEmpty(value.CodeStreet))
                            result += "код улицы,";
                    }
                    break;

                case FiasLevelEnum.Extr:
                    {

                        if (string.IsNullOrEmpty(value.CodeRegion))
                            result += "код региона,";

                        if (string.IsNullOrEmpty(value.CodeAuto))
                            result += "код автономии,";

                        if (string.IsNullOrEmpty(value.CodeArea))
                            result += "код района,";

                        if (string.IsNullOrEmpty(value.CodeCity))
                            result += "код города,";

                        if (string.IsNullOrEmpty(value.CodeCtar))
                            result += "код внутригородской территории,";

                        if (string.IsNullOrEmpty(value.CodePlace))
                            result += "код населенного пункта,";

                        if (string.IsNullOrEmpty(value.CodeStreet))
                            result += "код улицы,";

                        if (string.IsNullOrEmpty(value.CodeExtr))
                            result += "код доп. территории,";
                    }
                    break;

                case FiasLevelEnum.Sext:
                    {
                        if (string.IsNullOrEmpty(value.CodeRegion))
                            result += "код региона,";

                        if (string.IsNullOrEmpty(value.CodeAuto))
                            result += "код автономии,";

                        if (string.IsNullOrEmpty(value.CodeArea))
                            result += "код района,";

                        if (string.IsNullOrEmpty(value.CodeCity))
                            result += "код города,";

                        if (string.IsNullOrEmpty(value.CodeCtar))
                            result += "код внутригородской территории,";

                        if (string.IsNullOrEmpty(value.CodePlace))
                            result += "код населенного пункта,";

                        if (string.IsNullOrEmpty(value.CodeStreet))
                            result += "код улицы,";

                        if (string.IsNullOrEmpty(value.CodeExtr))
                            result += "код доп. территории,";

                        if (string.IsNullOrEmpty(value.CodeSext))
                            result += "код подчиненной доп. территории объекта,";
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Метод получения кода по уровню
        /// </summary>
        public static string GetCode(Fias value)
        {
            var result = "";

            switch (value.AOLevel)
            {
                case FiasLevelEnum.Region:
                    {
                        result += value.CodeRegion;
                    }
                    break;

                case FiasLevelEnum.AutonomusRegion:
                    {
                        result += value.CodeRegion;
                        result += value.CodeAuto;
                    }
                    break;

                case FiasLevelEnum.Raion:
                    {
                        result += value.CodeRegion;
                        result += value.CodeAuto;
                        result += value.CodeArea;
                    }
                    break;

                case FiasLevelEnum.City:
                    {
                        result += value.CodeRegion;
                        result += value.CodeAuto;
                        result += value.CodeArea;
                        result += value.CodeCity;
                    }
                    break;

                case FiasLevelEnum.Ctar:
                    {
                        result += value.CodeRegion;
                        result += value.CodeAuto;
                        result += value.CodeArea;
                        result += value.CodeCity;
                        result += value.CodeCtar;
                    }
                    break;

                case FiasLevelEnum.Place:
                    {
                        result += value.CodeRegion;
                        result += value.CodeAuto;
                        result += value.CodeArea;
                        result += value.CodeCity;
                        result += value.CodeCtar;
                        result += value.CodePlace;
                    }
                    break;

                case FiasLevelEnum.Street:
                    {
                        result += value.CodeRegion;
                        result += value.CodeAuto;
                        result += value.CodeArea;
                        result += value.CodeCity;
                        result += value.CodeCtar;
                        result += value.CodePlace;
                        result += value.CodeStreet;
                    }
                    break;

                case FiasLevelEnum.Extr:
                    {
                        result += value.CodeRegion;
                        result += value.CodeAuto;
                        result += value.CodeArea;
                        result += value.CodeCity;
                        result += value.CodeCtar;
                        result += value.CodePlace;
                        result += value.CodeStreet;
                        result += value.CodeExtr;
                    }
                    break;

                case FiasLevelEnum.Sext:
                    {
                        result += value.CodeRegion;
                        result += value.CodeAuto;
                        result += value.CodeArea;
                        result += value.CodeCity;
                        result += value.CodeCtar;
                        result += value.CodePlace;
                        result += value.CodeStreet;
                        result += value.CodeExtr;
                        result += value.CodeSext;
                    }
                    break;
            }

            return result;
        }
    }
}