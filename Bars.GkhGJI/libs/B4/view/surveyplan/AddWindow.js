Ext.define('B4.view.surveyplan.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.surveyplanaddwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    minWidth: 700,
    bodyPadding: 5,
    resizable: false,
    title: 'План проверки',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.PlanJurPersonGji'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 500
                },
                {
                    xtype: 'b4selectfield',
                    name: 'PlanJurPerson',
                    fieldLabel: 'План',
                    store: 'B4.store.dict.PlanJurPersonGji',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
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