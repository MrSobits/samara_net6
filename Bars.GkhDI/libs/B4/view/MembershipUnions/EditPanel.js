Ext.define('B4.view.membershipunions.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Членство в объединениях',
    itemId: 'membershipUnionsEditPanel',
    layout: 'border',

    requires: [
        'B4.view.membershipunions.GridPanel',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        
        'B4.enums.YesNoNotSet'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyPadding: 2,
                    bodyStyle: Gkh.bodyStyle,
                    height: 35,
                    items: [
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'В данном отчетном периоде организация состояла в объединениях',
                            labelStyle: 'font-weight:bold; color: #0440A5; font-size: 11px;',
                            store: B4.enums.YesNoNotSet.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'MembershipUnions',
                            itemId: 'cbMembershipUnions',
                            labelWidth: 405
                        }
                    ]
                },
                {
                    xtype: 'membershipunionsgridpanel',
                    region: 'center'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    idProperty: 'helpButton',
                                    text: 'Справка',
                                    tooltip: 'Справка',
                                    iconCls: 'icon-help'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'membershipUnionsButton',
                                    text: 'Членство в объединениях',
                                    tooltip: 'Членство в объединениях',
                                    iconCls: 'icon-pencil-go'
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
