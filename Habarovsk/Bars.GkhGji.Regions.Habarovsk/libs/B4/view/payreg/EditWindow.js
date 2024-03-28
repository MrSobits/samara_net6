Ext.define('B4.view.payreg.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.PayerType',
        'B4.store.dict.PhysicalPersonDocType',
        'B4.store.smev.GisGmp',
        'B4.store.smev.GisGmpForPayRegEditWindow',
        'B4.enums.IdentifierType',
        'B4.view.payreg.Grid',
        'B4.enums.GisGmpPaymentsKind',
        'B4.enums.GisGmpPaymentsType',
        'B4.enums.GisGmpChargeType',
        'B4.enums.TypeDocumentGji'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'payregEditWindow',
    title: 'Информация о платеже',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            xtype: 'container',
            layout: 'vbox',
            defaults: {
                margin: '0',
                labelWidth: 120,
                labelAlign: 'right',
                readOnly: true,
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Id',
                    itemId: 'dfId',
                    fieldLabel: 'Идентификатор',
                    allowBlank: true,
                    disabled: true,
                    flex: 1,
                    maxLength: 40,
                    editable: false,
                },
                {
                    xtype: 'textfield',
                    name: 'PaymentId',
                    itemId: 'dfPaymentId',
                    fieldLabel: 'Идентификатор платежа',
                    allowBlank: true,
                    disabled: false,
                    flex: 1,
                    maxLength: 40,
                    editable: false,
                },
                {
                    xtype: 'textfield',
                    name: 'Purpose',
                    itemId: 'dfPurpose',
                    fieldLabel: 'Назначение платежа',
                    allowBlank: true,
                    disabled: false,
                    flex: 1,
                    maxLength: 512,
                    editable: false,
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                        readOnly: true,
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Amount',
                            itemId: 'dfAmount',
                            fieldLabel: 'Сумма платежа',
                            allowBlank: true,
                            disabled: false,
                            flex: 1,
                            editable: false,
                            maxLength: 20,
                        },
                        {
                            xtype: 'datefield',
                            name: 'PaymentDate',
                            itemId: 'dfPaymentDate',
                            fieldLabel: 'Дата платежа',
                            allowBlank: true,
                            disabled: false,
                            flex: 1,
                            editable: false,
                            maxLength: 20,
                        },
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'SupplierBillID',
                    itemId: 'dfSupplierBillID',
                    fieldLabel: 'УИН',
                    allowBlank: true,
                    disabled: false,
                    flex: 1,
                    editable: false,
                    maxLength: 40,
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                        readOnly: true,
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Kbk',
                            itemId: 'dfKbk',
                            fieldLabel: 'КБК',
                            allowBlank: true,
                            disabled: false,
                            flex: 1,
                            editable: false,
                            maxLength: 20,
                        },
                        {
                            xtype: 'textfield',
                            name: 'OKTMO',
                            itemId: 'dfOKTMO',
                            fieldLabel: 'ОКТМО',
                            allowBlank: true,
                            disabled: false,
                            flex: 1,
                            editable: false,
                            maxLength: 20,
                        },
                    ]
                },                
                {
                    xtype: 'textfield',
                    name: 'PayerName',
                    itemId: 'dfPayerName',
                    fieldLabel: 'Наименование плательщика',
                    allowBlank: true,
                    disabled: false,
                    flex: 1,
                    editable: false,
                    maxLength: 512,
                },
               
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                        readOnly: true,
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'AccDocDate',
                            itemId: 'dfAccDocDate',
                            fieldLabel: 'Дата платёжного документа',
                            allowBlank: true,
                            disabled: false,
                            flex: 1,
                            editable: false,
                            maxLength: 20,
                        },
                        {
                            xtype: 'textfield',
                            name: 'AccDocNo',
                            itemId: 'dfAccDocNo',
                            fieldLabel: 'Номер платёжного документа',
                            allowBlank: true,
                            disabled: false,
                            flex: 1,
                            editable: false,
                            maxLength: 20,
                        },
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                        readOnly: true,
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'PaymentOrg',
                            itemId: 'dfPaymentOrg',
                            fieldLabel: 'Оплата через',
                            allowBlank: true,
                            disabled: false,
                            flex: 1,
                            editable: false,
                            maxLength: 40,
                        },
                        {
                            xtype: 'textfield',
                            name: 'PaymentOrgDescr',
                            itemId: 'dfPaymentOrgDescr',
                            fieldLabel: 'БИК / код ТОФК / УРН',
                            allowBlank: true,
                            disabled: false,
                            flex: 1,
                            editable: false,
                            maxLength: 40,
                        },
                    ]
                },
                
                {
                    xtype: 'textfield',
                    name: 'PayerId',
                    itemId: 'dfPayerId',
                    fieldLabel: 'Идентификатор плательщика',
                    allowBlank: true,
                    disabled: false,
                    flex: 1,
                    editable: false,
                    maxLength: 40,
                },      
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'vbox',
                            flex: 1,
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                readOnly: true,
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'BdiStatus',
                                    itemId: 'dfBdiStatus',
                                    fieldLabel: 'Статус плательщика (поле 101)',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    editable: false,
                                    maxLength: 2,
                                },         
                                {
                                    xtype: 'textfield',
                                    name: 'BdiTaxDocNumber',
                                    itemId: 'dfBdiTaxDocNumber',
                                    fieldLabel: 'Номер документа (поле 108)',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    editable: false,
                                    maxLength: 20,
                                },
                                
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'vbox',
                            flex: 1,
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                readOnly: true,
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'BdiPaytReason',
                                    itemId: 'dfBdiPaytReason',
                                    fieldLabel: 'Основание платежа (поле 106)',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    editable: false,
                                    maxLength: 2,
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'BdiTaxDocDate',
                                    itemId: 'dfBdiTaxDocDate',
                                    fieldLabel: 'Дата документа (поле 109)',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    editable: false,
                                    maxLength: 20,
                                },                                
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'vbox',
                            flex: 1,
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                readOnly: true,
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'BdiTaxPeriod',
                                    itemId: 'dfBdiTaxPeriod',
                                    fieldLabel: 'Период платежа (поле 107)',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    editable: false,
                                    maxLength: 20,
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Status',
                                    itemId: 'dfStatus',
                                    fieldLabel: 'Статус</br>платежа',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    editable: false,
                                    maxLength: 1,
                                },
                            ]
                        },
                    ]
                },
               
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'GisGmp',
                            fieldLabel: 'Начисление',
                            store: 'B4.store.smev.GisGmpForPayRegEditWindow',
                            editable: false,
                            flex: 1,
                            itemId: 'dfCalculation',
                            textProperty: 'UIN',
                            idProperty: 'Id',
                            allowBlank: true,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right',
                            width: 1000,
                            columns: [
                                { text: 'УИН', dataIndex: 'UIN', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Назначение платежа', dataIndex: 'BillFor', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Сумма', dataIndex: 'TotalAmount', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Документ', dataIndex: 'DocNumDate', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Идентификатор плательщика', dataIndex: 'AltPayerIdentifier', flex: 1, filter: { xtype: 'textfield' } },
                                {
                                    xtype: 'gridcolumn', text: 'Тип плательщика', dataIndex: 'PayerType', flex: 1, renderer: function (val) {
                                        return B4.enums.PayerType.displayRenderer(val);
                                    },
                                    filter: {
                                        xtype: 'combobox',
                                        store: B4.enums.PayerType.getItemsWithEmpty([null, '-']),
                                        operand: CondExpr.operands.eq,
                                        editable: false
                                    }
                                },
                                { text: 'Наименование плательщика', dataIndex: 'PayerName', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Нормативно-правовой акт', dataIndex: 'LegalAct', flex: 1, filter: { xtype: 'textfield' } }
                                //{ text: 'ОКТМО', dataIndex: 'OKTMO', flex: 1, filter: { xtype: 'textfield' } }

                            ]
                        }
                    ]
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
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Автосопоставление',
                                    tooltip: 'Автоматически сопоставить с начислением',
                                    iconCls: 'icon-accept',
                                    width: 150,
                                    itemId: 'btnFindGisGmp'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});