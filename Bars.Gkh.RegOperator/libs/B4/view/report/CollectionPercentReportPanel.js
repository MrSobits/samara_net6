Ext.define('B4.view.report.CollectionPercentReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.collectionPercentReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
       'B4.store.regop.ClosedChargePeriod'
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
                    name: 'MunicipalitiesR',
                    itemId: 'municipalityR',
                    fieldLabel: 'Муниципальный район',
                    store: 'B4.store.dict.municipality.MoArea',
                    editable: false,
                    emptyText: 'Все МР',
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'MunicipalitiesO',
                    itemId: 'municipalityO',
                    fieldLabel: 'Муниципальное образование',
                    store: 'B4.store.MunicipalityByParent',
                    editable: false,
                    emptyText: 'Все МО',
                    disabled: true,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Locality',
                    itemId: 'locality',
                    fieldLabel: 'Населенный пункт',
                    store: 'B4.store.LocalityByMo',
                    editable: false,
                    emptyText: 'Все нас. пункты',
                    disabled: true,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Address',
                    itemId: 'address',
                    fieldLabel: 'Адрес МКД',
                    store: 'B4.store.AddressByLocality',
                    selectionMode: 'MULTI',
                    editable: false,
                    emptyText: 'Все адреса',
                    disabled: true,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Period',
                    itemId: 'period',
                    store: 'B4.store.regop.ClosedChargePeriod',
                    fieldLabel: 'Период',
                    emptyText: 'Все'
                }
            ]
        });

        me.callParent(arguments);
    }
});