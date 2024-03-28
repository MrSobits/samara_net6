Ext.define('B4.view.actcheck.instrexamaction.RequisiteInfoFieldSet', {
    extend: 'Ext.form.FieldSet',

    alias: 'widget.instrexamactionrequisiteinfofieldset',

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
    items: [
        {
            xtype: 'container',
            layout: 'hbox',
            defaults: {
                flex: 1
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 110,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'StartDate',
                            fieldLabel: 'Дата начала обследования',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            name: 'EndDate',
                            fieldLabel: 'Дата окончания обследования',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'b4fiasselectaddress',
                            name: 'ExecutionPlace',
                            fieldLabel: 'Место проведения обследования',
                            flatIsVisible: false,
                            modalWindow: true,
                            flex: 1.75,
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
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 110,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'timefield',
                            name: 'StartTime',
                            fieldLabel: 'Время начала обследования',
                            submitFormat: 'Y-m-d H:i:s',
                            format: 'H:i'
                        },
                        {
                            xtype: 'timefield',
                            name: 'EndTime',
                            fieldLabel: 'Время окончания обследования',
                            submitFormat: 'Y-m-d H:i:s',
                            format: 'H:i',
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '5 0 0 0',
                            defaults: {
                                labelWidth: 110,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Территория',
                                    name: 'Territory',
                                    maxLength: 25
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Помещение',
                                    name: 'Premise',
                                    maxLength: 25
                                }
                            ]
                        }
                    ]
                }
            ]
        },
        {
            xtype: 'gkhtriggerfield',
            itemId: 'trigfActionInspectors',
            fieldLabel: 'Инспектор',
            modalWindow: true,
            editable: false,
            labelWidth: 110,
            labelAlign: 'right'
        }
    ]
});