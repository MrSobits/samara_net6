Ext.define('B4.view.membershipunions.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.membershipunionsgrid',
    store: 'MembershipUnions',
    itemId: 'membershipUnionsGrid',
    title: 'Членство в объединениях',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OfficialSite',
                    flex: 1,
                    text: 'Официальный сайт'
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});