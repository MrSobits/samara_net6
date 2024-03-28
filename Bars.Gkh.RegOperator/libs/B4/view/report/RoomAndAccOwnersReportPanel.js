Ext.define('B4.view.report.RoomAndAccOwnersReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.roomandaccownersreportpanel',
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
                            xtype: 'gkhtriggerfield',
                            name: 'HouseTypes',
                            fieldLabel: 'Тип дома',
                            emptyText: 'Все'
                        },
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'HouseConditions',
                            fieldLabel: 'Состояние дома',
                            emptyText: 'Все'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});