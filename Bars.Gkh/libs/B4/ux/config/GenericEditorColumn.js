Ext.define('B4.ux.config.GenericEditorColumn', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.genericeditorcolumn',

    requires: [
        'B4.ux.config.WindowedEditor'
    ],

    complexTypes: ['collection', 'dictionary', 'object'],

    typeField: 'type',

    initComponent: function() {
        this.callParent(arguments);
        this.renderer = this.renderer.bind(this);
    },

    getEditorInner: function(rec) {
        var type = rec.get(this.typeField) || { editor: 'raw' },
            readOnly = rec.get('readOnly'),
            editor = {
                readOnly: readOnly
            };
        switch (type.editor) {
            case 'custom':
                if (type.path && typeof type.path == 'string' && type.path.length > 0) {
                    Ext.Loader.syncRequire(type.path);
                }

                editor.xtype = type.xtype;
                editor.meta = type.meta;
                break;
            case 'text':
                editor.xtype = 'textfield';
                break;
            case 'bool':
                editor.xtype = 'checkbox';
                break;
            case 'number':
                editor.xtype = 'numberfield';
                editor.allowDecimals = type.decimals;
                editor.minValue = type.minValue;
                editor.maxValue = type.maxValue;
                editor.decimalSeparator = ',';
                break;
            case 'date':
                editor.xtype = 'datefield';
                editor.format = 'Y-m-dTH:i:s';
                editor.submitFormat = 'Y-m-d';
                break;
            case 'enum':
                editor.xtype = 'enumeditor';
                editor.values = type.values;
                break;
            case 'collection':
                editor.xtype = 'collectioneditor';
                editor.elementType = type.elementType;
                break;
            case 'dictionary':
                editor.xtype = 'dictionaryeditor';
                editor.keyType = type.keyType;
                editor.valueType = type.valueType;
                break;
            case 'object':
                editor.xtype = 'objecteditor';
                editor.properties = type.properties;
                editor.typeName = type.typeName;
                editor.displayName = type.displayName;
                break;
            case 'section':
                return false;
            case 'raw':
                editor.xtype = 'raweditor';
                editor.typeName = type.typeName;
                editor.sample = type.sample;
                break;
            default:
                if (rec.get(this.dataIndex) instanceof Object) {
                    editor.xtype = 'raweditor';
                } else {
                    editor.xtype = 'textfield';
                }
                break;
        }

        return editor;
    },

    getEditor: function (rec) {
        var editor = this.getEditorInner(rec);
        switch (editor.xtype) {
            case 'collectioneditor':
            case 'dictionaryeditor':
            case 'objecteditor':
            case 'raweditor':
                return {
                    xtype: 'windowededitor',
                    subEditorCfg: editor
                };
            default:
                return editor;
        }
    },

    renderer: function(val, meta, rec) {
        var editor = this.getEditorInner(rec),
            editorClass = Ext.ClassManager.getByAlias('widget.' + editor.xtype),
            previewRenderer = editorClass && editorClass.renderPreview;

        if (previewRenderer) {
            meta.tdCls = 'x-grid-complex-object-cell';
            return previewRenderer(val, editor);
        }

        return val;
    }
});