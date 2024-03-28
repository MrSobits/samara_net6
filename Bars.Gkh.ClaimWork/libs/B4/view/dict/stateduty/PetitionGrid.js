Ext.define('B4.view.dict.stateduty.PetitionGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.statedutypetitiongrid',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.store.dict.StateDutyPetition'
    ],

    title: 'Тип заявления',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.StateDutyPetition');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    dataIndex: 'PetitionToCourtType',
                    text: 'Наименование',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            store.load();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    store: store,
                    dock: 'bottom',
                    displayInfo: true
                }
            ]
        });

        me.callParent(arguments);
    }
});