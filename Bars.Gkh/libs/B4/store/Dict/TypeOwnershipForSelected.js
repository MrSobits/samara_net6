/**
 * Created by dmitriy.ivanov on 06.08.2014.
 */
Ext.define('B4.store.dict.TypeOwnershipForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.TypeOwnership'],
    autoLoad: false,
    storeId: 'typeOwnershipStoreForSelected',
    fields: [
        {name: 'Id' },
        {name: 'Name'}
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeOwnership'
    }
});