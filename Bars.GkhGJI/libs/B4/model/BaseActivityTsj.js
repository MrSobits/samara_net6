Ext.define('B4.model.BaseActivityTsj', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseActivityTsj'
    },
    fields: [
        { name: 'ActivityTsj', defaultValue: null },
        { name: 'DisposalDocumentNumber' },
        { name: 'DisposalDocumentDate' },
        { name: 'RealityObjects' },
        { name: 'DisposalTypeCheck' }
    ]
});