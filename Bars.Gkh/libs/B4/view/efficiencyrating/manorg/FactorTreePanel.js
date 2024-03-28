Ext.define('B4.view.efficiencyrating.manorg.FactorTreePanel', {
    extend: 'Ext.tree.Panel',

    alias: 'widget.efManorgFactorTreePanel',
    closable: false,
    animate: true,
    autoScroll: false,
    useArrows: true,
    containerScroll: true,
    loadMask: true,
    rootVisible: false,

    title: 'Факторы оценки конкурентоспособности',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.efficiencyrating.ManagingOrganizationDataValueTree', { autoDestroy: true });

        Ext.applyIf(me, {
            store: store,

            columns: [
                {
                    xtype: 'treecolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 1,
                    menuDisabled: true,
                    sortable: false,
                    draggable: false
                },
                {
                    dataIndex: 'Value',
                    text: 'Значение',
                    width: 100,
                    menuDisabled: true,
                    sortable: false,
                    draggable: false,
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : 0;
                    }
                },
                {
                    dataIndex: 'Dynamics',
                    text: 'Динамика',
                    width: 100,
                    menuDisabled: true,
                    sortable: false,
                    draggable: false,
                    renderer: function (val) {
                        return val && val ? Ext.util.Format.currency(val) : null;
                    }
                }
            ],

            listeners: {
                beforeitemexpand: function() {
                    Ext.suspendLayouts();
                },

                afteritemexpand: function() {
                    Ext.resumeLayouts(true);
                },

                beforeitemcollapse: function() {
                    Ext.suspendLayouts();
                },

                afteritemcollapse: function() {
                    Ext.resumeLayouts(true);
                }
            },

            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});