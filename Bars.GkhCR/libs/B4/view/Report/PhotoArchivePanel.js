Ext.define('B4.view.report.PhotoArchivePanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'photoArchivePanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.view.dict.programcr.Grid'
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
                    fieldLabel: 'Программа кап.ремонта',
                    store: 'B4.store.dict.ProgramCr',
                   

                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'RealityObjects',
                    itemId: 'tfRealityObject',
                    fieldLabel: 'Жилые дома'
                },
                {
                    xtype: 'hiddenfield',
                    name: 'controllerName',
                    value: 'PhotoArchivePrint'
                },
                {
                    xtype: 'hiddenfield',
                    name: 'controllerAction',
                    value: 'PrintReport'
                }

            ]
        });
        me.callParent(arguments);
    }
});