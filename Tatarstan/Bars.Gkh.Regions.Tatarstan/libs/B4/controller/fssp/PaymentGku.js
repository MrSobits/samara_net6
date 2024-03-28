Ext.define('B4.controller.fssp.PaymentGku', {
    extend: 'B4.base.Controller',
    requires: [],

    views: [
        'fssp.paymentgku.PaymentGkuGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'fssp.paymentgku.PaymentGkuGrid',
    mainViewSelector: 'paymentgkugrid',

    init: function() {
        var me = this;

        me.control({
            'paymentgkugrid [name=PeriodStart]': {
                change: me.onPeriodLimitChange
            },
            
            'paymentgkugrid [name=PeriodEnd]': {
                change: me.onPeriodLimitChange
            },
            
            'paymentgkugrid [name=Address]': {
                change: me.onAddressChange
            },

            'paymentgkugrid button[action=applyFilter]': {
                click: me.onApplyFilter
            },

            'paymentgkugrid button[action=dropFilter]': {
                click: me.onDropFilter
            }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);
        
        view.store.on('beforeload', me.onBeforeStoreLoad, me);
    },

    onPeriodLimitChange: function (field, newValue, oldValue)
    {
        if(!newValue){
            return;
        }
        
        var me = this,
            secondPeriodLimitFieldName = field.name === 'PeriodEnd' ? 'PeriodStart' : 'PeriodEnd',
            secondPeriodLimitField = field.up().down('[name=' + secondPeriodLimitFieldName + ']'),
            periodLimitValues = [];

        periodLimitValues[secondPeriodLimitFieldName] = secondPeriodLimitField.getValue();
        periodLimitValues[field.name] = newValue;
        
        if(periodLimitValues[secondPeriodLimitFieldName]){
            if(periodLimitValues['PeriodEnd'] < periodLimitValues['PeriodStart']){
                Ext.Msg.alert('Некорректный период!', 'Левая граница периода не может быть больше правой границы');
                field.setValue(oldValue);
                return;
            }
            
            var monthsInPeriodCount = periodLimitValues['PeriodEnd'].getMonth() - periodLimitValues['PeriodStart'].getMonth() + 
                12 * (periodLimitValues['PeriodEnd'].getYear() - periodLimitValues['PeriodStart'].getYear());
            
            if(monthsInPeriodCount > 12){
                Ext.Msg.alert('Некорректный период!', 'Период не может превышать 12 месяцев');
                field.setValue(oldValue);
                return;
            }
        }
    },
    
    onAddressChange: function(field, newValue, oldValue){
        var me = this;
        
        if(newValue === oldValue){
            return;
        }
        
        if(newValue){
            me.getIndEntrRegistrationNumber(field.addressId);
        }
    },
    
    getIndEntrRegistrationNumber: function(addressId){
        var me = this;

        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action('GetIndEntrRegistrationNumbers', 'Litigation'),
            params: {
                addressId: addressId
            }
        }).next(function (response) {
            var resp = Ext.JSON.decode(response.responseText),
                data = resp.data,
                registrationNumberField = me.getMainView().down('[name=RegistrationNumber]');
            
                registrationNumberField.setValue(data);
        }).error(function (response) {
            var resp = Ext.decode(response.responseText);
            me.controller.unmask();
            Ext.Msg.alert('Ошибка поиска регистрационного номера ИП!', resp.message);
        });
    },

    onApplyFilter: function(btn){
        var me = this,
            grid = me.getMainView();
        
        grid.getStore().load();
    },

    onDropFilter: function()
    {
        var me = this,
            grid = me.getMainView(),
            fields = [
                grid.down('[name=PeriodStart]'),
                grid.down('[name=PeriodEnd]'),
                grid.down('[name=RegistrationNumber]'),
                grid.down('[name=Address]')
            ];
        
        fields.forEach(function(field){
            field.setValue(null);
        });
    },
    
    onBeforeStoreLoad: function(store, operation){
        var me = this,
            grid = me.getMainView(),
            invalidFields = me.validate();

        if(invalidFields.length !== 0){
            Ext.Msg.alert('Ошибка!', 'Не заполнены обязательные поля: "' + invalidFields.join('", "') + '"');
            grid.getView().loadMask.hide();
            
            return false;
        }

        var addressId = grid.down('[name=Address]').getValue(),
            periodStart = grid.down('[name=PeriodStart]').getValue(),
            periodEnd = grid.down('[name=PeriodEnd]').getValue();
        
        Ext.apply(operation.params, {
            addressId: addressId,
            startDate: periodStart,
            endDate: periodEnd
        });
    },
    
    validate: function(){
        var me = this,
            grid = me.getMainView(),
            startPeriodField = grid.down('[name=PeriodStart]'),
            endPeriodField = grid.down('[name=PeriodEnd]'),
            addressField = grid.down('[name=Address]'),
            fields = [startPeriodField, endPeriodField, addressField],
            invalidFields = fields.filter(function(field){
                var fieldValue = field.getValue();
                
                return  fieldValue === null || fieldValue === 0;
            });
        
        return invalidFields.map(function (field){
            return field.fieldLabel;
        })
    }
});