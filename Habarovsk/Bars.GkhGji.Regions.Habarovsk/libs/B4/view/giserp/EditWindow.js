Ext.define('B4.view.giserp.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.ListDisposalForGisERP',
        'B4.store.dict.ProsecutorOffice',
        'B4.view.giserp.FileInfoGrid',
        'B4.view.giserp.GISERPResultViolationsGrid',
        'B4.enums.ERPAddressType',
        'B4.enums.ERPInspectionType',
        'B4.enums.ERPNoticeType',
        'B4.enums.ERPObjectType',
        'B4.enums.ERPReasonType',
        'B4.enums.KindKND',
        'B4.enums.ERPRiskType',
        'B4.enums.YesNoNotSet',
        'B4.enums.GisErpRequestType'
     
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'giserpEditWindow',
    title: 'Обмен данными с ГИС ЕРП',
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
                            title: 'Отправка информации о проверке',
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
                                            text: 'Отправить проверку',
                                            tooltip: 'Отправить проверку',
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
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ERPID',
                                            itemId: 'tfERPID',
                                            fieldLabel: 'ИД проверки',
                                            allowBlank: true,
                                            disabled: true,
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            editable: false,
                                            flex: 0.7
                                        }            
                                    ]
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'GisErpRequestType',
                                    fieldLabel: 'Тип запроса',
                                    displayField: 'Display',
                                    itemId: 'cbGisErpRequestType',
                                    flex: 1,
                                    store: B4.enums.GisErpRequestType.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false,
                                    editable: false
                                },                               
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        labelWidth: 150,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    title: 'Реквизиты проверки',
                                    items: [

                                        {
                                            xtype: 'b4selectfield',
                                            name: 'Disposal',
                                            textProperty: 'DocumentNumber',
                                            fieldLabel: 'Номер приказа',                                            
                                            itemId: 'dfProtocol',
                                            store: 'B4.store.ListDisposalForGisERP',
                                            flex: 1,
                                            editable: false,
                                            allowBlank: true,
                                            columns: [
                                                { text: 'Номер документа', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                                                { xtype: 'datecolumn', text: 'Дата', dataIndex: 'DocumentDate', flex: 0.5, format: 'd.m.Y', filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                                                { text: 'Статус', dataIndex: 'State', flex: 1, filter: { xtype: 'textfield' } },
                                                          
                                            ]
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'ProsecutorOffice',
                                            textProperty: 'Name',
                                            fieldLabel: 'Отдел прокуратуры',
                                            itemId: 'sfProsecutorOffice',
                                            store: 'B4.store.dict.ProsecutorOffice',
                                            flex: 1,
                                            editable: false,
                                            allowBlank: false,
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }                                             

                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'InspectionName',
                                            itemId: 'tfInspectionName',
                                            fieldLabel: 'Наименование проверки',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            maxLength: 512,
                                            editable: true,
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'CarryoutEvents',
                                            itemId: 'tfCarryoutEvents',
                                            fieldLabel: 'Мероприятия проверки',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: true
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'Goals',
                                            itemId: 'tfGoals',
                                            fieldLabel: 'Цели, задачи, предмет проверки',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: true
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
                                                    xtype: 'combobox',
                                                    name: 'ERPInspectionType',
                                                    fieldLabel: 'Тип проверки',
                                                    displayField: 'Display',
                                                    itemId: 'cbERPInspectionType',
                                                    flex: 1,
                                                    store: B4.enums.ERPInspectionType.getStore(),
                                                    valueField: 'Value',
                                                    allowBlank: false,
                                                    editable: false
                                                },
                                                {
                                                    xtype: 'combobox',
                                                    name: 'ERPNoticeType',
                                                    fieldLabel: 'Способ уведомления',
                                                    displayField: 'Display',
                                                    itemId: 'cbERPNoticeType',
                                                    flex: 1,
                                                    store: B4.enums.ERPNoticeType.getStore(),
                                                    valueField: 'Value',
                                                    allowBlank: false,
                                                    editable: false
                                                }                                              
                                                                                     
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
                                                    xtype: 'combobox',
                                                    name: 'ERPReasonType',
                                                    fieldLabel: 'Основание регистрации',
                                                    displayField: 'Display',
                                                    itemId: 'cbERPReasonType',
                                                    flex: 1,
                                                    labelWidth: 100,
                                                    store: B4.enums.ERPReasonType.getStore(),
                                                    valueField: 'Value',
                                                    allowBlank: false,
                                                    editable: false
                                                }                                               

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
                                                    xtype: 'combobox',
                                                    name: 'ERPRiskType',
                                                    fieldLabel: 'Категория риска',
                                                    displayField: 'Display',
                                                    itemId: 'cbERPRiskType',
                                                    flex: 1,
                                                    store: B4.enums.ERPRiskType.getStore(),
                                                    valueField: 'Value',
                                                    allowBlank: false,
                                                    editable: false
                                                },
                                                {
                                                    xtype: 'combobox',
                                                    name: 'ERPAddressType',
                                                    margin: '5 0 5 0',
                                                    fieldLabel: 'Тип места',
                                                    displayField: 'Display',
                                                    itemId: 'cbERPAddressType',
                                                    flex: 1,
                                                    store: B4.enums.ERPAddressType.getStore(),
                                                    valueField: 'Value',
                                                    allowBlank: false,
                                                    editable: false
                                                },
                                                {
                                                    xtype: 'combobox',
                                                    name: 'KindKND',
                                                    fieldLabel: 'Вид контроля (надзора)',
                                                    labelWidth: 100,
                                                    displayField: 'Display',
                                                    itemId: 'cbKindKND',
                                                    flex: 1,
                                                    store: B4.enums.KindKND.getStore(),
                                                    valueField: 'Value',
                                                    allowBlank: false,
                                                    editable: false
                                                }
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
                                                    name: 'SubjectAddress',
                                                    itemId: 'tfSubjectAddress',
                                                    fieldLabel: 'Адрес объекта',
                                                    allowBlank: false,
                                                    labelWidth: 110,
                                                    disabled: false,
                                                    flex: 2,
                                                    editable: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OKATO',
                                                    itemId: 'tfOKATO',
                                                    fieldLabel: 'Код территории',
                                                    allowBlank: false,
                                                    disabled: false,
                                                    flex: 1,
                                                    editable: true
                                                }
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
                                            layout: { type: 'vbox', align: 'stretch' },
                                            title: 'Результаты',
                                            border: false,
                                            bodyPadding: 3,
                                            frame: true,
                                            defaults: {
                                                labelWidth: 150,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'panel',
                                                    padding: '0 0 3 0',
                                                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                                    border: false,    
                                                    layout: 'hbox',
                                                    defaults: {
                                                        labelWidth: 120,
                                                        labelAlign: 'right',
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            flex: 1,
                                                            name: 'ACT_DATE_CREATE',
                                                            itemId: 'tfACT_DATE_CREATE',
                                                            fieldLabel: 'Дата акта',
                                                            format: 'd.m.Y'
                                                        },
                                                        {
                                                            xtype: 'datefield',
                                                            flex: 1,
                                                            name: 'START_DATE',
                                                            itemId: 'tfSTART_DATE',
                                                            fieldLabel: 'Дата проверки',
                                                            format: 'd.m.Y'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'DURATION_HOURS',
                                                            flex: 0.5,
                                                            itemId: 'tfDURATION_HOURS',
                                                            fieldLabel: 'Длительность, час.',
                                                            maxLength: 255
                                                        },
                                                    ]
                                                },
                                                {
                                                    xtype: 'panel',
                                                    padding: '0 0 3 0',
                                                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                                    border: false, 
                                                    layout: 'hbox',
                                                    defaults: {
                                                        labelWidth: 120,
                                                        labelAlign: 'right',
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'REPRESENTATIVE_POSITION',
                                                            itemId: 'tfREPRESENTATIVE_POSITION',
                                                            fieldLabel: 'Должность лица, присутствовавшего на проверке',
                                                            maxLength: 255
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'REPRESENTATIVE_FULL_NAME',
                                                            itemId: 'tfREPRESENTATIVE_FULL_NAME',
                                                            fieldLabel: 'ФИО лица, присутствовавшего на проверке',
                                                            maxLength: 255
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'panel',
                                                    padding: '0 0 3 0',
                                                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                                    border: false,
                                                    layout: 'hbox',
                                                    defaults: {
                                                        labelWidth: 120,
                                                        labelAlign: 'right',
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'combobox',
                                                            name: 'HasViolations',
                                                            fieldLabel: 'Нарушения выявлены',
                                                            displayField: 'Display',
                                                            itemId: 'cbHasViolations',
                                                            flex: 1,
                                                            store: B4.enums.YesNoNotSet.getStore(),
                                                            valueField: 'Value',
                                                            allowBlank: false,
                                                            editable: false
                                                        }      
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'giserpresultviolationsgrid',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'giserpfileinfogrid',
                                            flex: 1
                                        }
                                    ]
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
                            fieldLabel: 'Ответ ГИС ЕРП',
                            allowBlank: true,
                            disabled: false,
                            labelWidth: 100,
                            labelAlign: 'right',
                            editable: false,
                            flex: 0.7
                        }             
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});