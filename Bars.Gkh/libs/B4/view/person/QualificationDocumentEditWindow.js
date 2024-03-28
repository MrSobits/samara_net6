Ext.define('B4.view.person.QualificationDocumentEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.qualificationdocumenteditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 400,
    title: 'Документ квалификационного аттестата',
    closeAction: 'destroy',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField'
    ],

    bodyPadding: 7,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults:
            {
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Number',
                    fieldLabel: 'Номер бланк'
                },
                {
                    xtype: 'textfield',
                    name: 'StatementNumber',
                    fieldLabel: 'Номер заявления'
                },
                {
                    xtype: 'datefield',
                    name: 'IssuedDate',
                    fieldLabel: 'Дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4filefield',
                    name: 'Document',
                    fieldLabel: 'Файл заявления'
                },
                {
                    xtype: 'textareafield',
                    name: 'Note',
                    fieldLabel: 'Комментарий',
                    maxLength: 500
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
                                { xtype: 'b4savebutton', name: 'main' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});