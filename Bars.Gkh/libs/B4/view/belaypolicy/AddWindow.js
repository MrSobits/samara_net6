Ext.define('B4.view.belaypolicy.AddWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.store.BelayOrganization',
        'B4.store.dict.BelayOrgKindActivity',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'belayPolicyAddWindow',
    title: 'Страховой полис',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 200,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'BelayOrganization',
                    fieldLabel: 'Страховая организация',
                    store: 'B4.store.BelayOrganization',
                    textProperty: 'ContragentName',
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'ContragentName', flex: 1 }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'BelayOrgKindActivity',
                    fieldLabel: 'Вид страхуемой деятельности',
                    editable: false,
                    store: 'B4.store.dict.BelayOrgKindActivity'
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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