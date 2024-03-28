Ext.define('B4.view.actcheck.RealityObjectEditPanel', {
    extend: 'Ext.panel.Panel',
    itemId: 'actCheckRealityObjectEditPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    alias: 'widget.actCheckRealityObjectEditPanel',
    border: false,
    requires: [
        'B4.form.ComboBox',
        'B4.enums.YesNo',
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
                    margin: '7 5 5 5',
                    itemId: 'taRealityObjAddress',
                    fieldLabel: 'Адрес',
                    readOnly: true
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'b4combobox',
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1,
                        margin: 5,
                        floating: false,
                        editable: false,
                        maxWidth: 350,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    items: [
                        {
                            itemId: 'cbHaveViolation',
                            fieldLabel: 'Нарушения выявлены',
                            items: B4.enums.YesNoNotSet.getItems(),
                            name: 'HaveViolation'
                        },
                        {
                            itemId: 'cbNeedReferral',
                            labelWidth: 300,
                            maxWidth: 500,
                            fieldLabel: 'Требуется направление в Роспотребнадзор',
                            items: B4.enums.YesNo.getItems(),
                            name: 'NeedReferral'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    itemId: 'taDescription',
                    margin: 5,
                    height: 70,
                    fieldLabel: 'Описание',
                    name: 'Description',
                    maxLength: 2000
                },
                {
                    xtype: 'actCheckViolationGrid',
                    border: false,
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});