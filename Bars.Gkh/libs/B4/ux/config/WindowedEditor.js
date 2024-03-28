Ext.define('B4.ux.config.WindowedEditor', {
    extend: 'Ext.Component',
    alias: 'widget.windowededitor',

    subEditorCfg: null,

    value: null,

    editorWindow: null,
    subEditor: null,

    statics: {
        renderPreview: function (val, editor) {
            var subEditor = editor.subEditorCfg,
                editorClass = Ext.ClassManager.getByAlias('widget.' + subEditor.xtype),
                previewRenderer = editorClass && editorClass.renderPreview;

            if (previewRenderer) {
                return previewRenderer(val, editor);
            }

            return 'Редактирование';
        }
    },

    doSave: function () {
        this.value = this.subEditor.getValue();
        this.doClose();
    },

    doClose: function() {
        this.hide();
    },

    setValue: function (value) {
        var me = this;
        me.value = value;
        if (me.value) {
            me.createWindow();
            me.subEditor.setValue(me.value);
        }
    },

    reset: function () {
        delete this.value;
    },

    getValue: function () {
        return this.value;
    },

    isValid: function() {
        return true;
    },

    destroy: function () {
        this.destroyWindow();
        delete this.value;

        this.callParent();
    },

    destroyWindow: function() {
        if (this.editorWindow) {
            this.editorWindow.destroy();
            delete this.editorWindow;
            delete this.subEditor;
        }
    },

    createWindow: function() {
        var me = this,
            subEditor = me.subEditorCfg;

        if (!this.editorWindow) {
            me.subEditor = Ext.widget(Ext.applyIf(subEditor, { border: false, header: false }));
            me.editorWindow = Ext.create('Ext.window.Window', {
                title: me.subEditor.title || 'Редактирование',
                width: 800,
                height: 600,
                layout: 'fit',
                closable: false,
                modal: true,
                items: me.subEditor,
                tbar: [
                    {
                        xtype: 'buttongroup',
                        columns: 1,
                        items: [
                            {
                                xtype: 'b4savebutton',
                                handler: me.doSave.bind(me)
                            }
                        ]
                    },
                    '->',
                    {
                        xtype: 'buttongroup',
                        columns: 1,
                        items: [
                            {
                                xtype: 'b4closebutton',
                                handler: me.doClose.bind(me)
                            }
                        ]
                    }
                ]
            });
        }
    },

    focus: function () {
        var me = this;
        me.createWindow();
        me.editorWindow.show();
    },

    blur: function() {
        this.hide();
    },

    hide: function () {
        var me = this;
        me.destroyWindow();
        me.fireEvent('blur', me, null);
    }
});