Ext.define('B4.model.fuelinfo.FuelInfoPeriod', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FuelInfoPeriod'
    },
    fields: [
        { name: 'Id' },
        { name: 'Year' },
        { name: 'Month' },
        { name: 'Municipality' }
    ]
});