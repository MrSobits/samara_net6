Ext.define('B4.view.dict.groupworkppr.EditWindow', {
    extend: 'B4.form.Window',

    layout: { type: 'vbox', align: 'stretch' },
    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 300,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Группа ППР',
    itemId: 'groupWorkPprEditWindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.ComboBox',
        'B4.store.dict.TemplateService'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    maxLength: 50
                },
                {
                    xtype: 'b4combobox',
                    itemId: 'cbService',
                    name: 'Service',
                    editable: false,
                    fieldLabel: 'Группа работ ППР',
                    store: Ext.create('B4.store.dict.TemplateService'),
                    queryMode: 'local',
                    triggerAction: 'all'
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