Ext.define('B4.view.gisaddressmatching.HouseMunicipality', {
    extend: 'B4.form.Window',

    alias: 'widget.gisaddressmatchinghousemunicipality',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 450,
    minWidth: 450,
    height: 130,
    minHeight: 130,
    resizable: false,
    bodyPadding: 5,
    title: 'Присвоение муниципального образования для выбранных домов',
    closeAction: 'hide',
    //trackResetOnLoad: true,
    modal: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.enums.TypeHouse'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create("B4.store.gisaddressmatching.GisAddress");

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 140,
                editable: false,
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'MatchedHouses',
                    fieldLabel: 'Дома',
                    store: store,
                    blankText: 'Это поле обязательно для заполнения',
                    selectionMode: 'MULTI',
                    idProperty: 'Id',
                    textProperty: 'Address',
                    columns: [
                        {
                            text: 'Регион',
                            dataIndex: 'RegionName',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Населенный пункт',
                            dataIndex: 'CityName',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Улица',
                            dataIndex: 'StreetName',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Номер дома',
                            dataIndex: 'Number',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Поставщик',
                            dataIndex: 'Supplier',
                            flex: 2,
                            filter: {
                                xtype: 'textfield'
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Municipality',
                            width: 160,
                            text: 'Муниципальный район',
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListWithoutPaging'
                            }
                        }
                    ],
                    onSelectAll: function () {
                        var me = this,
                            oldValue = me.getValue(),
                            isValid = me.getErrors() != '';

                        me.updateDisplayedText('Выбраны все');
                        me.value = 'All';

                        me.fireEvent('validitychange', me, isValid);
                        me.fireEvent('change', me, 'All', oldValue);
                        me.validate();

                        me.selectWindow.hide();
                    }
                },
                {
                    xtype: 'b4combobox',
                    name: 'cmbMunicipality',
                    fieldLabel: 'Муниципальный район',
                    storeAutoLoad: false,
                    editable: false,
                    //valueField: 'Name',                    
                    url: '/Municipality/ListWithoutPaging'
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