Ext.define('B4.view.report.PlanOwnerCollectionPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.planownercollectionreportpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox'
    ],

    initComponent: function() {
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
                    xtype: 'b4combobox',
                    name: 'Year',
                    fieldLabel: 'Год',
                    editable: false,
                    displayField: 'Name',
                    valueField: 'Id',
                    items: []
                }
            ]
        });
        
        me.callParent(arguments);
    }
});