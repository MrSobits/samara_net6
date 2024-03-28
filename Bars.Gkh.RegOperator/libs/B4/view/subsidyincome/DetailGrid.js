Ext.define('B4.view.subsidyincome.DetailGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.store.regop.subsidyincome.Detail',
        'B4.ux.grid.column.Enum'
    ],

    alias: 'widget.subsidyincomedetailgrid',

    entityId: null,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.subsidyincome.Detail');

        store.on('beforeload', function(s, oper) {
            oper.params['subsidyIncomeId'] = me.entityId;
        });

        Ext.apply(me, {
            store: store,
            cls: 'x-large-head',
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    text: 'ID дома в файле',
                    dataIndex: 'RealObjId',
                    width: 50,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Адрес в файле',
                    dataIndex: 'RealObjAddress',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Муниципальный район в системе',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Адрес в системе',
                    dataIndex: 'Address',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Р/с',
                    dataIndex: 'PayAccNum',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Тип субсидии',
                    dataIndex: 'SubsidyDistrName',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateReceipt',
                    text: 'Дата поступления',
                    width: 70,
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Сумма',
                    dataIndex: 'Sum',
                    width: 70,
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : null;
                    },
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: 'true',
                        decimalSeparator: ',',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsDefined',
                    text: 'Определение домов',
                    width: 80,
                    trueText: 'Определен',
                    falseText: 'Не определен',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        store: Ext.create('Ext.data.Store', {
                            fields: ['value', 'name'],
                            data: [
                                { 'value': null, 'name': '-' },
                                { 'value': true, 'name': 'Определен' },
                                { 'value': false, 'name': 'Не определен' }
                            ]
                        }),
                        valueField: 'value',
                        displayField: 'name',
                        queryMode: 'local'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ConfirmStatus',
                    text: 'Статус',
                    width: 110,
                    filter: {
                        xtype: 'b4combobox',
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        store: Ext.create('Ext.data.Store', {
                            fields: ['value', 'name'],
                            data: [
                                { 'value': '', 'name': '-' },
                                { 'value': 'Подтвержден', 'name': 'Подтвержден' },
                                { 'value': 'Не подтвержден', 'name': 'Не подтвержден' },
                                { 'value': 'Удален', 'name': 'Удален' }
                            ]
                        }),
                        valueField: 'value',
                        displayField: 'name',
                        queryMode: 'local'
                    }
                }
            ],

            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }

        });
        me.callParent(arguments);

    }
});