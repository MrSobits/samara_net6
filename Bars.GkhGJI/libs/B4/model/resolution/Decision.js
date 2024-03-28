Ext.define('B4.model.resolution.Decision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Resolution', defaultValue: null },
        { name: 'IssuedBy', defaultValue: null },
        { name: 'ConsideringBy', defaultValue: null },
        { name: 'SignedBy', defaultValue: null },
        { name: 'AppealDate' },
        { name: 'TypeDecisionAnswer', defaultValue: 0 },
        { name: 'AppealNumber' },

        { name: 'TypeAppelantPresence', defaultValue: 0 },
        { name: 'RepresentativeFio' },

        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'DocumentName' },
        { name: 'Established', defaultValue: null },
        { name: 'Decided', defaultValue: null },  
        { name: 'Apellant' },
        { name: 'ApellantPosition' },
        { name: 'ApellantPlaceWork' }
    ]
});