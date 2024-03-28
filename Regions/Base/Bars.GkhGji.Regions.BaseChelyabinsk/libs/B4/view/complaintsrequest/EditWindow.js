Ext.define('B4.view.complaintsrequest.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.complaintsrequest.FileInfoGrid',
        'B4.form.EnumCombo',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.TypeComplainsRequest'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'complaintsrequestEditWindow',
    title: 'Запрос ДО',
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
                                     xtype: 'b4enumcombo',
                                     name: 'TypeComplainsRequest',
                                     labelWidth: 170,
                                     fieldLabel: 'Тип запроса',
                                     enumName: 'B4.enums.TypeComplainsRequest',
                                     flex: 1
                                 },
                                 {
                                     xtype: 'textarea',
                                     name: 'TextReq',
                                     itemId: 'tfGoals',
                                     fieldLabel: 'Запрос',
                                     allowBlank: true,
                                     disabled: false,
                                     flex: 1,
                                     editable: true
                                 },
                                 {
                                     xtype: 'textarea',
                                     name: 'Answer',
                                     itemId: 'tfAnswer',
                                     fieldLabel: 'Ответ',
                                     allowBlank: true,
                                     disabled: false,
                                     flex: 1,
                                     editable: true
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
                                            xtype: 'complaintsrequestfileinfogrid',
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
                            columns: 3,
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