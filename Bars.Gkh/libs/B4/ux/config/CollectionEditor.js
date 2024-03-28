Ext.define('B4.ux.config.CollectionEditor', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.collectioneditor',

    layout: 'fit',

    elementType: {},

    editorGrid: null,
    editorStore: null,
    cellEditing: null,

    mixins: {
        field: 'Ext.form.field.Field'
    },

    statics: {
        renderPreview: function(val) {
            return 'Коллекция, элементов: ' + (val ? val.length : 0);
        }
    },

    initComponent: function() {
        var me = this;

        me.editorStore = Ext.create('Ext.data.Store', {
            autoDestroy: true,
            fields: ['value', 'valueType']
        });

        me.cellEditing = Ext.create('Ext.grid.plugin.CellEditing', {
            clicksToEdit: 1
        });

        me.editorGrid = Ext.create('Ext.grid.Panel', {
            border: false,
            columns: [
                { header: me.elementType.displayName || 'Значение', flex: 1, dataIndex: 'value', xtype: 'genericeditorcolumn', typeField: 'valueType' }
            ],
            store: me.editorStore,
            plugins: [me.cellEditing],
            listeners: {
                'edit': {
                    fn: me.onEdit,
                    scope: me
                }
            }
        });

        Ext.applyIf(me, {
            title: me.fieldLabel || me.displayName || 'Редактирование коллекции',
            items: [
                me.editorGrid
            ]
        });

        if (!me.hideToolbar) {
            Ext.applyIf(me, {
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
        }

        me.initField();
        me.callParent(arguments);
    },

    doAddValue: function () {
        var me = this;

        me.cellEditing.completeEdit();

        var recs = me.editorStore.add({
                valueType: me.elementType,
                value: me.elementType.defaultValue
            }),
            sm = me.editorGrid.getSelectionModel();

        sm.select(recs);
        me.onEdit();
    },

    doRemoveValue: function () {
        var me = this,
            sm = me.editorGrid.getSelectionModel();
        me.cellEditing.completeEdit();
        if (sm.hasSelection()) {
            me.editorStore.remove(sm.getSelection());
            if (me.editorStore.getCount() > 0) {
                sm.select(0);
            }
        }

        me.onEdit();
    },

    onEdit: function () {
        var me = this,
            val = [];

        me.cellEditing.completeEdit();

        me.editorStore.each(function(r) {
            val.push(r.get('value'));
        });

        me.changed = true;
        me.mixins.field.setValue.call(me, val);
    },

    setValue: function() {
        var me = this;

        me.mixins.field.setValue.apply(me, arguments);

        me.editorStore.removeAll();
        Ext.each(me.value, function(v) {
            me.editorStore.add({
                value: v,
                valueType: me.elementType
            });
        });
    },

    isEqual: function () {
        var me = this;
        return !me.changed;
    },

    reset: function() {
        var me = this;

        if (me.editorStore) {
            me.editorStore.removeAll();
        }

        me.mixins.field.reset.apply(me, arguments);
        me.changed = false;
    },

    isValid: function() {
        return true;
    },

    destroy: function() {
        var me = this;

        me.callParent();

        delete me.value;
        delete me.elementType;

        if (me.editorGrid) {
            delete me.editorGrid;
            delete me.cellEditing;
            delete me.editorStore;
        }
    }
});