Ext.define('B4.view.gisgmp.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.enums.PayerType',
        'B4.store.dict.GISGMPPayerStatus',
        'B4.store.dict.PhysicalPersonDocType',
        'B4.store.ListResolutionForGisGMP',
        'B4.store.ListLicRequestForGisGMP',
        'B4.store.ListReissuanceForGisGMP',
        'B4.enums.IdentifierType',
        'B4.view.gisgmp.FileInfoGrid',
        'B4.view.gisgmp.GISGMPPaymentsGrid',
        'B4.enums.GisGmpPaymentsKind', 
        'B4.enums.GisGmpPaymentsType', 
        'B4.enums.GisGmpChargeType', 
        'B4.enums.TypeLicenseRequest',
        'B4.enums.TypeDocumentGji',
        'B4.enums.RequestState'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'gisgmpEditWindow',
    title: 'Обмен данными с ГИС ГМП',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 150,
                                margin: '5 0 5 0',
                                align: 'stretch',
                                labelAlign: 'right'
                            },
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            title: 'Отправка информации о начислении',
                            border: false,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        margin: '0 0 5 0',
                                        labelWidth: 80,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'button',
                                            text: 'Отправить начисление',
                                            tooltip: 'Отправить начисление',
                                            iconCls: 'icon-accept',
                                            width: 200,
                                            itemId: 'sendCalculateButton'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Проверить ответ',
                                            tooltip: 'Проверить ответ',
                                            iconCls: 'icon-accept',
                                            width: 200,
                                            itemId: 'getCalculateStatusButton'
                                        }
                                        
                                    ]
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'RequestState',
                                    fieldLabel: 'Состояние запроса',
                                    displayField: 'Display',
                                    itemId: 'dfRequestState',
                                    flex: 1,
                                    store: B4.enums.RequestState.getStore(),
                                    valueField: 'Value',
                                    allowBlank: true,
                                    editable: false
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'GisGmpChargeType',
                                    fieldLabel: 'Тип запроса',
                                    displayField: 'Display',
                                    itemId: 'dfGisGmpChargeType',
                                    flex: 1,
                                    store: B4.enums.GisGmpChargeType.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false,
                                    editable: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Reason',
                                    itemId: 'dfReason',
                                    fieldLabel: 'Основание',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    maxLength: 512,
                                    editable: true,
                                },
                                {
                                    xtype: 'label',
                                    forId: 'myFieldId',
                                    text: 'Основание обязательно для всех типов, кроме извещения о начислении',
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        margin: '0 0 5 0',
                                        labelWidth: 150,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            name: 'PayerType',
                                            fieldLabel: 'Вид плательщика',
                                            displayField: 'Display',
                                            itemId: 'dfPayerType',
                                            flex: 1,
                                            store: B4.enums.PayerType.getStore(),
                                            valueField: 'Value',
                                            allowBlank: false,
                                            editable: false
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'GISGMPPayerStatus',
                                            itemId: 'sfGISGMPPayerStatus',
                                            textProperty: 'Code',
                                            fieldLabel: 'Статус плательщика',
                                            store: 'B4.store.dict.GISGMPPayerStatus',
                                            flex: 1,
                                            editable: false,
                                            allowBlank: false,
                                            columns: [
                                                { text: 'Код', dataIndex: 'Code', flex: 0.5, filter: { xtype: 'textfield' } },
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    itemId: 'fsUrParams',
                                    defaults: {
                                        labelWidth: 250,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    title: 'Реквизиты плательщика - Юр.лицо',
                                    hidden: true,
                                    items: [                                     
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 80,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'INN',
                                                    itemId: 'dfINN',
                                                    fieldLabel: 'ИНН',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 10,
                                                    regex: /^(\d{10})$/,
                                                    regexText: '10 цифр'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'KPP',
                                                    itemId: 'dfKPP',
                                                    fieldLabel: 'КПП',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 9,
                                                    regex: /^([^0^\D]\d|\d[^0^\D])\d{2}[A-Z0-9]{2}\d{3}$/,
                                                    regexText: 'Не соответствует формату КПП'
                                                },
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 150,
                                                labelAlign: 'right',
                                            },
                                            items: [                                                  
                                                {
                                                    xtype: 'checkbox',
                                                    itemId: 'dfIsRF',
                                                    name: 'IsRF',
                                                    fieldLabel: 'Резидент РФ',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    labelWidth: 120,
                                                    editable: true
                                                },
                                                {
                                                   xtype: 'combobox',
                                                   name: 'IdentifierType',
                                                   fieldLabel: 'Идентификатор ЮЛ',
                                                   displayField: 'Display',
                                                   itemId: 'dfIdentifierType',
                                                   flex:1,
                                                   store: B4.enums.IdentifierType.getStore(),
                                                   valueField: 'Value',
                                                   allowBlank: true,
                                                   editable: false
                                                },
                                                {   
                                                    xtype: 'textfield',
                                                    name: 'KIO',
                                                    fieldLabel: 'КИО',
                                                    allowBlank: true,
                                                    itemId: 'dfKIO',
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 5,
                                                    regex: /^(\d{5})$/
                                                }, 

                                            ]
                                        },
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    itemId: 'fsFizParams',
                                    defaults: {
                                        labelWidth: 150,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    title: 'Реквизиты плательщика - физ.лицо',
                                    hidden: true,
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'PhysicalPersonDocType',
                                            fieldLabel: 'Вид документа',
                                            store: 'B4.store.dict.PhysicalPersonDocType',
                                            editable: false,
                                            flex: 1,
                                            itemId: 'dfPhysicalPersonDocType',
                                            allowBlank: true,
                                            columns: [
                                                { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }

                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentSerial',
                                            itemId: 'dfDocumentSerial',
                                            fieldLabel: 'Серия документа',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: true,
                                            maxLength: 20
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNumber',
                                            itemId: 'dfDocumentNumber',
                                            fieldLabel: 'Номер документа',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: true,
                                            maxLength: 20
                                        },
                                        //{
                                        //    xtype: 'textfield',
                                        //    name: 'KIO',
                                        //    fieldLabel: 'КИО',
                                        //    allowBlank: true,
                                        //    itemId: 'dfKIO',
                                        //    disabled: false,
                                        //    flex: 1,
                                        //    editable: true,
                                        //    maxLength: 20,
                                        //    regex: /^(\d{5})$/
                                        //}                                        
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    itemId: 'fsIpParams',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    title: 'Реквизиты плательщика - ИП',
                                    items: [
                                    {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 80,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'INN',
                                                    itemId: 'dfINN2',
                                                    fieldLabel: 'ИНН',
                                                    allowBlank: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 12,
                                                    regex: /^(\d{12})$/,
                                                    regexText: '12 цифр'
                                                },
                                             ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        labelWidth: 150,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    title: 'Реквизиты платежа',
                                    items: [

                                                //{
                                                //    xtype: 'b4selectfield',
                                                //    name: 'Protocol',
                                                //    textProperty: 'DocumentNumber',
                                                //    fieldLabel: 'Постановление',
                                                //    store: 'B4.store.ListResolutionForGisGMP',
                                                //    flex: 1,
                                                //    editable: false,
                                                //    allowBlank: true,
                                                //    columns: [
                                                //        { text: 'Номер документа', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                                                //        { xtype: 'datecolumn', text: 'Дата', dataIndex: 'DocumentDate', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } }
                                                //    ]
                                                //},
                                                {
                                                    xtype: 'combobox',
                                                    name: 'TypeLicenseRequest',
                                                    fieldLabel: 'Вид основания',
                                                    displayField: 'Display',
                                                    itemId: 'dfTypeLicenseRequest',
                                                    flex: 1,
                                                    store: B4.enums.TypeLicenseRequest.getStore(),
                                                    valueField: 'Value',
                                                    allowBlank: true,
                                                    editable: false
                                                },
                                                  {
                                                      xtype: 'b4selectfield',
                                                      name: 'Protocol',
                                                      textProperty: 'DocumentNumber',
                                                      fieldLabel: 'Постановление',
                                                      itemId: 'dfProtocol',
                                                      store: 'B4.store.ListResolutionForGisGMP',
                                                      flex: 1,
                                                      editable: false,
                                                      allowBlank: true,
                                                      columns: [
                                                          { text: 'Номер документа', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                                                          { xtype: 'datecolumn', text: 'Дата', dataIndex: 'DocumentDate', flex: 0.5, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                                                          { xtype: 'gridcolumn', text: 'Тип документа', dataIndex: 'TypeDocumentGji', flex: 1,  renderer: function(val) {
                                                              return B4.enums.TypeDocumentGji.displayRenderer(val);
                                                          }, 
                                                          filter: {
                                                              xtype: 'combobox',
                                                              store: B4.enums.TypeDocumentGji.getItemsWithEmpty([null, '-']),
                                                              operand: CondExpr.operands.eq,
                                                              editable: false
                                                          } }
                                                      ]
                                                },
                                                  {
                                                      xtype: 'b4selectfield',
                                                      name: 'LicenseReissuance',
                                                      textProperty: 'RegisterNum',
                                                      itemId: 'dfLicenseReissuance',
                                                      fieldLabel: 'Заявка на переоформление лицензии',
                                                      store: 'B4.store.ListReissuanceForGisGMP',
                                                      flex: 1,
                                                      editable: false,
                                                      allowBlank: true,
                                                      hidden: true,
                                                      columns: [
                                                          { text: 'Номер документа', dataIndex: 'RegisterNum', flex: 1, filter: { xtype: 'textfield' } },
                                                          { xtype: 'datecolumn', format: 'd.m.Y', text: 'Дата', dataIndex: 'ReissuanceDate', flex: 0.5, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                                                          { text: 'Лицензиат', dataIndex: 'Contragent', flex: 1, filter: { xtype: 'textfield' } }                                                          
                                                      ]
                                                  },  
                                                  {
                                                      xtype: 'b4selectfield',
                                                      name: 'ManOrgLicenseRequest',
                                                      textProperty: 'RegisterNumber',
                                                      itemId: 'dfManOrgLicenseRequest',
                                                      fieldLabel: 'Заявка на выдачу лицензии',
                                                      store: 'B4.store.ListLicRequestForGisGMP',
                                                      flex: 1,
                                                      editable: false,
                                                      allowBlank: true,
                                                      hidden: true,
                                                      columns: [
                                                          { text: 'Номер документа', dataIndex: 'RegisterNumber', flex: 1, filter: { xtype: 'textfield' } },
                                                          { xtype: 'datecolumn', format: 'd.m.Y', text: 'Дата', dataIndex: 'DateRequest', flex: 0.5, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                                                          { text: 'Контрагент', dataIndex: 'Contragent', flex: 1, filter: { xtype: 'textfield' } }  
                                                      ]
                                                  }, 

                                                {
                                                    xtype: 'textfield',
                                                    name: 'BillFor',
                                                    itemId: 'dfBillFor',
                                                    fieldLabel: 'Назначение платежа',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true,
                                                    maxLength: 255,
                                                    regex: /^\S+[\S\s]*\S+$/,
                                                    regexText: 'Не соответствует формату ГИС ГМП'
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 80,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'numberfield',
                                                    name: 'TotalAmount',
                                                    itemId: 'dfTotalAmount',
                                                    fieldLabel: 'Сумма',
                                                    decimalSeparator: ',',
                                                    minValue: 0,
                                                    allowBlank: false,
                                                    flex: 0.5,
                                                },    
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y H:i:s',
                                                    labelWidth: 150,
                                                    allowBlank: false,
                                                    name: 'BillDate',
                                                    fieldLabel: 'Дата начисления',
                                                    itemId: 'dfBillDate',
                                                    flex: 0.5,
                                                },                                            
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 80,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'KBK',
                                                    itemId: 'dfKBK',
                                                    fieldLabel: 'КБК',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 0.5,
                                                    editable: true,
                                                    maxLength: 50,
                                                    regex: /^(\d{20})$/
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OKTMO',
                                                    itemId: 'dfOKTMO',
                                                    fieldLabel: 'ОКТМО',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 0.5,
                                                    editable: true,
                                                    maxLength: 20,
                                                    regex: /^(\d{8}|\d{11})$/
                                                },
                                            ]
                                        },
                                    ]
                                },                                
                                {
                                    xtype: 'tabpanel',
                                    border: false,
                                    flex: 1,
                                    defaults: {
                                        border: false
                                    },
                                    items: [
                                        {
                                            xtype: 'gisgmpfileinfogrid',
                                            flex: 1
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 170,
                                falign: 'stretch',
                                labelAlign: 'right'
                            },
                            hidden: true,
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            title: 'Получение информации об оплатах',
                            border: false,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        margin: '0 0 5 0',
                                        labelWidth: 150,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        //{
                                        //    xtype: 'button',
                                        //    text: 'Запросить платежи',
                                        //    tooltip: 'Запросить платежи',
                                        //    iconCls: 'icon-accept',
                                        //    itemId: 'sendPayButton'
                                        //},
                                        //{
                                        //    xtype: 'button',
                                        //    text: 'Проверить ответ',
                                        //    tooltip: 'Проверить ответ',
                                        //    iconCls: 'icon-accept',
                                        //    itemId: 'getPayStatusButton'
                                        //},
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        margin: '5 0 5 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            name: 'GisGmpPaymentsKind',
                                            fieldLabel: 'Вид информации о платежах',
                                            displayField: 'Display',
                                            itemId: 'dfGisGmpPaymentsKind',
                                            flex: 1,
                                            store: B4.enums.GisGmpPaymentsKind.getStore(),
                                            valueField: 'Value',
                                            allowBlank: true,
                                            editable: false
                                        },
                                        {
                                            xtype: 'combobox',
                                            name: 'GisGmpPaymentsType',
                                            fieldLabel: 'Вид информации о платежах',
                                            displayField: 'Display',
                                            itemId: 'dfGisGmpPaymentsType',
                                            flex: 1,
                                            store: B4.enums.GisGmpPaymentsType.getStore(),
                                            valueField: 'Value',
                                            allowBlank: true,
                                            editable: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        margin: '5 0 5 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'PaymentKBK',
                                            itemId: 'dfPaymentKBK',
                                            fieldLabel: 'Оплаты по КБК',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: true,
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            allowBlank: true,
                                            name: 'GetPaymentsStartDate',
                                            fieldLabel: 'Дата оплат с',
                                            itemId: 'dfGetPaymentsStartDate',
                                            flex: 0.5,
                                        }, 
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            allowBlank: true,
                                            name: 'GetPaymentsEndDate',
                                            fieldLabel: 'Дата оплат по',
                                            itemId: 'dfGetPaymentsEndDate',
                                            flex: 0.5,
                                        }
                                    ]
                                },
                                {
                                    xtype: 'tabpanel',
                                    border: false,
                                    flex: 1,
                                    defaults: {
                                        border: false
                                    },
                                    items: [
                                        {
                                            xtype: 'gisgmpfileinfogrid',
                                            flex: 1
                                        }
                                        
                                    ]
                                }

                            ]
                        },
                        {                            
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            title: 'Все оплаты по начислению',
                            border: false,
                            bodyPadding: 10,
                            //xtype: 'container',
                            //layout: 'vbox',

                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ReconcileAnswer',
                                    itemId: 'dfReconcileAnswer',
                                    fieldLabel: 'Ответ ГИС ГМП на запрос квитирования',
                                    allowBlank: true,
                                    disabled: false,
                                    labelWidth: 230,
                                    labelAlign: 'right',
                                    editable: false,
                                    flex: 1,

                                },
                                {
                                    xtype: 'gisgmppaymentsgrid',
                                    flex: 1
                                }

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
                            xtype: 'textfield',
                            name: 'Answer',
                            itemId: 'dfAnswer',
                            fieldLabel: 'Ответ ГИС ГМП',
                            allowBlank: true,
                            disabled: false,
                            labelWidth: 100,
                            labelAlign: 'right',
                            editable: false,
                            flex: 0.7
                        },
                        {
                            xtype: 'textfield',
                            name: 'UIN',
                            itemId: 'dfUIN',
                            fieldLabel: 'УИН',
                            allowBlank: true,
                            disabled: false,
                            labelWidth: 50,
                            labelAlign: 'right',
                            editable: false,
                            flex: 0.3
                        },
                        //{
                        //    xtype: 'tbfill'
                        //},                        
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});