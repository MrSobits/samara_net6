Ext.define('B4.view.realityobj.RealityObjectConditionHouse', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.realityobjectconditionhousecmp',
    requires: [
        'B4.enums.ConditionHouse'
    ],

    displayField: 'Display',
    valueField: 'Value',
    forPermission: 'ConditionHouse',
    itemId: 'cbConditionHouseRealityObject',
    store: B4.enums.ConditionHouse.getStore()
});