/**
 * Created by dmitriy.ivanov on 06.08.2014.
 */
Ext.define('B4.store.dict.TypeOwnershipForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.TypeOwnership'],
    autoLoad: false,
    storeId: 'typeOwnershipStoreForSelect',
    fields: [
        {name: 'Id'},
        {name: 'Name'}
    ],
    proxy:{
        type: 'b4proxy',
        controllerName: 'TypeOwnership'
    }
});