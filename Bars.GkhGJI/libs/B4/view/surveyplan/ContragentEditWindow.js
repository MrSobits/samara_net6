Ext.define('B4.view.surveyplan.ContragentEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.surveyPlanContragentEditWindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    minWidth: 700,
    bodyPadding: 5,
    resizable: false,
    title: 'Редактирование',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.enums.Month',
        'B4.view.surveyplan.ContragentAttachmentGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Контрагент',
                    layout: 'form',
                    defaults: {
                        labelWidth: 130
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            readOnly: true
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 35
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    flex: 1,
                                    name: 'Inn',
                                    fieldLabel: 'ИНН',
                                    readOnly: true
                                },
                                {
                                    xtype: 'textfield',
                                    flex: 1,
                                    name: 'Ogrn',
                                    fieldLabel: 'ОГРН',
                                    readOnly: true,
                                    margin: '0 0 0 5'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'JuridicalAddress',
                            fieldLabel: 'Юридический адрес',
                            readOnly: true
                        }
                    ]
                },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 35
                },
                items: [
                            {
                                xtype: 'b4enumcombo',
                                enumName: 'B4.enums.Month',
                                fieldLabel: 'Месяц проведения проверки',
                                labelWidth: 170,
                                name: 'PlanMonth'
                            },
                            {
                                xtype: 'textfield',
                                fieldLabel: 'Год проведения проверки',
                                labelWidth: 155,
                                maxLength: 4,
                                name: 'PlanYear',
                                margin: '0 0 0 20'
                            }
                        ]
                    },
                {
                    xtype: 'fieldset',
                    title: 'Исключение из плана',
                    layout: 'form',
                    defaults: {
                        labelWidth: 130
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'IsExcluded',
                            fieldLabel: 'Контрагент исключен'
                        },
                        {
                            xtype: 'textarea',
                            name: 'ExclusionReason',
                            fieldLabel: 'Причина исключения',
                            maxLength: 2000,
                            disabled: true
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Приложения',
                    layout: 'fit',
                    height: 250,
                    items: [
                        {
                            xtype: 'surveyPlanContragentAttachmentGrid',
                            height: 200
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