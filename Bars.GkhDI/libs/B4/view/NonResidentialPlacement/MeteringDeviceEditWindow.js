Ext.define('B4.view.nonresidentialplacement.MeteringDeviceEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 600,
    bodyPadding: 5,
    itemId: 'nonResidentialPlacementMeteringDeviceEditWindow',
    title: 'Редактирование записи',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.MeteringDevice'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
              {
                  xtype: 'b4selectfield',
                  name: 'MeteringDevice',
                  fieldLabel: 'Прибор учета',
                  anchor: '100%',
                 

                  store: 'B4.store.dict.MeteringDevice',
                  editable: false,
                  allowBlank: false
              }              
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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
                            columns: 1,
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