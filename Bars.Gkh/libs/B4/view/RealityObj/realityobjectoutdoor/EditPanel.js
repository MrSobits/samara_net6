Ext.define('B4.view.realityobj.realityobjectoutdoor.EditPanel', {
    extend: 'Ext.form.Panel',
    
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.realityobj.realityobjectoutdoor.RealityObjectsInOutdoorGrid',
        'B4.store.dict.MunicipalityFiasOktmo'
    ],
    mixins: ['B4.mixins.window.ModalMask'],

    closable: true,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    autoScroll: true,

    alias: 'widget.realityobjectoutdooreditpanel',
    title: 'Общие сведения',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 170
                    },
                    title: 'Общие сведения',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'MunicipalityFiasOktmo',
                            fieldLabel: 'Населенный пункт',
                            textProperty: 'OffName',
                            store: 'B4.store.dict.MunicipalityFiasOktmo',
                            allowBlank: false,
                            columns: [
                                {
                                    text: 'Населенный пункт',
                                    dataIndex: 'OffName',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'Муниципальное образование',
                                    dataIndex: 'Municipality',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            flex: 1,
                            fieldLabel: 'Наименование двора',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            flex: 1,
                            fieldLabel: 'Код двора',
                            allowBlank: false
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 170
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'Area',
                                    flex: 1,
                                    fieldLabel: 'Площадь двора (кв. м.)',
                                    minValue: 0,
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'AsphaltArea',
                                    flex: 1,
                                    fieldLabel: 'Площадь асфальта (кв. м.)',
                                    minValue: 0,
                                },
                            ]
                        },
                        {
                            xtype: 'numberfield',
                            name: 'RepairPlanYear',
                            flex: 1,
                            fieldLabel: 'Плановый год ремонта',
                            minValue: 2000,
                            maxValue: 2100,
                            margin: '5 0'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Description',
                            flex: 1,
                            fieldLabel: 'Примечание',
                            margin: '5 0'
                        },
                        {
                            xtype: 'realityobjectsinoutdoorgrid'
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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