﻿Ext.define('B4.view.realityobj.changevalueshistory.Window', {
    extend: 'B4.form.Window',
    alias: 'widget.changeValuesHistoryeWindows',
        
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.mixins.window.ModalMask',
        'B4.view.realityobj.changevalueshistory.WindowsGrid'
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    layout: 'fit',
    width: 700,
    height: 550,
    resizable: true,

    title: 'Детальная информация',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'changeValuesHistoryWindowsGrid'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: {
                                            fn: function (btn) {
                                                btn.up('window').close();
                                            }
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