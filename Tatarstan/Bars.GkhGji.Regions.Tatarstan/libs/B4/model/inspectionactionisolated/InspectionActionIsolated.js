Ext.define('B4.model.inspectionactionisolated.InspectionActionIsolated', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionActionIsolated'
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
        { name: 'ActionIsolated' },
        { name: 'PersonName' },
        { name: 'Inn' },
        { name: 'ContragentId' }
    ]
});