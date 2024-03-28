Ext.define('B4.model.regop.ChargePeriod', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'IsClosed' },
        { name: 'Name' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'ChargePeriod'
    }
});