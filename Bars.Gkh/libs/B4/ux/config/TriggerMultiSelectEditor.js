Ext.define('B4.ux.config.TriggerMultiSelectEditor', {
    extend: 'B4.view.Control.GkhTriggerField',
    alias: 'widget.triggermultiselecteditor',

    requires: [
        'B4.base.Store',
        'B4.base.Proxy',
        'B4.view.SelectWindow.MultiSelectWindow'
    ],

    isGetOnlyIdProperty: false,

    //#region overrides

    textProperty: 'Name',
    valueProperty: 'Id',
    storePath: null,

    //#endregion

    //#region State

    win: null,

    //#endregion

    onTrigger1Click: function() {
        this.callParent(arguments);

        var win = this.getWindow();
        win.show();
    },

    getWindow: function() {
        var me = this;
        if (!me.win) {
            var store = Ext.create(me.storePath, {
                listeners: {
                    beforeload: me.storeBeforeLoad
                }
            });

            me.win = Ext.create('B4.view.SelectWindow.MultiSelectWindow', {
                storeSelect: store,
                storeSelected: Ext.create('B4.base.Store', { autoLoad: false, proxy: { type: 'b4proxy' } }),
                columnsGridSelect: [{ xtype: 'gridcolumn', header: 'Наименование', dataIndex: me.textProperty, flex: 1 }],
                columnsGridSelected: [{ xtype: 'gridcolumn', header: 'Наименование', dataIndex: me.textProperty, flex: 1 }],
                constrain: true,
                modal: true,
                selModelMode: 'MULTI',
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            me.win.down('b4closebutton').on('click', function() {
                me.win.close();
            });

            me.win.down('b4deletebutton').on('click', function () {
                var store = me.win.storeSelected,
                    grid = me.win.down('#multiSelectGrid');

                store.removeAll();
                grid.getSelectionModel().b4deselectAll();
            });

            me.win.down('b4savebutton').on('click', function () {
                me.onSelectRequestHandler();
            });

            me.win.down('#multiSelectGrid').on('select', function(grid, record) {
                me.selectRecord(record);
            });

            me.win.on('selectedgridrowactionhandler', function (action, record) {
                var selectedStore = me.win.down('#multiSelectedGrid').getStore(),
                    selectGrid = me.win.down('#multiSelectGrid');

                if (action === 'delete') {
                    selectedStore.remove(record);
                    selectGrid.getSelectionModel().deselect(record);
                }
            });

            store.load();
        }

        return me.win;
    },

    setValue: function(objects) {
        var me = this,
            display = '';

        objects = Ext.Array.from(objects);
        objects = Ext.Array.filter(objects, function(obj) { return !Ext.isEmpty(obj); });

        if (objects.length > 0) {
            Ext.Array.each(objects,
                function (obj) {
                    if (!Ext.isEmpty(obj)) {
                        display += display ? (', ' + obj[me.textProperty]) : obj[me.textProperty];
                    }
                });

            me.updateDisplayedText(display);
        }

        me.callParent(arguments);
        me.mixins.field.checkDirty.call(this);
    },

    isEqual: function(v1, v2) {
        if (Ext.isEmpty(v1) && Ext.isEmpty(v2)) {
            return true;
        }

        if (Ext.isEmpty(v1)) {
            return false;
        }

        if (Ext.isEmpty(v2)) {
            return false;
        }

        v1 = Ext.Array.from(v1);
        v2 = Ext.Array.from(v2);

        if (v1.length !== v2.length) {
            return false;
        }

        return Ext.Array.difference(v1, v2).length > 0;
    },

    onSelectRequestHandler: function () {
        var me = this,
            grid = me.win.down('#multiSelectedGrid');

        if (grid) {
            me.setValue(me.extractValues(grid.getStore().getRange()));
            me.win.close();
        }
    },

    extractValues: function(records) {
        var me = this;
        return Ext.Array.map(records, me.extractValue, me);
    },

    extractValue: function(record) {
        var obj = {};
        obj[this.valueProperty] = record.get(this.valueProperty);
        obj[this.textProperty] = record.get(this.textProperty);

        return obj;
    },

    storeBeforeLoad: function(store, opts) {
        
    },

    selectRecord: function(record) {
        var me = this,
            grid = me.win.down('#multiSelectedGrid'),
            store = grid.getStore();

        if (store.indexOf(record) === -1) {
            store.add(record);
        }
    }
});