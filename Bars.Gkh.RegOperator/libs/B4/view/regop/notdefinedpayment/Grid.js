Ext.define('B4.view.regop.notdefinedpayment.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.form.EnumCombo',
        'B4.store.regop.ImportedPayment',
        'B4.enums.ImportedPaymentType',
        'B4.enums.PersonalAccountNotDeterminationStateReason'
    ],

    title: 'Реестр неопределенных оплат',

    alias: 'widget.notdefinedpaymentgrid',

    closable: true,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.ImportedPayment');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'actioncolumn',
                    scope: me,
                    width: 20,
                    action: 'TransferToImportedPayment',
                    tooltip: 'Переход',
                    icon: 'content/img/icons/arrow_out.png',
                    handler: function(gridView, rowIndex, colIndex, el, e, record) {
                        Ext.History.add('bank_doc_import_details/' + record.get('BankDocumentImportId') + '/onlynotdefined/');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    text: 'Номер реестра',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата реестра',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentAgentName',
                    text: 'Наименование платежного агента',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Account',
                    text: 'Лицевой счет (файл)',
                    flex: 1,
                    filter: { xtype: 'textfield' }
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
                                B4.enums.ImportedPaymentType.SocialSupport
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
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.PersonalAccountNotDeterminationStateReason',
                    dataIndex: 'PersonalAccountNotDeterminationStateReason',
                    flex: 1,
                    filter: true,
                    header: 'Причина несоответствия'
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