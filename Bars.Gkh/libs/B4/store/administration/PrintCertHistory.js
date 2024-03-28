Ext.define('B4.store.administration.PrintCertHistory', {
    extend: 'B4.base.Store',
	requires: ['B4.model.administration.PrintCertHistory'],
    autoLoad: false,
	storeId: 'printCertHistoryStore',
	model: 'B4.model.administration.PrintCertHistory'
});