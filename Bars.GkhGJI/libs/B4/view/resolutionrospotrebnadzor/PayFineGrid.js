Ext.define('B4.view.resolutionrospotrebnadzor.PayFineGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeDocumentPaidGji'
    ],

    alias: 'widget.resolutionRospotrebnadzorPayFineGrid',
    title: 'Оплаты штрафов',
    store: 'resolutionrospotrebnadzor.PayFine',
    itemId: 'resolutionRospotrebnadzorPayFineGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            features: [{
                ftype: 'summary'
            }],
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeDocumentPaid',
                    flex: 1,
                    text: 'Документ оплаты',
                    editor: {
                        xtype: 'combobox', editable: false,
                        store: B4.enums.TypeDocumentPaidGji.getStore(),
                        displayField: 'Display',
                        valueField: 'Value',
                        editable: false
                    },
                    renderer: function (val) { return B4.enums.TypeDocumentPaidGji.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    flex: 1,
                    text: 'Номер',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 50
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 100,
                    editor: 'datefield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Amount',
                    flex: 1,
                    text: 'Сумма штрафа',
                    editor: {
                        xtype: 'numberfield',
                        decimalSeparator: ',',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return 'Итого: ' + val;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GisUip',
                    flex: 1,
                    text: 'УИП'
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
                                    xtype: 'button',
                                    name: 'saveButton',
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});