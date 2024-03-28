Ext.define('B4.view.version.ImportWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 400,
    bodyPadding: 5,
    itemId: 'importWindow',
    alias: 'widget.versionimportwin',
    title: 'Импорт',
    trackResetOnLoad: true,
    resizable: false,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    versionId: null,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4filefield',
                    name: 'FileImport',
                    labelWidth: 50,
                    labelAlign: 'right',
                    fieldLabel: 'Файл',
                    allowBlank: false,
                    possibleFileExtensions: 'xls,xlsx'
                },
                {
                    xtype: 'displayfield'
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
                            xtype: 'tbfill'
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
