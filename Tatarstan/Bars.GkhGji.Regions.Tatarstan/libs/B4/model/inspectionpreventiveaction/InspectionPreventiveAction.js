Ext.define('B4.model.inspectionpreventiveaction.InspectionPreventiveAction', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionPreventiveAction'
    },
    fields: [
        { name: 'State', defaultValue: null },
        { name: 'Municipality' },
        { name: 'TypeJurPerson' },
        { name: 'JurPerson' },
        { name: 'Address'},
        { name: 'Inspectors' },
        { name: 'TypeForm' },
        { name: 'TypeObject' },
        { name: 'PreventiveAction' }
    ]
});