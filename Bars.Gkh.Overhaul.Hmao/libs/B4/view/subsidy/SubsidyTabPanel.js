Ext.define('B4.view.subsidy.SubsidyTabPanel', {
    extend: 'Ext.tab.Panel',

    alias: 'widget.subsidytabpanel',

    closable: true,

    objectId: null,

    requires: [
        'B4.view.subsidy.SubsidyPanel',
        'B4.view.program.FourthStageGrid',
        'B4.view.subsidy.DefaultPlanCollectionInfoGrid',
        'B4.store.dict.municipality.ByParam'
    ],

    title: 'Субсидирование',
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.municipality.ByParam', { remoteFilter: false });

        //store.load();

        Ext.applyIf(me, {
            dockedItems: [
                {
                    dock: 'top',
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'combobox',
                            store: store,
                            name: 'Municipality',
                            fieldLabel: 'Муниципальное образование',
                            labelWidth: 200,
                            width: 600,
                            margin: '5 0',
                            labelAlign: 'right',
                            valueField: 'Id',
                            displayField: 'Name',
                            listeners: {
                                change: function (cmp, newValue) {
                                    if (cmp.store.isLoading()) {
                                        return;
                                    }

                                    cmp.store.clearFilter();
                                    if (!Ext.isEmpty(newValue)) {
                                        cmp.store.filter({
                                            property: 'Name',
                                            anyMatch: true,
                                            exactMatch: false,
                                            caseSensitive: false,
                                            value: newValue
                                        });
                                    }
                                }
                            }
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'subsidypanel'
                },
                {
                    xtype: 'programfourthstagegrid'
                }/*,
                {
                    xtype: 'defaultplancollectioninfogrid'
                }*/
            ]
        });

        me.callParent(arguments);
    }
});