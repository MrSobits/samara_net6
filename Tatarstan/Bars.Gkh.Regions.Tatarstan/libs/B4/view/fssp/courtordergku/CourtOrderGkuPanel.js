Ext.define('B4.view.fssp.courtordergku.CourtOrderGkuPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.courtordergkupanel',

    requires: [
        'B4.view.fssp.courtordergku.AddressMatchingPanel',
        'B4.view.fssp.courtordergku.LitigationPanel',
        'B4.view.fssp.courtordergku.UploadDownloadInfoPanel'
    ],

    title: 'Реестр судебных распоряжений по ЖКУ',

    bodyStyle: Gkh.bodyStyle,
    closable: true,
    autoScroll: true,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    initComponent: function () {
        var me = this;

        Ext.apply(me,
            {
                items: [
                    {
                        xtype: 'tabpanel',
                        flex: 1,
                        items: [
                            {
                                xtype: 'litigationpanel'
                            },
                            {
                                xtype: 'uploaddownloadinfopanel'
                            },
                            {
                                xtype: 'fsspaddressmatchingpanel'
                            }
                        ]
                    }
                ]
            });

        me.callParent(arguments);
    }
});