Ext.define('B4.model.dict.ReportingPeriod', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ReportingPeriodDict'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'State' },
        { name: 'PeriodDi' },
        { name: 'Synchronizing' },
        { name: 'Is_988' }
    ]
});