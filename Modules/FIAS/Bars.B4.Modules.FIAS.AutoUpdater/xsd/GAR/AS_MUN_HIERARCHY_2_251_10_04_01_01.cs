﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Xml.Serialization;

using Bars.B4.Modules.FIAS.AutoUpdater.Utils;

// 
// Этот исходный код был создан с помощью xsd, версия=4.8.3928.0.
// 

/// <summary>
/// Класс для десериализации сущности "Муниципальная иерархия"
/// </summary>
[FiasEntityName("as_mun_hierarchy", "ITEMS")]
[XmlTypeAttribute("ITEM")]
public partial class MunicipalityHierarchy {
    
    /// <summary>
    /// Id адресного объекта
    /// </summary>
    [XmlAttribute()]
    public long OBJECTID { get; set; }
    
    /// <summary>
    /// Id родителя адресного объекта
    /// </summary>
    [XmlAttribute()]
    public long PARENTOBJID { get; set; }

    /// <summary>
    /// ОКТМО
    /// </summary>
    [XmlAttribute()]
    public string OKTMO { get; set; }
    
    /// <summary>
    /// Дата окончания действия
    /// </summary>
    [XmlAttribute()]
    public System.DateTime ENDDATE { get; set; }
    
    /// <summary>
    /// Признак активности
    /// </summary>
    [XmlAttribute()]
    public HIERARCHYISACTIVE ISACTIVE { get; set; }
}