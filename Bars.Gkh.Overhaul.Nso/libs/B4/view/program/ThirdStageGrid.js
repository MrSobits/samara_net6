Ext.define('B4.view.program.ThirdStageGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.programthirdstagegrid',
    
    requires: [
        'B4.grid.feature.Summary',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        
        'B4.ux.grid.column.Edit',
        
        'B4.store.program.ThirdStage',
        'B4.form.ComboBox'
    ],

    //title: 'Долгосрочная программа',
    //closable: true,

    features: [{
        ftype: 'b4_summary'
    }],
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.program.ThirdStage', { autoLoad: true });

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    origScope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 100,
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    },
                    renderer: function (val) {
                        if (typeof val === "string") {
                            return val;
                        }
                        else if (typeof val === "object") {
                            return val && val.Address ? val.Address : '';
                        }
                        return '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjects',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Стоимость (руб)',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 150,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
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
                            columns: 7,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4addbutton',
                                    action: 'makelongprogram',
                                    text: 'Расчет ДПКР'
                                },
                                {
                                    xtype: 'button',
                                    cmd: 'order', 
                                    iconCls: 'icon-build',
                                    text: 'Очередность'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    action: 'Export'
                                },
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Сохранить изменения'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Удалить программу',
                                    iconCls: 'icon-delete',
                                    action: 'DeleteDpkr'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сохранить версию программы',
                                    action: 'NewVersion'
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