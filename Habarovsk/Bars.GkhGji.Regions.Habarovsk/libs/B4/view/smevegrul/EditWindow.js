Ext.define('B4.view.smevegrul.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        //'B4.enums.MVDTypeAddress',
        'B4.store.dict.RegionCodeMVD',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.SelectField',
        'B4.view.smevegrul.FileInfoGrid',
        'B4.enums.InnOgrn'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'smevegrulEditWindow',
    title: 'Запрос в ЕГРЮЛ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [{
                xtype: 'tabpanel',
                border: false,
                flex: 1,
                defaults: {
                    border: false
                },
                items: [{
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 100,
                            margin: '5 0 5 0',
                            align: 'stretch',
                            labelAlign: 'right'
                        },
                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        title: 'Форма запроса',
                        border: false,
                        bodyPadding: 10,
                        items: [{
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 250,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Реквизиты субъекта запроса',
                                items: [{
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 170,
                                            labelAlign: 'right',
                                        },
                                        items: [{
                                                xtype: 'combobox',
                                                name: 'InnOgrn',
                                                fieldLabel: 'Вид идентификатора',
                                                displayField: 'Display',
                                                itemId: 'dfInnOgrn',
                                                width: 400,
                                                store: B4.enums.InnOgrn.getStore(),
                                                valueField: 'Value',
                                                allowBlank: false,
                                                editable: false
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'INNReq',
                                                fieldLabel: 'ИНН/ОГРН',
                                                allowBlank: false,
                                                width: 450,
                                                disabled: false,
                                                editable: true,
                                                maxLength: 20,
                                                regex: /^(\d{10}|\d{12})$/,
                                                itemId: 'dfInn'
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
                                items: [{
                                    xtype: 'smevegrulfileinfogrid',
                                    flex: 1
                                }]
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
                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        title: 'Ответ',
                        border: false,
                        bodyPadding: 10,
                        items: [{
                                xtype: 'container',
                                layout: 'hbox',
                                defaults: {
                                    xtype: 'combobox',
                                    margin: '0 0 5 0',                                    
                                    labelAlign: 'right',
                                },
                                items: [{
                                        xtype: 'button',
                                        text: 'Получить сведения',
                                        tooltip: 'Получить сведения',
                                        iconCls: 'icon-accept',
                                        width: 200,
                                        //    action: 'romExecute',
                                        itemId: 'sendGetrequestButton'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'Answer',
                                        fieldLabel: 'Ответ на запрос',
                                        itemId: 'dfAnswerGet',
                                        allowBlank: true,
                                        disabled: false,
                                        flex: 1,
                                        editable: false,
                                        maxLength: 1000,
                                        labelWidth: 100,
                                    },
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 250,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Общие сведения о юридическом лице',
                                items: [{
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 80,
                                            labelAlign: 'right',
                                        },
                                        items: [{
                                                xtype: 'datefield',
                                                name: 'OGRNDate',
                                                fieldLabel: 'Дата ОРГН',
                                                format: 'd.m.Y',
                                                labelWidth: 150,
                                                //   flex: 0.5,
                                                allowBlank: true,

                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'OGRN',
                                                fieldLabel: 'ОГРН',
                                                allowBlank: true,
                                                width: 300,
                                                //    flex: 1,
                                                disabled: false,
                                                editable: false,
                                                maxLength: 20
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'INN',
                                                fieldLabel: 'ИНН',
                                                allowBlank: true,
                                                width: 200,
                                                //   flex: 1,
                                                disabled: false,
                                                editable: false,
                                                maxLength: 20
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //    margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [{
                                                xtype: 'textfield',
                                                name: 'KPP',
                                                fieldLabel: 'КПП',
                                                allowBlank: true,
                                                disabled: false,
                                                editable: false,
                                                flex: 0.5,
                                                maxLength: 50
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'OPFName',
                                                fieldLabel: 'ОПФ',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                maxLength: 50
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //    margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [{
                                            xtype: 'textfield',
                                            name: 'Name',
                                            fieldLabel: 'Полное наименование',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: false,
                                            maxLength: 1000
                                        }]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //    margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [{
                                            xtype: 'textfield',
                                            name: 'ShortName',
                                            fieldLabel: 'Краткое наименование',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: false,
                                            maxLength: 1000
                                        }]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //    margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [{
                                            xtype: 'textfield',
                                            name: 'AddressUL',
                                            fieldLabel: 'Адрес регистрации',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 1,
                                            editable: false,
                                            maxLength: 1000
                                        }]
                                    }

                                ]
                            },
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 250,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Регистрационные данные',
                                items: [{
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                        },
                                        items: [{
                                                xtype: 'textfield',
                                                name: 'CreateWayName',
                                                fieldLabel: 'Вид регистрации',
                                                allowBlank: true,
                                                flex: 1,
                                                //    flex: 1,
                                                disabled: false,
                                                editable: false

                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'CodeRegOrg',
                                                fieldLabel: 'Код органа',
                                                allowBlank: true,
                                                flex: 0.5,
                                                disabled: false,
                                                editable: false,
                                                maxLength: 20
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //    margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [{
                                                xtype: 'textfield',
                                                name: 'RegOrgName',
                                                fieldLabel: 'Регистрирующий орган',
                                                allowBlank: true,
                                                disabled: false,
                                                editable: false,
                                                flex: 1
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'AddressRegOrg',
                                                fieldLabel: 'Адрес органа',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //    margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [{
                                                xtype: 'textfield',
                                                name: 'AuthorizedCapitalType',
                                                fieldLabel: 'Тип уставного капиталла',
                                                allowBlank: true,
                                                disabled: false,
                                                flex: 1,
                                                editable: false,
                                                maxLength: 1000
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'AuthorizedCapitalAmmount',
                                                fieldLabel: 'Размер уставного капиталла',
                                                allowBlank: true,
                                                disabled: false,
                                                flex: 1,
                                                editable: false,
                                                maxLength: 50
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //    margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [

                                            {
                                                xtype: 'textfield',
                                                name: 'Pozition',
                                                fieldLabel: 'Должность',
                                                allowBlank: true,
                                                disabled: false,
                                                flex: 1,
                                                editable: false,
                                                maxLength: 1000
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'FIO',
                                                fieldLabel: 'ФИО',
                                                allowBlank: true,
                                                disabled: false,
                                                flex: 1,
                                                editable: false,
                                                maxLength: 1000
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //    margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [{
                                                xtype: 'textfield',
                                                name: 'OKVEDCodes',
                                                fieldLabel: 'ОКВЭД',
                                                allowBlank: true,
                                                disabled: false,
                                                flex: 0.5,
                                                editable: false,
                                                maxLength: 1000
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'OKVEDNames',
                                                fieldLabel: 'Виды деятельности',
                                                allowBlank: true,
                                                disabled: false,
                                                flex: 1,
                                                editable: false,
                                                maxLength: 1000
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //    margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [{
                                            xtype: 'textfield',
                                            name: 'StateNameUL',
                                            fieldLabel: 'Статус ЮЛ',
                                            allowBlank: true,
                                            disabled: false,
                                            flex: 0.5,
                                            editable: false,
                                            maxLength: 1000
                                        }]
                                    }

                                ]
                            }
                        ]
                    }
                ]
            }],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                        xtype: 'buttongroup',
                        columns: 3,
                        items: [{
                                xtype: 'b4savebutton'
                            },
                            {
                                xtype: 'gkhbuttonprint'
                            },
                            {
                                xtype: 'button',
                                iconCls: 'icon-page',
                                action: 'PrintExtract',
                                //href: '/ExtractPrinter/PrintExtractForClaimWork/?id=',
                                text: 'Выписка в PDF',
                                tooltip: 'Выписка в PDF'
                            }
                        ]
                    },
                    {
                        xtype: 'tbfill'
                    },
                    {
                        xtype: 'buttongroup',
                        columns: 2,
                        items: [{
                            xtype: 'b4closebutton'
                        }]
                    }
                ]
            }]
        });

        me.callParent(arguments);
    }
});