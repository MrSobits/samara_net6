Ext.define('B4.view.baselicenseapplicants.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    alias: 'widget.baselicenseapplicantspanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.baselicenseapplicants.FilterPanel',
        'B4.view.baselicenseapplicants.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            title: 'Проверки соискателей лицензии',
            items: [
                {
                    xtype: 'baselicenseapplicantsfilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'baselicenseapplicantsgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});