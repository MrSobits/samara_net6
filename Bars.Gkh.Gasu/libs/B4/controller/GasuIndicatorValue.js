Ext.define('B4.controller.GasuIndicatorValue', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.view.gasuindicator.ValueGrid',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Administration.ExportData.GasuIndicatorValue.Edit', applyTo: 'b4savebutton', selector: 'gasuindicatorvaluegrid' },
                { name: 'Administration.ExportData.GasuIndicatorValue.CreateValues', applyTo: 'button[action=CreateValues]', selector: 'gasuindicatorvaluepanel' },
                { name: 'Administration.ExportData.GasuIndicatorValue.SendService', applyTo: 'button[action=SendService]', selector: 'gasuindicatorvaluepanel' }
            ]
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'gasuIndicatorValueGridAspect',
            modelName: 'GasuIndicatorValue',
            gridSelector: 'gasuindicatorvaluegrid'
        }
    ],
    
    models: ['GasuIndicatorValue'],
    views: ['gasuindicator.ValueGrid',
            'gasuindicator.ValuePanel'],

    mainView: 'gasuindicator.ValueGrid',
    mainViewSelector: 'gasuindicatorvaluegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'gasuindicatorvaluepanel'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    init: function () {
        var me = this;
        
        this.control({
            'gasuindicatorvaluepanel button[action=CreateValues]': {
                'click': { fn: me.onCreateValues, scope: me }
            },
            'gasuindicatorvaluepanel button[action=SendService]': {
                'click': { fn: me.onSendService, scope: me }
            },
            'gasuindicatorvaluepanel b4combobox[name=Month]': {
                'change': { fn: me.onChangeMonth, scope: me }
            },
            'gasuindicatorvaluepanel b4combobox[name=Year]': {
                'change': { fn: me.onChangeYear, scope: me }
            }
        });
        
        this.callParent(arguments);
    },
    
    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('gasuindicatorvaluepanel');
        me.bindContext(view);
        me.application.deployView(view);
    },
    
    onCreateValues: function (btn) {
        var me = this,
            year = btn.up('gasuindicatorvaluepanel').down('[name=Year]').getValue(),
            month = btn.up('gasuindicatorvaluepanel').down('[name=Month]').getValue();
        
        if (!year || !month) {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать Год и Месяц отправки');
            return false;
        }
        
        me.mask('Идет формирование перечня показателей...', me.getMainView());
        B4.Ajax.request({
            url: B4.Url.action('CreateRecords', 'GasuIndicatorValue'),
            timeout: 9999999,
            params: {
                year: year,
                month: month
            }
        }).next(function () {
            me.unmask();
            me.changeFilters(year, month);
            
            B4.QuickMsg.msg('Успешно', 'Перечень показателей сформирован успешно!', 'success');
        }).error(function (e) {
            me.unmask();
            
            Ext.Msg.alert('Ошибка!', (e.message || 'Во время формирования перечня показателей произошла ошибка'));
        });
    },
    
    onSendService: function (btn) {
        var me = this,
            year = btn.up('gasuindicatorvaluepanel').down('[name=Year]').getValue(),
            month = btn.up('gasuindicatorvaluepanel').down('[name=Month]').getValue();

        if (!year || !month) {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать Год и Месяц отправки');
            return false;
        }

        me.mask('Идет отправка данных...', me.getMainView());
        B4.Ajax.request({
            url: B4.Url.action('SendService', 'GasuIndicatorValue'),
            timeout: 9999999,
            params: {
                year: year,
                month: month
            }
        }).next(function () {
            me.unmask();
            me.changeFilters(year, month);

            B4.QuickMsg.msg('Успешно', 'Данные отправлены!', 'success');
        }).error(function (e) {
            me.unmask();

            Ext.Msg.alert('Ошибка!', (e.message || 'Во время отправки данных произошла ошибка'));
        });
    },
    
    onChangeMonth : function(fld, newValue) {
        var me = this,
            year = fld.up('gasuindicatorvaluepanel').down('[name=Year]').getValue();
        me.changeFilters(year, newValue);
    },
    
    onChangeYear: function (fld, newValue) {
        var me = this,
            month = fld.up('gasuindicatorvaluepanel').down('[name=Month]').getValue();
        me.changeFilters(newValue, month);
    },
    
    changeFilters: function (year, month) {
        var me = this,
            store = me.getMainView().down('gasuindicatorvaluegrid').getStore();
        
        store.clearFilter(true);
        
        if (year > 0 && month > 0) {
            store.filter([
                { property: "year", value: year },
                { property: "month", value: month }
            ]);
        }
    }
});