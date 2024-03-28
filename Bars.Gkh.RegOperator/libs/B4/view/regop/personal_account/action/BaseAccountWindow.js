Ext.define('B4.view.regop.personal_account.action.BaseAccountWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.baseaccountwin',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    modal: true,
    closeAction: 'destroy',
    saveBtnClickListeners: null,
    bodyPadding: 10,

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'b4savebutton'
                        }, '->', {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function(btn) {
                                    btn.up('window').close();
                                }
                            }
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});