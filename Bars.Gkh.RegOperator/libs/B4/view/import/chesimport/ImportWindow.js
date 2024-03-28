Ext.define('B4.view.import.chesimport.ImportWindow', {
    extend: 'B4.form.Window',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 400,
    bodyPadding: 5,
    alias: 'widget.chesimportimportwindow',
    title: 'Импорт',
    resizable: false,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 50,
                anchor: '100%',
                labelAlign: 'right',
                allowBlank: false,
                layout: {
                    type: 'anchor'
                }
            },
            items: [
                {
                    xtype: 'container',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">' +
                        'Выбирите импортируемые данные. Допустимые типы файлов: архив в формате zip.<br>Содержимое архива: файлы в формате *.csv</span>'
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileImport',
                    fieldLabel: 'Файл',
                    itemId: 'fileImport'
                },
                {
                    xtype: 'checkbox',
                    name: 'IsTemporaryImport',
                    checked: true,
                    boxLabel: 'Загрузка данных для анализа',
                    margin: '5px 0 0 55px',
                    readOnly: true
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
