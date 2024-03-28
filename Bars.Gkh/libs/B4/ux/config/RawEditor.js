Ext.define('B4.ux.config.RawEditor', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.raweditor',

    layout: 'fit',

    typeName: null,
    sample: null,

    mixins: {
        field: 'Ext.form.field.Field'
    },

    statics: {
        renderPreview: function (val, editor) {
            return 'Сырой объект (' + Ext.htmlEncode(editor.typeName) + ')';
        }
    },

    initComponent : function() {
        var me = this,
            emptyText = null;

        if (me.sample) {
            emptyText = 'Должно получиться что-то вроде: ' + Ext.JSON.encode(me.sample).replace(/"/g, '\'');
        }

        Ext.applyIf(me, {
            title: me.fieldLabel || me.displayName || 'Редактирование сырого объекта: ' + Ext.htmlEncode(me.typeName),
            items: {
                xtype: 'textarea',
                itemId: 'editorArea',
                margin: 5,
                emptyText: emptyText,
                isFormField: false,
                listeners: {
                    'blur': {
                        fn: me.onEditorBlur,
                        scope: me
                    }
                }
            }
        });

        me.initField();
        me.callParent(arguments);
    },

    onEditorBlur: function (c) {
        if (c.isDirty()) {
            var me = this,
                value = c.getValue();

            me.mixins.field.setValue.call(me, Ext.JSON.decode(value));
        }
    },

    setValue: function () {
        var me = this,
            editorArea = me.down('editorArea');

        me.mixins.field.setValue.apply(me, arguments);

        if (me.value) {
            editorArea.setValue(Ext.JSON.encode(me.value));
        }
    },

    isValid: function() {
        return true;
    },

    destroy: function () {
        this.callParent();
        delete this.value;
    }
});