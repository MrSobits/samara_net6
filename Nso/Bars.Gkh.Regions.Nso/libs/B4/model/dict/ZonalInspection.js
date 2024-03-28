Ext.define('B4.model.dict.ZonalInspection', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ZonalInspection'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'ZoneName' },
        { name: 'BlankName' },
        { name: 'ShortName' },
        { name: 'Address' },
        { name: 'NameSecond' },
        { name: 'ZoneNameSecond' },
        { name: 'BlankNameSecond' },
        { name: 'ShortNameSecond' },
        { name: 'AddressSecond' },
        { name: 'Phone' },
        { name: 'Email' },
        { name: 'Okato' },
        { name: 'NameGenetive' },
        { name: 'NameDative' },
        { name: 'NameAccusative' },
        { name: 'NameAblative' },
        { name: 'NamePrepositional' },
        { name: 'ShortNameGenetive' },
        { name: 'ShortNameDative' },
        { name: 'ShortNameAccusative' },
        { name: 'ShortNameAblative' },
        { name: 'ShortNamePrepositional' },
        { name: 'IndexOfGji' },
        { name: 'AppealCode' },
        { name: 'Oktmo' },
        { name: 'Locality' },
        { name: 'DepartmentCode' }
    ]
});