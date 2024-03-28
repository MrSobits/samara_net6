Ext.define('B4.view.dictionaries.RecordWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.recordwindow',
    requires: [
        'B4.view.dictionaries.RecordGrid',
        'B4.ux.button.Close'
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    layout: 'fit',
    width: 650,
    height: 500,
    maximizable: true,
    resizable: true,

    title: 'Записи справочника',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'recordgrid',
                    border: false
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    handler: function() {
                                        this.up('window').close();
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    loadRecords: function(dictionaryCode) {
        var grid = this.down('recordgrid'),
            store = grid.getStore();

        store.clearFilter(true);
        store.filter('dictionaryCode', dictionaryCode);
    }
});