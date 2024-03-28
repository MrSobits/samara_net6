Ext.define('B4.view.regop.bankdocumentimport.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.bankdocimporteditpanel',
    title: 'Реестр платежного агента',
    closable: true,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'Ext.ux.CheckColumn',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.regop.ImportedPayment',
        'B4.enums.ImportedPaymentType',
        'B4.enums.ImportedPaymentState',
        'B4.form.EnumCombo',
        'B4.enums.ImportedPaymentPersAccDeterminateState',
        'B4.enums.ImportedPaymentPaymentConfirmState',
        'B4.ux.grid.column.Enum',
        'B4.form.GridStateColumn'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.ImportedPayment');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Общая информация',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                readOnly: true,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'ImportDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата операции',
                                    width: 250,
                                    labelWidth: 125
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер сводного реестра',
                                    labelWidth: 125,
                                    width: 500
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PaymentAgentName',
                                    fieldLabel: 'Наименование платежного агента',
                                    labelWidth: 150,
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            padding: '5 0 5 0',
                            defaults: {
                                readOnly: true,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата сводного реестра',
                                    width: 250,
                                    labelWidth: 125
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ImportedSum',
                                    fieldLabel: 'Сумма по реестру (руб.)',
                                    labelWidth: 125,
                                    width: 500
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'BankStatement',
                                    fieldLabel: 'Банковская выписка',
                                    labelWidth: 150,
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                readOnly: true,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'FileName',
                                    fieldLabel: 'Наименование файла',
                                    width: 250,
                                    labelWidth: 125
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Статус определения ЛС',
                                    enumName: 'B4.enums.PersonalAccountDeterminationState',
                                    name: 'PersonalAccountDeterminationState',
                                    labelWidth: 125,
                                    width: 500
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Статус подтверждения оплат',
                                    enumName: 'B4.enums.PaymentConfirmationState',
                                    name: 'PaymentConfirmationState',
                                    labelWidth: 150,
                                    flex: 1
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'gridpanel',
                    cls: 'x-large-head',
                    title: 'Детализация по оплатам в разрезе лицевых счетов',
                    store: store,
                    flex: 1,
                    plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters', { pluginId: 'headerFilter' })],
                    selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'MULTI' }),
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Account',
                            text: 'Лицевой счет (файл)',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PersonalAccount',
                            text: 'Лицевой счет в системе',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PersAccNumExternalSystems',
                            text: 'Номер ЛС во внешней системе',
                            flex: 1,
                            hidden: true,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'b4gridstatecolumn',
                            dataIndex: 'PersonalAccountState',
                            minWidth: 130,
                            maxWidth: 140,
                            menuText: 'Статус ЛС',
                            text: 'Статус ЛС',
                            filter: {
                                xtype: 'b4combobox',
                                url: '/State/GetListByType',
                                storeAutoLoad: false,
                                operand: CondExpr.operands.eq,
                                listeners: {
                                    storebeforeload: function (field, store, options) {
                                        options.params.typeId = 'gkh_regop_personal_account';
                                    },
                                    storeloaded: {
                                        fn: function (field) {
                                            field.getStore().insert(0, { Id: null, Name: '-' });
                                        }
                                    }
                                }
                            },
                            scope: this
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'AddressByImport',
                            text: 'Адрес (файл)',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'OwnerByImport',
                            text: 'Абонент (файл)',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ReceiverNumber',
                            text: 'Р/С получателя (файл)',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Address',
                            text: 'Адреc',
                            flex: 1,
                            hidden: true,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Owner',
                            text: 'Абонент',
                            flex: 1,
                            hidden: true,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ReceiverNumberFact',
                            text: 'Р/С получателя',
                            flex: 1,
                            hidden: true,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            text: 'Тип оплаты',
                            dataIndex: 'PaymentType',
                            flex: 1,
                            renderer: function(val) {
                                return B4.enums.ImportedPaymentType.displayRenderer(val);
                            },
                            filter: {
                                xtype: 'b4combobox',
                                items: Ext.Array.filter(B4.enums.ImportedPaymentType.getItemsWithEmpty([null, '-']), function(item) {
                                    return Ext.Array.contains([
                                        null,
                                        B4.enums.ImportedPaymentType.Penalty,
                                        B4.enums.ImportedPaymentType.ChargePayment,
                                        B4.enums.ImportedPaymentType.SocialSupport,
                                        B4.enums.ImportedPaymentType.Refund,
                                        B4.enums.ImportedPaymentType.PenaltyRefund
                                    ], item[0]);
                                }),
                                editable: false,
                                operand: CondExpr.operands.eq,
                                valueField: 'Value',
                                displayField: 'Display'
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'PaymentDate',
                            format: 'd.m.Y',
                            text: 'Дата оплаты',
                            flex: 1,
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'AcceptDate',
                            format: 'd.m.Y',
                            text: 'Дата подтверждения',
                            flex: 1,
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'numbercolumn',
                            text: 'Сумма',
                            dataIndex: 'Sum',
                            format: '0.00',
                            flex: 1,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PaymentNumberUs',
                            text: 'Номер платежа в Системе ПА',
                            width: 130,
                            hidden: true,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            dataIndex: 'PersonalAccountDeterminationState',
                            enumName: 'B4.enums.ImportedPaymentPersAccDeterminateState',
                            filter: true,
                            width: 130,
                            text: 'Статус определения ЛС'
                        },
                        {
                            xtype: 'actioncolumn',
                            dataIndex: 'PaymentConfirmationState',
                            width: 170,
                            text: 'Статус подтверждения оплат',
                            filter: {
                                xtype: 'b4combobox',
                                items: B4.enums.ImportedPaymentPaymentConfirmState.getItemsWithEmpty([null, 'Все']),
                                editable: false,
                                operand: CondExpr.operands.eq,
                                valueField: 'Value',
                                displayField: 'Display'
                            },
                            //renderer не использовать, тк он подменяется 
                            defaultRenderer: function (value) {
                                return '<div style="float: left;">' + (value ? B4.enums.ImportedPaymentPaymentConfirmState.displayRenderer(value) : '') +
                                    "</div><img data-qtip='История изменений' style='display:block; float: right;' src='content/img/icons/book_open.png'>";
                            },
                            processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                                if (type == 'click' && e.target.localName == 'img') {
                                    var record = view.getStore().getAt(recordIndex);
                                    view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                                }
                            }
                        },
                        {
                            xtype: 'booleancolumn',
                            dataIndex: 'IsDeterminateManually',
                            text: 'ЛС сопоставлен вручную',
                            flex: 1,
                            maxWidth: 100,
                            trueText: 'Да',
                            falseText: 'Нет',
                            hidden: true,
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
                                        { 'value': true, 'name': 'Да' },
                                        { 'value': false, 'name': 'Нет' }
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
                            xtype: 'toolbar',
                            dock: 'top',
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    items: [
                                        {
                                            xtype: 'button',
                                            action: 'InternalAccept',
                                            tooltip: 'По нажатию произойдет подтверждение оплаты выбранных записей',
                                            text: 'Подтвердить оплаты',
                                            iconCls: 'icon-tick'
                                        },
                                        {
                                            xtype: 'button',
                                            action: 'InternalCancel',
                                            text: 'Отменить подтверждение',
                                            iconCls: 'icon-decline'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Сопоставить ЛС',
                                            textAlign: 'left',
                                            action: 'Compare'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'buttongroup',
                                    defaults: {
                                        margin: 2
                                    },
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            boxLabel: 'Несоответствие по Р/С получателя',
                                            fieldStyle: 'vertical-align: middle;',
                                            style: 'font-size: 11px !important;',
                                            margin: '-2 0 0 10',
                                            name: 'RsNotEqual',
                                            listeners: {
                                                afterRender: function(obj) {
                                                    try {
                                                        Ext.QuickTips.register({
                                                            target: obj.getEl(),
                                                            title: '',
                                                            text: '<span style="">' + "При установке галочки будут отображены записи, у которых значение \"Р/С получателя (файл)\" не соответствует значению \"Р/С получателя\"" + '</span>',
                                                            enabled: true,
                                                            trackMouse: true
                                                        });
                                                    } catch (e) {
                                                    }
                                                }
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
                    viewConfig: {
                        enableTextSelection: true
                    }
                }
            ]
        });

        me.callParent(arguments);
    }
});