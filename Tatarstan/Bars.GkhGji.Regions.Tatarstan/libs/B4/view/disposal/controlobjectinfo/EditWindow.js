Ext.define('B4.view.disposal.controlobjectinfo.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    itemId: 'disposalControlObjectEditWindow',
    title: 'Сведения об объекте контроля',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.disposal.ControlObjectInfoRealityObject',
        'B4.store.disposal.DisposalControlObjectKind'
    ],

    initComponent: function () {
        var me = this,
            realityObjectStore = Ext.create('B4.store.disposal.ControlObjectInfoRealityObject'),
            controlObjectKindStore = Ext.create('B4.store.disposal.DisposalControlObjectKind');
            
        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Проверяемый дом',
                    name: 'InspGjiRealityObject',
                    textProperty: 'Address',
                    store: realityObjectStore,
                    columns: [
                        { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield'}}
                    ],
                    editable: false,
                    allowBlank: false,
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ControlObjectKind',
                    store: controlObjectKindStore,
                    fieldLabel: 'Вид объекта контроля',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        { text: 'Нименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'}},
                        { 
                            text: 'Тип объекта контроля', 
                            dataIndex: 'ControlObjectType', 
                            flex: 1,
                            filter: { xtype: 'textfield'},
                            renderer: function (val){
                                if(val && val.Name){
                                    return val.Name;
                                }
                                return '';
                            }, 
                        }
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