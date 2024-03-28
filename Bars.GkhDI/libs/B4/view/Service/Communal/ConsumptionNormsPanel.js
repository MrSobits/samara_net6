Ext.define('B4.view.service.communal.ConsumptionNormsPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.consumptionnormspanel',
    mixins: ['B4.mixins.window.ModalMask'],
    width: 760,
    height: 600,
    bodyPadding: 5,
    itemId: 'consumptionNormsPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.TemplateService',
        'B4.store.dict.UnitMeasure',
        'B4.store.service.ContragentForProvider',
        'B4.form.SelectField',
        'B4.form.ComboBox'

    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor', 
                    height: 230,
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items:
                    [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsCnLivingHouse',
                            defaults: {
                                labelWidth: 190,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Норматив потребления коммунальной услуги в жилых помещениях',
                            items: [
                                {
                                    xtype: 'gkhdecimalfield',
                                    name: 'ConsumptionNormLivingHouse',
                                    fieldLabel: 'Норматив потребления коммунальной услуги в жилых помещениях',
                                    itemId: 'cnLivingHouse',
                                    maxLength: 300
                                },

                                {
                                    xtype: 'b4selectfield',
                                    name: 'UnitMeasureLivingHouse',
                                    flex: 1,
                                    itemId: 'cnLivingHouselUnitMeasure',
                                    fieldLabel: 'Единица измерения норматива потребления коммунальной услуги',
                                    anchor: '100%',
                                    editable: false,
                                    store: 'B4.store.dict.UnitMeasure'
                                },

                                {
                                    xtype: 'textareafield',
                                    name: 'AdditionalInfoLivingHouse',
                                    padding: '10 0 0 0',
                                    fieldLabel: 'Дополнительно',
                                    itemId: 'cnLivingHouseAddInfo',
                                    maxLength: 300
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            itemId: 'fsCnHousing',
                            defaults: {
                                labelWidth: 190,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Норматив потребления коммунальной услуги на общедомовые нужды',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ConsumptionNormHousing',
                                    fieldLabel: 'Норматив потребления коммунальной услуги на общедомовые нужды',
                                    itemId: 'cnHousing',
                                    maxLength: 300
                                },

                                {
                                    xtype: 'b4selectfield',
                                    name: 'UnitMeasureHousing',
                                    flex: 1,
                                    itemId: 'cnHousingUnitMeasure',
                                    fieldLabel: 'Единица измерения норматива потребления услуги на домовые нужды',
                                    anchor: '100%',
                                    editable: false,
                                    store: 'B4.store.dict.UnitMeasure'
                                },

                                {
                                    xtype: 'textareafield',
                                    name: 'AdditionalInfoHousing',
                                    padding: '10 0 0 0',
                                    fieldLabel: 'Дополнительно',
                                    itemId: 'cnHousingAddInfo',
                                    maxLength: 300
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
