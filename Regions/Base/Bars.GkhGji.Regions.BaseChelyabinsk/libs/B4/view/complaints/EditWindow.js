Ext.define('B4.view.complaints.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.complaintseditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
    },
    width: 1200,
    minWidth: 800,
    height: 650,
    resizable: true,
    bodyPadding: 10,
    itemId: 'complaintsEditWindow',
    title: 'Жалоба',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhIntField',    
        'B4.store.complaints.Decision',
        'B4.form.FileField',
        'B4.store.Contragent',
        'B4.view.complaints.FileInfoGrid',
        'B4.enums.CourtPracticeState',    
        'B4.form.ComboBox',
        'B4.enums.CompleteReject',
        'B4.store.dict.InstanceGji',
        'B4.enums.RequesterRole',
        'B4.enums.IdentityDocumentType',
        'B4.enums.Gender',
        'B4.view.complaints.ExecutantGrid',
        'B4.view.complaints.StepGrid',
        'B4.view.complaintsrequest.Grid',
        //'B4.view.courtpractice.ZonalInspGrid',
        //'B4.view.courtpractice.AnswerGrid',
        'B4.store.dict.Inspector'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
    
                {
                    xtype: 'container',
                    layout: 'hbox',
                    margins: '5 10 5 10',
                    defaults: {
                        labelWidth: 120,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'ComplaintId',
                            allowBlank: false,
                            fieldLabel: 'ИД Жалобы',
                            flex: 1,
                            maxLength: 50
                        },
                        {
                            xtype: 'textfield',
                            name: 'AppealNumber',
                            allowBlank: false,
                            fieldLabel: 'Номер проверки',
                            flex: 1,
                            maxLength: 50
                        },
                        {
                            xtype: 'textfield',
                            name: 'ComplaintDate',
                            allowBlank: false,
                            fieldLabel: 'Дата направления',
                            flex: 1,
                            maxLength: 50
                        },
                        {
                            xtype: 'textfield',
                            name: 'ComplaintState',
                            allowBlank: true,
                            fieldLabel: 'Статус в ТОР',
                            flex: 1,
                            maxLength: 100
                        },
                        {
                            xtype: 'button',
                            text: 'Обновить данные',
                            tooltip: 'Обновить данные по жалобе',
                            iconCls: 'icon-accept',
                            flex: 0.6,
                            itemId: 'sendCalculateButton'
                        }
                        //{
                        //    xtype: 'datefield',
                        //    name: 'RequestDate',
                        //    labelAlign: 'right',
                        //    flex: 1,
                        //    fieldLabel: 'Дата получения',
                        //    format: 'd.m.Y',
                        //}
                    ]    
                },             
                {
                    xtype: 'fieldset',
                    defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                align: 'stretch',
                                margin: '0 0 5 0',
                            },
                        title: 'Заявитель',
                        bodyPadding: 10,
                    items: [
                    {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '0 0 5 0',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right'
                            },
                        items: [
                                {
                                    xtype: 'combobox',
                                    name: 'RequesterRole',
                                    fieldLabel: 'Тип заявителя',
                                    displayField: 'Display',
                                    labelWidth: 120,
                                    itemId: 'cbRequesterRole',
                                    flex: 1,
                                    store: B4.enums.RequesterRole.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false,
                                    editable: false
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    textProperty: 'Name',
                                    name: 'RequesterContragent',
                                    fieldLabel: 'Контрагент',
                                    flex: 1,
                                    labelWidth: 80,
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    allowBlank: true
                                },                        
                                {
                                    xtype: 'textfield',
                                    name: 'LegalAddress',
                                    allowBlank: true,
                                    labelWidth: 120,
                                    fieldLabel: 'Адрес контрагента',
                                    flex: 1,
                                    maxLength: 500
                                }    
                            ]
                    },
                    {
                        xtype: 'container',
                        layout: 'hbox',
                        margin: '0 0 5 0',
                        defaults: {
                            labelWidth: 120,
                            labelAlign: 'right'
                        },
                        items: [
                            {
                                xtype: 'textfield',
                                name: 'WorkingPosition',
                                allowBlank: true,
                                fieldLabel: 'Должность',
                                flex: 1,
                                maxLength: 100
                            },                          
                            {
                                xtype: 'textfield',
                                name: 'RequesterFIO',
                                allowBlank: true,
                                labelWidth: 40,
                                fieldLabel: 'ФИО',
                                flex: 1,
                                maxLength: 100
                            },
                            {
                                xtype: 'textfield',
                                name: 'RegAddess',
                                allowBlank: true,
                                labelWidth: 120,
                                fieldLabel: 'Адрес регистрации',
                                flex: 1,
                                maxLength: 1500
                            }
                        ]
                    },
                    {
                        xtype: 'container',
                        layout: 'hbox',
                        margins: '0 0 5 0',
                        defaults: {
                            labelWidth: 120,
                            labelAlign: 'right'
                        },
                        items: [
                            {
                                xtype: 'combobox',
                                name: 'IdentityDocumentType',
                                fieldLabel: 'Тип документа',
                                displayField: 'Display',
                                itemId: 'cbIdentityDocumentType',
                                flex: 1,
                                store: B4.enums.IdentityDocumentType.getStore(),
                                valueField: 'Value',
                                allowBlank: false,
                                editable: false
                            },
                            {
                                xtype: 'textfield',
                                name: 'DocSeries',
                                allowBlank: true,
                                labelWidth: 60,
                                fieldLabel: 'Серия',
                                flex: 1,
                                maxLength: 100
                            },
                            {
                                xtype: 'textfield',
                                name: 'DocNumber',
                                allowBlank: true,
                                labelWidth: 60,
                                fieldLabel: 'Номер',
                                flex: 1,
                                maxLength: 50
                            },
                            {
                                xtype: 'textfield',
                                name: 'SNILS',
                                allowBlank: true,
                                labelWidth: 60,
                                fieldLabel: 'СНИЛС',
                                flex: 1,
                                maxLength: 50
                            }
                        ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margins: '0 0 5 0',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'INNFiz',
                                    allowBlank: true,
                                    labelWidth: 120,
                                    fieldLabel: 'ИНН физлица',
                                    flex: 1,
                                    maxLength: 100
                                }, 
                                {
                                    xtype: 'textfield',
                                    name: 'EsiaOid',
                                    allowBlank: true,
                                    labelWidth: 140,
                                    fieldLabel: 'ИД заявителя в ЕСИА',
                                    flex: 1,
                                    maxLength: 100
                                }
                            ]
                        },
                    {
                        xtype: 'container',
                        layout: 'hbox',
                        margins: '0 0 5 0',
                        defaults: {
                            labelWidth: 120,
                            labelAlign: 'right'
                        },
                        items: [
                            {
                                xtype: 'datefield',
                                name: 'BirthDate',
                                labelAlign: 'right',
                                flex: 1,
                                fieldLabel: 'Дата рождения',
                                format: 'd.m.Y',
                            },
                            {
                                xtype: 'textfield',
                                name: 'BirthAddress',
                                allowBlank: true,
                                labelWidth: 120,
                                fieldLabel: 'Место рождения',
                                flex: 1.5,
                                maxLength: 1000
                            },
                            {
                                xtype: 'combobox',
                                name: 'Gender',
                                fieldLabel: 'Пол',
                                displayField: 'Display',
                                itemId: 'cbGender',
                                labelWidth: 40,
                                flex: 0.5,
                                store: B4.enums.Gender.getStore(),
                                valueField: 'Value',
                                allowBlank: false,
                                editable: false
                            },
                            {
                                xtype: 'textfield',
                                name: 'Nationality',
                                allowBlank: true,
                                labelWidth: 120,
                                fieldLabel: 'Национальность',
                                flex: 1,
                                maxLength: 50
                            }
                        ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margins: '0 0 5 0',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right'
                            },
                            items: [

                                {
                                    xtype: 'textfield',
                                    name: 'MobilePhone',
                                    allowBlank: true,
                                    labelWidth: 120,
                                    fieldLabel: 'Моб. телефон',
                                    flex: 1,
                                    maxLength: 100
                                },                               
                                {
                                    xtype: 'textfield',
                                    name: 'Email',
                                    allowBlank: true,
                                    labelWidth: 60,
                                    fieldLabel: 'Email',
                                    flex: 1,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Ogrnip',
                                    allowBlank: true,
                                    labelWidth: 60,
                                    fieldLabel: 'ОГРНИП',
                                    flex: 1,
                                    maxLength: 50
                                }
                            ]
                        }
                    ]    
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 120,
                        labelAlign: 'right',
                        align: 'stretch',
                        margin: '0 0 5 0',
                    },
                    title: 'Жалоба',
                    bodyPadding: 10,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '0 0 5 0',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'TypeAppealDecision',
                                    allowBlank: true,
                                    labelWidth: 120,
                                    fieldLabel: 'Обжалуемое решение',
                                    flex: 1,
                                    maxLength: 1000
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfLifeEvent',
                                    name: 'LifeEvent',
                                    allowBlank: true,
                                    labelWidth: 120,
                                    fieldLabel: 'Жизненная ситуация',
                                    flex: 1,
                                    maxLength: 1500
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '0 0 5 0',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textarea',
                                    name: 'CommentInfo',
                                    itemId: 'tfCommentInfo',
                                    fieldLabel: 'Комментарий',
                                    allowBlank: true,
                                    disabled: false,
                                    flex: 1,
                                    editable: true
                                }
                            ]
                        }
                        
                        
                    ]
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
                                //{
                                //    xtype: 'textfield',
                                //    name: 'DecisionReason',
                                //    itemId: 'tfDecisionReason',
                                //    fieldLabel: 'Причина решения',
                                //    allowBlank: true,
                                //    disabled: false,
                                //    editable: true
                                //},
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.complaints.Decision',
                                    textProperty: 'FullName',
                                    itemId: 'sfSMEVComplaintsDecision',
                                    name: 'SMEVComplaintsDecision',
                                    fieldLabel: 'Решение',
                                    editable: false,
                                    columns: [
                                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'FullName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            xtype: 'b4enumcolumn',
                                            enumName: 'B4.enums.CompleteReject',
                                            dataIndex: 'CompleteReject',
                                            text: 'Тип',
                                            flex: 1,
                                            filter: true,
                                        }                                        
                                    ],
                                    allowBlank: true
                                },  
                                {
                                    xtype: 'panel',
                                    bodyStyle: Gkh.bodyStyle,
                                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                    border: false,
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textarea',
                                            name: 'Answer',
                                            itemId: 'tfAnswer',
                                            fieldLabel: 'Обоснование решения',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 3,
                                            editable: true
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'FileInfo',
                                            flex: 1,
                                            fieldLabel: 'Файл'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            autoScroll: true,
                            title: 'Ходатайства',
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
                                    bodyStyle: Gkh.bodyStyle,
                                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                    border: false,
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 150,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textarea',
                                            name: 'PauseResolutionPetition',
                                            itemId: 'tfPauseResolutionPetition',
                                            fieldLabel: 'Ходатайство о приостановлении исполнения решения',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: true
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'RenewTermPetition',
                                            itemId: 'tfRenewTermPetition',
                                            fieldLabel: 'Ходатайство о восстановлении срока рассмотрения жалобы',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: true
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'complaintsstepgrid',
                            disabled: true,
                            flex: 1
                        },
                        {
                            xtype: 'complaintsexecutantgrid',
                            disabled: true,
                            flex: 1
                        },
                        {
                            xtype: 'complaintsrequestgrid',
                            disabled:true,
                            flex: 1
                        },
                        {
                            xtype: 'complaintsfileinfogrid',
                            disabled: true,
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
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