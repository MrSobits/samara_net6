Ext.define('B4.model.metavalueconstructor.DataMetaInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DataMetaInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Parent' },
        { name: 'Name'},
        { name: 'Code'},
        { name: 'Weight', defaultValue: null },
        { name: 'Formula' },
        { name: 'Group'},
        { name: 'Level', defaultValue: 0 },
        { name: 'DataValueType' },
        { name: 'MinLength' },
        { name: 'MaxLength' },
        { name: 'Decimals' },
        { name: 'Required', defaultValue: false },
        { name: 'DataFillerName' }
    ]
});