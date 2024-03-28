Ext.define('B4.ux.config.PredefinedValuesComboEditor', {
    extend: 'Ext.form.ComboBox',
    alias: 'widget.predefinedvaluescombobox',

    requires: ['Ext.data.ArrayStore'],

    editable: false,

    valueField: 'Id',
    displayField: 'Name',

    initComponent: function () {
        var config = this.meta;
        Ext.apply(this, {
            mode: 'local',
            triggerAction: 'all',
            store: Ext.create('Ext.data.ArrayStore', {
                autoDestroy: true,
                id: 0,
                fields: config.fields ? config.fields : [config.valueField || this.valueField, config.displayField || this.displayField],
                data: Ext.Array.map(config.possibleValues, function(item) { return [item, item] })
            })
        });

        this.callParent(arguments);
    }
});