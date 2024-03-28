Ext.define('B4.view.actcheck.actioneditwindowbaseitem.RequisiteInfoFieldSet', {
    extend: 'Ext.form.FieldSet',

    alias: 'widget.actcheckactionrequisiteinfofieldset',

    requires: [
        'B4.form.ComboBox',
        'B4.form.FiasSelectAddress',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField'
    ],

    title: 'Реквизиты',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    // Используется для приписки
    // наименования действия
    innerMessage: 'действия',
    
    // Видимость контейнера
    // с полями продолжения действия
    continueContainerHidden: true,

    // Расположение полей
    // "Дата окончания..." и "Место проведения..."
    dateAndPlaceContainerLayout: 'hbox',

    initComponent: function () {
        var me = this,
            startTimeInnerMessage = (me.continueContainerHidden 
                ? 'проведения ' : 'начала ') + me.innerMessage;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 110,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        format: 'H:i'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'StartDate',
                            fieldLabel: 'Дата начала ' + me.innerMessage,
                            format: 'd.m.Y',
                            labelWidth: 110,
                            flex: 4
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            flex: 2.5,
                            items: [
                                {
                                    xtype: 'component',
                                    width: 20,
                                },
                                {
                                    xtype: 'timefield',
                                    name: 'StartTime',
                                    fieldLabel: 'Время ' + startTimeInnerMessage + ' c',
                                    submitFormat: 'Y-m-d H:i:s',
                                    labelAlign: 'right',
                                    labelWidth: 110,
                                    flex: 1,
                                    format: 'H:i'
                                }
                            ]
                        },
                        {
                            xtype: 'timefield',
                            name: 'EndTime',
                            fieldLabel: 'по',
                            submitFormat: 'Y-m-d H:i:s',
                            labelWidth: 55,
                            margin: '5 0 0 0',
                            flex: 1.5
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        format: 'H:i'
                    },
                    hidden: me.continueContainerHidden,
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'ContinueDate',
                            fieldLabel: 'Дата продолжения ' + me.innerMessage,
                            format: 'd.m.Y',
                            labelWidth: 110,
                            flex: 4
                        },
                        {
                            xtype: 'timefield',
                            name: 'ContinueStartTime',
                            fieldLabel: 'Время продолжения ' + me.innerMessage + ' c',
                            submitFormat: 'Y-m-d H:i:s',
                            labelWidth: 130,
                            flex: 2.5
                        },
                        {
                            xtype: 'timefield',
                            name: 'ContinueEndTime',
                            fieldLabel: 'по',
                            submitFormat: 'Y-m-d H:i:s',
                            labelWidth: 55,
                            margin: '5 0 0 0',
                            flex: 1.5
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: me.dateAndPlaceContainerLayout,
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'EndDate',
                            fieldLabel: 'Дата окончания ' + me.innerMessage,
                            labelWidth: 110,
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'b4fiasselectaddress',
                            name: 'ExecutionPlace',
                            fieldLabel: 'Место проведения ' + me.innerMessage,
                            flatIsVisible: false,
                            modalWindow: true,
                            labelWidth: me.dateAndPlaceContainerLayout === 'hbox' ? 130 : 110,
                            fieldsRegex: {
                                tfHousing: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                },
                                tfBuilding: {
                                    regex: /^\d+$/,
                                    regexText: 'В это поле можно вводить только цифры'
                                }
                            }
                        }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    itemId: 'trigfActionInspectors',
                    fieldLabel: 'Инспектор',
                    modalWindow: true,
                    editable: false,
                    margin: '5 0 0 0'
                }
            ]
        });

        me.callParent(arguments);
    }
});