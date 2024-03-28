Ext.define('B4.view.claimwork.IndividualPrintWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.individualprintwin',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    modal: true,
    width: 600,
    height: 550,
    title: 'Выбор адреса',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.AccountDetail'),
            selModel = Ext.create('Ext.selection.CheckboxModel',
                {
                    mode: 'MULTI'
                });

        Ext.applyIf(me,
            {
                items: [
                    {
                        xtype: 'grid',
                        name: 'PrintAccountGrid',
                        columnLines: true,
                        flex: 1,
                        store: store,
                        selModel: selModel,
                        columns: [
                            {
                                dataIndex: 'OwnerName',
                                flex: 1,
                                text: 'Абонент',
                                filter: {
                                    xtype: 'textfield'
                                }
                            },
                            {
                                dataIndex: 'RoomAddress',
                                flex: 1,
                                text: 'Адрес помещения',
                                filter: {
                                    xtype: 'textfield'
                                }
                            },
                            {
                                dataIndex: 'PersonalAccountNum',
                                flex: 1,
                                text: 'Номер ЛС',
                                filter: {
                                    xtype: 'textfield'
                                }
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'toolbar',
                                items: [
                                    {
                                        xtype: 'buttongroup',
                                        items: [
                                            {
                                                xtype: 'button',
                                                action: 'print',
                                                iconCls: 'icon-printer',
                                                text: 'Выбрать'
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'tbfill'
                                    },
                                    {
                                        xtype: 'buttongroup',
                                        items: [
                                            {
                                                xtype: 'b4closebutton',
                                                listeners: {
                                                    click: function (btn) {
                                                        btn.up('individualprintwin').close();
                                                    }
                                                }
                                            }
                                        ]
                                    }
                                ]
                            }
                        ],
                        plugins: [
                            Ext.create('B4.ux.grid.plugin.HeaderFilters')
                        ],
                        viewConfig: {
                            loadMask: true
                        }
                    },
                    {
                        xtype: 'b4pagingtoolbar',
                        displayInfo: true,
                        store: store,
                        dock: 'bottom'
                    }
                ]
            });

        me.callParent(arguments);
    }
});