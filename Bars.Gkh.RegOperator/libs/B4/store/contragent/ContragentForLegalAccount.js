Ext.define('B4.store.contragent.ContragentForLegalAccount', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Contragent'],
    autoLoad: false,
    storeId: 'contragentStore',
    model: 'B4.model.Contragent',
    load: function (options) {
        options = options || {};
        options.params = options.params || { };

        if (options.params.exceptLegalAccountOwners === undefined) {
            //Исключить из результатов контрагентов, которые уже зарегистрированы в реестре абонентов.
            options.params.exceptLegalAccountOwners = true;
        }

        return this.callParent([options]);
    }
});
