Ext.define('B4.view.email.EditWindow', {
    extend: 'B4.form.Window',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    minHeight: 250,
    bodyPadding: 5,
    itemId: 'emailGjiEditWindow',
    title: 'Электронное письмо',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.view.email.PreviewAttachmentGrid',
        'B4.enums.EmailGjiType',
        'B4.view.email.AttachmentGrid',
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'From',
                    fieldLabel: 'От кого'
                },
                {
                    xtype: 'textfield',
                    name: 'SenderInfo',
                    fieldLabel: 'Отправитель'
                },
                {
                    xtype: 'textarea',
                    name: 'Theme',
                    fieldLabel: 'Тема'
                },
                {
                    xtype: 'datefield',
                    name: 'EmailDate',
                    fieldLabel: 'Дата письма',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    name: 'GjiNumber',
                    fieldLabel: 'Номер'
                },
                {
                    xtype: 'b4enumcombo',
                    anchor: '100%',
                    fieldLabel: 'Тип письма',
                    enumName: 'B4.enums.EmailGjiType',
                    name: 'EmailType'
                },
                {
                    xtype: 'checkbox',
                    itemId: 'cbRegistred',
                    name: 'Registred',
                    fieldLabel: 'Зарегистрировано',
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Содержание',
                    name: 'Content'
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    layout: {
                        align: 'stretch'
                    },
                    enableTabScroll: true,
                    defaults:
                    {
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        flex: 1,
                        margins: -1,
                        border: false
                    },
                    items: [
                        {
                            xtype: 'panel',
                            name: 'attachmentpanel',
                            title: 'Вложения письма',
                            items: [
                                {
                                    xtype: 'emailgjiattachmentgrid',
                                    flex: 1
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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