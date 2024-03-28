Ext.define('B4.view.realityobj.housingcommunalservice.AccountGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.hseaccountgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.YesNo',
        'B4.form.ComboBox',
        'B4.ux.grid.filter.YesNo'
    ],

    title: 'Лицевые счета дома',
    store: 'realityobj.housingcommunalservice.Account',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    dataIndex: 'PaymentCode',
                    text: 'Номер лиц.счета',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'Apartment',
                    text: 'Номер квартиры',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                //{
                //    dataIndex: 'Living',
                //    text: 'Жилое помещение',
                //    flex: 1,
                //    renderer: function (val) {
                //        return val ? 'Нет' : 'Да';
                //    }
                //},
                {
                    dataIndex: 'Charged',
                    text: 'Начислено ',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    flex: 1,
                    sortable: false
                },
                {
                    dataIndex: 'Payment',
                    text: 'Оплачено ',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    flex: 1,
                    sortable: false
                },
                {
                    dataIndex: 'Debt',
                    text: 'Задолженность ',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    flex: 1,
                    sortable: false
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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