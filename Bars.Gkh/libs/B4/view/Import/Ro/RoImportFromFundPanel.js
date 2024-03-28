Ext.define('B4.view.Import.Ro.RoImportFromFundPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField'
    ],

    title: 'Импорт жилых домов из фонда',
    alias: 'widget.roimportfromfundpanel',
    layout: 'anchor',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,

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
                    itemId: 'ctnText',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите импортируемые данные. Допустимые типы файлов: zip.<br>Максимальный размер файлов 5Mb</span>'
                },
                {
                    xtype: 'form',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    itemId: 'importForm',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 45
                    },
                    items: [
                        {
                            xtype: 'b4filefield',
                            name: 'FileImport',
                            fieldLabel: 'Файл',
                            allowBlank: false,
                            flex: 1,
                            itemId: 'fileImport',
                            possibleFileExtensions: 'zip'
                        },
                        {
                            xtype: 'button',
                            text: 'Загрузить',
                            tooltip: 'Загрузить',
                            iconCls: 'icon-accept',
                            itemId: 'loadIdIsButton'
                        }
                    ]
                },
                {
                    xtype: 'checkbox',
                    name: 'CreateObjects',
                    checked: false,
                    style: 'margin-left: 10px; margin-top: 20px; ',
                    boxLabel: 'Создавать новые дома при импорте',
                    labelWidth: 250,
                    itemId: 'chbCreateObjects'
                },
                {
                    xtype: 'checkbox',
                    name: 'ReplaceData',
                    checked: false,
                    style: 'margin-left: 10px',
                    boxLabel: 'Заменять текущие значения технического паспорта',
                    labelWidth: 250,
                    itemId: 'chbReplaceData'
                },
                {
                    xtype: 'displayfield',
                    itemId: 'log'
                }
            ]
        });

        me.callParent(arguments);
    }
});
