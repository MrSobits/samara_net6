Ext.define('B4.view.GeneralStateHistory.Window', {
    extend: 'B4.form.Window',
    alias: 'widget.generalstatehistorywindow',

    modal: true,
    layout: 'fit',
    width: 600,
    height: 500,
    bodyPadding: 5,

    title: 'История изменения',
    trackResetOnLoad: true,

    entityId: 0,
    stateCode: null,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.GeneralStateHistory');

        store.on('beforeload', this.onHistoryBeforeLoad, this);
   
        Ext.applyIf(me, {
            items: [
                 Ext.create('B4.view.GeneralStateHistory.Grid', {
                     bodyStyle: 'backrgound-color:transparent;',
                     store: store
                 })
            ]
        });

        me.callParent(arguments);
    },

    onHistoryBeforeLoad: function (store, operation) {
        operation.params = operation.params || {};

        if (this.entityId > 0 && this.stateCode) {
            operation.params.entityId = this.entityId;
            operation.params.stateCode = this.stateCode;
        }
    }
});