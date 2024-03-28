Ext.define('B4.view.emailnewsletter.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 650,
    minWidth: 500,
    minHeight: 700,
    autoScroll: true,
    bodyPadding: 5,
    itemId: 'emailNewsletterEditWindow',
    title: 'Рассылка на электронную почту',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.emailnewsletter.LogGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
            },
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'Тема',
                    name: 'Header',
                    itemId: 'tfHeader',
                    maxLength: 500
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Текст',
                    name: 'Body',
                    itemId: 'tfBody',
                    maxLength: 1500
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Адресаты',
                    name: 'Destinations',
                    itemId: 'tfDestinations',
                    maxLength: 1000
                },
                {
                    xtype: 'b4filefield',
                    name: 'Attachment',
                    fieldLabel: 'Вложение',
                    itemId: 'tfAttachment',
                    possibleFileExtensions: 'pdf',
                },
                {
                    xtype: 'textfield',
                    name: 'Sender',
                    fieldLabel: 'Отправитель',
                    disabled: true
                },
                {
                    xtype: 'emailnewsletterLogGrid'
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
                                    xtype: 'b4savebutton',
                                    itemId: 'saveBtn'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отправить Email',
                                    tooltip: 'Отправить Email',
                                    iconCls: 'icon-accept',
                                    itemId: 'sendEmailButton'
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