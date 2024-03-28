Ext.define('B4.view.regop.personal_account.ChoicesDebtorWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',
        'Ext.ux.CheckColumn'
    ],

    title: 'Выбор претензионно-исковой работы',
    alias: 'widget.choicesdebtorwindow',
    enableColumnHide: true,

    trackResetOnLoad: true,
    modal: true,
    width: 600,
    minWidth: 600,
    minHeight: 500,
    height: 500,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.ChoicesDebtor');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'gridpanel',
                    cls: 'x-large-head',
                    store: store,
                    flex: 1,
                    columnLines: true,
                    selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'SINGLE' }),
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'State',
                            menuText: 'Статус',
                            text: 'Статус задолженности',
                            width: 150,
                            renderer: function(val) { return val && val.Name ? val.Name : ''; }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'CurrChargeDebt',
                            flex: 1,
                            text: 'Сумма текущей задолженности',
                            filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq },
                            renderer: function(val) {
                                return val === 0 ? '' : val;
                            }
                        },
                        {
                            xtype: 'booleancolumn',
                            dataIndex: 'IsDebtPaid',
                            text: 'Задолженность погашена',
                            flex: 1,
                            maxWidth: 100,
                            trueText: 'Да',
                            falseText: 'Нет'
                        }
                    ],
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
                                            xtype: 'button',
                                            text: 'Выбрать',
                                            textAlign: 'left',
                                            name: 'Pir',
                                            action: 'Accept',
                                        }
                                    ]
                                },
                                { xtype: 'tbfill' },
                                {
                                    xtype: 'buttongroup',
                                    columns: 2,
                                    items: [
                                        {
                                            xtype: 'b4closebutton',
                                            handler: function() { this.up('window').close(); }
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
                }
            ]
        });

        me.callParent(arguments);
    }
});