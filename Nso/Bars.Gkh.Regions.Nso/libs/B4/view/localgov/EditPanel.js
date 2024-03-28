//ToDo данный файл перекрывается поскольку добавляются новые дополнительные поля из сущности NsoLocalGovernment


Ext.define('B4.view.localgov.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    width: 500,
    minWidth: 535,
    bodyPadding: 5,
    itemId: 'localGovEditPanel',
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.Contragent',
        'B4.ux.button.Save',
        'B4.store.dict.Municipality',
        'B4.view.Control.GkhTriggerField',
        
        'B4.enums.OrgStateRole'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Общая информация',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Contragent',
                            fieldLabel: 'Контрагент',
                            store: 'B4.store.Contragent',
                            allowBlank: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            editable: false
                        },
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'municipalityInspectors',
                            itemId: 'localgovMunicipalitiesTrigerField',
                            fieldLabel: 'Муниципальные образования',
                            editable: false
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Статус',
                            store: B4.enums.OrgStateRole.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'OrgStateRole',
                            editable: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'NameDepartamentGkh',
                            fieldLabel: 'Наименование подразделения, ответственного за ЖКХ',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Описание',
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Контакты',
                    items: [
                        {
                            xtype: 'container',
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 190,
                                labelAlign: 'right',
                                padding: '5px',
                                flex:1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Fio',
                                    fieldLabel: 'ФИО'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Email',
                                    fieldLabel: 'E-mail'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 190,
                                labelAlign: 'right',
                                padding: '5px',
                                flex: 1,
                                allowBlank: false
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Phone',
                                    fieldLabel: 'Телефон'
                                    
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'OfficialSite',
                                    fieldLabel: 'Официальный сайт'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    title: 'Регламенты осуществления МЖК',
                    items: [
                        {
                            xtype: 'container',
                            padding: '5 0',
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 190,
                                flex: 1,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'RegNumNotice',
                                    fieldLabel: 'Рег. номер уведомления',
                                    maxLength: 100
                                },
                                {
                                    xtype: 'datefield',
                                    width: 300,
                                    name: 'RegDateNotice',
                                    fieldLabel: 'Дата регистрации уведомления',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5 0',
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 190,
                                flex: 1,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'NumNpa',
                                    fieldLabel: '№ НПА органа МО',
                                    maxLength: 100
                                },
                                {
                                    xtype: 'datefield',
                                    width: 300,
                                    name: 'DateNpa',
                                    fieldLabel: 'Дата НПА органа МО',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            padding: '5 0',
                            name: 'NameNpa',
                            anchor: '100%',
                            fieldLabel: 'Наименование НПА',
                            maxLength: 300
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Регламенты взаимодействия органа МЖК и ГЖИ',
                    items: [
                        {
                            xtype: 'container',
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                padding: '5px 5px',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    flex:1,
                                    defaults: {
                                        labelWidth: 190,
                                        labelAlign: 'right'
                                    },
                                    title: 'МО',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            anchor: '100%',
                                            name: 'InteractionMuNum',
                                            fieldLabel: 'Номер ',
                                            maxLength: 100
                                        },
                                        {
                                            xtype: 'datefield',
                                            width: 300,
                                            name: 'InteractionMuDate',
                                            fieldLabel: 'Дата',
                                            format: 'd.m.Y'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'component',
                                    width: 10
                                },
                                {
                                    xtype: 'fieldset',
                                    flex: 1,
                                    defaults: {
                                        labelWidth: 190,
                                        labelAlign: 'right'
                                    },
                                    title: 'ГЖИ',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            anchor: '100%',
                                            name: 'InteractionGjiNum',
                                            fieldLabel: 'Номер ',
                                            maxLength: 100
                                        },
                                        {
                                            xtype: 'datefield',
                                            width: 300,
                                            name: 'InteractionGjiDate',
                                            fieldLabel: 'Дата',
                                            format: 'd.m.Y'
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});