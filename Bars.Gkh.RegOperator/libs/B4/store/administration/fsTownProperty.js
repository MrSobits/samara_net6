Ext.define('B4.store.administration.fsTownProperty', {
    extend: 'B4.base.Store',

    proxy: {
        type: 'b4proxy',
        controllerName: 'FsImportSetup',
        listAction: 'GetObjectMeta'
    },

    fields: [
        { name: 'PropertyName' },
        { name: 'DisplayName' }
    ]
});