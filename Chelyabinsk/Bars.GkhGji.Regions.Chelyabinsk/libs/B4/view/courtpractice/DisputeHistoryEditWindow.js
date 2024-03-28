﻿Ext.define('B4.view.courtpractice.DisputeHistoryEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.cpDisputeHistoryeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 1200,
    minWidth: 800,
    height: 500,
    resizable: true,
    bodyPadding: 3,
    itemId: 'cpDisputeHistoryEditwindow',
    title: 'Судебное дело',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [        
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhIntField',
        'B4.store.courtpractice.DocsForSelect',
        'B4.view.Control.GkhButtonPrint',
        'B4.store.dict.RevenueSourceGji',
        'B4.store.dict.RevenueFormGji',
        'B4.enums.TypeDocumentGji',
        'B4.form.FileField',
        'B4.store.Contragent',
        'B4.enums.CourtPracticeState',
        'B4.enums.CourtMeetingResult',
        'B4.enums.LawyerInspector',
        'B4.enums.DisputeCategory',
        'B4.enums.DisputeType',
        'B4.store.dict.TypeFactViolation',
        'B4.store.dict.JurInstitution',
        'B4.GjiTextValuesOverride',
        'B4.view.courtpractice.RealityObjectGrid',
        'B4.view.courtpractice.InspectorGrid',
        'B4.view.courtpractice.FileGrid',
        'B4.form.ComboBox',
        'B4.store.dict.InstanceGji',
        'B4.view.Control.GkhTriggerField',
        //'B4.view.courtpractice.ZonalInspGrid',
        //'B4.view.courtpractice.AnswerGrid',
        'B4.store.dict.Inspector',
        'B4.store.appealcits.AdmonitionForSelect'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    margins: '10 0 5 0',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            allowBlank: false,
                            labelWidth: 70,
                            fieldLabel: 'Номер дела',
                            flex: 0.5,
                            maxLength: 50
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'JurInstitution',
                            itemId: 'sfJurInstitution',
                            textProperty: 'Name',
                            labelWidth: 40,
                            flex: 1,
                            allowBlank: false,
                            labelAlign: 'right',
                            fieldLabel: 'Суд',
                            store: 'B4.store.dict.JurInstitution',
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            editable: false
                        },
                        {
                            xtype: 'combobox',
                            name: 'CourtPracticeState',
                            fieldLabel: 'Статус рассмотрения',
                            displayField: 'Display',
                            itemId: 'cbCourtPracticeState',
                            labelWidth: 150,
                            flex: 1,
                            store: B4.enums.CourtPracticeState.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'textarea',
                            name: 'PausedComment',
                            fieldLabel: 'Комментарий',
                            allowBlank: true,
                            flex: 1,
                            hidden: true,
                            itemId: 'tfPausedComment',
                            labelWidth: 90,
                            height: 40,
                            disabled: false,
                            editable: true
                        }
                        //{
                        //    xtype: 'b4selectfield',
                        //    name: 'TypeFactViolation',
                        //    itemId: 'sfTypeFactViolation',
                        //    textProperty: 'Name',
                        //    labelWidth: 120,
                        //    flex: 2,
                        //    allowBlank: false,
                        //    labelAlign: 'right',
                        //    fieldLabel: 'Суд',
                        //    store: 'B4.store.dict.TypeFactViolation',
                        //    columns: [
                        //        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                        //    ],
                        //    editable: false
                        //}
                    ]    
                },                
                {
                    xtype: 'container',
                    layout: 'hbox',
                    margins: '0 0 5 0',
                    defaults: {
                        labelWidth: 60,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'DisputeType',
                            fieldLabel: 'Вид спора',
                            displayField: 'Display',
                            itemId: 'cbDisputeType',
                            labelWidth: 60,
                            flex: 1.5,
                            store: B4.enums.DisputeType.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'combobox',
                            name: 'DisputeCategory',
                            fieldLabel: 'Категория спора',
                            displayField: 'Display',
                            itemId: 'cbDisputeCategory',
                            labelWidth: 60,
                            flex: 1,
                            store: B4.enums.DisputeCategory.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'TypeFactViolation',
                            itemId: 'sfTypeFactViolation',
                            textProperty: 'Name',
                            labelWidth: 50,
                            flex: 1.5,
                            allowBlank: false,
                            labelAlign: 'right',
                            fieldLabel: 'Предмет спора',
                            store: 'B4.store.dict.TypeFactViolation',
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            editable: false
                        },                   
                      
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'courtPracticeDoc',
                    itemId: 'sfDocumentGji',
                    fieldLabel: 'Документы ГЖИ'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Admonition',
                    itemId: 'admon',
                    store: 'B4.store.appealcits.AdmonitionForSelect',
                    textProperty: 'DocumentNumber',
                    fieldLabel: 'Предостережение',
                    editable: false,
                    columns: [
                        {
                            xtype: 'datecolumn',
                            format: 'd.m.Y',
                            text: 'Дата документа',
                            flex: 2,
                            dataIndex: 'DocumentDate',
                            filter: { xtype: 'datefield' }
                        },
                        {
                            text: 'Документ',
                            flex: 1,
                            dataIndex: 'DocumentName',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Номер документа',
                            flex: 3,
                            dataIndex: 'DocumentNumber',
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'MKDLicRequest',
                    itemId: 'sfMKDLicRequest',
                    textProperty: 'StatementNumber',
                    allowBlank: true,
                    labelAlign: 'right',
                    fieldLabel: 'Заявление',
                    store: 'B4.store.mkdlicrequest.MKDLicRequest',
                    columns: [
                        { text: 'Номер заявления', dataIndex: 'StatementNumber', flex: 1, filter: { xtype: 'textfield' } },
                        { xtype: 'datecolumn', format: 'd.m.Y', text: 'Дата заявления', dataIndex: 'StatementDate', flex: 0.5, filter: { xtype: 'datefield' } },
                        { text: 'Заявитель', dataIndex: 'Contragent', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'AppealCitsDecision',
                    itemId: 'sfAppealCitsDecision',
                    textProperty: 'AppealNumber',
                    allowBlank: true,
                    labelAlign: 'right',
                    fieldLabel: 'Решение Х',
                    store: 'B4.store.appealcits.Decision',
                    columns: [
                        { text: 'Номер жалобы', dataIndex: 'AppealNumber', flex: 1, filter: { xtype: 'textfield' } },
                        { xtype: 'datecolumn', format: 'd.m.Y', text: 'Дата жалобы', dataIndex: 'AppealDate', flex: 0.5, filter: { xtype: 'datefield' } },
                        { text: 'Заявитель', dataIndex: 'Apellant', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'courtpracticeTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            autoScroll: true,
                            title: 'Рассмотрение',
                            border: false,
                            bodyPadding: 3,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'panel',
                                    padding: '0 0 3 0',
                                    padding: '5px 15px 5px 15px',
                                    bodyStyle: Gkh.bodyStyle,
                                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                    border: false,
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 80,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DateCourtMeeting',
                                            labelAlign: 'left',
                                            fieldLabel: 'Дата с/з',
                                            format: 'd.m.Y',
                                            labelWidth: 50,
                                            width: 200
                                        },                                       
                                        {
                                            xtype: 'numberfield',
                                            name: 'FormatHour',
                                            margin: '0 0 0 10',
                                            fieldLabel: 'Время с/з',
                                            labelWidth: 60,
                                            width: 120,
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
                                            name: 'FormatMinute',
                                            width: 45,
                                            maxValue: 59,
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'combobox',
                                            name: 'DisputeResult',
                                            fieldLabel: 'Результат обжалования',
                                            displayField: 'Display',
                                            itemId: 'cbCourtMeetingResult',
                                            labelWidth: 160,
                                            store: B4.enums.CourtMeetingResult.getStore(),
                                            valueField: 'Value',
                                            width: 500,
                                            allowBlank: false,
                                            editable: false
                                        },
                                    ]
                                },
                                {
                                    xtype: 'panel',
                                    padding: '0 0 0 0',
                                    margin: '0 5 5 0',
                                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                    border: false,
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 80,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                     
                                        //{
                                        //    fieldLabel: 'Время с/з',
                                        //    name: 'CourtMeetingTime',
                                        //    xtype: 'timefield',
                                        //    format: 'H:i',
                                        //    flex: 0.5,
                                        //    labelWidth: 80,
                                        //    submitFormat: 'Y-m-d H:i:s',
                                        //    minValue: '8:00',
                                        //    maxValue: '22:00'
                                        //},                                      
                                        {
                                            xtype: 'checkbox',
                                            itemId: 'cbInLaw',
                                            labelAlign: 'left',
                                            name: 'InLaw',
                                            flex: 0.3,
                                            fieldLabel: 'Вступило в законную силу',
                                            allowBlank: false,
                                            disabled: false,                                           
                                            labelWidth: 90,
                                            editable: true
                                        },
                                        {
                                            xtype: 'datefield',
                                            flex: 0.5,
                                            itemId: 'cbInLawDate',
                                            name: 'InLawDate',
                                            hidden:true,
                                            fieldLabel: 'Дата вступления в законную силу',
                                            labelWidth: 100,
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'checkbox',
                                            itemId: 'cbDispute',
                                            margin: '5 0 0 0',
                                            name: 'Dispute',
                                            flex: 0.3,
                                            fieldLabel: 'Обжалование',
                                            allowBlank: false,
                                            disabled: false,
                                            labelWidth: 100,
                                            editable: true
                                        },
                                        {
                                            xtype: 'b4combobox',
                                            labelAlign: 'right',
                                            name: 'InstanceGji',
                                            hidden: true,
                                            margin: '5 0 0 0',
                                            fieldLabel: 'Инстанция',
                                            fields: ['Id', 'Name', 'Code'],
                                            url: '/InstanceGji/List',
                                            queryMode: 'local',
                                            triggerAction: 'all',
                                            itemId: 'cbDisputeInstance',
                                            labelWidth: 100
                                        }
                                    ]
                                },                                
                                {
                                    xtype: 'panel',
                                    padding: '0 0 0 0',
                                    margin: '5 5 5 5',
                                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                    border: false,
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 40,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            itemId: 'cbCourtCosts',
                                            name: 'CourtCosts',
                                            labelAlign: 'left',
                                            flex: 0.2,
                                            fieldLabel: 'Судебные расходы',
                                            allowBlank: false,
                                            disabled: false,
                                            labelWidth: 85,
                                            editable: true
                                        }, 
                                        {
                                            xtype: 'numberfield',
                                            name: 'CourtCostsPlan',
                                            itemId: 'nfCourtCostsPlan',
                                            fieldLabel: 'Заявлено, руб.',
                                            hidden: true,
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            flex: 1,
                                            decimalSeparator: ',',
                                            labelWidth: 55,
                                            minValue: 0,
                                            allowBlank: true,
                                            flex: 0.5,
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'CourtCostsFact',
                                            itemId: 'nfCourtCostsFact',
                                            fieldLabel: 'Взыскано, руб.',
                                            hidden: true,
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            flex: 1,
                                            decimalSeparator: ',',
                                            minValue: 0,
                                            labelWidth: 80,
                                            allowBlank: true,
                                            flex: 0.5,
                                        },  
                                        {
                                            xtype: 'checkbox',
                                            itemId: 'cbInterimMeasures',
                                            name: 'InterimMeasures',
                                            flex: 0.3,
                                            fieldLabel: 'Обеспечительные меры',
                                            allowBlank: true,
                                            disabled: false,
                                            labelWidth: 120,
                                            editable: true
                                        },
                                        {
                                            xtype: 'datefield',
                                            flex: 0.5,
                                            itemId: 'cbInterimMeasuresDate',
                                            name: 'InterimMeasuresDate',
                                            hidden: true,
                                            fieldLabel: 'Дата принятия обеспечительных мер',
                                            labelWidth: 140,
                                            format: 'd.m.Y'
                                        }                                          
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Discription',
                                    fieldLabel: 'Описание',
                                    labelAlign: 'left',
                                    allowBlank: true,
                                    labelWidth: 80,
                                    disabled: false,
                                    editable: true
                                },
                                {
                                    xtype: 'panel',
                                    padding: '0 0 0 0',
                                    margin: '5 5 5 5',
                                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                    border: false,
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'PerformanceList',
                                            fieldLabel: 'Исполнительный лист',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'PerformanceProceeding',
                                            fieldLabel: 'Исполнительное производство',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: true
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
                            columns: 1,
                            items: [
  
                            ]
                        },
                        //{
                        //    xtype: 'buttongroup',
                        //    items: [
                        //        {
                        //            xtype: 'gkhbuttonprint',
                        //            itemId: 'btnPrint'
                        //        }
                        //    ]
                        //},
                        {
                            xtype: 'tbfill'
                        },                       
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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