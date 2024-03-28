Ext.define('B4.model.housingfundmonitoring.HousingFundMonitoringPeriod', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HousingFundMonitoringPeriod'
    },
    fields: [
        { name: 'Id' },
        { name: 'Year' },
        { name: 'Municipality' }
    ]
});