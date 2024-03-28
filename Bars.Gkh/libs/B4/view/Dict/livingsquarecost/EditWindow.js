Ext.define('B4.view.dict.livingsquarecost.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'livingsquarecostEditWindow',
    title: 'Стоимость квадратного метра жилой площади',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.MunicipalitySelectTree',
        'B4.form.SelectField',
        'B4.form.TreeSelectField',
        'B4.store.dict.Municipality'

    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 180,
                labelAlign: 'right'
            },
            items: [

                {
                    xtype: 'treeselectfield',
                    name: 'Municipality',
                    itemId: 'tsfMunicipality',
                    fieldLabel: 'Муниципальное образование',
                    titleWindow: 'Выбор муниципального образования',
                    store: 'B4.store.dict.MunicipalitySelectTree',
                    editable: false
                },                
                {
                    xtype: 'numberfield',
                    name: 'Year',
                    fieldLabel: 'Год',
                    labelAlign: 'right',
                    labelWidth: 130,
                    anchor: '100%',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'Cost',
                    fieldLabel: 'Стоимость',
                    labelAlign: 'right',
                    labelWidth: 130,
                    anchor: '100%',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    minValue: 0
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