Ext.define('B4.view.wizard.preparedata.ParametersStepFrame', {
    extend: 'B4.view.wizard.WizardBaseStepFrame',
    stepId: 'pageParameters',
    title: 'Параметры экспорта',
    layout: 'vbox',
    frame: true,
    items: [{
        xtype: 'label',
        name: 'Parameters',
        text: 'Для данного метода отсутствуют параметры фильтрации',
        padding: '15 5 15 5'
    }],

    allowBackward: function () {
        return this.wizard.dataSupplierIsRequired && !this.wizard.autoDataSupplier;
    },

    allowForward: function () {
        return true;
    },

    doBackward: function () {
        this.wizard.setCurrentStep('dataSupplier');
    },
   
    doForward: function () {
        var me = this;

        me.wizard.mask();
        B4.Ajax.request({
            url: B4.Url.action('SchedulePrepareData', 'GisIntegration'),
            params: me.getPrepareDataParams(),
            timeout: 9999999
        }).next(function () {
            me.wizard.result = {
                state: 'success',
                message: 'Задача подготовки данных успешно запланирована.'
                    + '<br><br>'
                    + 'Выполнение задачи отражено в реестре задач.'
            }
            me.wizard.setCurrentStep('finish');

            me.wizard.openTaskTree = true;

            me.wizard.unmask();
        }, me).error(function (e) {
            me.wizard.result = {
                state: 'error',
                message: e.message || 'Не удалось запланировать подготовку данных'
            };
            me.wizard.setCurrentStep('finish');
            me.wizard.unmask();
        }, me);

        return;
    },

    getPrepareDataParams: function () {
        var me = this,
            params = {},
            result = {
                    exporter_Id: me.wizard.exporter_Id
                },
            dataSupplierIds = me.wizard.dataSupplierIds || [0];

        Ext.each(dataSupplierIds,
            function (dataSupplierId) {
                var p = me.getParams(dataSupplierId);
                if (Ext.isObject(p)) {
                    Ext.iterate(p,
                        function(k, v) {
                            if (Ext.isArray(v)) {
                                p[k] = v.join(',');
                            }
                        });
                }

                params[dataSupplierId] = p;
            });
        
        result['params'] = Ext.encode(params);

        return result;
    },

    // переопределяем для каждого конкретного экспортера
    getParams: function (dataSupplierId) { return {}; }
});