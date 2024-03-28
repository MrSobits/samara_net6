Ext.define('B4.view.mkdlicrequest.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.mkdlicrequesteditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 1200,
    minWidth: 800,
    height: 650,
    resizable: true,
    bodyPadding: 3,
    itemId: 'mkdlicrequestEditWindow',
    title: 'Заявление',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',    
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.MKDLicTypeRequest',
        'B4.enums.LicStatementResult',
        'B4.enums.DisputeResult',      
        'B4.store.Contragent',
        'B4.store.dict.Inspector',
        'B4.view.mkdlicrequest.FileGrid',
        'B4.view.mkdlicrequest.RealityObjectGrid',
        'B4.form.ComboBox'     
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelAlign: 'right'
                    },
                    title: 'Заявление',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '5',
                            defaults: {
                                labelWidth: 80,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ExecutantDocGji',
                                    itemId: 'sfMExecutantDocGji',
                                    textProperty: 'Name',
                                    labelWidth: 110,
                                    flex: 1,
                                    allowBlank: false,
                                    labelAlign: 'right',
                                    fieldLabel: 'Статус заявителя',
                                    store: 'B4.store.dict.ExecutantDocGji',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    editable: false
                                },
                                //{
                                //    xtype: 'b4combobox',
                                //    itemId: 'cbExecutant',
                                //    name: 'Executant',
                                //    allowBlank: false,
                                //    editable: false,
                                //    labelWidth: 110,
                                //    flex: 1,
                                //    fieldLabel: 'Тип исполнителя',
                                //    fields: ['Id', 'Name', 'Code'],
                                //    url: '/ExecutantDocGji/List',
                                //    queryMode: 'local',
                                //    triggerAction: 'all'
                                //},
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    textProperty: 'ShortName',
                                    name: 'Contragent',
                                    flex: 1,
                                    fieldLabel: 'Юр. лицо',
                                    itemId: 'sfContragent',
                                    disabled: false,
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListWithoutPaging'
                                            }
                                        },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPerson',
                                    allowBlank: true,
                                    hidden: false,
                                    flex: 1,
                                    labelWidth: 70,
                                    fieldLabel: 'Физ. лицо'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'StatementNumber',
                                    allowBlank: false,
                                  //  labelWidth: 56,
                                    labelWidth: 110,
                                    fieldLabel: 'Номер заявления',
                                    flex: 0.5,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'StatementDate',
                                    allowBlank: false,
                                    labelWidth: 110,
                                    fieldLabel: 'Дата регистрации заявления',
                                    format: 'd.m.Y',
                                    flex: 0.5
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'MKDLicTypeRequest',
                                    itemId: 'sfMKDLicTypeRequest',
                                    textProperty: 'Name',
                                    labelWidth: 140,
                                    flex: 1,
                                    allowBlank: false,
                                    labelAlign: 'right',
                                    fieldLabel: 'Содержание заявления',
                                    store: 'B4.store.dict.MKDLicTypeRequest',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    editable: false
                                },
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            items: [
                         
                                //{
                                //    xtype: 'b4selectfield',
                                //    name: 'RealityObject',
                                //    itemId: 'sfRealityObject',
                                //    textProperty: 'Address',
                                //   // labelWidth: 43,
                                //    labelWidth: 110,
                                //    flex: 1,
                                //    allowBlank: false,
                                //    labelAlign: 'right',
                                //    fieldLabel: 'Адрес МКД',
                                //    store: 'B4.store.RealityObject',
                                //    columns: [
                                //        {
                                //            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                                //            filter: {
                                //                xtype: 'b4combobox',
                                //                operand: CondExpr.operands.eq,
                                //                storeAutoLoad: false,
                                //                hideLabel: true,
                                //                editable: false,
                                //                valueField: 'Name',
                                //                emptyItem: { Name: '-' },
                                //                url: '/Municipality/ListWithoutPaging'
                                //            }
                                //        },
                                //        { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                                //    ],
                                //    editable: false
                                //},
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    textProperty: 'ShortName',
                                    name: 'StatmentContragent',
                                    flex: 1,
                                    fieldLabel: 'Контрагент',
                                    itemId: 'sfStatmentContragent',
                                    disabled: false,
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListWithoutPaging'
                                            }
                                        },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                }
                            ]
                        }
                        
                    ]
                },         
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelAlign: 'right'
                    },
                    title: 'Рассмотрение',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '5',
                            defaults: {
                                labelWidth: 110,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'disposalInspectors',
                                    itemId: 'trigFInspectors',
                                    fieldLabel: 'Исполнители',
                                    flex: 3,
                                    allowBlank: true,
                                    editable: false
                                },
                                //{
                                //    xtype: 'b4selectfield',
                                //    name: 'Inspector',
                                //    itemId: 'sfInspector',
                                //    textProperty: 'Fio',
                                //    labelWidth: 110,
                                //    flex: 1,
                                //    allowBlank: false,
                                //    labelAlign: 'right',
                                //    fieldLabel: 'Исполнитель',
                                //    store: 'B4.store.dict.Inspector',
                                //    columns: [
                                //        { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } },
                                //        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } }
                                //    ],
                                //    editable: false
                                //},
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'LicStatementResult',
                                    labelWidth: 160,
                                    fieldLabel: 'Результат рассмотрения',
                                    enumName: 'B4.enums.LicStatementResult',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ConclusionNumber',
                                    allowBlank: true,
                                    //  labelWidth: 56,
                                    labelWidth: 110,
                                    fieldLabel: 'Номер решения',
                                    flex: 0.5,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ConclusionDate',
                                    allowBlank: true,
                                    labelWidth: 50,
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    flex: 0.5
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'LicStatementResultComment',
                                    fieldLabel: 'Описание',
                                    maxLength: 1000,
                                    flex: 1,
                                    allowBlank: true
                                }
                            ]
                        }
                        
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelAlign: 'right'
                    },
                    title: 'Обжалование',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '5',
                            defaults: {
                                labelWidth: 110,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbObjection',
                                    name: 'Objection',
                                    boxLabel: 'Обжалуется'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'ObjectionResult',
                                    itemId: 'cbObjectionResult',
                                    disabled:true,
                                    labelWidth: 160,
                                    fieldLabel: 'Результат обжалования',
                                    enumName: 'B4.enums.DisputeResult',
                                    flex: 1
                                }
                            ]
                        }                        

                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'mkdlicrequestTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'mkdlicrequestrogrid',
                            disabled: true,
                            flex: 1
                        },
                        {
                            xtype: 'mkdlicrequestfilegrid',
                            disabled: true,
                            flex: 1
                        },                        
                        {
                            xtype: 'mkdLicrequestquerygrid',
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
                                    text: 'Перейти к судебной практике',
                                    iconCls: 'icon-application-go',
                                    textAlign: 'left',
                                    itemId: 'btnCourtPractice'
                                },
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