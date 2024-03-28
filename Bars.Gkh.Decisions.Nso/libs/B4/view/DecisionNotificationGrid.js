Ext.define('B4.view.DecisionNotificationGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.decisionnotificationgrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.DecisionNotification',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Enum',
        'B4.enums.CrFundFormationType'
    ],

    title: 'Сводный реестр уведомлений о решениях общего собрания',
    closable: true,
    enableColumnHide: true,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.DecisionNotification');

        Ext.applyIf(me, {
            store: store,
            cls:'x-large-head',
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    menuText: 'Статус',
                    text: 'Статус',
                    sortable: false,
                    width: 150,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, st, options) {
                                options.params.typeId = 'gkh_decision_notification';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
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
                    dataIndex: 'Number',
                    text: 'Исходящий номер',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'Date',
                    text: 'Дата',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    width: 70
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Mu',
                    text: 'Муниципальный район',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MoSettlement',
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListSettlementWithoutPaging'
                    },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Адрес',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'FormFundType',
                    text: 'Способ формирования фонда',
                    filter: true,
                    enumName: 'B4.enums.CrFundFormationType',
                    flex: 1
                },         
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OrgName',
                    text: 'Владелец счета',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CreditOrgName',
                    text: 'Кредитная организация',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HasCertificate',
                    text: 'Справка',
                    filter: { xtype: 'textfield' },
                    width: 55
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccountNum',
                    text: 'Расчетный счет',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'OpenDate',
                    text: 'Открыт',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    width: 70
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CloseDate',
                    format: 'd.m.Y',
                    text: 'Закрыт',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    width: 70
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomeNum',
                    text: 'Входящий номер',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'RegistrationDate',
                    text: 'Дата регистрации',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    width: 70
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