Ext.define('B4.model.dict.KindStatementEdoInteg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'KindStatementCompareEdo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CompareId', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'CodeEdo' },
        { name: 'KindStatement', defaultValue: null }
    ]
});