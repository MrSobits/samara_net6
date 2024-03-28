Ext.define('B4.model.cscalculation.MOCoefficient', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MOCoefficient'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'UnitMeasure' },
        { name: 'Code' },
        { name: 'Value' },
        { name: 'DateFrom' },
        { name: 'DateTo' },
        { name: 'ParentMO' },
        { name: 'CategoryCSMKD' },
        { name: 'Municipality' }
    ]
});