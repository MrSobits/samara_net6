Ext.define('B4.store.appealcits.AppealCitsBaseStatement', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Id', 'NumberGji', 'Number'],
    proxy: {
        autoLoad: false,
        type: 'ajax',
        reader: {
            type: 'json',
            root: 'data'
        }
    }
});