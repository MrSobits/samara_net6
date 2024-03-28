
Ext.define('B4.view.regop.personal_account.PersonalAccountGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.form.ComboBox',

        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.store.regop.personal_account.PersonalAccount'
    ],

    title: '',

    alias: 'widget.',

    store: 'regop.personal_account.PersonalAccount',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
				columns: [
				{
					xtype: 'b4editcolumn',
					scope: me
				},
				{ text: '', dataIndex: 'Room', flex: 1, filter: { xtype: 'textfield' }},
				{ text: '', dataIndex: 'AccountOwner', flex: 1, filter: { xtype: 'textfield' }},
				{ text: '', dataIndex: 'PersonalAccountNum', flex: 1, filter: { xtype: 'textfield' }},
				{ text: '', dataIndex: 'AreaShare', flex: 1, filter: { xtype: 'textfield' }},
				{ text: '', dataIndex: 'ChargedSum', flex: 1, filter: { xtype: 'textfield' }},
				{ text: '', dataIndex: 'PaidSum', flex: 1, filter: { xtype: 'textfield' }},
				{ text: '', dataIndex: 'PenaltySum', flex: 1, filter: { xtype: 'textfield' }},
				{ text: '', dataIndex: 'Id', flex: 1, filter: { xtype: 'textfield' }},
				{
					xtype: 'b4deletecolumn',
					scope: me
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});

