Ext.define('B4.view.payment.ImportWindow', {
    extend: 'B4.form.Window',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 400,
    bodyPadding: 5,
    itemId: 'importPaymentWindow',
    title: 'Импорт',
    resizable: false,
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
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
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выбирите импортируемые данные. Допустимые типы файлов: txt, zip.<br>Максимальный размер файлов 5Mb</span>'
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileImport',
                    fieldLabel: 'Файл',
                    labelWidth: 80,
                    itemId: 'fileImport'
                },
                {
                    xtype: 'checkbox',
                    name: 'Resave',
                    checked: true,
                    fieldLabel: 'Перезаписывать существующие данные',
                    labelWidth: 250,
                    itemId: 'chbResave'
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
