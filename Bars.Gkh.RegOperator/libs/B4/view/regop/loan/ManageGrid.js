Ext.define('B4.view.regop.loan.ManageGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'Ext.grid.feature.Summary',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.form.ComboBox',
        'B4.store.regop.loan.Manage',
        'B4.enums.TaskStatus'
    ],

    title: 'Управление займами',

    alias: 'widget.loanmanagegrid',
    closable: true,
    cls: 'x-large-head',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.loan.Manage'),
            renderer = function (val) {
                val = val || 0;
                return Ext.util.Format.currency(val);
            };

        me.relayEvents(store, ['load'], 'loanmanagegrid.');

        Ext.apply(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel', {
                mode: 'MULTI'
            }),
            columns: [
                {
                    text: 'Муниципальное образование',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListSettlementWithoutPaging'
                    }
                },
                {
                    text: 'Муниципальный район',
                    dataIndex: 'Settlement',
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
                    text: 'Адрес',
                    dataIndex: 'Address',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер р/с',
                    dataIndex: 'CalcAccountNumber',
                    flex: 1,
                    filter: {
                        xtype: 'textfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq,
                        maskRe: /[0-9]/
                    }
                },
                {
                    text: 'Перечень работ',
                    dataIndex: 'WorkNames',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Год',
                    dataIndex: 'Year',
                    flex: 0.5,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq,
                        maxLength: 4

                    }
                },
                {
                    text: 'Собираемость, %',
                    dataIndex: 'Collection',
                    flex: 1,
                    renderer: renderer,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Потребность',
                    dataIndex: 'NeedSum',
                    flex: 1,
                    renderer: renderer,
                    summaryType: 'sum',
                    summaryRenderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Средства собственников',
                    dataIndex: 'OwnerSum',
                    flex: 1,
                    renderer: renderer,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Субсидии',
                    dataIndex: 'SubsidySum',
                    flex: 1,
                    renderer: renderer,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Иные средства',
                    dataIndex: 'OtherSum',
                    flex: 1,
                    renderer: renderer,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    header: 'Статус задачи',
                    dataIndex: 'Task',
                    width: 110,
                    sortable: false,
                    renderer: function(val) {
                        return val ? B4.enums.TaskStatus.displayRenderer(val.Status) : '';
                    }
                },
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    tooltip: 'Переход к результатам',
                    icon: B4.Url.content('content/img/icons/arrow_right.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var scope = this.origScope;

                        // Если scope не задан в конфиге, то берем грид которому принадлежит наша колонка
                        if (!scope)
                            scope = this.up('grid');

                        scope.fireEvent('rowaction', scope, 'gotoresult', rec);
                    },
                    renderer: function (val, meta, rec) {
                        if (!rec.get('Task')) {
                            meta.style += 'display:none;';
                        }
                    },
                    sortable: false
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
                            height: 76,
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    margins: '5 0 0 0'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сформировать займ',
                                    action: 'takeloan',
                                    icon: 'content/img/icons/cog_go.png'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            title: 'Фильтры',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            height: 76,
                            width: 480,
                            columns: 1,
                            defaults: {
                                xtype: 'b4combobox',
                                labelWidth: 160,
                                labelAlign: 'right',
                                editable: false,
                                allowBlank: false
                            },
                            items: [
                                {
                                    fieldLabel: 'Муниципальное образование',
                                    name: 'municipality',
                                    storeAutoLoad: false,
                                    valueField: 'Id',
                                    displayField: 'Name',
                                    url: '/Municipality/ListMoAreaWithoutPaging'
                                },
                                {
                                    fieldLabel: 'Краткосрочный план',
                                    name: 'programcr',
                                    storeAutoLoad: false,
                                    valueField: 'Id',
                                    displayField: 'Name',
                                    url: '/ProgramCr/ListWithoutPaging'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            title: 'Информация о средствах',
                            height: 76,
                            columns: 2,
                            padding: '0 10 10 0',
                            defaults: {
                                xtype: 'numberfield',
                                decimalSeparator: ',',
                                readOnly: true,
                                labelWidth: 130,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    name: 'NeedSum',
                                    fieldLabel: 'Необходимые средства',
                                    value: 0,
                                    renderer: function (val) {
                                        return Ext.util.Format.currency(val);
                                    }
                                },
                                {
                                    xtype: 'tbfill'
                                },
                                {
                                    name: 'AvailableSum',
                                    fieldLabel: 'Текущее сальдо',
                                    value: 0,
                                    renderer: function (val) {
                                        return Ext.util.Format.currency(val);
                                    }
                                },
                                {
                                    name: 'AvailableLoanSum',
                                    fieldLabel: 'С учетом неснижаемого размера фонда',
                                    value: 0,
                                    bodyStyle: 'vertical-align:top;',
                                    labelStyle: 'padding: 0; line-height: 12px;',
                                    renderer: function(val) {
                                        return Ext.util.Format.currency(val);
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
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            features: [{
                ftype: 'summary'
            }],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});