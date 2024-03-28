Ext.define('B4.store.RequestStatePerson', {
    extend: 'B4.base.Store',
	requires: ['B4.model.RequestStatePerson'],
    autoLoad: false,
	storeId: 'requestStatePersonStore',
	model: 'B4.model.RequestStatePerson'
});