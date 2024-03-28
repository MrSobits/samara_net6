Ext.define('B4.model.regop.personal_account.ImportStatus', {
    extend: 'B4.base.Model',

    fields: [
		{ name: 'Id' },
		{ name: 'FileName' },
		{ name: 'CreateDate' },
		{ name: 'ProcState' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'ImportQueueItem'
    }
});
