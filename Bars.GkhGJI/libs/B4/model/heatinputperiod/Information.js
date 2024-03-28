Ext.define('B4.model.heatinputperiod.Information', {

    extend: 'B4.base.Model',

    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'HeatInputInformation'
    },

    fields: [
        { name: 'Id' },
        { name: 'TypeHeatInputObject' },
        { name: 'Count' },
        { name: 'CentralHeating' },
        { name: 'IndividualHeating' },
        { name: 'Percent' },
        { name: 'NoHeating' }
    ]
});