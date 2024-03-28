Ext.define('B4.view.report.ShortProgramByTypeWorkPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.shortprogbytypeworkpanel',
    title: '',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: { type: 'vbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 200,
                        width: 600,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'Municipalities',
                            fieldLabel: 'Муниципальные образования',
                            emptyText: 'Все МО'
                        },
                        {
                            xtype: 'datefield',
                            allowBlank: false,
                            name: 'DateTimeReport',
                            fieldLabel: 'Дата отчета'
                        },
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'TypeWorks',
                            fieldLabel: 'Виды работ',
                            emptyText: 'Все виды работ'
                        }
                    ]
                }  
            ]
        });

        me.callParent(arguments);
    }
});