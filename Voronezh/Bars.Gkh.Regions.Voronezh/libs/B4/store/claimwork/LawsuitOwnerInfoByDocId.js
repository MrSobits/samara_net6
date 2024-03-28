Ext.define('B4.store.claimwork.LawsuitOwnerInfoByDocId', {
    extend: 'B4.base.Store',
    fields: ['Id', 'Name'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'DataAreaOwnerMerger',
        listAction: 'GetListOwnerByDocId'
    }
});

