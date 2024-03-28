Ext.define('B4.view.regop.cproc.types.ComPrBaseWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.comprbasewindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    modal: true,
    closeAction: 'destroy',
    saveBtnClickListeners: null,
    width: 500,
    height: 350,
    layout: 'fit',
    
    initComponent: function () {
        var me = this;
        me.callParent(arguments);
    }
});