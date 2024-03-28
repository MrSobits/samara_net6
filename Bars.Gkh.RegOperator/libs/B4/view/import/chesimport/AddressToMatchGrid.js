Ext.define('B4.view.import.chesimport.AddressToMatchGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update'
    ],

    alias: 'widget.chesaddresstomatchgrid',
    columnLines: true,
    title: 'Адреса из файла',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.import.chesimport.ChesNotMatchAddress');

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExternalAddress',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    hidden: true,
                    dataIndex: 'HouseGuid',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-application-double',
                                    text: 'Сопоставить',
                                    menu: {
                                        items: [
                                            {
                                                text: 'Автоматически',
                                                action: 'automatch'
                                            },
                                            {
                                                text: 'Вручную',
                                                action: 'manualmatch'
                                            }
                                        ]
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
