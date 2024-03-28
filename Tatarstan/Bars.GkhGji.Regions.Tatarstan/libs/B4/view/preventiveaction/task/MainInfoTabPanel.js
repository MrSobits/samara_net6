Ext.define('B4.view.preventiveaction.task.MainInfoTabPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.preventiveactiontaskmaininfotabpanel',

    requires: [
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.form.FiasSelectAddress',
        'B4.ux.form.field.GkhTimeField',
        
        'B4.store.dict.Inspector',

        'B4.enums.PreventiveActionType',
        'B4.enums.PreventiveActionVisitType',
        'B4.enums.PreventiveActionCounselingType',

        'B4.view.preventiveaction.task.PlannedActionGrid'
    ],

    title: 'Основная информация',
    bodyStyle: Gkh.bodyStyle,
    border: false,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    defaults: {
        xtype: 'container',
        layout: {
            type: 'hbox',
            align: 'stretch'
        },
        labelAlign: 'right',
        labelWidth: 140
    },

    initComponent: function(){
        var me = this;

        Ext.apply(me, {
            margin: 5,
            items: [
                {
                    defaults: {
                        border: false,
                        bodyStyle: Gkh.bodyStyle,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: { 
                            bodyStyle: Gkh.bodyStyle,
                            labelAlign: 'left',
                            labelWidth: 110,
                            flex: 1
                        }
                    },
                    items: [
                        {
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'ActionType',
                                    fieldLabel: 'Вид мероприятия',
                                    enumName: 'B4.enums.PreventiveActionType',
                                    readOnly: true,
                                    margin: '0 0 10 0'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'VisitType',
                                    fieldLabel: 'Тип визита',
                                    enumName: 'B4.enums.PreventiveActionVisitType',
                                    readOnly: true
                                },
                                {
                                    xtype: 'b4fiasselectaddress',
                                    name: 'ActionLocation',
                                    fieldLabel: 'Место проведения</br>мероприятия',
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
                                    margin: 0,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'CounselingType',
                                    fieldLabel: 'Способ консультирования',
                                    enumName: 'B4.enums.PreventiveActionCounselingType',
                                    margin: '0 0 5 0'
                                }
                            ],
                            flex: 2.5
                        },
                        {
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'ActionStartDate',
                                    fieldLabel: 'Дата начала проведения мероприятия',
                                    margin: '0 0 10 0',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'gkhtimefield',
                                    name: 'ActionStartTime',
                                    maxValue: '23:30',
                                    fieldLabel: 'Время начала мероприятия',
                                    increment: 30
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ActionEndDate',
                                    fieldLabel: 'Дата окончания проведения мероприятия',
                                    margin: '0 0 10 0',
                                },
                            ],
                            flex: 1
                        }
                    ],
                    margin: '0 0 5 0'
                },
                {
                    xtype: 'fieldset',
                    title: 'Должностные лица',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'TaskingInspector',
                            fieldLabel: 'ДЛ, вынесшее<br/>задание',
                            textProperty: 'Fio',
                            store: 'B4.store.dict.Inspector',
                            columns: [
                                { text: 'ФИО', dataIndex: 'Fio', flex: 1 },
                                { text: 'Код', dataIndex: 'Code', flex: 1 }
                            ],
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Executor',
                            fieldLabel: 'Ответственный<br/>за исполнение',
                            textProperty: 'Fio',
                            store: 'B4.store.dict.Inspector',
                            columns: [
                                { text: 'ФИО', dataIndex: 'Fio', flex: 1 },
                                { text: 'Код', dataIndex: 'Code', flex: 1 }
                            ],
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'StructuralSubdivision',
                            fieldLabel: 'Структурное<br/>подразделение',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'plannedactiongrid',
                    itemId: 'plannedActionGrid',
                    height: 250,
                    margin: 5
                },
                {
                    xtype: 'fieldset',
                    margin: 5,
                    title: 'Уведомление о проведении мероприятия',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'container',
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        }
                    },
                    items: [
                        {
                            defaults: {
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'NotificationDate',
                                    fieldLabel: 'Дата уведомления',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'OutgoingLetterDate',
                                    fieldLabel: 'Дата исходящего письма',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'NotificationSent',
                                    fieldLabel: 'Уведомление передано',
                                    enumName: 'B4.enums.YesNo'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'NotificationType',
                                    fieldLabel: 'Способ уведомления',
                                    enumName: 'B4.enums.NotificationType'
                                }
                            ],
                            flex: 1
                        },
                        {
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 180
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'NotificationDocumentNumber',
                                    fieldLabel: 'Номер документа'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'OutgoingLetterNumber',
                                    fieldLabel: 'Номер исходящего письма'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'NotificationReceived',
                                    fieldLabel: 'Уведомление получено',
                                    enumName: 'B4.enums.YesNo'
                                },
                                {
                                    xtype: 'checkbox',
                                    checked: false,
                                    inputValue: 10,
                                    labelSeparator: '',
                                    hideLabel: true,
                                    boxLabel: 'Отказ от участия в профилактическом мероприятии',
                                    name: 'ParticipationRejection',
                                    margin: '0 0 0 185'
                                }
                            ],
                            flex: 1
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});