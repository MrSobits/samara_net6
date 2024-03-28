Ext.define('B4.model.dpkrdocument.DpkrDocumentRealityObjectForSelect', {
    extend: 'B4.model.dpkrdocument.DpkrDocumentRealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrDocumentRealityObject',
        listAction: 'GetRealityObjectsList'
    }
});