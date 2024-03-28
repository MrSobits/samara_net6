Ext.define('B4.view.wizard.preparedata.DataSupplierStepFrame',
    {
        extend: 'B4.view.wizard.WizardBaseStepFrame',

        requires: [
                'B4.view.integrations.gis.DataSupplierGrid'
            ],

        stepId: 'dataSupplier',
        title: 'Выбор поставщиков данных',
        layout: 'border',
        border: false,
        items: [
                {
                    region: 'west',
                    width: 150,
                    baseCls: 'icon_wizard'
                },
                {
                    region: 'center',
                    layout: 'fit',
                    defaults: {
                            layout: 'fit'
                        },
                    items: [
                            {
                                xtype: 'datasuppliergrid',
                                border: false
                            }
                        ]
                }
            ],

        allowBackward: function() {
            return false;
        },

        allowForward: function() {
            return !this.wizard.dataSupplierIsRequired || this.wizard.dataSupplierIds;
        },

        doForward: function() {
            var me = this;

            me.wizard.setCurrentStep('pageParameters');
        },

        init: function() {
            var me = this,
                grid = me.down('datasuppliergrid'),
                store = grid.getStore();

            if (!me.wizard.dataSupplierIsRequired) {
                me.wizard.setCurrentStep('pageParameters');
                return;
            }

            me.wizard.mask();
            B4.Ajax.request({
                url: B4.Url.action('ListContragents', 'GisIntegration'),
                params: {
                    exporter_Id: me.wizard.exporter_Id
                },
                timeout: 9999999
            })
                .next(function (response) {
                    var json = Ext.JSON.decode(response.responseText);

                    me.wizard.unmask();

                    if (json.data.length === 1) {
                        me.wizard.dataSupplierIds = [ json.data[0].Id ];
                        me.wizard.autoDataSupplier = true;
                        me.wizard.setCurrentStep('pageParameters');
                        return;
                    }

                    store.loadData(json.data);
                },
                    me)
                .error(function (e) {
                    me.wizard.result = {
                        state: 'error',
                        message: e.message || 'Не удалось получить список поставщиков данных'
                    }
                    me.wizard.setCurrentStep('finish');
                    me.wizard.unmask();
                },
                    me);
        },

        firstInit: function() {
            var me = this,
                grid = me.down('datasuppliergrid'),
                sm = grid.getSelectionModel();

            sm.on('selectionchange',
                function(sm, selected) {
                    me.wizard.dataSupplierIds = selected && selected.length
                                                ? selected.map(function(rec) {
                                                    return rec.get('Id');
                                                })
                                                : null;

                    me.fireEvent('selectionchange', me);
                });
        }
    });