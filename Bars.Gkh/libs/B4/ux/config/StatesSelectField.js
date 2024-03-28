Ext.define('B4.ux.config.StatesSelectField', {
    extend: 'B4.ux.config.TriggerMultiSelectEditor',
    alias: 'widget.statesselectfield',

    //#region override

    storePath: 'B4.store.StateForSelect',

    //#endregion

    storeBeforeLoad: function(store, opts) {
        opts.params.typeId = 'gkh_regop_personal_account';
    }
});