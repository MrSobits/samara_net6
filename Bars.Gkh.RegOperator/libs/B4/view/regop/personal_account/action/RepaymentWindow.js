Ext.define('B4.view.regop.personal_account.action.RepaymentWindow', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.repaymentwindow',

    requires: [
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.grid.Panel',
        'B4.enums.CrFundFormationType'
    ],

    modal: true,
    closable: false,
    maximized: true,
    width: 1000,
    minWidth: 900,
    height: 550,
    minHeight: 300,
    title: 'Перераспределение оплаты',
    closeAction: 'destroy',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    bodyPadding: 0,
    border: null,
    accountOperationCode: 'RepaymentOperation',
    accountIds: null,

    initComponent: function () {
        var me = this,
            targetAccountStore = Ext.create('B4.store.regop.personal_account.Distribution'),
            paymentStore = Ext.create('B4.base.Store', {
                    autoLoad: false,
                    proxy: {
                        type: 'b4proxy',
                        controllerName: 'BasePersonalAccount',
                        listAction: 'GetOperationDataForUI',
                        timeout: 1000 * 60 * 5
                    },
                    fields: [
                        'Id',
                        'AccountId',
                        'PersonalAccountNum',
                        'Owner',
                        'Address',
                        'RoPayAccountNum',
                        'AccountFormVariant',
                        'PaymentDate',
                        'BaseTariffSum',
                        'DecisionTariffSum',
                        'PenaltySum',
                        'BaseTariffTransferId',
                        'DecisionTariffTransferId',
                        'PenaltyTransferId'
                    ],
                    listeners: {
                        beforeload: function(store, operation) {
                            Ext.apply(operation.params, me.getParams());

                            if (!operation.params.startDate || !operation.params.endDate) {
                                return false;
                            }
                        }
                    }
                }),
            renderer = function(val) {
                return Ext.util.Format.currency(val);
            };

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    flex: 0.7,
                    title: 'Снятие оплаты',
                    bodyStyle: Gkh.bodyStyle,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items:[
                        {
                            xtype: 'form',
                            border: null,
                            bodyPadding: '10px 10px 0 10px',
                            bodyStyle: Gkh.bodyStyle,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                flex: 1,
                                labelWidth: 130,
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items:[
                                {
                                    xtype: 'textfield',
                                    name: 'Reason',
                                    maxLength: 200,
                                    fieldLabel: 'Причина',
                                    readOnly: false
                                    
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'Document',
                                    fieldLabel: 'Документ-основание',
                                    readOnly: false,
                                    editable: false,
                                    allowBlank: false
                                }, 
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DateStart',
                                            labelWidth: 130,
                                            fieldLabel: 'Период оплаты с',
                                            allowBlank: false,
                                            maxValue: new Date()
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DateEnd',
                                            labelWidth: 120,
                                            fieldLabel: 'Период оплаты по',
                                            allowBlank: false,
                                            maxValue: new Date()
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'b4grid',
                            name: 'PaymentGrid',
                            margin: '10px 0 0 0',
                            store: paymentStore,
                            columnLines: true,
                            flex: 1,
                            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel'),
                            columns: [
                                { header: 'Номер ЛС', xtype: 'gridcolumn', dataIndex: 'PersonalAccountNum', width: 100 },
                                { header: 'Абонент', xtype: 'gridcolumn', dataIndex: 'Owner', flex: 1 },
                                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1 },
                                { header: 'Р/С дома', xtype: 'gridcolumn', dataIndex: 'RoPayAccountNum', flex: 1 },
                                { header: 'Способ формирования фонда', xtype: 'gridcolumn', dataIndex: 'AccountFormVariant', flex: 1 },
                                { header: 'Дата оплаты', xtype: 'datecolumn', format: 'd.m.Y', dataIndex: 'PaymentDate', width: 100 },
                                { header: 'Оплата по базовому тарифу', xtype: 'gridcolumn', dataIndex: 'BaseTariffSum', flex: 1, renderer: renderer },
                                { header: 'Оплата по тарифу решения', xtype: 'gridcolumn', dataIndex: 'DecisionTariffSum', flex: 1, renderer: renderer },
                                { header: 'Оплата по пени', xtype: 'gridcolumn', dataIndex: 'PenaltySum', flex: 1, renderer: renderer }
                            ],
                            dockedItems: [
                                {
                                    xtype: 'b4pagingtoolbar',
                                    displayInfo: true,
                                    store: paymentStore,
                                    dock: 'bottom'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4grid',
                    name: 'EnrollGrid',
                    title: 'Зачисление оплаты',
                    columnLines: true,
                    flex: 0.5,
                    store: targetAccountStore,
                    selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', { mode: 'SINGLE' }),
                    columns: [
                        { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'PersonalAccountNum', width: 100, filter: { xtype: 'textfield' } },
                        { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RoomAddress', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Владелец', xtype: 'gridcolumn', dataIndex: 'AccountOwner', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            header: 'Статус',
                            xtype: 'gridcolumn',
                            dataIndex: 'State',
                            width: 120,
                            renderer: function(v) {
                                return '<div style="float: left;">' + (v && v.Name ? v.Name : '') + '</div>';
                            },
                            filter: {
                                xtype: 'b4combobox',
                                url: '/State/GetListByType',
                                storeAutoLoad: false,
                                operand: CondExpr.operands.eq,
                                listeners: {
                                    storebeforeload: function(field, store, options) {
                                        options.params.typeId = 'gkh_regop_personal_account';
                                    },
                                    storeloaded: {
                                        fn: function(field) {
                                            field.getStore().insert(0, { Id: null, Name: '-' });
                                        }
                                    }
                                }
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            dataIndex: 'AccountFormationVariant',
                            text: 'Способ формирования фонда КР',
                            enumName: 'B4.enums.CrFundFormationType',
                            flex: 1,
                            maxWidth: 180
                        },
                        { header: 'Р/С получателя', xtype: 'gridcolumn', dataIndex: 'RoPayAccountNum', flex: 0.8, maxWidth: 140 , filter: { xtype: 'textfield' } }
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: targetAccountStore,
                            dock: 'bottom'
                        }
                    ],
                    plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                    viewConfig: {
                        loadMask: true
                    }
                }
            ],

            getForm: function() {
                return me.down('form');
            }
        });

        me.callParent(arguments);
    },

    getParams: function() {
        var me = this,
            params = {
                operationCode: me.accountOperationCode,
                accIds: Ext.JSON.encode(me.accountIds),
                startDate: me.down('[name=DateStart]').getValue(),
                endDate: me.down('[name=DateEnd]').getValue()
            };

        return params;
    },
    listeners: {
        beforeclose: function(win) {
            Ext.Msg.confirm('Внимание',
                'Закрыть форму без сохранения изменений?',
                function(result) {
                    if (result === 'yes') {
                        win.clearListeners(); // чистим все слушателей, чтобы опять сюда не попасть
                        win.close();
                    }
                });

            return false;
        }
    }       
});         
            