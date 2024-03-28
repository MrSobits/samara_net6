Ext.define('B4.model.cmnestateobj.StructuralElementGroup', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StructuralElementGroup'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CommonEstateObject', defaultValue: null },
        { name: 'Name' },
        { name: 'Required' },
        { name: 'UseInCalc', defaultValue: true },
        { name: 'Formula' },
        { name: 'FormulaName' },
        { name: 'FormulaDescription' },
        { name: 'FormulaParams' }
    ]
});