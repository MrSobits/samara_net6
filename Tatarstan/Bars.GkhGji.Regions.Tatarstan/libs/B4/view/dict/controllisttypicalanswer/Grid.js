Ext.define('B4.view.dict.ControlListTypicalAnswer.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Типовые ответы на вопросы проверочного листа',
    alias: 'widget.controllisttypicalanswergrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.ControlListTypicalAnswer');
        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TorId',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Уникальный идентификатор',
                    editor: {
                        xtype: 'textfield',
                        regex: new RegExp('^[0-9A-Fa-f]{8}-([0-9A-Fa-f]{4}-){3}[0-9A-Fa-f]{12}$'),
                        regexText: 'Введен некорректный GUID',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Answer',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Ответ на вопрос проверочного листа',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    dock: 'bottom',
                    displayInfo: true,
                    store: store
                }
            ]
        });

        me.callParent(arguments);
    }
});