Ext.define('B4.view.egsointegration.EgsoIntegrationWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.egsointegrationwindow',

    requires: [
        'B4.Date',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.enums.EgsoTaskType',
        'B4.ux.grid.plugin.HeaderFilters'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    closeAction: 'destroy',
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 800,
    maxWidth: 800,
    width: 800,
    height: 800,
    bodyPadding: 3,

    title: 'Задача',
    trackResetOnLoad: true,

    initComponent:
        function () {
            var me = this,
                store = Ext.create('B4.store.egso.Municipality'),
                years = Ext.Array.map(B4.Date.rangeYears(new Date('2014')), function(a) {return [a, a]}); 

            me.relayEvents(store, ['beforeload'], 'store.');

            Ext.applyIf(me, {
                defaults: {
                    labelAlign: 'right',
                    labelWidth: 160
                },
                items: [
                    {
                        xtype: 'b4enumcombo',
                        name: 'TaskType',
                        allowBlank: false,
                        fieldLabel: 'Задача',
                        width: 450,
                        minWidth: 450,
                        enumName: B4.enums.EgsoTaskType
                    },
                    {
                        xtype: 'b4combobox',
                        name: 'Year',
                        allowBlank: false,
                        fieldLabel: 'Отправить данные за год',
                        editable: false,
                        items: years
                    },
                {
                    xtype: 'grid',
                    name: 'MunicipalitiesGrid',
                    store: store,
                    flex: 1,
                    columns: [
                        {
                            text: 'Территория',
                            dataIndex: 'Territory',
                            xtype: 'gridcolumn',
                            flex: 2
                        },
                        {
                            text: 'Значение',
                            dataIndex: 'Value',
                            xtype: 'numbercolumn',
                            format: '0',
                            flex: 1,
                            editor: {
                                xtype: 'numberfield',
                                allowBlank: false,
                                minValue: 0,
                                allowDecimals: false
                            }
                        }
                    ],
                    plugins: [
                        Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                        Ext.create('Ext.grid.plugin.CellEditing', {
                            clicksToEdit: 1,
                            pluginId: 'cellEditing'
                        })],
                    viewConfig: {
                        loadMask: true
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { 
                                    xtype: 'button',
                                    name: 'Execute',
                                    icon: 'content/img/icons/accept.png',
                                    text: 'Выполнить'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});