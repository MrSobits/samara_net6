Ext.define('B4.view.service.CustomServiceWindow', {
    extend: 'B4.form.Window',
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 500,
    bodyPadding: 2,
    closeAction: 'destroy',
    trackResetOnLoad: true,
    title: 'Выберите тип услуги',
    itemId: 'customServiceWindow',
    layout: 'anchor',

    requires: [
        'B4.ux.button.Close',
        'B4.store.dict.TemplateService',
        'B4.form.SelectField',
        
        'B4.enums.KindServiceDi'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Вид услуги',
                    store: B4.enums.KindServiceDi.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    itemId: 'KindServiceDi'
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'TemplateService',
                    fieldLabel: 'Услуга',
                    itemId: 'templateService',
                   

                    store: 'B4.store.dict.TemplateService',
                    columns:
                    [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 },
                        { text: 'Вид услуги', dataIndex: 'KindServiceDi', flex: 1, renderer: function (val) { return B4.enums.KindServiceDi.displayRenderer(val); } }
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'customServiceButton',
                                    text: 'Продолжить',
                                    tooltip: 'Продолжить',
                                    iconCls: 'icon-add'
                                },
                                {
                                    xtype: 'tbfill'
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
