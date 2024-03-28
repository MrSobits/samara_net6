Ext.define('B4.view.report.TurnoverBalanceSheetPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.turnoverBalanceSheetPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField'
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
                    allowBlank: false,
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
                    xtype: 'b4selectfield',
                    store: 'B4.store.regop.ClosedChargePeriod',
                    itemId: 'period',
                    textProperty: 'Name',
                    editable: false,
                    allowBlank: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
                    fieldLabel: 'Период'
                }
            ]
        });

        me.callParent(arguments);
    }
});