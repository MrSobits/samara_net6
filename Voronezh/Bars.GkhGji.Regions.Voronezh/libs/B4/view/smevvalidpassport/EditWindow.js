Ext.define('B4.view.smevvalidpassport.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.smevvalidpassport.FileInfoGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'smevvalidpassportEditWindow',
    title: 'Запрос',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
             items: [
             {
                 xtype: 'tabpanel',
                 border: false,
                 flex: 1,
                 defaults: {
                     border: false
                 },
                 items: [
                     {
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
                         items: [
                             {
                                 xtype: 'fieldset',
                                 defaults: {
                                     labelWidth: 250,
                                     anchor: '100%',
                                     labelAlign: 'right'
                                 },
                                 title: 'Ответ',
                                 items: [
                                     {
                                         xtype: 'container',
                                         layout: 'hbox',
                                         defaults: {
                                             xtype: 'combobox',
                                             //     margin: '10 0 5 0',
                                             labelWidth: 120,
                                             labelAlign: 'right',
                                         },
                                         items: [
                                             {
                                                 xtype: 'textfield',
                                                 name: 'DocStatus',
                                                 fieldLabel: 'Статус',
                                                 readOnly: true,
                                                 flex: 1
                                             },
                                             {
                                                 xtype: 'textfield',
                                                 name: 'InvalidityReason',
                                                 fieldLabel: 'Причина недейств.',
                                                 readOnly: true,
                                                 flex: 1
                                             },
                                             {
                                                 xtype: 'datefield',
                                                 name: 'InvaliditySince',
                                                 fieldLabel: 'Недейств. с',
                                                 readOnly: true,
                                                 flex: 1
                                             }
                                         ]
                                     }
                                 ]
                             },
                             {
                                 xtype: 'fieldset',
                                 defaults: {
                                     labelWidth: 250,
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
                                             //     margin: '10 0 5 0',
                                             labelWidth: 120,
                                             labelAlign: 'right',
                                         },
                                         items: [
                                             {
                                                 xtype: 'textfield',
                                                 name: 'DocSerie',
                                                 fieldLabel: 'Серия',
                                                 allowBlank: false,
                                                 flex: 1
                                             },
                                             {
                                                 xtype: 'textfield',
                                                 name: 'DocNumber',
                                                 fieldLabel: 'Номер',
                                                 allowBlank: false,
                                                 flex: 1
                                             },
                                             {
                                                 xtype: 'datefield',
                                                 name: 'DocIssueDate',
                                                 fieldLabel: 'Дата выдачи',
                                                 allowBlank: false,
                                                 flex: 1
                                             }
                                         ]
                                     }
                                 ]
                             },                           
                            {
                                xtype: 'tabpanel',
                                border: false,
                                flex: 1,
                                defaults: {
                                    border: false
                                },
                                items: [
                                {
                                        xtype: 'smevvalidpassportfileinfogrid',
                                    flex: 1
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