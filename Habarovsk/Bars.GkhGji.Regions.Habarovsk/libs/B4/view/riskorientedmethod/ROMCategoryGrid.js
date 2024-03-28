Ext.define('B4.view.riskorientedmethod.ROMCategoryGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.romcategorygrid',

    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.KindKND',
        'B4.enums.YearEnums',
        'B4.enums.RiskCategory',
         'B4.form.ComboBox',
        'B4.form.GridStateColumn',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.ux.button.Close',
         'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
    ],
    closable: true,
    title: 'Реестр категорий риска',
    store: 'riskorientedmethod.ROMCategory',

    initComponent: function () {
        var me = this;
        var currKindKNDEnums = B4.enums.KindKND.getItemsWithEmpty([null, '-']);
        var newKindKNDEnums = [];
        Ext.iterate(currKindKNDEnums, function (val, key) {
            newKindKNDEnums.push(val);
        });

        var currYearEnums = B4.enums.YearEnums.getItemsWithEmpty([null, '-']);
        var newYearEnums = [];
        Ext.iterate(currYearEnums, function (val, key) {
            newYearEnums.push(val);
        });

        var currRiskCategoryEnums = B4.enums.RiskCategory.getItemsWithEmpty([null, '-']);
        var newRiskCategoryEnums = [];
        Ext.iterate(currRiskCategoryEnums, function (val, key) {
            newRiskCategoryEnums.push(val);
        });


        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 160,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_romcategory';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'KindKND',
                     flex: 1,
                     text: 'Вид КНД',
                     renderer: function (val) {
                         return B4.enums.KindKND.displayRenderer(val);
                     },
                     filter: {
                         xtype: 'b4combobox',
                         items: newKindKNDEnums,
                         editable: false,
                         operand: CondExpr.operands.eq,
                         valueField: 'Value',
                         displayField: 'Display'
                     }
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearEnums',
                    flex: 0.5,
                    text: 'Год',
                    renderer: function (val) {
                        return B4.enums.YearEnums.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: newYearEnums,
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
               {
                   xtype: 'gridcolumn',
                   dataIndex: 'Contragent',
                   flex: 1,
                   text: 'Контрагент',
                   filter: { xtype: 'textfield' },
               },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentINN',
                    flex: 0.5,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Result',
                    flex: 0.5,
                    text: 'Результат'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RiskCategory',
                    flex: 0.5,
                    text: 'Категория риска',
                    renderer: function (val) {
                        return B4.enums.RiskCategory.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: newRiskCategoryEnums,
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    flex: 1,
                    text: 'Инспектор',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CalcDate',
                    flex: 0.5,
                    text: 'Дата расчета',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    var planDate = record.get('PerfomanceDate');
                    var factDate = record.get('PerfomanceFactDate');
                    var currentdate = new Date();
                    var planDateDt = new Date(planDate);
                    var datetime = currentdate.getFullYear() + "-" + currentdate.getDate();

                    if (planDateDt <= currentdate && factDate === null) {
                        
                        return 'back-red';
                    }
                  
                    return '';
                }
            },
            dockedItems: [
                    {
                        xtype: 'toolbar',
                        dock: 'top',
                        items: [
                            {
                                xtype: 'b4addbutton'
                            },
                              
                            {
                                xtype: 'b4updatebutton'
                            },
                            {
                                xtype: 'button',
                                iconCls: 'icon-table-go',
                                text: 'Экспорт',
                                textAlign: 'left',
                                itemId: 'btnExport'
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