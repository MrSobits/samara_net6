Ext.define('B4.view.dict.inspectionreasonerknm.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.enums.ERKNMDocumentType',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Основание КНМ(ПМ)',
    store: 'dict.InspectionReasonERKNM',
    alias: 'widget.inspectionreasonerknmgrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 50
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ERKNMId',
                    flex: 1,
                    text: 'ИД',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 5
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1500
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ERKNMDocumentType',
                    flex: 0.5,
                    text: 'Документ',
                    renderer: function (val) {
                        return B4.enums.ERKNMDocumentType.displayRenderer(val);
                    },
                    editor: {
                        xtype: 'b4combobox',
                        valueField: 'Value',
                        displayField: 'Display',
                        items: B4.enums.ERKNMDocumentType.getItems(),
                        editable: false
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
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});