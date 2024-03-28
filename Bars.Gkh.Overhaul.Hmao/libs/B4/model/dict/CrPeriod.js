Ext.define('B4.model.dict.CrPeriod', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CrPeriod'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'YearStart' },
        { name: 'YearEnd' }
    ]
});