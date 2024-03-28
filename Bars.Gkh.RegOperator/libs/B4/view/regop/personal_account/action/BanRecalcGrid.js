Ext.define('B4.view.regop.personal_account.action.BanRecalcGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.banrecalcgrid',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.grid.toolbar.Paging',
        'B4.base.Store'
    ],

    cls: 'x-large-head',

    initComponent: function () {
        var me = this,
            banRecalcInfoStore = Ext.create('B4.base.Store', {
                autoLoad: false,
                idProperty: 'Id',
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'BasePersonalAccount',
                    listAction: 'GetOperationDataForUI',
                    timeout: 60 * 1000, // 1 минута
                    extraParams: {
                        operationCode: 'BanRecalcOperation'
                    },
                },
                fields: [
                    { name: 'Id' },
                    { name: 'Municipality' },
                    { name: 'RoomAddress' },
                    { name: 'PersonalAccountNum' }
                ]
            });

        Ext.applyIf(me, {
            store: banRecalcInfoStore,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальный район',
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Адрес',
                    dataIndex: 'RoomAddress',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер ЛС',
                    dataIndex: 'PersonalAccountNum',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            viewConfig: {
                loadMask: true
            },

            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: banRecalcInfoStore,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});