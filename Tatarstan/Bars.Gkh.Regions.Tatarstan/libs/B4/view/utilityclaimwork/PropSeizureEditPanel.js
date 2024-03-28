Ext.define('B4.view.utilityclaimwork.PropSeizureEditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Постановление о наложении ареста на имущество',
    alias: 'widget.utilitypropseizureeditpanel',
    frame: true,
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.store.dict.JurInstitution',
        'B4.form.EnumCombo',
        'B4.enums.OwnerType',
        'B4.ux.button.AcceptMenuButton'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    autoScroll: true,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        xtype: 'container',
                        border: false,
                        layout: 'hbox',
                        shrinkWrap: true,
                        margin: 6
                    },
                    items: [
                       {
                           defaults: {
                               xtype: 'textfield',
                               labelAlign: 'right',
                               padding: '5 8 0 0',
                               labelWidth: 100
                           },
                           items: [
                               {
                                   xtype: 'datefield',
                                   name: 'DocumentDate',
                                   fieldLabel: 'Дата',
                                   format: 'd.m.Y',
                                   width: 250
                               },
                               {
                                   name: 'DocumentNumber',
                                   fieldLabel: 'Номер документа',
                                   maxLength: 300,
                                   labelWidth: 150,
                                   width: 300
                               }
                           ]
                       },
                       {
                           defaults: {
                               xtype: 'textfield',
                               labelAlign: 'right',
                               padding: '5 8 0 0',
                               labelWidth: 100
                           },
                           items: [
                               {
                                   name: 'Year',
                                   fieldLabel: 'Год',
                                   width: 250
                               },
                               {
                                   name: 'Number',
                                   fieldLabel: 'Номер',
                                   labelWidth: 150,
                                   width: 300
                               },
                               {
                                   name: 'SubNumber',
                                   fieldLabel: 'Подномер',
                                   labelWidth: 150,
                                   width: 300
                               }
                           ]
                       }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    minHeight: 500,
                    flex: 1,
                    border: true,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            title: 'Реквизиты',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            flex: 1,
                            border: false,
                            defaults: {
                                labelWidth: 200,
                                labelAlign: 'right',
                                padding: '5 8 8 0'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        xtype: 'textfield',
                                        labelWidth: 200,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            flex: 1,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                xtype: 'textfield',
                                                labelWidth: 200,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    name: 'Document',
                                                    fieldLabel: 'Документ-основание'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DeliveryDate',
                                                    fieldLabel: 'Дата вручения',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            flex: 1,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                xtype: 'textfield',
                                                labelWidth: 200,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'checkbox',
                                                    name: 'IsCanceled',
                                                    fieldLabel: 'Аннулировано'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'CancelReason',
                                                    fieldLabel: 'Причина аннулирования'
                                                }
                                            ]
                                        }
                                    ]
                                },


                                {
                                    xtype: 'fieldset',
                                    title: 'Кем вынесено',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right',
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
                                                xtype: 'textfield',
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    store: 'B4.store.dict.JurInstitution',
                                                    textProperty: 'Name',
                                                    editable: false,
                                                    columns: [
                                                        {
                                                            text: 'Наименование',
                                                            dataIndex: 'Name',
                                                            flex: 1,
                                                            filter: { xtype: 'textfield' }
                                                        }
                                                    ],
                                                    name: 'JurInstitution',
                                                    fieldLabel: 'Орган, вынесший постановление'
                                                },
                                                {
                                                    name: 'Official',
                                                    fieldLabel: 'Должностное лицо'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'Location',
                                            fieldLabel: 'Местонахождение'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Взыскатель',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Creditor',
                                            fieldLabel: 'Взыскатель'
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'BankDetails',
                                            fieldLabel: 'Реквизиты'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Постановление вынесено в отношении',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            flex: 1,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                xtype: 'textfield',
                                                labelWidth: 200,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'AccountOwner',
                                                    fieldLabel: 'ФИО/Наименование'
                                                },
                                                {
                                                    xtype: 'textarea',
                                                    name: 'AccountOwnerBankDetails',
                                                    fieldLabel: 'Реквизиты физ. лица'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            enumName: 'B4.enums.OwnerType',
                                            name: 'OwnerType',
                                            fieldLabel: 'Тип исполнителя'
                                        }
                                    ]
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
                     layout: {
                         type: 'hbox',
                         align: 'stretch'
                     },
                     items: [
                         {
                             xtype: 'buttongroup',
                             items: [
                                 {
                                     xtype: 'b4savebutton',
                                     type: 'mainForm'
                                 },
                                 {
                                     xtype: 'button',
                                     iconCls: 'icon-delete',
                                     action: 'delete',
                                     text: 'Удалить',
                                     textAlign: 'left'
                                 },
                                 {
                                     xtype: 'acceptmenubutton'
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