Ext.define('B4.view.program.ImportWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.programimportwin',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 400,
    bodyPadding: 5,
    title: 'Импорт',
    resizable: false,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 80,
                anchor: '100%',
                labelAlign: 'right',
                allowBlank: false,
                layout: {
                    type: 'anchor'
                }
            },
            items: [
                {
                    xtype: 'b4filefield', editable: false,
                    name: 'FileImport',
                    fieldLabel: 'Файл',
                    itemId: 'fileImport'
                },
                {
                    xtype: 'checkbox',
                    name: 'AddStructEl',
                    checked: false,
                    hidden: true,
                    style: 'margin-left: 10px; margin-top: 20px; ',
                    boxLabel: 'Добавлять КЭ в паспорт жилого дома',
                    margin: '0 0 0 80'
                },
                {
                    xtype: 'displayfield',
                    itemId: 'log'
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
