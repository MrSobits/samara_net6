Ext.define('B4.model.DPKRActualCriterias', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DPKRActualCriterias'
    },
    fields: [
        { name: 'Id'},
        { name: 'Text'},
        { name: 'Operator'},
        { name: 'DateStart'},
        { name: 'DateEnd'},
        { name: 'Status'},
        { name: 'TypeHouse'},
        { name: 'ConditionHouse'},
        { name: 'IsNumberApartments'},
        { name: 'NumberApartmentsCondition'},
        { name: 'NumberApartments'},
        { name: 'IsYearRepair'},
        { name: 'YearRepairCondition'},
        { name: 'YearRepair'},
        { name: 'CheckRepairAdvisable'},
        { name: 'CheckInvolvedCr'},
        { name: 'IsStructuralElementCount'},
        { name: 'StructuralElementCountCondition'},
        { name: 'StructuralElementCount' },
        { name: 'SEStatus' },
    ]
});