Ext.define('B4.view.actcheck.RealityObjectEditPanel', {
    extend: 'Ext.panel.Panel',
    itemId: 'actCheckRealityObjectEditPanel',
    layout: { type: 'vbox', align: 'stretch' },
    alias: 'widget.actCheckRealityObjectEditPanel',
    bodyPadding: 5,
    frame: true,
    border: false,
    requires: [
        'B4.form.ComboBox',
        'B4.enums.YesNoNotSet',
        'B4.view.actcheck.ViolationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    itemId: 'taRealityObjAddress',
                    fieldLabel: 'Адрес',
                    readOnly: true
                },
                {
                    xtype: 'b4combobox',
                    itemId: 'cbHaveViolation',
                    floating: false,
                    editable: false,
                    fieldLabel: 'Нарушения выявлены',
                    displayField: 'Display',
                    items: B4.enums.YesNoNotSet.getItems(),
                    valueField: 'Value',
                    name: 'HaveViolation',
                    maxWidth: 500
                },
                {
                    xtype: 'textarea',
                    itemId: 'taDescription',
                    height: 70,
                    fieldLabel: 'Описание',
                    name: 'Description',
                    maxLength: 1000
                },
                {
                    xtype: 'textarea',
                    padding: '5 0 0 0',
                    itemId: 'taNotRevealedViolations',
                    name: 'NotRevealedViolations',
                    fieldLabel: 'Невыявленные нарушения',
                    height: 70,
                    maxLength: 1000
                },
                {
                    xtype: 'actCheckViolationGrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});