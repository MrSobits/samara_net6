Ext.define('B4.view.transferrf.RecObjGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',
        
        'B4.ux.grid.feature.GroupingSummaryTotal',

        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.view.Control.GkhDecimalField'
    ],
    alias: 'widget.transferrfrecobjgrid',
    title: 'Реестр жилых домов включенных в договор',
    store: 'transferrf.RecObj',
    itemId: 'transferRfRecObjGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    },
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObjectName',
                    flex: 3,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GkhCode',
                    flex: 1,
                    text: 'Код МЖФ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Sum',
                    text: 'Перечисленная сумма',
                    editor: { xtype: 'gkhdecimalfield' },
                    flex: 1,
                    filter:
                    {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
        }
                }
            ],
            selType: 'cellmodel',
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        edit: function (editor, e) {
                            if (e.rowIdx + 1 == e.grid.getStore().getTotalCount()) {
                                return true;
                            }

                            editor.startEditByPosition({ row: e.rowIdx + 1, column: e.colIdx });
                            return false;
                        }
                    }
                })
            ],
            features: [
                {
                    ftype: 'groupingsummarytotal',
                    groupHeaderTpl: '{name}'
                }
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'transferRfRecObjSaveButton',
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