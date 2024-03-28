Ext.define('B4.controller.gisrealestate.AnalysisOfIndicators', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: [
      'indicator.AnalysisOfIndicators'
    ],

    requires: [
        'B4.aspects.InlineGrid'
    ],

    mainView: 'indicator.AnalysisOfIndicators',
    mainViewSelector: 'indicatoranalysisofinicators',

    init: function () {
        var me = this;

        me.control({
            'indicatoranalysisofinicators b4selectfield[name=RealEstateType]': {
                'change': {
                    fn: me.changeRealEstateType,
                    scope: me
                }
            },
            'indicatoranalysisofinicators menuitem': {
                'click': {
                    fn: me.generateReport,
                    scope: me
                }
            }
        });
        
        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('indicatoranalysisofinicators');
        this.bindContext(view);
        this.application.deployView(view);
    },
    
    changeRealEstateType: function (selectfield, value) {
        var indifield = selectfield.up('panel').down('b4selectfield[name=RealEstateTypeIndicator]');
        indifield.setValue(undefined);
        indifield.setDisabled(!value);
        indifield.getStore().getProxy().setExtraParam('RealEstateTypeId', value ? value.Id : undefined);
    },
    
    generateReport: function (button) {
        var panel = button.up('indicatoranalysisofinicators');
        
        if (!panel.getForm().isValid()) {
            Ext.Msg.alert('Ошибка', 'Заполните все поля и проверьте период формирования отчета!');
            return;
        }
        
        panel.getEl().mask('Генерация отчета...');

        var reportParams = {
            type: panel.down('b4selectfield[name=RealEstateType]').getValue(),
            municipalities: panel.down('b4selectfield[name=Municipality]').getValue(),
            indicators: panel.down('b4selectfield[name=RealEstateTypeIndicator]').getValue(),
            dateFrom: panel.down('b4monthpicker[name=DateFrom]').getValue(),
            dateTo: panel.down('b4monthpicker[name=DateTo]').getValue()
        };

        B4.Ajax.request({
            url: 'BillingReport/GetReport',
            params: {
                ReportName: 'Отчет по индикаторам',
                ClassName: 'Bars.Gkh.Gis.Reports ReportIndicator',
                ExportFormat: button.formatId,
                ReportParams: Ext.encode(reportParams)
            },

            timeout: 600000
        }).next(function (response) {
            //Если отчет не надо выгружать => отобразить его на форме (exportFormat = 32 - html)
            var url = Ext.decode(response.responseText);
            if (button.formatId === 1) {
                // если pdf - открываем в новом окне
                window.open(url);
            } else {
                // иначе скачиваем
                document.location.href = url;
            }

            panel.getEl().unmask();
        })

        .error(function (response) {
            panel.getEl().unmask();
            Ext.Msg.alert('Ошибка!', !Ext.isString(response.message) ? 'При создании отчета произошла ошибка!' : response.message);
        });
    }
});