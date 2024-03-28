Ext.define('B4.ux.config.TriggerSingleSelectEditor', {
    extend: 'B4.view.Control.GkhTriggerField',
    alias: 'widget.triggersingleselecteditor',

    requires: [
        'B4.view.SelectWindow.SingleSelectWindow'
    ],

    isGetOnlyIdProperty: false,
    editable: false,

    //#region overrides

    textProperty: 'Name',
    valueProperty: 'Id',
    storePath: null,
    columnsGridSelect: null,

    //#endregion

    //#region State

    win: null,

    getStore: function () {
        var me = this;
        return Ext.create(me.storePath);
    },

    //#endregion

    onTrigger1Click: function() {
        this.callParent(arguments);

        var win = this.getWindow();
        win.show();
    },

    getWindow: function() {
        var me = this,
            columns = me.columnsGridSelect ||
            [
                {
                    xtype: 'gridcolumn',
                    header: 'Наименование',
                    dataIndex: me.textProperty,
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ];
        if (!me.win) {
            var store = me.getStore();
            store.on('beforeload', me.storeBeforeLoad, me);

            me.win = Ext.create('B4.view.SelectWindow.SingleSelectWindow', {
                storeSelect: store,
                columnsGridSelect: columns,
                constrain: true,
                modal: true,
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            me.win.down('b4closebutton').on('click', function() {
                me.win.close();
            });

            me.win.down('b4savebutton').on('click', function () {
                me.saveRecord();
            });

            store.load();
        }

        return me.win;
    },

    setValue: function(object) {
        var me = this,
            display = object && object[me.textProperty] || '';

        me.updateDisplayedText(display);

        me.callParent(arguments);
        me.mixins.field.checkDirty.call(this);
    },

    extractValue: function(record) {
        var obj = {};
        obj[this.valueProperty] = record.get(this.valueProperty);
        obj[this.textProperty] = record.get(this.textProperty);

        return obj;
    },

    saveRecord: function () {
        var me = this,
            grid = me.win.down('grid'),
            records = grid.getSelectionModel().getSelection();

        if (grid && records instanceof Array && records.length > 0) {
            me.setValue(me.extractValue(records[0]));
            me.win.close();
        }
    },

    isEqual: function (v1, v2) {
        var me = this;

        if (Ext.isEmpty(v1) && Ext.isEmpty(v2)) {
            return true;
        }

        if (Ext.isEmpty(v1)) {
            return false;
        }

        if (Ext.isEmpty(v2)) {
            return false;
        }

        return v1[me.valueProperty] === v2[me.valueProperty];
    },

    storeBeforeLoad: function (store, opts) {

    },
});