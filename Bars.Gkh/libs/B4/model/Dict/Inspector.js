Ext.define('B4.model.dict.Inspector', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'inspector'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'CodeEdo' },
        { name: 'Position' },
        { name: 'InspectorPosition'},
        { name: 'Fio' },
        { name: 'ShortFio' },
        { name: 'Phone' },
        { name: 'Email' },
        { name: 'Description' },
        { name: 'IsHead', defaultValue: false },
        { name: 'Active', defaultValue: true },
        { name: 'FioGenitive' },
        { name: 'ZonalInspection' },
        { name: 'FioDative' },
        { name: 'FioAccusative' },
        { name: 'FioAblative' },
        { name: 'GisGkhGuid' },
        { name: 'ERKNMTitleSignerGuid' },
        { name: 'ERKNMPositionGuid' },
        { name: 'FioPrepositional' },
        { name: 'PositionGenitive' },
        { name: 'PositionDative' },
        { name: 'PositionAccusative' },
        { name: 'PositionAblative' },
        { name: 'PositionPrepositional' },
        { name: 'IsActive'}
    ]
});