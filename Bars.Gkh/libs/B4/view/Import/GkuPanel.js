﻿Ext.define('B4.view.Import.GkuPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField'
    ],

    title: 'Импорт лицевых счетов (Сведения из ЖКУ)',
    alias: 'widget.gkuimportpanel',
    layout: 'form',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    closable: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
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
                        labelWidth: 100
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
                            xtype: 'component',
                            width: 10
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
                    xtype: 'displayfield',
                    itemId: 'log'
                }
            ]
        });

        me.callParent(arguments);
    }
});
