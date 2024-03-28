Ext.define('B4.view.actcheck.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
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

    alias: 'widget.actCheckViolationGrid',
    title: 'Нарушения',
    store: 'actcheck.Violation',
    itemId: 'actCheckViolationGrid',
    border: true,
    selectionSavingBuffer: 10,
    viewConfig: {
        autoFill: true
    },
    clicksToEdit: 1,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGjiPin',
                    text: 'Номер ПиН',
                    width: 150
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGjiName',
                    flex: 1,
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
                    xtype: 'gridcolumn',
                    fieldLabel: 'Наименование нормативно-правового документа',
                    editor:
                    {
                        xtype: 'b4selectfield',
                        name: 'NormativeDoc',
                    
                        anchor: '100%',
                        store: 'B4.store.dict.NormativeDoc',
                        allowBlank: false,
                        editable: false,
                        columns: [
                            { text: 'Наименование', dataIndex: 'Name', flex: 1 },
                            { text: 'Код', dataIndex: 'Code', flex: 1 }
                        ]
                    }
                },
                {
                    xtype: 'gridcolumn',
                    fieldLabel: 'Пункт нормативно-правового документа',
                    editor:
                    {
                        xtype: 'b4selectfield',
                        name: 'NormativeDocItem',

                        anchor: '100%',
                        store: 'B4.store.dict.NormativeDocItem',
                        allowBlank: false,
                        editable: false,
                        columns: [
                            { text: 'Номер', dataIndex: 'Number', flex: 1 },
                            { text: 'Текст', dataIndex: 'Text', flex: 1 }
                        ]
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
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'actViolationGridAddButton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});