Ext.define('B4.view.builder.EditPanel', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.form.ComboBox',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.YesNoNotSet'
    ],
    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    bodyPadding: 5,
    itemId: 'builderEditPanel',
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right'
                    },
                    layout: { type: 'vbox', align: 'stretch' },
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
                                {
                                    text: 'Муниципальный район',
                                    dataIndex: 'Municipality',
                                    flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListMoAreaWithoutPaging'
                                    }
                                },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            editable: false
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                pack: 'start'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 190,
                                flex: 1,
                                editable: false
                            },
                            items: [
                                {
                                    xtype: 'combobox', editable: false,
                                    fieldLabel: 'Применение прогрессивных технологий',
                                    name: 'AdvancedTechnologies',
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value'
                                },
                                {
                                    xtype: 'combobox', editable: false,
                                    fieldLabel: 'Согласие на предоставление информации',
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    name: 'ConsentInfo'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                pack: 'start'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 190,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'combobox', editable: false,
                                    fieldLabel: 'Выполнение работ без субподрядчика',
                                    name: 'WorkWithoutContractor',
                                    editable: false,
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value'
                                },
                                {
                                    xtype: 'gkhdecimalfield',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    name: 'Rating',
                                    fieldLabel: 'Рейтинг'
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
                    title: 'Сведения о налоговом органе',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'TaxInfoAddress',
                            fieldLabel: 'Адрес налогового органа',
                            maxLength: 1000
                        },
                        {
                            xtype: 'textfield',
                            name: 'TaxInfoPhone',
                            fieldLabel: 'Телефон налогового органа',
                            maxLength: 50
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл'
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
                    id: 'DocFieldSet',
                    defaults: {
                        xtype: 'container',
                        layout: 'anchor',
                        flex: 1,
                        defaults: {
                            anchor: '100%',
                            labelAlign: 'right'
                        }
                    },
                    title: 'Документы',
                    layout: {
                        type: 'hbox',
                        pack: 'start'
                    },
                    items: [
                        {
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    name: 'FileLearningPlan',
                                    id: 'FileLearningPlan',
                                    fieldLabel: 'План обучения (переподготовки) кадров',
                                    labelWidth: 260
                                }
                            ]
                        },
                        {
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    name: 'FileManningShedulle',
                                    id: 'FileManningShedulle',
                                    fieldLabel: 'Штатное расписание кадров',
                                    labelWidth: 190
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
                            action: 'GoToContragent',
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Перейти к контрагенту',
                                    iconCls: 'icon-arrow-out',
                                    panel: me,
                                    handler: function () {
                                        var me = this,
                                            form = me.panel.getForm(),
                                            record = form.getRecord(),
                                            contragentId = record.get('Contragent') ? record.get('Contragent').Id : 0;

                                        if (contragentId) {
                                            Ext.History.add(Ext.String.format('contragentedit/{0}/', contragentId));
                                        }
                                    }
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