Ext.define('B4.ux.config.OrganizationFormsEditor', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.organizationformseditor',
    layout: 'fit',

    editorGrid: null,
    editorStore: null,
    cellEditing: null,

    mixins: {
        field: 'Ext.form.field.Field'
    },

    storeBeforeLoad: function (store, opts) {
        opts.params.typeId = 'gkh_regop_personal_account';
    },

    initComponent: function () {
        var me = this;

        me.editorStore = Ext.create('Ext.data.Store', {
            autoDestroy: true,
            fields: ['Id', 'Name']
        });

        me.editorGrid = Ext.create('Ext.grid.Panel', {
            border: false,
            columns: [
                { header: 'Наименование', flex: 1, dataIndex: 'Name', xtype: 'gridcolumn' }
            ],
            store: me.editorStore,
            listeners: {
                'edit': {
                    fn: me.onEdit,
                    scope: me
                }
            }
        });

        Ext.applyIf(me, {
            items: [
                me.editorGrid
            ],
            tbar: [
                {
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [
                        {
                            xtype: 'b4addbutton',
                            handler: me.doAddValue.bind(me)
                        }, {
                            xtype: 'b4deletebutton',
                            handler: me.doRemoveValue.bind(me)
                        }
                    ]
                }
            ]
        });

        me.initField();
        me.callParent(arguments);
    },

    doAddValue: function () {
        this.getWindow().show();
    },

    doRemoveValue: function () {
       var me = this,
            sm = me.editorGrid.getSelectionModel();
       if (sm.hasSelection()) {
            me.editorStore.remove(sm.getSelection());
            if (me.editorStore.getCount() > 0) {
                sm.select(0);
            }
        }

        var res = [];
        me.editorStore.each(function (rec) {
            res.push({ Id: rec.get('Id'), Name: rec.get('Name') });
        });

        me.changed = true;
        me.mixins.field.setValue.call(me, res);
    },

    setValue: function () {
        var me = this;
        me.mixins.field.setValue.apply(me, arguments);
        me.editorStore.removeAll();
        Ext.each(me.value, function (v) {
            me.editorStore.add({
                Id: v.Id,
                Name: v.Name
            });
        });
    },

    isEqual: function () {
        var me = this;
        return !me.changed;
    },

    reset: function () {
        var me = this;

        if (me.editorStore) {
            me.editorStore.removeAll();
        }

        me.mixins.field.reset.apply(me, arguments);
        me.changed = false;
    },

    isValid: function () {
        return true;
    },

    destroy: function () {
        var me = this;

        me.callParent();
        Ext.destroy(me.win);
        
        delete me.Id;
        delete me.Name;

        if (me.editorGrid) {
            delete me.editorGrid;
            delete me.cellEditing;
            delete me.editorStore;
        }
    },

    getWindow: function () {
        var me = this;
        if (!me.win) {
            var store = Ext.create('B4.store.OrganizationFormsForSelect', {
                listeners: {
                    beforeload: me.storeBeforeLoad
                }
            });

            me.win = Ext.create('B4.view.SelectWindow.MultiSelectWindow', {
                storeSelect: store,
                storeSelected: me.editorStore,
                columnsGridSelect: [{ xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1 }],
                columnsGridSelected: [{ xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1 }],
                constrain: true,
                modal: true,
                selModelMode: 'MULTI',
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            me.win.down('b4closebutton').on('click', function () {
                me.win.close();
            });

            me.win.down('b4savebutton').on('click', function () {
                me.onSelectRequestHandler();
            });

            me.win.down('#multiSelectGrid').on('select', function (grid, record) {
                me.selectRecord(record);
            });

            me.win.on('selectedgridrowactionhandler', function (action, record) {
                if (action === 'delete') {
                    this.down('#multiSelectedGrid').getStore().remove(record);
                }
            });

            store.load();
        }

        me.win.down('#multiSelectGrid').getSelectionModel().deselectAll();

        return me.win;
    },

    selectRecord: function (record) {
        var me = this,
            grid = me.win.down('#multiSelectedGrid'),
            store = grid.getStore();

        var res = [];
        grid.getStore().each(function (rec) {
            res.push(rec.get('Id'));
        });

        if (res.indexOf(record.data.Id) === -1) {
            store.add(record);
        }
    },

    onSelectRequestHandler: function () {
        var me = this,
            grid = me.win.down('#multiSelectedGrid');

        if (grid) {
            var res = [];
            grid.getStore().each(function (rec) {
                res.push({ Id: rec.get('Id'), Name: rec.get('Name') });
            });
            me.changed = true;
            me.mixins.field.setValue.call(me, res);
        }
        me.win.close();
    }
});