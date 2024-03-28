Ext.define('B4.view.dpkrdocument.VersionContainer', {
    extend: 'Ext.tab.Panel',

    requires: [
        'B4.view.dpkrdocument.VersionGrid'
    ],

    alias: 'widget.dpkrdocumentversioncontainer',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'dpkrdocumentversiongrid',
                    title: 'Версии ДПКР',
                    height: 250,
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});