Ext.define('B4.view.fssp.paymentgku.PgmuSelectAddress', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.pgmuselectaddress',
    requires: ['B4.view.fssp.paymentgku.PgmuSelectAddressWindow'],
    
    editable: false,

    trigger1Cls: 'x-form-search-trigger',
    trigger2Cls: 'x-form-clear-trigger',

    onTrigger1Click: function () {
        var me = this;
        if (!me.win) {
            me.createControls();
        }
        
        if(me.data) {
            me.loadData();
        }
        
        me.win.show();
    },

    onTrigger2Click: function () {
        this.setValue(null);
    },

    validateForm: function (wind) {
        var me = this,
            form = wind.getForm();

        if (form) {
            if (!form.isValid()) {
                Ext.Msg.alert('Сообщение', 'Адрес заполнен не корректно');
                return false;
            }
        }

        return true;
    },

    setPgmuAddressValues: function(addressValue, data){
        var me = this;
        
        Ext.Ajax.request({
            method: 'GET',
            url: B4.Url.action('/PgmuAddress/GetAddressId'),
            params: { valuesDict: Ext.JSON.encode(Object.fromEntries(data)) },
            success: function (response) {
                var id = Ext.JSON.decode(response.responseText),
                    oldValue;

                oldValue = me.getValue()
                me.addressId = id || 0;
                me.data = data;
                me.setRawValue.call(me, addressValue || '');
                me.fireEvent('change', me, id, oldValue);
            }
        });
    },
    
    selectComboBoxValue: function(combo){
        var me = this,
            value = me.data.get(combo.propertyName);
        
        if(value) {
            combo.getStore().add(value);
            combo.setValue(value);
            combo.fireEvent('select', combo, combo.getStore().getRange());
        }
    },

    loadData: function () {
        var me = this;
        
        me.selectComboBoxValue(me.win.districtComboBox);
        me.selectComboBoxValue(me.win.townComboBox);
        me.selectComboBoxValue(me.win.streetComboBox);
        me.selectComboBoxValue(me.win.houseComboBox);
        me.selectComboBoxValue(me.win.buildingComboBox);
        me.selectComboBoxValue(me.win.apartmentComboBox);
        me.selectComboBoxValue(me.win.roomComboBox);
    },
    
    acceptForm: function (wind) {
        var me = this;
        if (me.validateForm(wind)) {
            var addressValue = wind.addressField.getValue(),
                data = wind.addressDictionary;
            
            me.setValue(data, addressValue);
            me.win.destroy();
        }
    },

    cancelForm: function () {
        var me = this;
        
        me.win.destroy();
    },

    resetForm: function () {
        var me = this;
        
        if(me.win)
        {
            me.win.getForm().reset();
        }
    },

    setValue: function (data, addressValue) {
        var me = this;
        
        if (!data) {
            me.addressId = 0;
            me.data = null;
            me.setRawValue.call(me, null);
            me.resetForm();
        } else {
            me.setPgmuAddressValues(addressValue, data);
        }
    },

    getValue: function () {
        var me = this;
        if (!me.addressId) {
            return 0;
        }
        return me.addressId;
    },

    createControls: function () {
        var me = this;

        if (!me.win) {
            me.win = Ext.create('B4.view.fssp.paymentgku.PgmuSelectAddressWindow',
                {
                    constrain: true,
                    autoDestroy: true,
                    closeAction: 'destroy',
                    renderTo: B4.getBody().getActiveTab().getEl()
                });

            me.win.on('acceptform', me.acceptForm, me);
            me.win.on('resetform', me.resetForm, me);
            me.win.on('cancelform', me.cancelForm, me);
            me.win.on('destroy', function () {
                me.win = null;
            }, me);
        }
    }
});