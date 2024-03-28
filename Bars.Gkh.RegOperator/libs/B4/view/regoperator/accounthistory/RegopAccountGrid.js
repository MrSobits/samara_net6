Ext.define('B4.view.regoperator.accounthistory.RegopAccountGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.accounthistoryRegopAccountGrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.RegopAccountRealityObject',
        'B4.store.regoperator.CalcAccountForSelect'
    ],

    title: 'Расчетные счета',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.RegopAccountRealityObject');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    dataIndex: 'Municipality',
                    text: 'Муниципальный район',
                    flex: 1,
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
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    width: 200,
                    text: 'Дата начала действия',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    format: 'd.m.Y',
                    width: 200,
                    text: 'Дата окончания действия',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
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
                            xtype: 'b4selectfield',
                            fieldLabel: 'Расчетный счет',
                            labelAlign: 'right',
                            name: 'CalcAccount',
                            store: 'B4.store.regoperator.CalcAccountForSelect',
                            textProperty: 'AccountNumber',
                            editable: false,
                            padding: '0 5',
                            columns: [{ text: 'Номер расчетного счета', dataIndex: 'AccountNumber', flex: 1, filter: { xtype: 'textfield' } }]
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        me.getStore().load();
                                    }
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