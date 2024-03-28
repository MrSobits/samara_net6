Ext.define('B4.ux.config.ObjectEditor', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.objecteditor',

    layout: 'fit',

    properties: [],
    typeName: null,
    displayName: null,

    editorGrid: null,
    editorStore: null,

    mixins: {
        field: 'Ext.form.field.Field'
    },

    statics: {
        renderPreview: function (val, editor) {
            var properties = [];

            var editors = B4.utils.config.Helper.getItems(editor.properties);
            Ext.each(editors, function(editor) {
                var value = B4.ux.config.ObjectEditor._getValue(val, editor),
                    editorClass = Ext.ClassManager.getByAlias('widget.' + editor.xtype),
                    previewRenderer = editorClass && editorClass.renderPreview,
                    preview = previewRenderer ? previewRenderer(value, editor) : value;

                properties.push(editor.displayName + '=' + preview);
            });

            return (editor.displayName || Ext.htmlEncode(editor.typeName)) + ' (' + Ext.util.Format.ellipsis(properties.join(', '), 150) + ')';
        },

        _getValue: function (value, editor) {
            var v = (value ? value[editor.name] : null);
            return v === null ? editor.defaultValue : v;
        }
    },

    initComponent: function () {
        var me = this;

        me.editorStore = Ext.create('Ext.data.Store', {
            autoDestroy: true,
            fields: ['name', 'displayName', 'type', 'value', 'readOnly']
        });

        me.editorGrid = Ext.create('Ext.grid.Panel', {
            border: false,
            columns: [
                { header: 'Ключ', flex: 1, dataIndex: 'displayName' },
                { header: 'Значение', flex: 1, dataIndex: 'value', xtype: 'genericeditorcolumn' }
            ],
            store: me.editorStore,
            plugins: [
                {
                    ptype: 'genericcellediting',
                    clicksToEdit: 1
                }
            ],
            listeners: {
                'edit': {
                    fn: me.onEdit,
                    scope: me
                }
            }
        });

        Ext.applyIf(me, {
            title: me.fieldLabel || me.displayName || Ext.htmlEncode(me.typeName),
            items: me.editorGrid
        });

        this.initField();
        this.callParent(arguments);
    },

    onEdit: function () {
        var me = this,
            val = {};

        me.editorStore.each(function (r) {
            val[r.get('name')] = r.get('value');
        });

        me.changed = true;
        me.mixins.field.setValue.call(me, val);
    },

    isEqual: function () {
        var me = this;
        return !me.changed;
    },

    setValue: function() {
        var me = this,
            recs = {};

        me.mixins.field.setValue.apply(me, arguments);

        // связываем поля с метаописанием
        Ext.each(me.properties, function(p) {
            var value = B4.ux.config.ObjectEditor._getValue(me.value, p);

            recs[p.name] = {
                name: p.name,
                displayName: p.displayName,
                type: p.type,
                readOnly: p.readOnly,
                value: value
            };
        });

        // добиваем тем, что не связалось по тем или иным причинам
        // ну мало ли
        Ext.iterate(me.value, function(k, v) {
            if (!recs[k]) {
                recs[k] = {
                    name: k,
                    displayName: k,
                    value: v
                };
            }
        });

        me.editorStore.removeAll();
        // закидываем в стор
        Ext.iterate(recs, function(k, v) {
            me.editorStore.add(v);
        });
    },

    reset: function () {
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

    destroy: function () {
        this.callParent();

        delete this.value;
        delete this.properties;

        if (this.editorGrid) {
            delete this.editorGrid;
            delete this.editorStore;
        }
    }
});