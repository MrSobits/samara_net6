Ext.define('B4.view.MkdChangeNotificationFileEdit', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minHeight: 250,
    bodyPadding: 5,
    alias: 'widget.mkdchangenotificationfileedit',
    title: 'Форма приложения',

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
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
                    fieldLabel: 'Наименование',
                    name: 'Name',
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Номер документа',
                    name: 'Number',
                    maxLength: 100,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'Date',
                    fieldLabel: 'Дата документа',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'textarea',
                    name: 'Desc',
                    fieldLabel: 'Описание',
                    maxLength: 500,
                    flex: 1
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    editable: false
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