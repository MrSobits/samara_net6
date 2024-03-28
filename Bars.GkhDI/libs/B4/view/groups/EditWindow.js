Ext.define('B4.view.groups.EditWindow', {
    extend: 'B4.form.Window',
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 800,
    height: 450,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Шаблонная услуга управляющей компании',
    itemId: 'groupsDiEditWindow',
    layout: 'anchor',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.view.groups.RealityObjGroupGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    maxLength: 300,
                    labelWidth: 150,
                    labelAlign: 'right',
                    anchor: '100%'
                },
                {
                    xtype: 'direalityobjgroupgrid',
                    anchor: '100% -25'
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
                                },
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
