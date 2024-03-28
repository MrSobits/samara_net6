Ext.define('B4.model.EconFeasibilityCalcWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EconFeasibilitiWork'
    },
    fields: [
        { name: 'Id' },
        { name: 'Year' },       
        { name: 'Sum' },
        { name: 'CommonEstateObjects' }
    ]
});