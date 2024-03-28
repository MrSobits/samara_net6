//стор акта устранения нарушения в акте проверки
Ext.define('B4.store.actcheck.ActRemoval', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ActRemoval'],
    autoLoad: false,
    storeId: 'actCheckActRemovalStore',
    model: 'B4.model.ActRemoval'
});