Ext.define('B4.view.fssp.courtordergku.UploadFileWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.uploadfilewindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    title: 'Файл',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Выберите файл',
                    name: 'FileImport',
                    allowBlank: false,
                    labelAlign: 'right',
                    anchor: '100%',
                    flex: 1,
                    itemId: 'fileImport',
                    possibleFileExtensions: 'xls,xlsx'
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
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function (btn) {
                                            btn.up('window').close();
                                        }
                                    }
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