Ext.define('B4.view.license.DisclosureInfoLicensePanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    bodyStyle: Gkh.bodyStyle,
    style: 'background: none repeat scroll 0 0 #DFE9F6',
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Лицензии',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    alias: 'widget.disinfolicensepanel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.license.DisclosureInfoGrid',
        'B4.form.ComboBox',
        'B4.enums.YesNoNotSet'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'HasLicense',
                    items: B4.enums.YesNoNotSet.getItems(),
                    displayField: 'Display',
                    valueField: 'Value',
                    fieldLabel: 'Имеется ли лицензия на осуществление деятельности по управлению многоквартирными домами',
                    editable: false,
                    value: 30,
                    maxWidth: 600,
                    labelWidth: 450
                },
                {
                    xtype: 'disinfolicensegrid',
                    flex: 1,
                    disabled: true
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    action: 'savehaslicense'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
