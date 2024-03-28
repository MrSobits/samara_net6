Ext.define('B4.view.appealcits.AppealCitsAnswerAttachmentEditWindow', {
    extend: 'B4.form.Window',
    closable: true,
    width: 500,
    bodyPadding: 10,
    title: 'Вложение',
    alias: 'widget.appealcitsanswerattachmenteditwindow',
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',

        'B4.form.FileField'
    ],    

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                anchor: '100%',
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    allowBlank: false,
                    fieldLabel: 'Наименование',
                    maxLength: 100
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 500
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileInfo',
                    allowBlank: false,
                    editable: false,
                    fieldLabel: 'Файл'
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
                            itemId: 'statusButtonGroup',
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
