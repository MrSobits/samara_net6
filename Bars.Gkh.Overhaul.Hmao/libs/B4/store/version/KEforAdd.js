Ext.define('B4.store.version.KEforAdd', {
    extend: 'B4.base.Store',
    requires: ['B4.model.version.KEforAdd'],
    autoLoad: false,
    model: 'B4.model.version.KEforAdd',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramVersion',
        listAction: 'GetKEforAdd',
       // timeout: 30 * 60 * 1000 // 30 минут
    }
});