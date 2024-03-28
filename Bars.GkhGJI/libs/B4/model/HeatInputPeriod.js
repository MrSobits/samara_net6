Ext.define('B4.model.HeatInputPeriod', {

    extend: 'B4.base.Model',

    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'HeatInputPeriod'
    },

    fields: [
        { name: 'Id' },
        { name: 'Year' },
        { name: 'Month' },
        { name: 'Municipality' }
    ]
});