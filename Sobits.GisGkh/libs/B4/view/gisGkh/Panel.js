Ext.define('B4.view.gisGkh.Panel', {
    extend: 'Ext.panel.Panel',

    closable: true,
    alias: 'widget.gisgkhintegrationpanel',
    title: 'Интеграция с ГИС ЖКХ',
    requires: [
        //'B4.view.gisGkh.TaskGrid',
        //'B4.view.gisGkh.RoomMatchingPanel',
        //'B4.view.Dictionaries.GisGkhDictionariesGrid'
    ],

    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [
        {
            xtype: 'tabpanel',
            flex: 1,
            layout: {
                align: 'stretch'
            },
            enableTabScroll: true,
            defaults:
            {
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                flex: 1,
                margins: -1,
                border: false
            },
            items: [
                {
                    xtype: 'panel',
                    name: 'taskpanel',
                    title: 'Задачи',
                    items: [
                        {
                            xtype: 'gisgkhtaskgrid',
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    name: 'dictpanel',
                    title: 'Справочники',
                    items: [
                        {
                            xtype: 'gisgkhdictgrid',
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'roommatchingpanel',
                    title: 'Сопоставление помещений'
                },
                {
                    xtype: 'gisgkhdownloadgrid',
                    title: 'Скачивание файлов'
                }
            ]
        }
    ]
});