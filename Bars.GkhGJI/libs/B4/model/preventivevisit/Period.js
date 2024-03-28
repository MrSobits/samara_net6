Ext.define('B4.model.preventivevisit.Period', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveVisitPeriod'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PreventiveVisit', defaultValue: null },
        { name: 'DateCheck' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'TimeStart' },
        { name: 'TimeEnd' }
    ]
});