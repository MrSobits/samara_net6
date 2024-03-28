Ext.define('B4.view.disposal.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.disposaleditpanel',
    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'disposalEditPanel',
    title: '',
    autoScroll: true,

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.Inspector',
        'B4.store.PoliticAuthority',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.Control.GkhTriggerField',
        'B4.view.disposal.TypeSurveyGrid',
        'B4.view.disposal.AnnexGrid',
        'B4.view.disposal.ExpertGrid',
        'B4.view.disposal.ProvidedDocGrid',
        'B4.view.disposal.SubjectVerificationGrid',
        'B4.view.disposal.DocConfirmGrid',
        'B4.enums.TypeAgreementResult',
        'B4.enums.TypeAgreementProsecutor',
        'B4.enums.YesNo',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.disposal.SurveyPurposeGrid',
        'B4.view.disposal.SurveyObjectiveGrid',
        'B4.view.disposal.InspFoundationGrid',
        'B4.view.disposal.InspFoundationCheckGrid',
        'B4.view.disposal.AdminRegulationGrid'
    ],

    initComponent: function() {
        var me = this;

        me.title = B4.DisposalTextValues.getSubjectiveCase();

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    overflowY: 'hidden',
                    overflowX: 'hidden',
                    id: 'disposalTopPanel',
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
                                    labelWidth: 50,
                                    width: 200,
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    labelWidth: 140,
                                    width: 295,
                                    name: 'DocumentNumber',
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 300
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
                                    width: 200,
                                    name: 'DocumentYear',
                                    itemId: 'nfDocumentYear',
                                    fieldLabel: 'Год',
                                    labelWidth: 50,
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
                                    xtype: 'textfield',
                                    labelAlign: 'right',
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
                    itemId: 'disposalTabPanel',
                    border: false,
                    flex: 1,
                    autoScroll: true,
                    listeners: {
                        render: function(p){
                            p.body.on('scroll', function (e) {
                                var elementDisposalTopPanel = Ext.getCmp('disposalTopPanel').body.dom;
                                elementDisposalTopPanel.scrollLeft = e.target.scrollLeft;
                                elementDisposalTopPanel.scrollTop = e.target.scrollTop;
                            }, p);
                        }
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Реквизиты',
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
                                                    editable: false
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
                                                            labelWidth: 170,
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
                                                            labelWidth: 170,
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
                                                            fieldLabel: 'Время с ',
                                                            name: 'TimeVisitStart',
                                                            xtype: 'timefield',
                                                            format: 'H:i',
                                                            labelWidth: 170,
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
                                                    labelWidth: 170,
                                                    maxLength: 500
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    boxLabel: 'Выезд инспектора в командировку',
                                                    name: 'OutInspector',
                                                    itemId: 'cbOutInspector',
                                                    padding: '0 0 0 175'
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
                                                    store: 'B4.store.PoliticAuthority',
                                                    textProperty: 'ContragentName',
                                                    name: 'PoliticAuthority',
                                                    fieldLabel: "Орган прокуратуры",
                                                    columns: [
                                                        { header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                                                        { header: 'Контрагент', xtype: 'gridcolumn', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } },
                                                        { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                                    ],
                                                    dockedItems: [
                                                        {
                                                            xtype: 'b4pagingtoolbar',
                                                            displayInfo: true,
                                                            store: 'B4.store.PoliticAuthority',
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
                                                fieldLabel: 'ДЛ, вынесшее ' + B4.DisposalTextValues.getSubjectiveCase(),
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
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.Inspector',
                                            name: 'ResponsibleExecution',
                                            itemId: 'sflResponsibleExecution',
                                            fieldLabel: 'Ответственный за исполнение',
                                            textProperty: 'Fio',
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
                                            editable: false
                                        },
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'disposalInspectors',
                                            itemId: 'trigFInspectors',
                                            fieldLabel: 'Инспекторы',
                                            allowBlank: false,
                                            editable: false
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
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    flex: 1,
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelWidth: 190,
                                                        labelAlign: 'right',
                                                        width: 300
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            format: 'd.m.Y',
                                                            name: 'NcDate',
                                                            fieldLabel: 'Дата'
                                                        },
                                                        {
                                                            xtype: 'datefield',
                                                            format: 'd.m.Y',
                                                            name: 'NcDateLatter',
                                                            fieldLabel: 'Дата исходящего письма'
                                                        },
                                                        {
                                                            xtype: 'combobox',
                                                            editable: false,
                                                            displayField: 'Display',
                                                            valueField: 'Value',
                                                            name: 'NcSent',
                                                            fieldLabel: 'Уведомление передано',
                                                            store: B4.enums.YesNo.getStore()
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 0 10',
                                                    flex: 1,
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelWidth: 190,
                                                        labelAlign: 'right',
                                                        width: 300
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'NcNum',
                                                            fieldLabel: 'Номер документа'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'NcNumLatter',
                                                            fieldLabel: 'Номер исходящего письма'
                                                        },
                                                        {
                                                            xtype: 'combobox',
                                                            editable: false,
                                                            displayField: 'Display',
                                                            valueField: 'Value',
                                                            name: 'NcObtained',
                                                            fieldLabel: 'Уведомление получено',
                                                            store: B4.enums.YesNo.getStore()
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Мотивированный запрос',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    flex: 1,
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelWidth: 190,
                                                        labelAlign: 'right',
                                                        width: 300
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'MotivatedRequestNumber',
                                                            fieldLabel: 'Номер запроса'
                                                        },
                                                        {
                                                            xtype: 'datefield',
                                                            format: 'd.m.Y',
                                                            name: 'MotivatedRequestDate',
                                                            fieldLabel: 'Дата запроса'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 0 10',
                                                    flex: 1,
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelWidth: 190,
                                                        labelAlign: 'right',
                                                        width: 300
                                                    }
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'FactViols',
                                    fieldLabel: 'Факты нарушения',
                                    editable: false,
                                    labelWidth: 190,
                                    labelAlign: 'right'
                                }
                            ]
                        },
                        {
                            xtype: 'disposaltypesurveygrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalsubjectverificationgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalexpertgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalsurveypurposegrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalsurveyobjectivegrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalinspfoundationcheckgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalinspfoundationgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposaladminregulationgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposalprovideddocgrid',
                            flex: 1
                        },
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Уведомление о проверке',
                            itemId: 'tabDisposalNoticeOfInspection',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            autoScroll: true,
                            defaults: {
                                labelWidth: 200,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype:'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'NoticeDateProtocol',
                                            fieldLabel: 'Дата составления протокола',
                                            format: 'd.m.Y',
                                            width: 300
                                        },
                                        {
                                            fieldLabel: 'Время составления протокола',
                                            name: 'NoticeTimeProtocol',
                                            xtype: 'timefield',
                                            format: 'H:i',
                                            submitFormat: 'Y-m-d H:i:s',
                                            minValue: '8:00',
                                            maxValue: '22:00',
                                            width: 300
                                        }
                                    ]
                                },
                                {
                                    padding: '5 0 0 0',
                                    xtype: 'textfield',
                                    fieldLabel: 'Место составления протокола',
                                    name: 'NoticePlaceCreation'
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'NoticeDescription',
                                    fieldLabel: 'Место и время сбора представителей',
                                    maxLength: 200,
                                    height: 50
                                }
                            ]
                        },
                        {
                            xtype: 'disposalannexgrid',
                            flex: 1
                        },
                        {
                            xtype: 'disposaldocconfirm',
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