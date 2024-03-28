Ext.define('B4.model.passport.Part', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Parent' },
        { name: 'OrderNum' },
        { name: 'PassportStruct' },
        { name: 'Uo', type: 'boolean', defaultValue: false },
        { name: 'Pku', type: 'boolean', defaultValue: false },
        { name: 'Pr', type: 'boolean', defaultValue: false },
        { name: 'IntegrationCode' },
        { name: 'Attributes', defaultValue: [{ Name: 'Атрибуты' }] }
    ],
    proxy: {
        type: 'memory'
    }
});