Ext.define('B4.view.import.MskCeoPointPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField'
    ],

    title: 'Импорт баллов ДПКР (Москва)',
    alias: 'widget.mskceopointimportpanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
    bodyPadding: 5,
    closable: true,

    initComponent: function() {
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
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите импортируемые данные. Допустимые типы файлов: xls,xlsx.'
                },
                {
                    xtype: 'form',
                    border: false,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
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
                            width: 500,
                            flex: 1,
                            itemId: 'fileImport',
                            possibleFileExtensions: 'xls,xlsx'
                        },
                        {
                            xtype: 'button',
                            text: 'Загрузить',
                            tooltip: 'Загрузить',
                            iconCls: 'icon-accept',
                            itemId: 'loadIdIsButton',
                            style: 'margin-left: 10px'
                        }
                    ]
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