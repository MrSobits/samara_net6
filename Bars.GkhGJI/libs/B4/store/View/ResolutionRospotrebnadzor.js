Ext.define('B4.store.view.ResolutionRospotrebnadzor', {
    requires: ['B4.model.ResolutionRospotrebnadzor'],
    extend: 'B4.base.Store',
    autoLoad: false,
    model: 'B4.model.ResolutionRospotrebnadzor',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionRospotrebnadzor',
        listAction: 'ListView'
    }
});