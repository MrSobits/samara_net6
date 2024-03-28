Ext.define('B4.view.smevndfl.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.smevndfl.FileInfoGrid',
        'B4.view.smevndfl.AnswerGrid',
        'B4.store.dict.FLDocType'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'smevndflEditWindow',
    title: 'Запрос о доходах ФЛ',
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
                                    labelWidth: 100,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Реквизиты субъекта запроса',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'PeriodYear',
                                            fieldLabel: 'Отчетный год',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfPeriodYear'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'RegDate',
                                            fieldLabel: 'Дата заявления',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfRegDate'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'SNILS',
                                            fieldLabel: 'СНИЛС',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfSNILS'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'BirthDate',
                                            fieldLabel: 'Дата рождения',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfBirthDate'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        margin: '5 0 0 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'FamilyName',
                                            fieldLabel: 'Фамилия',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfFamilyName'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FirstName',
                                            fieldLabel: 'Имя',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfFirstName'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Patronymic',
                                            fieldLabel: 'Отчество',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfPatronymic'
                                        }

                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //margin: '5 0 5 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'RegNumber',
                                            fieldLabel: 'Номер заявления',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfRegNumber'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.FLDocType',
                                            name: 'DocumentCode',
                                            editable: true,
                                            fieldLabel: 'Код документа',
                                            allowBlank: false,
                                            flex: 1,
                                            margin: '5 0 0 0',
                                            textProperty: 'Name',
                                            isGetOnlyIdProperty: true,
                                            columns: [
                                                { text: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } },
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'SeriesNumber',
                                            fieldLabel: 'Серия и номер',
                                            allowBlank: false,
                                            flex: 1,
                                            margin: '5 0 0 0',
                                            disabled: false,
                                            editable: true,
                                            maxLength: 50,
                                            itemId: 'dfSeriesNumber'
                                        }

                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'INNUL',
                                            fieldLabel: 'ИНН ФЛ',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'tfINNUL'
                                        },
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
                                    xtype: 'smevndflfileinfogrid',
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
                        items: [
                            
                            {
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
                                        readOnly: true
                                    },
                                ]
                            },
                            {
                                    xtype: 'smevndflanswergrid',
                                    flex: 1
                                
                            },
                            //{
                            //    xtype: 'fieldset',
                            //    defaults: {
                            //        labelWidth: 250,
                            //        anchor: '100%',
                            //        labelAlign: 'right'
                            //    },
                            //    title: 'Сведения о налоговом агенте',
                            //    items: [
                            //        {
                            //            xtype: 'container',
                            //            layout: 'hbox',
                            //            defaults: {
                            //                xtype: 'combobox',
                            //                //     margin: '10 0 5 0',
                            //                labelWidth: 100,
                            //                labelAlign: 'right',
                            //            },
                            //        items: [
                            //            //{
                            //            //    xtype: 'textfield',
                            //            //    name: 'INNUL',
                            //            //    fieldLabel: 'ИНН',
                            //            //    allowBlank: true,
                            //            //    flex: 0.5,
                            //            //    disabled: false,
                            //            //    editable: false,
                            //            //    readOnly: true
                            //            //},
                            //            {
                            //                xtype: 'textfield',
                            //                name: 'KPP',
                            //                fieldLabel: 'КПП',
                            //                allowBlank: true,
                            //                flex: 0.5,
                            //                disabled: false,
                            //                editable: false,
                            //                readOnly: true
                            //            },
                            //            {
                            //                xtype: 'textfield',
                            //                name: 'OrgName',
                            //                fieldLabel: 'Организация',
                            //                allowBlank: true,
                            //                flex: 1,
                            //                disabled: false,
                            //                editable: false,
                            //                readOnly: true
                            //            }
                            //            ]
                            //        }
                            //    ]
                            //},
                            //{
                            //    xtype: 'fieldset',
                            //    defaults: {
                            //        labelWidth: 250,
                            //        anchor: '100%',
                            //        labelAlign: 'right'
                            //    },
                            //    title: 'Сведения о доходах',
                            //    items: [
                            //        {
                            //            xtype: 'container',
                            //            layout: 'hbox',
                            //            defaults: {
                            //                xtype: 'combobox',
                            //                //     margin: '10 0 5 0',
                            //                labelWidth: 100,
                            //                labelAlign: 'right',
                            //            },
                            //            items: [
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'Rate',
                            //                    fieldLabel: 'Ставка',
                            //                    allowBlank: true,
                            //                    flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'RevenueCode',
                            //                    fieldLabel: 'Код дохода',
                            //                    allowBlank: true,
                            //                    flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'Month',
                            //                    fieldLabel: 'Месяц',
                            //                    allowBlank: true,
                            //                    flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'RevenueSum',
                            //                    fieldLabel: 'Сумма дохода',
                            //                    allowBlank: true,
                            //                    flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                }
                            //            ]
                            //        },
                            //        ,
                            //        {
                            //            xtype: 'container',
                            //            layout: 'hbox',
                            //            defaults: {
                            //                xtype: 'combobox',
                            //                margin: '10 0 5 0',
                            //                labelWidth: 100,
                            //                labelAlign: 'right',
                            //            },
                            //            items: [
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'DutyBase',
                            //                    fieldLabel: 'Налоговая база',
                            //                    allowBlank: true,
                            //                    flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'RecoupmentCode',
                            //                    fieldLabel: 'Код вычета',
                            //                    allowBlank: true,
                            //                    flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'RecoupmentSum',
                            //                    fieldLabel: 'Сумма вычета',
                            //                    allowBlank: true,
                            //                    flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                }
                            //            ]
                            //        },
                            //        {
                            //            xtype: 'container',
                            //            layout: 'hbox',
                            //            defaults: {
                            //                xtype: 'combobox',
                            //                //     margin: '10 0 5 0',
                            //                labelWidth: 100,
                            //                labelAlign: 'right',
                            //            },
                            //            items: [
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'DutySum',
                            //                    fieldLabel: 'Сумма налога исчисленная',
                            //                    allowBlank: true,
                            //                    flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'UnretentionSum',
                            //                    fieldLabel: 'Сумма налога не удержанная',
                            //                    allowBlank: true,
                            //                    flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                },
                            //                {
                            //                    xtype: 'textfield',
                            //                    name: 'RevenueTotalSum',
                            //                    fieldLabel: 'Общая сумма дохода',
                            //                    allowBlank: true,
                            //                    flex: 1,
                            //                    disabled: false,
                            //                    editable: false,
                            //                    readOnly: true
                            //                }
                            //            ]
                            //        }
                            //    ]
                            //}
                        ]
                    }
                ]
            }],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                        xtype: 'buttongroup',
                        columns: 2,
                        items: [{
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