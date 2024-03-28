Ext.define('B4.view.report.AreaDataSheetReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.areaDataSheetReportPanel',
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
                    name: 'HouseTypes',
                    itemId: 'tfHouseType',
                    fieldLabel: 'Тип дома',
                    emptyText: 'Все'
                }
            ]
        });

        me.callParent(arguments);
    }
});