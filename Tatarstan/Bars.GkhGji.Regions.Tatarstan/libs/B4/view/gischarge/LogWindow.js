Ext.define('B4.view.gischarge.LogWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close'
    ],

    alias: 'widget.gischargelogwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {type: 'vbox', align: 'stretch'},
    width: 500,
    height: 400,
    bodyPadding: 5,
    
    title: 'Лог отправки',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textarea',
                    name: 'Log',
                    flex: 1,
                    readOnly: true
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function() {
                                            me.close();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});