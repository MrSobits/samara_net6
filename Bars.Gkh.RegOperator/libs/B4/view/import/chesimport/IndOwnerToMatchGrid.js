Ext.define('B4.view.import.chesimport.IndOwnerToMatchGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update'
    ],

    alias: 'widget.chesimportindownertomatchgrid',
    columnLines: true,
    title: 'Абоненты из файла',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.import.chesimport.ChesNotMatchIndAccountOwner');

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
                    flex: 1,
                    text: 'ФИО',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'BirthDate',
                    width: 80,
                    text: 'Дата рождения',
                    filter: { xtype: 'datefield' }
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
                                                action: 'automatch',
                                                disabled: true
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
