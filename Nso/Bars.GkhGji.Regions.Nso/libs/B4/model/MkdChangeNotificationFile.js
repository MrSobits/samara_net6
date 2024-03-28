Ext.define('B4.model.MkdChangeNotificationFile', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MkdChangeNotificationFile'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Number' },
		{ name: 'Date' },
		{ name: 'Desc' },
		{ name: 'MkdChangeNotification' },
		{ name: 'File' }
    ]
});