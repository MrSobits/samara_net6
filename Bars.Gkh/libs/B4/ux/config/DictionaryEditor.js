Ext.define('B4.ux.config.DictionaryEditor', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.dictionaryeditor',

    layout: 'fit',

    keyType: {},
    valueType: {},

    editorGrid: null,
    editorStore: null,
    cellEditing: null,

    mixins: {
        field: 'Ext.form.field.Field'
    },

    statics: {
        renderPreview: function(val) {
            return 'Словарь, ' + Ext.Object.getSize(val) + ' ключей: ' + Ext.util.Format.ellipsis(Ext.Object.getKeys(val).join(', '), 100);
        }
    },

    initComponent: function() {
        var me = this;

        me.editorStore = Ext.create('Ext.data.Store', {
            autoDestroy: true,
            fields: ['key', 'keyType', 'value', 'valueType']
        });

        me.cellEditing = Ext.create('B4.ux.config.GenericCellEditing', {
            clicksToEdit: 1
        });

        me.editorGrid = Ext.create('Ext.grid.Panel', {
            border: false,
            columns: [
                { header: me.keyType.displayName || 'Ключ', flex: 1, dataIndex: 'key', xtype: 'genericeditorcolumn', typeField: 'keyType' },
                { header: me.valueType.displayName || 'Значение', flex: 1, dataIndex: 'value', xtype: 'genericeditorcolumn', typeField: 'valueType' }
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
            title: me.fieldLabel || me.displayName || 'Редактирование словаря',
            items: me.editorGrid,
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

    doAddValue: function() {
        var me = this;

        me.cellEditing.completeEdit();
        var recs = me.editorStore.add({
                key: me.keyType.defaultValue,
                keyType: me.keyType,
                value: me.valueType.defaultValue,
                valueType: me.valueType
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
            val = {};

        me.cellEditing.completeEdit();

        me.editorStore.each(function(r) {
            val[r.get('key')] = r.get('value');
        });

        me.changed = true;
        me.mixins.field.setValue.call(me, val);
    },

    setValue: function() {
        var me = this;

        me.mixins.field.setValue.apply(me, arguments);

        me.editorStore.removeAll();
        Ext.iterate(me.value, function(k, v) {
            me.editorStore.add({
                key: k,
                keyType: me.keyType,
                value: v,
                valueType: me.valueType
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
        delete me.keyType;
        delete me.valueType;

        if (me.editorGrid) {
            delete me.editorGrid;
            delete me.cellEditing;
            delete me.editorStore;
        }
    }
});