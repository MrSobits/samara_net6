Ext.define('B4.model.dict.EnergyEfficiencyClasses', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EnergyEfficiencyClasses'
    },
    fields: [
        { name: 'Id' },
        { name: 'Code' },
        { name: 'Designation' },
        { name: 'Name' },
        { name: 'DeviationValue' }
    ]
});