/*
Example:
    {
        xtype: 'gkhintfield',
        name: 'ShareSF',
        fieldLabel: 'Доля участия СФ(%)'
    }
*/

Ext.define("B4.view.Control.GkhIntField", {
    extend: 'Ext.form.field.Number',
    alias: 'widget.gkhintfield',

    //hideTrigger: true,
    keyNavEnabled: false,
    labelAlign: 'right',
    mouseWheelEnabled: false,
    allowDecimals: false
});