Ext.define('B4.view.dpkrdocument.RealityObjectContainer', {
    extend: 'Ext.tab.Panel',

    requires: [
        'B4.view.dpkrdocument.RealityObjectGrid'
    ],

    alias: 'widget.dpkrdocumentrealityobjectcontainer',

    autoScroll: true,
    layout: 'anchor',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'dpkrdocumentrealityobjectgrid',
                    itemId: 'includedRealityObjectGrid',
                    title: 'Включенные дома',
                    storeIsExcluded: false,
                    height: 250,
                    flex: 1
                },
                {
                    xtype: 'dpkrdocumentrealityobjectgrid',
                    itemId: 'excludedRealityObjectGrid',
                    title: 'Исключенные дома',
                    storeIsExcluded: true,
                    height: 250,
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});