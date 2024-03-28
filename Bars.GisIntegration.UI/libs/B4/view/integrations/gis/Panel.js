Ext.define('B4.view.integrations.gis.Panel', {
    extend: 'Ext.panel.Panel',

    closable: true,
    alias: 'widget.gisintegrationpanel',
    title: 'Интеграция с ГИС ЖКХ',
    requires: [
        'B4.view.integrations.gis.TaskTree',
        'B4.view.dictionaries.DictionariesGrid'
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
                            xtype: 'gistasktree',
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
                            xtype: 'dictionariesgrid',
                            flex: 1
                        }
                    ]
                }
            ]
        }
    ]
});