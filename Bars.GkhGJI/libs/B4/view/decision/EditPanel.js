Ext.define('B4.view.decision.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.decisioneditpanel',
    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'decisionEditPanel',
    title: 'Решение',
    autoScroll: true,

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.Inspector',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.Control.GkhTriggerField',
        'B4.enums.TypeAgreementResult',
        'B4.enums.TypeAgreementProsecutor',
        'B4.enums.YesNo',
        'B4.enums.RiskCategory',
        'B4.form.FileField',
        'B4.form.EnumCombo',
        'B4.enums.KindKNDGJI',
        'B4.store.dict.ProsecutorOffice',
        'B4.view.decision.AdminRegulationGrid',
        'B4.view.decision.DecisionControlMeasuresGrid',
        'B4.view.decision.VerificationGrid',
        'B4.view.decision.ProvidedDocGrid',
        'B4.view.decision.ControlListGrid',
        'B4.view.decision.InspectionReasonGrid',
        'B4.view.decision.ControlSubjectGrid',
        'B4.view.GjiDocumentCreateButton'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    overflowY: 'hidden',
                    overflowX: 'hidden',
                    id: 'decisionTopPanel',
                    border: false,
                    frame: true,
                    defaults: {
                        border: false,
                        labelWidth: 170,
                        xtype: 'panel',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            padding: '10px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelWidth: 170,
                                    width: 300,
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата и время решения',
                                    format: 'd.m.Y',
                                    allowBlank: true
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'HourOfProceedings',
                                    margin: '0 0 0 10',
                                    fieldLabel: '',
                                    width: 45,
                                    maxValue: 23,
                                    minValue: 0
                                },
                                {
                                    xtype: 'label',
                                    text: ':',
                                    margin: '5'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'MinuteOfProceedings',
                                    width: 45,
                                    maxValue: 59,
                                    minValue: 0
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 140,
                                    width: 295,
                                    name: 'DocumentNumber',
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 140,
                                    width: 295,
                                    name: 'ERPID',
                                    itemId: 'tfERPID',
                                    fieldLabel: 'Номер в ЕРКНМ',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'button',
                                    text: 'Разместить в ЕРКНМ',
                                    name: 'calculateButton',
                                    tooltip: 'Разместить в ЕРКНМ',
                                    action: 'ERKNMRequest',
                                    iconCls: 'icon-accept',
                                    itemId: 'calculateButton'
                                }
                            ]
                        },
                        {
                            padding: '0 15px 10px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            defaults: {
                                xtype: 'gkhintfield'
                            },
                            items: [
                                {
                                    width: 300,
                                    name: 'DocumentYear',
                                    itemId: 'nfDocumentYear',
                                    fieldLabel: 'Год',
                                    labelWidth: 170,
                                    hideTrigger: true
                                },
                                {
                                    width: 295,
                                    name: 'DocumentNum',
                                    itemId: 'nfDocumentNum',
                                    fieldLabel: 'Номер',
                                    labelWidth: 140,
                                    hideTrigger: true
                                },
                                {
                                    name: 'LiteralNum',
                                    itemId: 'nfLiteralNum',
                                    fieldLabel: 'Буквенный подномер',
                                    labelAlign: 'right',
                                    xtype: 'textfield',
                                    labelWidth: 140,
                                    width: 295
                                },
                                {
                                    name: 'DocumentSubNum',
                                    itemId: 'nfDocumentSubNum',
                                    fieldLabel: 'Подномер',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'decisionTabPanel',
                    border: false,
                    flex: 1,
                    autoScroll: true,
                    listeners: {
                        render: function(p){
                            p.body.on('scroll', function (e) {
                                var elementDisposalTopPanel = Ext.getCmp('decisionTopPanel').body.dom;
                                elementDisposalTopPanel.scrollLeft = e.target.scrollLeft;
                                elementDisposalTopPanel.scrollTop = e.target.scrollTop;
                            }, p);
                        }
                    },
                    items: [
                        {
                            xtype: 'form',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Реквизиты',
                            name: 'Requisite',
                            border: false,
                            bodyPadding: 5,
                            frame: true,
                            autoScroll: true,
                            minWidth: 900,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            flex: .7,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right',
                                                readOnly: true,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'disposalBaseName',
                                                    itemId: 'tfBaseName',
                                                    fieldLabel: 'Основание обследования'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'disposalPlanName',
                                                    itemId: 'tfPlanName',
                                                    fieldLabel: 'Документ основания'
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'KindCheck',
                                                    fieldLabel: 'Вид проверки',
                                                    displayField: 'Name',
                                                    url: '/KindCheckGji/List',
                                                    valueField: 'Id',
                                                    itemId: 'cbTypeCheck',
                                                    readOnly: false,
                                                    editable: false,
                                                    allowBlank: false
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'KindKNDGJI',
                                                    fieldLabel: 'Вид контроля(надзора)',
                                                    displayField: 'Display',
                                                    itemId: 'cbKindKNDGJI',
                                                    store: B4.enums.KindKNDGJI.getStore(),
                                                    valueField: 'Value',
                                                    allowBlank: false,
                                                    readOnly: false,
                                                    editable: false
                                                },
                                                {
                                                    xtype: 'b4enumcombo',
                                                    enumName: 'B4.enums.RiskCategory',
                                                    name: 'RiskCategory',
                                                    readOnly: false,
                                                    fieldLabel: 'Категория риска'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            //padding: '0 0 5 0',
                                            flex: 1,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        labelAlign: 'right',
                                                        allowBlank: false,
                                                        format: 'd.m.Y'
                                                    },
                                                    items: [
                                                        {
                                                            name: 'DateStart',
                                                            itemId: 'dfDateStart',
                                                            fieldLabel: 'Период проведения проверки с',
                                                            labelWidth: 200,
                                                            flex: 0.6
                                                        },
                                                        {
                                                            name: 'DateEnd',
                                                            itemId: 'dfDateEnd',
                                                            fieldLabel: 'по',
                                                            labelWidth: 60,
                                                            flex: 0.4
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        labelAlign: 'right',
                                                        format: 'd.m.Y'
                                                    },
                                                    items: [
                                                        {
                                                            name: 'ObjectVisitStart',
                                                            itemId: 'dfObjectVisitStart',
                                                            fieldLabel: 'Выезд на объект с',
                                                            labelWidth: 200,
                                                            flex: 0.6
                                                        },
                                                        {
                                                            name: 'ObjectVisitEnd',
                                                            itemId: 'dfObjectVisitEnd',
                                                            fieldLabel: 'по',
                                                            labelWidth: 60,
                                                            flex: 0.4
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '5 0 0 0',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        labelAlign: 'right',
                                                        format: 'd.m.Y'
                                                    },
                                                    items: [
                                                        {
                                                            fieldLabel: 'Время с',
                                                            name: 'TimeVisitStart',
                                                            xtype: 'timefield',
                                                            format: 'H:i',
                                                            labelWidth: 200,
                                                            submitFormat: 'Y-m-d H:i:s',
                                                            minValue: '8:00',
                                                            maxValue: '22:00',
                                                            flex: 0.6
                                                        },
                                                        {
                                                            fieldLabel: 'по',
                                                            name: 'TimeVisitEnd',
                                                            xtype: 'timefield',
                                                            format: 'H:i',
                                                            labelWidth: 60,
                                                            submitFormat: 'Y-m-d H:i:s',
                                                            minValue: '8:00',
                                                            maxValue: '22:00',
                                                            flex: 0.4
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    padding: '5 0 0 0',
                                                    name: 'PeriodCorrect',
                                                    fieldLabel: 'Срок проверки',
                                                    labelAlign: 'right',
                                                    labelWidth: 200,
                                                    maxLength: 500
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    boxLabel: 'Выезд инспектора в командировку',
                                                    name: 'OutInspector',
                                                    itemId: 'cbOutInspector',
                                                    padding: '0 0 0 205'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 190,
                                        labelAlign: 'right'
                                    },
                                    title: 'Согласование с прокуратурой',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                editable: false,
                                                displayField: 'Display',
                                                valueField: 'Value',
                                                readOnly: false,
                                                labelWidth: 180,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    name: 'TypeAgreementProsecutor',
                                                    itemId: 'cbTypeAgreementProsecutor',
                                                    fieldLabel: 'Согласование с прокуратурой',
                                                    store: B4.enums.TypeAgreementProsecutor.getStore(),
                                                    flex: 0.7
                                                },
                                                {
                                                    name: 'TypeAgreementResult',
                                                    itemId: 'cbTypeAgreementResult',
                                                    fieldLabel: 'Результат согласования',
                                                    store: B4.enums.TypeAgreementResult.getStore(),
                                                    flex: 1,
                                                    labelWidth: 170
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '5 0 0 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    store: 'B4.store.dict.ProsecutorOffice',
                                                    textProperty: 'Name',
                                                    name: 'ProsecutorOffice',
                                                    fieldLabel: "Орган прокуратуры",
                                                    columns: [
                                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                                    ],
                                                    dockedItems: [
                                                        {
                                                            xtype: 'b4pagingtoolbar',
                                                            displayInfo: true,
                                                            store: 'B4.store.dict.ProsecutorOffice',
                                                            dock: 'bottom'
                                                        }
                                                    ],
                                                    flex: 0.7,
                                                    editable: false
                                                },
                                                {
                                                    xtype: 'container',
                                                    flex: 1,
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            format: 'd.m.Y',
                                                            labelWidth: 250,
                                                            labelAlign: 'right',
                                                            name: 'DateStatement',
                                                            width: 350,
                                                            fieldLabel: 'Дата и время формирования заявления'
                                                        },
                                                        {
                                                            name: 'TimeStatement',
                                                            xtype: 'timefield',
                                                            format: 'H:i',
                                                            submitFormat: 'Y-m-d H:i:s',
                                                            padding: '0 0 0 5',
                                                            width: 60,
                                                            minValue: '8:00',
                                                            maxValue: '22:00'
                                                        },
                                                        {
                                                            xtype: 'component',
                                                            flex: 1
                                                        }
                                                    ]
                                                }
                                                
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '5 0 0 0',
                                            itemId: 'approveContainer',
                                            hidden:true,
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    labelAlign: 'right',
                                                    name: 'ProcAprooveDate',
                                                    flex: 1,
                                                    fieldLabel: 'Дата согласования'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    padding: '5 0 0 0',
                                                    name: 'ProcAprooveNum',
                                                    fieldLabel: 'Номер документа о согласовании',
                                                    labelAlign: 'right',
                                                    flex: 1,
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'b4filefield',
                                                    name: 'ProcAprooveFile',
                                                    flex: 1,
                                                    fieldLabel: 'Файл документа о согласовании',
                                                    editable: false
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '5 0 0 0',
                                            itemId: 'approveresContainer',
                                            hidden: true,
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    padding: '5 0 0 0',
                                                    name: 'PositionProcAproove',
                                                    fieldLabel: 'Должность согласовавшего',
                                                    labelAlign: 'right',
                                                    flex: 1,
                                                    maxLength: 50
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    padding: '5 0 0 0',
                                                    name: 'FioProcAproove',
                                                    fieldLabel: 'ФИО согласовавшего',
                                                    labelAlign: 'right',
                                                    flex: 1,
                                                    maxLength: 250
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '5 0 0 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    labelWidth: 180,
                                                    name: 'ProsecutorDecDate',
                                                    fieldLabel: 'Дата решения прокурора'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 165,
                                                    name: 'ProsecutorDecNumber',
                                                    fieldLabel: 'Номер решения прокурора'
                                                }
                                            ]
                                        }
                                      ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Уведомление о проверке',
                    
                                    itemId: 'noticeFieldset',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                anchor: '100%',
                                                labelWidth: 150,
                                                labelAlign: 'right'
                                            },
                                            items: [                                               
      
                                                {
                                                    xtype: 'textfield',
                                                    name: 'NcNum',
                                                    flex: 1,
                                                    labelWidth: 160,
                                                    fieldLabel: 'Номер документа'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    name: 'NcDate',
                                                    flex: 1,
                                                    fieldLabel: 'Дата'
                                                },                                                       
                                                {
                                                    xtype: 'combobox',
                                                    editable: false,
                                                    displayField: 'Display',
                                                    valueField: 'Value',
                                                    name: 'NcSent',
                                                    flex: 1,
                                                    fieldLabel: 'Уведомление передано',
                                                    store: B4.enums.YesNo.getStore()
                                                }                                     
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Требование о предоставлении документов',

                                    itemId: 'requirFieldset',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                anchor: '100%',
                                                labelWidth: 150,
                                                labelAlign: 'right'
                                            },
                                            items: [

                                                {
                                                    xtype: 'textfield',
                                                    name: 'RequirNum',
                                                    flex: 1,
                                                    labelWidth: 160,
                                                    fieldLabel: 'Номер документа'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    format: 'd.m.Y',
                                                    name: 'RequirDate',
                                                    flex: 1,
                                                    fieldLabel: 'Дата'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 190,
                                        labelAlign: 'right'
                                    },
                                    title: 'Должностные лица',
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            textProperty: 'Fio',
                                            name: 'IssuedDisposal',
                                            fieldLabel: 'ДЛ, вынесшее решение',
                                            columns: [
                                                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            dockedItems: [
                                                {
                                                    xtype: 'b4pagingtoolbar',
                                                    displayInfo: true,
                                                    store: 'B4.store.dict.Inspector',
                                                    dock: 'bottom'
                                                }
                                            ],
                                            itemId: 'sfIssuredDisposal',
                                            allowBlank: false,
                                            editable: false
                                    },
                                    {
                                        xtype: 'gkhtriggerfield',
                                        name: 'decisionInspectors',
                                        itemId: 'trigFInspectors',
                                        fieldLabel: 'Инспекторы',
                                        allowBlank: false,
                                        editable: false
                                    }
                                        ]
                                },
                                {
                                    xtype: 'tabtextarea',
                                    margin: 5,
                                    height: 70,
                                    fieldLabel: 'Дополнительная информация',
                                    name: 'AdditionalInfo',
                                    itemId: 'taAdditionalInfo',
                                    maxLength: 2000
                                },
                            ]
                        },
                        {
                            xtype: 'decisioninspectionreasongrid',
                            flex: 1
                        },
                        {
                            xtype: 'decisioncontrollistgrid',
                            flex: 1
                        },
                        {
                            xtype: 'decisioncontrolsubjectgrid',
                            flex: 1
                        },
                        {
                            xtype: 'decisionadminregulationgrid',
                            flex: 1
                        },
                        {
                            xtype: 'decisioncontrolmeasuresgrid',
                            flex: 1
                        },                        
                        {
                            xtype: 'decisionannexgrid',
                            flex: 1
                        },
                        {
                            xtype: 'decisionexpertgrid',
                            flex: 1
                        },
                        {
                            xtype: 'decisionverifsubjpanel',
                            flex: 1
                        },
                        {
                            xtype: 'decisionprovideddocgrid',
                            flex: 1
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGJIToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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