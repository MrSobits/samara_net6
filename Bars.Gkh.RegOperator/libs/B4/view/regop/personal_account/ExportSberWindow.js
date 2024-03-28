Ext.define('B4.view.regop.personal_account.ExportSberWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.persaccexportsberwindow',
    
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.regop.personal_account.PaymentAgentGrid'
    ],
    
    modal: true,
    width: 800,
    height: 500,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closeAction: 'destroy',
    closable: false,
    
    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            items: [
                {
                    xtype: 'persaccpaymentagentgrid',
                    flex: 1,
                    border: 0
                }
            ]
        });

        me.callParent(arguments);
    }
});
