Ext.define('B4.view.menu.ManagingOrgRealityObjData', {
    extend: 'Ext.panel.Panel',

    bodyPadding: 5,
    trackResetOnLoad: true,
    itemId: 'managingOrgRealityObjData',
    border: false,
    margins: -1,

    requires: [
        'B4.view.menu.ManagingOrgRealityObjDataGrid',
        'B4.view.groups.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                Ext.create('B4.view.menu.ManagingOrgRealityObjDataGrid', {
                    margins: -1
                })//,
                //Ext.create('B4.view.groups.Grid', {
                //    margins: -1
                //})
            ]
        });

        me.callParent(arguments);
    }
});
