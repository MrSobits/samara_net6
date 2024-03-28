Ext.define('B4.view.al.ReportPanel', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.helpers.al.ReportParamFieldBuilder',
        'B4.view.reportHistory.Grid'
    ],

    alias: 'widget.reportpanel',
    title: 'Панель отчетов',
    layout: 'border',
    closable: true,

    items: [
        {
            xtype: 'panel',
            border: false,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            region: 'west',
            split: true,
            width: '41%',
            items: [
                {
                    xtype: 'panel',
                    height: 55,
                    ui: 'report',
                    border: false,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            height: 1,
                            xtype: 'container',
                            ui: 'search-helper'
                        },
                        {
                            xtype: 'textfield',
                            ui: 'search-input',
                            emptyText: 'Поиск...',
                            height: 30,
                            margin: '15px 15px 0 15px',
                            enableKeyEvents: true,
                            name: 'searchfield'
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    margin: 0,
                    flex: 1,
                    ui: 'report',
                    itemId: 'panelReports',
                    disabled: true,
                    overflowY: 'auto',
                    items: [],
                    defaults: {
                        margin: '15px 15px 10px 15px'
                    },
                    layout: {
                        type: 'accordion',
                        activeOnTop: false,
                        fill: false
                    },
                    border: false
                }
            ]
        },
        {
            xtype: 'tabpanel',
            ui: 'report-tabpanel',
            region: 'center',
            itemId: 'panelTabs',
            items: [
                {
                    xtype: 'reporthistorygrid'
                }
            ]
        }
    ]

});