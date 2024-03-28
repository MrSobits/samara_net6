Ext.define('B4.view.import.chesimport.LegalOwnerToMatchGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update'
    ],

    alias: 'widget.chesimportlegalownertomatchgrid',
    columnLines: true,
    title: 'Абоненты из файла',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.import.chesimport.ChesNotMatchLegalAccountOwner');

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountNumber',
                    width: 80,
                    text: 'ЛС',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 3,
                    text: 'Наименование ЮЛ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inn',
                    width: 80,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Kpp',
                    width: 80,
                    text: 'КПП',
                    filter: { xtype: 'textfield' }
                },
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
