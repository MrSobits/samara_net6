Ext.define('B4.store.version.RealityObjectforAdd', {
    extend: 'B4.base.Store',
    requires: ['B4.model.version.RealityObjectforAdd'],
    autoLoad: false,
    model: 'B4.model.version.RealityObjectforAdd',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'GetRealityObjectforAdd',
        // timeout: 30 * 60 * 1000 // 30 минут
    }
});