Ext.define('B4.view.jurinstitution.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    alias: 'widget.jurinstitutioneditwin',
    layout: { type: 'vbox', align: 'stretch' },
    width: 950,
    height: 700,
    bodyPadding: 5,
    title: 'Учреждение в судебной практике',
    requires: [
        'B4.form.FiasSelectAddress',
        'B4.form.EnumCombo',
        'B4.form.SelectField',

        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.enums.JurInstitutionType',
        'B4.enums.CourtType',
        'B4.store.dict.Municipality',
        'B4.view.jurinstitution.RealObjGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                flex: 1
            },
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            layout: 'anchor',
                            title: 'Общие сведения',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                anchor: '100%',
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                 {
                                     xtype: 'b4enumcombo',
                                     fieldLabel: 'Учреждение',
                                     enumName: 'B4.enums.JurInstitutionType',
                                     includeEmpty: false,
                                     name: 'JurInstitutionType',
                                     value: 10
                                 },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Тип суда',
                                    enumName: 'B4.enums.CourtType',
                                    includeEmpty: false,
                                    name: 'CourtType',
                                    value: 10
                                },
                                {
                                    xtype: 'b4selectfield',
                                    fieldLabel: 'Муниципальное образование',
                                    name: 'Municipality',
                                    store: 'B4.store.dict.Municipality',
                                    editable: false,
                                    allowBlank: false,
                                    columns: [
                                        {
                                            text: 'Наименование', dataIndex: 'Name', flex: 1,
                                            filter: {
                                                xtype: 'textfield'
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    fieldLabel: 'Полное наименование',
                                    allowBlank: false,
                                    maxLength: 250
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ShortName',
                                    fieldLabel: 'Краткое наименование',
                                    allowBlank: false,
                                    maxLength: 250
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Code',
                                    fieldLabel: 'Код',
                                    allowBlank: false,
                                    maxLength: 50
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Реквизиты',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4fiasselectaddress',
                                            labelWidth: 140,
                                            labelAlign: 'right',
                                            name: 'FiasAddress',
                                            flatIsReadOnly: true,
                                            fieldsToHideNames: ['tfFlat'],
                                            fieldsRegex: {
                                                tfHousing: {
                                                    regex: /^\d+$/,
                                                    regexText: 'В это поле можно вводить только цифры'
                                                },
                                                tfBuilding: {
                                                    regex: /^\d+$/,
                                                    regexText: 'В это поле можно вводить только цифры'
                                                }
                                            },
                                            fieldLabel: 'Адрес',
                                            allowBlank: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'OutsideAddress',
                                            fieldLabel: 'Адрес за пределами',
                                            allowBlank: false,
                                            maxLength: 500
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                xtype: 'textfield',
                                                labelWidth: 140,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    name: 'PostCode',
                                                    fieldLabel: 'Почтовый индекс',
                                                    maxLength: 6
                                                },
                                                {
                                                    name: 'Phone',
                                                    fieldLabel: 'Телефон',
                                                    maxLength: 250
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                xtype: 'textfield',
                                                labelWidth: 140,
                                                labelAlign: 'right',
                                                flex: 1,
                                                maxLength: 250
                                            },
                                            items: [
                                                {
                                                    name: 'Email',
                                                    fieldLabel: 'Электронная почта'
                                                },
                                                {
                                                    name: 'Website',
                                                    fieldLabel: 'Сайт'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    type: 'Judge',
                                    title: 'Судья',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                xtype: 'textfield',
                                                labelWidth: 130,
                                                labelAlign: 'right',
                                                flex: 1,
                                                maxLength: 250
                                            },
                                            items: [
                                                {
                                                    name: 'JudgePosition',
                                                    fieldLabel: 'Должность'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                xtype: 'textfield',
                                                labelWidth: 130,
                                                labelAlign: 'right',
                                                flex: 1,
                                                maxLength: 250
                                            },
                                            items: [
                                                {
                                                    name: 'JudgeSurname',
                                                    fieldLabel: 'Фамилия'
                                                },
                                                {
                                                    name: 'JudgeName',
                                                    fieldLabel: 'Имя'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                xtype: 'textfield',
                                                labelWidth: 130,
                                                labelAlign: 'right',
                                                flex: 1,
                                                maxLength: 250
                                            },
                                            items: [
                                                {
                                                    name: 'JudgePatronymic',
                                                    fieldLabel: 'Отчество'
                                                },
                                                {
                                                    name: 'JudgeShortFio',
                                                    fieldLabel: 'Фамилия и инициалы'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    type: 'Print',
                                    title: 'Печатная форма',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                xtype: 'textarea',
                                                labelWidth: 130,
                                                labelAlign: 'right',
                                                flex: 1,
                                                maxLength: 250
                                            },
                                            items: [
                                                {
                                                    name: 'HeaderText',
                                                    fieldLabel: 'Текст заголовка'
                                                }
                                            ]
                                        },                                     
                                        
                                    ]
                                }
                            ]
                        },                       
                        {
                            xtype: 'jurinstitutionrealobjgrid',
                            title: 'Территориальная подсудность',
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
                                }
                            ]
                        },
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