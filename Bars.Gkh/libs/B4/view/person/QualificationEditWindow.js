Ext.define('B4.view.person.QualificationEditWindow',
{
    extend: 'B4.form.Window',
    alias: 'widget.personqualificationeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 900,
    minWidth: 700,
    title: 'Квалификационный аттестат',
    closeAction: 'destroy',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.enums.TypeCancelationQualCertificate',
        'B4.form.SelectField',
        'B4.store.person.RequestToExam',
        'B4.view.person.QualificationDocumentGrid',
        'B4.view.person.TechnicalMistakeGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me,
        {
            items: [
                {
                    xtype: 'tabpanel',
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6;',
                    bodyPadding: 5,
                    defaults: {
                        xtype: 'container',
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        }
                    },
                    items: [
                        {
                            title: 'Сведения об аттестате',
                            minHeight: 500,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'form',
                                    padding: '0 0 5px 0',
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'textfield',
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                flex: 1,
                                                maxLength: 100
                                            },
                                            items: [
                                                {
                                                    name: 'Number',
                                                    fieldLabel: 'Номер КА'
                                                },
                                                {
                                                    name: 'BlankNumber',
                                                    fieldLabel: 'Номер бланка'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'datefield',
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                flex: 1,
                                                format: 'd.m.Y'
                                            },
                                            items: [
                                                {
                                                    name: 'IssuedDate',
                                                    fieldLabel: 'Дата выдачи'
                                                },
                                                {
                                                    name: 'EndDate',
                                                    fieldLabel: 'Дата окончания действия'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'datefield',
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                flex: 1,
                                                format: 'd.m.Y'
                                            },
                                            items: [
                                                {
                                                    name: 'RecieveDate',
                                                    fieldLabel: 'Дата получения'
                                                },
                                                {
                                                    xtype: 'tbfill'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'RequestToExam',
                                            fieldLabel: 'Заявка на допуск к экзамену',
                                            store: 'B4.store.person.RequestToExam',
                                            editable: false,
                                            columns: [
                                                {
                                                    text: 'Номер',
                                                    dataIndex: 'RequestNum',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    xtype: 'datecolumn',
                                                    text: 'Дата',
                                                    dataIndex: 'RequestDate',
                                                    flex: 1,
                                                    filter: { xtype: 'datefield' },
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    text: 'Номер протокола',
                                                    dataIndex: 'ProtocolNum',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                flex: 1,
                                                maxLength: 100
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4filefield',
                                                    name: 'FileIssueApplication',
                                                    fieldLabel: 'Файл заявления о выдаче квалификационного аттестата'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'ApplicationDate',
                                                    fieldLabel:
                                                        'Дата подачи заявления о выдаче квалификационного аттестата'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'FileNotificationOfExamResults',
                                            fieldLabel: 'Уведомление лицензионной комиссии о результатах экзамена',
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'File',
                                            fieldLabel: 'Документ аттестата'
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'checkboxfield',
                                                    name: 'HasDuplicate',
                                                    fieldLabel: 'Выдан дубликат/переоформлен'
                                                },
                                                {
                                                    xtype: 'checkboxfield',
                                                    name: 'IsFromAnotherRegion',
                                                    fieldLabel: 'КА другого региона'
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'RegionCode',
                                                    fieldLabel: 'Наименование региона',
                                                    operand: CondExpr.operands.eq,
                                                    width: 400,
                                                    storeAutoLoad: true,
                                                    editable: false,
                                                    url: '/RegionCode/GetAll',
                                                    flex: 2
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'tabpanel',
                                    name: 'DublicatePanel',
                                    margin: '5px 0 0 0',
                                    flex: 1,
                                    type: 'duplicateSet',
                                    disabled: true,
                                    items: [
                                        {
                                            xtype: 'qualificationdocumentgrid',
                                            name: 'Duplicate',
                                            title: 'Дубликаты'
                                        },
                                        {
                                            xtype: 'qualificationdocumentgrid',
                                            name: 'Renew',
                                            title: 'Переоформление'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            title: 'Сведения об аннулировании',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200
                            },
                            items: [
                                {
                                    xtype: 'checkboxfield',
                                    name: 'HasCancelled',
                                    fieldLabel: 'Аттестат аннулирован'
                                },
                                {
                                    xtype: 'fieldset',
                                    type: 'cancelledSet',
                                    disabled: true,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    title: 'Сведения об аннулировании',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            padding: '0 0 5 0',
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'combobox',
                                                    editable: false,
                                                    fieldLabel: 'Основание',
                                                    store: B4.enums.TypeCancelationQualCertificate.getStore(),
                                                    displayField: 'Display',
                                                    valueField: 'Value',
                                                    name: 'TypeCancelation',
                                                    width: 200,
                                                    labelWidth: 200,
                                                    flex: 2

                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'CancelationDate',
                                                    fieldLabel: 'Дата',
                                                    format: 'd.m.Y',
                                                    labelWidth: 100,
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            padding: '0 0 10 0',
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'CancelNumber',
                                                    fieldLabel: 'Номер протокола',
                                                    maxLength: 100,
                                                    labelWidth: 200,
                                                    flex: 2
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'CancelProtocolDate',
                                                    fieldLabel: 'Дата протокола',
                                                    format: 'd.m.Y',
                                                    labelWidth: 100,
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'CancelFile',
                                            fieldLabel: 'Файл протокола',
                                            labelWidth: 200,
                                            labelAlign: 'right',
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'checkboxfield',
                                    name: 'HasRenewed',
                                    fieldLabel: 'Решение об аннулировании отменено'
                                },
                                {
                                    xtype: 'fieldset',
                                    type: 'renewSet',
                                    margin: 0,
                                    disabled: true,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right'
                                    },
                                    title: 'Сведения о факте отмены решения об аннулировании',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'CourtName',
                                            fieldLabel: 'Наименование суда',
                                            maxLength: 400
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 10 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'CourtActNumber',
                                                    fieldLabel: 'Номер судебного акта',
                                                    maxLength: 100
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'CourtActDate',
                                                    fieldLabel: 'Дата судебного акта',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'ActFile',
                                            fieldLabel: 'Файл акта',
                                            flex: 1
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'technicalmistakegrid',
                            layout: 'fit',
                            height: 500
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
                            items: [
                                { xtype: 'b4savebutton', name: 'main' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});