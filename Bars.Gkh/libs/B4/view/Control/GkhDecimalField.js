/*
Example:
    {
        xtype: 'gkhdecimalfield',
        name: 'ShareSF',
        fieldLabel: 'Доля участия СФ(%)'
    }
*/

Ext.define('B4.view.Control.GkhDecimalField', {
    extend: 'Ext.form.field.Number',
    alias: 'widget.gkhdecimalfield',

    hideTrigger: true,
    keyNavEnabled: false,
    labelAlign: 'right',
    mouseWheelEnabled: false,
    decimalSeparator: ','
});