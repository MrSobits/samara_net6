Ext.define('B4.view.program.SubStageGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.programsubstagegrid',

    requires: [
        'B4.grid.feature.Summary',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',        
        'B4.ux.grid.column.Edit',        
        'B4.store.program.SubStage',
        'B4.form.ComboBox',
        'B4.view.Control.GkhButtonImport'
    ],

    features: [{
        ftype: 'b4_summary'
    }],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.program.SubStage', { autoLoad: false });

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
                    summaryRenderer: function() {
                        return Ext.String.format('Итого:');
                    }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Municipality',
                //    width: 160,
                //    text: 'Муниципальное образование',
                //    filter: {
                //        xtype: 'b4combobox',
                //        operand: CondExpr.operands.eq,
                //        storeAutoLoad: false,
                //        hideLabel: true,
                //        editable: false,
                //        valueField: 'Name',
                //        emptyItem: { Name: '-' },
                //        url: '/Municipality/ListWithoutPaging'
                //    }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    },
                    renderer: function(val) {
                        if (typeof val === "string") {
                            return val;
                        } else if (typeof val === "object") {
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
                    renderer: function(value) {
                        return Ext.util.Format.currency(value);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function(value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                },
                {
                    xtype: 'actioncolumn',
                    action: 'hide',
                    width: 18,
                    align: 'center',
                    icon: B4.Url.content('content/img/icons/script_delete.png'),
                    tooltip: 'Удалить',
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
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            flex: 1,
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    items: [
                                        {
                                            xtype: 'b4updatebutton'
                                        },
                                        {
                                            xtype: 'button',
                                            cmd: 'order',
                                            iconCls: 'icon-build',
                                            text: 'Очередность'
                                        },
                                        //{
                                        //    xtype: 'button',
                                        //    text: 'Удалить программу',
                                        //    iconCls: 'icon-delete',
                                        //    action: 'DeleteDpkr'
                                        //},
                                        //{
                                        //    xtype: 'button',
                                        //    text: 'Сохранить версию программы',
                                        //    action: 'NewVersion'
                                        //},
                                        //{
                                        //    xtype: 'gkhbuttonimport'
                                        //}
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex: 1,
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'component',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'label',
                                            name:'DateCalc',
                                            width: 170,
                                            padding: '5 0 0 0',
                                            text: ''
                                        }
                                    ]
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