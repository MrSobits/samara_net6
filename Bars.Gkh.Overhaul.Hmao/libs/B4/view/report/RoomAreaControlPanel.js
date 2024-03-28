Ext.define('B4.view.report.RoomAreaControlPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'reportRoomAreaControlPanel',
    alias: 'widget.reportroomareacontrolpanel',
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
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'ConditionHouses',
                    itemId: 'tfConditionHouse',
                    fieldLabel: 'Состояние дома',
                    emptyText: 'Все'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'TypeOwnerships',
                    itemId: 'tfTypeOwnership',
                    fieldLabel: 'Форма собственности',
                    emptyText: 'Все'
                },
                {
                    xtype: 'b4combobox',
                    name: 'CollectBy',
                    itemId: 'cbCollectBy',
                    fieldLabel: 'Собирать по',
                    editable: false,
                    items: [
                        // enum Report.RoomAreaControlReport.DataSource
                        [0, 'Реестр жилых домов'],
                        [1, 'Долгосрочная программа']],
                    value: 0
                }
            ]
        });

        me.callParent(arguments);
    }
});