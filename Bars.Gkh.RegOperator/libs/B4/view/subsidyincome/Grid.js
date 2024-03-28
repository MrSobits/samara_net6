Ext.define('B4.view.subsidyincome.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.subsidyincomegrid',

    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.form.ComboBox',

        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhButtonImport',
        'B4.store.regop.subsidyincome.Distribution',
        'B4.store.regop.SubsidyIncome',
        'B4.enums.SubsidyIncomeStatus',
        'B4.enums.SubsidyIncomeDefineType'
    ],

    title: 'Реестр субсидий',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.SubsidyIncome'),
            distrTypeStore = Ext.create('B4.store.regop.subsidyincome.Distribution');

            distrTypeStore.load();

        Ext.apply(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateReceipt',
                    format: 'd.m.Y',
                    maxWidth: 110,
                    text: 'Дата операции',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    text: 'Количество записей',
                    dataIndex: 'DetailsCount',
                    width: 125,
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Сумма',
                    dataIndex: 'Sum',
                    maxWidth: 110,
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'DistributeState',
                    enumName: 'B4.enums.SubsidyIncomeStatus',
                    filter: true, 
                    width: 130,
                    text: 'Статус'
                },
                {
                    text: 'Банковская выписка',
                    dataIndex: 'BankAccountStatement',
                    flex: 1,
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'SubsidyIncomeDefineType',
                    enumName: 'B4.enums.SubsidyIncomeDefineType',
                    filter: true, 
                    width: 150,
                    text: 'Определение домов'
                },
                {
                    dataIndex: 'TypeSubsidyDistr',
                    flex: 1,
                    text: 'Типы субсидий',
                    sortable: false,
                    renderer: function (value) {
                        var distr;

                        Ext.each(value.split(','), function (code) {

                            distrTypeStore.each(function (rec) {
                                if (rec.get('Code') === code) {
                                    if (distr) {
                                        distr = Ext.String.format('{0}, {1}', distr, rec.get('Name'));
                                    } else {
                                        distr = rec.get('Name');
                                    }
                                }
                            });
                        });

                        return distr;
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            title: 'Действия',
                            columns: 5,
                            items: [
                                {
                                    xtype: 'gkhbuttonimport'
                                },
                                {   xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    action: 'confirm',
                                    iconCls: 'icon-money-add',
                                    text: 'Подтвердить'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отменить подтверждение',
                                    iconCls: 'icon-cross',
                                    action: 'undoconfirm'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Удалить',
                                    iconCls: 'icon-money-delete',
                                    action: 'delete'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            title: 'Фильтры',
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'ShowConfirmed',
                                    boxLabel: 'Показать подтвержденные',
                                    fieldStyle: 'vertical-align: middle;'
                                },
                                {
                                    xtype: 'component',
                                    width: 10
                                },
                                {
                                    xtype: 'checkbox',
                                    boxLabel: 'Показать удаленные',
                                    name: 'ShowDeleted'
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
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});