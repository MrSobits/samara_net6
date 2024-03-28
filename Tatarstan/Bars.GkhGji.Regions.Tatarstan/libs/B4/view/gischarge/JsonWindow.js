Ext.define('B4.view.gischarge.JsonWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close'
    ],

    alias: 'widget.gischargejsonwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {type: 'vbox', align: 'stretch'},
    width: 500,
    height: 400,
    bodyPadding: 5,
    
    title: 'Проверка по обращению граждан',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textarea',
                    name: 'JsonObject',
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