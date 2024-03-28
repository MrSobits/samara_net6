/**
 * стор контрагентов с исключенными дочерними контрагентами
 * чтобы не было циклических ссылок
 */
Ext.define('B4.store.contragent.ContragentExceptChildren', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Contragent'],
    autoLoad: false,
    storeId: 'contragentForSelectStore',
    model: 'B4.model.Contragent',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Contragent',
        listAction: 'ListExceptChildren'
    }
});