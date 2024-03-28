Ext.define('B4.view.actisolated.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.actisolatedviolationgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.SelectField',
        'B4.store.dict.NormativeDoc',
        'B4.store.dict.NormativeDocItem',
    ],

    title: 'Нарушения',
    border: true,
    selectionSavingBuffer: 10,
    viewConfig: {
        autoFill: true
    },
    clicksToEdit: 1,
    overflowX: 'auto',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.actisolated.Violation');

        me.relayEvents(store, ['beforeload'], 'store.');
        me.relayEvents(store, ['load'], 'store.');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EventResult',
                    text: 'Результаты проведения мероприятия',
                    width: 200,
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGjiPin',
                    text: 'Номер ПиН',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGjiName',
                    width: 200,
                    text: 'Текст нарушения'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePlanRemoval',
                    text: 'Срок устранения',
                    format: 'd.m.Y',
                    width: 100,
                    editor: 'datefield'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFactRemoval',
                    text: 'Дата факт. устранения',
                    format: 'd.m.Y',
                    width: 150
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateCancel',
                    text: 'Дата отмены',
                    format: 'd.m.Y',
                    width: 120
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
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'actViolationGridAddButton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'realObjViolationSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }
                            ]
                        }
                    ]
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