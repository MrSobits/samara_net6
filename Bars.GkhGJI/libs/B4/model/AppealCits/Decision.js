Ext.define('B4.model.appealcits.Decision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'IssuedBy', defaultValue: null },
        { name: 'ConsideringBy', defaultValue: null },
        { name: 'AppealDate' },
        { name: 'SignedBy', defaultValue: null },
        { name: 'TypeDecisionAnswer', defaultValue: 0 },
        { name: 'AppealNumber' },
        { name: 'DocumentNumber' },
        { name: 'TypeAppelantPresence', defaultValue: 0 },
        { name: 'RepresentativeFio' },
        { name: 'ApellantPlaceWork' },
        { name: 'DocumentDate' },
        { name: 'DocumentName' },
        { name: 'Established', defaultValue: null },
        { name: 'Decided', defaultValue: null },
        { name: 'Resolution', defaultValue: null },      
        { name: 'Apellant' },
        { name: 'ApellantPosition' },
    ]
});