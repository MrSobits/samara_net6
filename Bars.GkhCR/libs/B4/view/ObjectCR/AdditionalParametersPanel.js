Ext.define('B4.view.objectcr.AdditionalParametersPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.additionalparameterspanel',
    title: 'Дополнительные параметры',
    autoScroll: true,
    closable: true,
    frame: true,
    bodyPadding: 5,

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Save',
        'B4.form.EnumCombo'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Статус по выдаче технических условий КТС',
                    defaults: {
                        bodyStyle: Gkh.bodyStyle,
                        border: false
                    },
                    items: [
                        {
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: 5,
                                        labelWidth: 250,
                                        maxWidth: 500,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'RequestKtsDate',
                                            fieldLabel: 'Дата поступления запроса в КТС',
                                            format: 'd.m.Y',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'TechConditionKtsDate',
                                            fieldLabel: 'Дата выдачи технических условий',
                                            format: 'd.m.Y',
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: 5,
                                        labelWidth: 250,
                                        maxWidth: 1010,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'TechConditionKtsRecipient',
                                            fieldLabel: 'Технические условия выданы (кому)',
                                            flex: 1
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Статус по выдаче технических условий МУП Водоканал',
                    defaults: {
                        bodyStyle: Gkh.bodyStyle,
                        border: false
                    },
                    items: [
                        {
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: 5,
                                        labelWidth: 250,
                                        maxWidth: 500,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'RequestVodokanalDate',
                                            fieldLabel: 'Дата поступления запроса в МУП Водоканал',
                                            format: 'd.m.Y',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'TechConditionVodokanalDate',
                                            fieldLabel: 'Дата выдачи технических условий',
                                            format: 'd.m.Y',
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: 5,
                                        labelWidth: 250,
                                        maxWidth: 1010,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'TechConditionVodokanalRecipient',
                                            fieldLabel: 'Технические условия выданы (кому)',
                                            flex: 1
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Статус по проектированию',
                    defaults: {
                        bodyStyle: Gkh.bodyStyle,
                        border: false
                    },
                    items: [
                        {
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: 5,
                                        labelWidth: 250,
                                        maxWidth: 500,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'EntryForApprovalDate',
                                            fieldLabel: 'Дата поступления проекта на согласование',
                                            format: 'd.m.Y',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ApprovalKtsDate',
                                            fieldLabel: 'Дата согласования проекта в КТС',
                                            format: 'd.m.Y',
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: 5,
                                        labelWidth: 250,
                                        maxWidth: 500,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'ApprovalVodokanalDate',
                                            fieldLabel: 'Дата согласования проекта в МУП Водоканал',
                                            format: 'd.m.Y',
                                            flex: 1
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Статус по монтажу объекта',
                    defaults: {
                        bodyStyle: Gkh.bodyStyle,
                        border: false
                    },
                    items: [
                        {
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: 5,
                                        labelWidth: 250,
                                        maxWidth: 500,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'InstallationPercentage',
                                            fieldLabel: 'Процент монтажа проекта',
                                            decimalPrecision : 2,
                                            flex: 1
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Статус по по приемке объекта Заказчиком',
                    defaults: {
                        bodyStyle: Gkh.bodyStyle,
                        border: false
                    },
                    items: [
                        {
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: 5,
                                        labelWidth: 250,
                                        maxWidth: 500,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'ClientAccepted',
                                            fieldLabel: 'Статус приемки объекта Заказчиком',
                                            enumName: 'B4.enums.AcceptType',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ClientAcceptedChangeDate',
                                            fieldLabel: 'Дата изменения статуса приемки объекта Заказчиком',
                                            format: 'd.m.Y',
                                            flex: 1
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Статус по по приемке инспектором Ростехнадзора',
                    defaults: {
                        bodyStyle: Gkh.bodyStyle,
                        border: false
                    },
                    items: [
                        {
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        padding: 5,
                                        labelWidth: 250,
                                        maxWidth: 500,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'InspectorAccepted',
                                            fieldLabel: 'Статус приемки объекта инспектором Ростехнадзора',
                                            enumName: 'B4.enums.AcceptType',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'InspectorAcceptedChangeDate',
                                            fieldLabel: 'Дата изменения статуса приемки объекта инспектором',
                                            format: 'd.m.Y',
                                            flex: 1
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});