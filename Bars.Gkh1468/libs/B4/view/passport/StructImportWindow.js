Ext.define('B4.view.passport.StructImportWindow', {
    extend: 'Ext.window.Window',
    modal: true,
    requires: [
        'B4.form.FileField'
    ],

    alias: 'widget.structimportwin',
    width: 380,
    resizable: false,
    title: 'Импорт',
    items: {
        xtype: 'form',
        border: 0,
        margin: '10 10 10 10',
        bodyStyle: Gkh.bodyStyle,
        items: [
            {
                xtype: 'container',
                items: [
                    {
                        xtype: 'b4filefield',
                        name: 'importfile',
                        fieldLabel: 'Путь к файлу',
                        width: 320,
                        required: true
                    },
                    {
                        xtype: 'container',
                        layout: 'hbox',
                        items: [
                            {
                                xtype: 'component',
                                flex: 1
                            },
                            {
                                xtype: 'button',
                                text: 'Импорт',
                                width: 120,
                                listeners: {
                                    click: function (btn) {
                                        var form = btn.up('form'),
                                            win = form.up('structimportwin');
                                        form.submit({
                                            url: B4.Url.action('Import', 'PassportStruct'),
                                            success: function () {
                                                B4.QuickMsg.msg('Импорт', 'Импорт прошел успешно', 'success');
                                                win.close();
                                            },
                                            failure: function (f, action) {
                                                var json = Ext.JSON.decode(action.response.responseText);
                                                B4.QuickMsg.msg('Импорт', json.message, 'error');
                                            }
                                        });
                                    }
                                }
                            }
                        ]
                    }
                ]
            }
        ]
    }
});