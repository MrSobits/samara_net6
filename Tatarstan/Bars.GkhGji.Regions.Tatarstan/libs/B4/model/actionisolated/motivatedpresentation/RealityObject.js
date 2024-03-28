Ext.define('B4.model.actionisolated.motivatedpresentation.RealityObject', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivatedPresentationViolation',
        listAction: 'RealityObjectsListForMotivatedPresentationActionToWarningDocRule'
    },
    fields: [
        { name: 'Address' },
        { name: 'Municipality' },
        { name: 'RealityObjectId' },
    ]
});