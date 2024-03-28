Ext.define('B4.store.appealcits.AdmonitionForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.appealcits.Admonition'],
    autoLoad: false,
    storeId: 'admonitionForSelect',
    model: 'B4.model.appealcits.Admonition',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdmonitionOperations',
        listAction: 'ListAdmonitionForSelect'
    }
});