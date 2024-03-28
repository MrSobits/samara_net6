Ext.define('B4.model.heatinputperiod.Boiler', {

    extend: 'B4.base.Model',

    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'HeatInputInformation',
        listAction: 'GetBoilerInfo'
    },

    fields: [
        { name: 'Title' },
        { name: 'Count' },
        { name: 'Started' },
        { name: 'Percent' },
        { name: 'NotStarted' }
    ]
});