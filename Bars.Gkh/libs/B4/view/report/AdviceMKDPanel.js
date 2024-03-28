Ext.define('B4.view.report.AdviceMKDPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'reportAdviceMKDPanel',
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
                    xtype: 'b4combobox',
                    name: 'ControlType',
                    itemId: 'cbControlType',
                    fieldLabel: 'Тип управления',
                    editable: false,
                    items: [[0, 'УК'], [1, 'Непосредственное управление']]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                }
            ]
        });

        me.callParent(arguments);
    }
});