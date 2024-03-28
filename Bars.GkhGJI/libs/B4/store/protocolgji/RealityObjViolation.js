Ext.define('B4.store.protocolgji.RealityObjViolation', {
    extend: 'B4.base.Store',
    requires: ['B4.model.protocolgji.Violation'],
    autoLoad:false,
    storeId: 'protocolRealityObjViolationStore',
    model: 'B4.model.protocolgji.Violation',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolViolation',
        listAction: 'ListRealityObject'
    }
});