Ext.define('B4.model.appealcits.Source', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsSource'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'RevenueSource', defaultValue: null },
        { name: 'RevenueForm', defaultValue: null },
        { name: 'RevenueSourceNumber' },
        { name: 'SSTUDate' },
        { name: 'RevenueDate' }
    ]
});