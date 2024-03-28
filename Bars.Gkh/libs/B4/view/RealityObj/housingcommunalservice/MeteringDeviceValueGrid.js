Ext.define('B4.view.realityobj.housingcommunalservice.MeteringDeviceValueGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.hsemeteringdevicevaluegrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Показания общедомовых приборов учета',
    store: 'realityobj.housingcommunalservice.MeteringDeviceValue',
    closable: true,

    initComponent: function () {
        var me = this,
            yearsItems = [[0, '-']],
            i;

        for (i = 1990; i <= (new Date()).getFullYear(); i++) {
            yearsItems.push([i, i.toString()]);
        }
        
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    dataIndex: 'Service',
                    text: 'Услуга',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'MeterSerial',
                    text: 'Номер ПУ',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'MeterType',
                    text: 'Тип ПУ',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'CurrentReadingDate',
                    text: 'Дата снят тек',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'PrevReadingDate',
                    text: 'Дата снят пред',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    dataIndex: 'CurrentReading',
                    text: 'Показание тек',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'PrevReading',
                    text: 'Показание пред',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'NonLivingExpense',
                    text: 'Расход по нежилым помещениям',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'Expense',
                    text: 'Расход',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        },
                        {
                            xtype: 'b4combobox',
                            fieldLabel: 'Месяц',
                            itemId: 'cbMeteringMonth',
                            labelWidth: 50,
                            items: [[0, '-'], [1, 'Январь'], [2, 'Февраль'],  [3, 'Март'],     [4, 'Апрель'],  [5, 'Май'],   [6, 'Июнь'],
                                    [7, 'Июль'], [8, 'Август'], [9, 'Сентябрь'], [10, 'Октябрь'], [11, 'Ноябрь'], [12, 'Декабрь']],
                            editable: false,
                            operand: CondExpr.operands.eq,
                            valueField: 'Value',
                            displayField: 'Display'
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'filterYear',
                            itemId: 'cbMeteringYear',
                            labelWidth: 50,
                            fieldLabel: 'Год',
                            maxWidth: 280,
                            width: 200,
                            editable: false,
                            items: yearsItems
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