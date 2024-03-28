Ext.define('B4.view.report.CalendarCostPlaningPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.calendarcostplanpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr'
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
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    itemId: 'sfProgramCr',
                    textProperty: 'Name',
                    fieldLabel: 'Программа',
                    store: 'B4.store.dict.ProgramCr',
                    columns: [
                        { xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, text: 'Наименование', filter: { xtype: 'textfield' } }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    fieldLabel: 'Муниципальный район',
                    emptyText: 'Все МР'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Settlements',
                    fieldLabel: 'Муниципальное образование',
                    emptyText: 'Все МО'
                }
            ]
        });
        me.callParent(arguments);
    }
});