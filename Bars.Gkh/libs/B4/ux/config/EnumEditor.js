Ext.define('B4.ux.config.EnumEditor', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.enumeditor',

    values: [],

    queryMode: 'local',
    displayField: 'displayName',
    valueField: 'value',

    store: null,

    editable: false,

    statics: {
        renderPreview: function (val, editor) {
            var value = editor.values.filter(function(e) { return e.value == val; })[0];
            return value ? value.displayName : val;
        }
    },

    initComponent: function () {
        var me = this;

        me.store = Ext.create('Ext.data.Store', {
            fields: ['displayName', 'value']
        });

        me.store.loadData(this.values);
        me.callParent(arguments);
    }
});