Ext.define('B4.model.delegacy.Delegacy', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Delegacy'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'OperatorIS', defaultValue: null },
        { name: 'InformationProvider', defaultValue: null },
        { name: 'StartDate' },
        { name: 'EndDate' }
    ]
});
