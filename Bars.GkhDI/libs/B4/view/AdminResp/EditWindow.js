Ext.define('B4.view.adminresp.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 800,
    height: 520,
    minHeight: 500,
    minWidth: 800,
    bodyPadding: 5,
    itemId: 'adminRespEditWindow',
    title: 'Редактирование записи',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',        
        'B4.form.SelectField',
        'B4.form.FileField',        
        'B4.store.dict.SupervisoryOrg',        
        'B4.view.adminresp.ActionsGrid',
        'B4.enums.TypePersonAdminResp'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 220,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'tabpanel',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    flex:1,
                    //height: 500,
                    border: false,
                    items: [
                        {
                            xtype: 'panel',
                            flex: 1,
                            border: false,
                            frame: true,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 220,
                                labelAlign: 'right'
                            },
                            title: 'Общие сведения',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Лицо, привлеченное к административной ответственности',
                                    anchor: '100%',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 220,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            floating: false,
                                            name: 'TypePerson',
                                            fieldLabel: 'Тип лица, привлеченного к административной ответственности',
                                            displayField: 'Display',
                                            store: B4.enums.TypePersonAdminResp.getStore(),
                                            valueField: 'Value',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Fio',
                                            fieldLabel: 'ФИО должностного лица'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Position',
                                            fieldLabel: 'Должность'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'SupervisoryOrg',
                                    fieldLabel: 'Контролирующий орган',
                   

                                    store: 'B4.store.dict.SupervisoryOrg',
                                    allowBlank: false,
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        labelWidth: 220
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            name: 'AmountViolation',
                                            fieldLabel: 'Количество выявленных нарушений',
                                            allowDecimals: false
                                        },
                                        {
                                            id: 'dateImpositionPenalty',
                                            xtype: 'datefield',
                                            name: 'DateImpositionPenalty',
                                            fieldLabel: 'Дата наложения штрафа',
                                            allowBlank: false,
                                            format: 'd.m.Y'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '0 0 5 0',
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        labelWidth: 220
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            name: 'SumPenalty',
                                            fieldLabel: 'Сумма штрафа',
                                            decimalSeparator: ','
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DatePaymentPenalty',
                                            fieldLabel: 'Дата оплаты штрафа',
                                            format: 'd.m.Y'
                                        } 
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'TypeViolation',
                                    fieldLabel: 'Предмет административного нарушения'
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Документ о применении мер административного воздействия',
                                    anchor: '100%',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 220,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentName',
                                            maxLength: 300,
                                            fieldLabel: 'Наименование документа'
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1,
                                                labelWidth: 220
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    maxLength: 300,
                                                    name: 'DocumentNum',
                                                    fieldLabel: 'Номер'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateFrom',
                                                    fieldLabel: 'Дата',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            fieldLabel: 'Документ',
                                            allowBlank: false,
                                            name: 'File',
                                            maxFileSize:15728640,
                                            possibleFileExtensions: 'odt,ods,odp,doc,docx,xls,xlsx,ppt,tif,tiff,pptx,txt,dat,jpg,jpeg,png,pdf,gif,rtf'
                                        },
                                        {
                                            xtype: 'label',
                                            html: '<div style="padding-left:225px">'
                                                + 'Допустимые расширения файлов: .odt, .ods, .odp, .doc, .docx, .xls, .xlsx, .ppt, .tif, .tiff, .pptx, .txt, .dat, .jpg, .jpeg, .png, .pdf, .gif, .rtf'
                                                + '<div/>'
                                        },
                                        {
                                            xtype: 'label',
                                            html: '<div style="padding-left:225px">'
                                                + 'Максимальный размер файла: 15Мб'
                                                + '<div/>'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'adminrespactionsgrid',
                            flex: 1
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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