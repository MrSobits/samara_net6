Ext.define('B4.view.mobileappaccountcomparsion.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.enums.MobileAccountComparsionDecision'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'mobileappaccountcomparsionEditWindow',
    title: 'Форма редактирования запроса',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120
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
                            labelWidth: 120,
                            margin: '5 0 5 0',
                            align: 'stretch',
                            labelAlign: 'right'
                        },
                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        title: 'Форма запроса',
                        border: false,
                        bodyPadding: 10,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                margin: '0 0 5 0',
                                labelWidth: 110,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'DecisionType',
                                    fieldLabel: 'Решение',
                                    flex:1,
                                    enumName: 'B4.enums.MobileAccountComparsionDecision'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsWorkOut',
                                    flex: 0.5,
                                    fieldLabel: 'Отработано',
                                    labelAlign: 'right'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 120,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Данные из системы',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 120,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'PersonalAccountNumber',
                                            fieldLabel: 'Номер ЛС',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfApplicantType'
                                        },                                      
                                        {
                                            xtype: 'textfield',
                                            name: 'ExternalAccountNumber',
                                            fieldLabel: 'Внешний номер ЛС',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfApplicantInn'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 120,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'PersonalAccountOwnerFIO',
                                            fieldLabel: 'ФИО собственника',
                                            allowBlank: false,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfApplicantEmail'
                                        }
                                    ]
                                }]
                        },                        
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 120,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Данные мобильного приложения',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'textfield',
                                        labelWidth: 120,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'MobileAccountNumber',
                                            fieldLabel: 'Номер ЛС',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfDocumentType'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'MobileAccountOwnerFIO',
                                            fieldLabel: 'ФИО собственника',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: true,
                                            itemId: 'dfDocumentName'
                                        }
                                    ]
                                }
                            ]
                        }]
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