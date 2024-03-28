Ext.define('B4.model.workwintercondition.Information', {

    extend: 'B4.base.Model',

    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkWinterCondition'
    },

    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'RowNumber' },
        { name: 'Measure' },
        { name: 'Okei' },
        { name: 'Total' },
        { name: 'PreparationTask' },
        { name: 'PreparedForWork' },
        { name: 'FinishedWorks' },
        { name: 'Percent' }
    ]
});