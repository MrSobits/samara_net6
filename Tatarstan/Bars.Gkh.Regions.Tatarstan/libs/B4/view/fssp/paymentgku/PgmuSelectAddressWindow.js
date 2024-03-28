Ext.define('B4.view.fssp.paymentgku.PgmuSelectAddressWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    itemId: 'fiasSelectAddressWindow',
    title: 'Адрес',
    closeAction: 'destroy',
    padding: 2,

    requires: [
        'B4.form.ComboBox',
    ],

    districtComboBox: null,
    townComboBox: null,
    streetComboBox: null,
    houseComboBox: null,
    buildingComboBox: null,
    apartmentComboBox: null,
    roomComboBox: null,
    addressField: null,
    addressDictionary: null,
    fieldFillingAddressDictionary: null,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents(
            'acceptform',
            'resetform',
            'cancelform'
        );
    },

    initComponent: function () {
        var me = this,
            innerFields = [],
            config = {
                editable: true,
                disabled: true,
                labelWidth: 60,
                labelAlign: 'right',
                storeAutoLoad: false,
                flex: 1,
                margin: '5 35 5 35',
                typeAhead: false,
                mode: 'remote',
                triggerAction: 'query',
                autoSelect: true,
                queryParam: 'value',
                valueField: 'Value',
                displayField: 'Value',
                queryCaching: false,
                loadingText: 'Загрузка...',
                trigger1Cls: 'x-form-clear-trigger',
                url: '/PgmuAddress/GetAddressObjectList',
                selectOnFocus: false,
                // Переопределяем для того, чтобы комобобокс не блокировал возможность выбора элемента, полностью совпадающим с запросом
                findRecordByValue: function(){},
                setValue: function (value) {
                    if (value && typeof value == 'object') {
                        var data = value;
                        if (value[0] && value[0].data)
                            data = value[0].data;

                        value = data[this.valueField];
                    }

                    this.self.superclass.setValue.call(this, [value]);
                },
                onTrigger1Click: function() {
                    if(this.bindedCombo){
                        me[this.bindedCombo].allowBlank = false;
                    }
                    
                    this.clearValue();
                    this.setEditable(true);
                },
                listeners: {
                    storebeforeload: {
                        fn: function (field, store, options) {
                            options.params.propertyName = field.propertyName;
                            options.params.parentValuesDict = Ext.JSON.encode(Object.fromEntries(me.addressDictionary));
                        },
                        scope: me
                    },
                    change: {
                        fn: function(combo, newValue, oldValue){
                            if(newValue){
                                return;
                            }
                            
                            me.clearComboBox(combo);
                            if(combo.dependedCombo){
                                if(Ext.isArray(combo.dependedCombo)){
                                    combo.dependedCombo.forEach(function(obj){
                                        var dependedCombo = me[obj];
                                        
                                        dependedCombo.setDisabled(true);
                                        me.clearComboBox(dependedCombo);
                                    })
                                }
                                else{
                                    var dependedCombo = me[combo.dependedCombo];
                                    
                                    dependedCombo.setDisabled(true);
                                    me.clearComboBox(dependedCombo);
                                }
                            }

                            me.fillAddressField();
                        },
                        scope: me
                    }
                }
            };
        me.addressDictionary = new Map();
        me.fieldFillingAddressDictionary = new Map();
        
        // Инициализация компонентов окна
        var label = Ext.create('Ext.form.Label', {
            html: '<div style="line-height: 1.5; text-align: center">' +
            'Для заполнения адреса необходимо последовательно выбрать район, город, затем улицу и дом.<br>' +
            'Для очистки выбранных значений используйте "крестик" напротив полей.</div>',
            margin: 10
        });
        innerFields.push(label);
        
        innerFields.push(Ext.create('Ext.menu.Separator', {
            style: {
                backgroundColor: '#9cbbe7'
            },
            width : '100%',
            margin: '0 0 10 0'
        }));
        
        me.districtComboBox = Ext.create('B4.form.ComboBox', Ext.applyIf({
            fieldLabel: 'Район',
            emptyText: 'Введите название района...',
            level: 1,
            editable: true,
            disabled: false,
            propertyName: 'District',
            minChars: 1
        }, config));
        me.districtComboBox.on('select', function (combo, records) {
            me.selectFunction(combo, records);
        });
        innerFields.push(me.districtComboBox);
        
        me.townComboBox = Ext.create('B4.form.ComboBox', Ext.applyIf({
            fieldLabel: 'Город',
            emptyText: 'Введите название города...',
            level: 2,
            predicate: 'г.',
            allowBlank: false,
            disabled: false,
            propertyName: 'Town',
            dependedCombo: 'streetComboBox',
            minChars: 3
        }, config));
        me.townComboBox.on('select', function (combo, records) {
            me.selectFunction(combo, records);
        });
        innerFields.push(me.townComboBox);

        me.streetComboBox = Ext.create('B4.form.ComboBox', Ext.applyIf({
            fieldLabel: 'Улица',
            emptyText: 'Введите название улицы...',
            level: 3,
            predicate: 'ул.',
            allowBlank: false,
            propertyName: 'Street',
            dependedCombo: ['houseComboBox', 'buildingComboBox'],
            minChars: 3
        }, config));
        me.streetComboBox.on('select', function (combo, records) {
            me.selectFunction(combo, records);
        });
        innerFields.push(me.streetComboBox);

        me.houseComboBox = Ext.create('B4.form.ComboBox', Ext.applyIf({
            fieldLabel: 'Дом',
            emptyText: 'Введите номер дома...',
            level: 4,
            predicate: 'д.',
            allowBlank: false,
            propertyName: 'House',
            dependedCombo: ['apartmentComboBox', 'roomComboBox'],
            minChars: 1
        }, config));
        me.houseComboBox.on('select', function (combo, records) {
            me.selectFunction(combo, records);
        });
        innerFields.push(me.houseComboBox);

        me.buildingComboBox = Ext.create('B4.form.ComboBox', Ext.applyIf({
            fieldLabel: 'Корпус',
            emptyText: 'Введите номер корпуса...',
            level: 5,
            predicate: 'корп.',
            allowBlank: true,
            propertyName: 'Building',
            minChars: 1
        }, config));
        me.buildingComboBox.on('select', function (combo, records) {
            me.selectFunction(combo, records);
        });
        innerFields.push(me.buildingComboBox);

        me.apartmentComboBox = Ext.create('B4.form.ComboBox', Ext.applyIf({
            fieldLabel: 'Квартира',
            emptyText: 'Введите номер квартиры...',
            level: 6,
            predicate: 'кв.',
            allowBlank: false,
            propertyName: 'Apartment',
            bindedCombo: 'roomComboBox',
            minChars: 1
        }, config));
        me.apartmentComboBox.on('select', function (combo, records) {
            var roomCombo = combo.up('window').down('[propertyName=Room]');
            
            roomCombo.allowBlank = true;
            roomCombo.clearInvalid();
            me.selectFunction(combo, records);
        });
        innerFields.push(me.apartmentComboBox);

        me.roomComboBox = Ext.create('B4.form.ComboBox', Ext.applyIf({
            fieldLabel: 'Комната',
            emptyText: 'Введите номер комнаты...',
            level: 7,
            predicate: 'ком.',
            allowBlank: false,
            propertyName: 'Room',
            bindedCombo: 'apartmentComboBox',
            minChars: 1
        }, config));
        me.roomComboBox.on('select', function (combo, records) {
            var apartmentCombo = combo.up('window').down('[propertyName=Apartment]');

            apartmentCombo.allowBlank = true;
            apartmentCombo.clearInvalid();
            me.selectFunction(combo, records);
        });
        innerFields.push(me.roomComboBox);

        innerFields.push(Ext.create('Ext.menu.Separator', {
            style: {
                backgroundColor: '#9cbbe7'
            },
            width : '100%',
            margin: '10 0 0 0'
        }));
        
        me.addressField = Ext.create('Ext.form.field.Text', {
            name: 'Address',
            labelWidth: 60,
            labelAlign: 'right',
            fieldLabel: 'Адрес',
            readOnly: true,
            margin: '20 35 20 35',
            flex: 1,
        });
        innerFields.push(me.addressField);
        
        Ext.applyIf(me, {
            items: innerFields,
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items:
                                [
                                    {
                                        text: 'Применить',
                                        iconCls: 'icon-accept',
                                        handler: function () {
                                            me.fireEvent('acceptform', me);
                                        },
                                        scope: this
                                    },
                                    {
                                        text: 'Сбросить',
                                        iconCls: 'icon-cross',
                                        handler: function () {
                                            me.fireEvent('resetform', me);
                                        },
                                        scope: me
                                    }
                                ]
                        }
                    ]
                }
                ]
        });
        
        me.callParent(arguments);
    },
    
    selectFunction: function (combo, records){
        var me = this,
            record = records[0];
        
        if (record) {
            var value = record.data.Value === '' ?  record.raw : record.data.Value;
            
            combo.setEditable(false);
            
            me.addressDictionary.set(combo.propertyName, value);
            me.fieldFillingAddressDictionary.set(combo.level,  {
               predicate: combo.predicate,
               value: value
            });

            if(combo.dependedCombo) {
                if (Ext.isArray(combo.dependedCombo)) {
                    combo.dependedCombo.forEach(function (obj) {
                        var dependedCombo = me[obj];

                        dependedCombo.setDisabled(false);
                    })
                } else {
                    var dependedCombo = me[combo.dependedCombo];
                    
                    dependedCombo.setDisabled(false);
                }
            }

            me.fillAddressField();
        }
    },

    fillAddressField: function()
    {
        var me = this,
            addressField = me.down('textfield[name=Address]'),
            resultValue = '';
        
        [...me.fieldFillingAddressDictionary].sort()
            .forEach(function(addressObjectMap){
                var value,
                    addressObject = addressObjectMap[1];
                
                if(!addressObject.predicate || addressObject.value.includes(addressObject.predicate)){
                    value = addressObject.value;    
                }
                else{
                    value = addressObject.predicate + ' ' + addressObject.value;
                }
                
                if(resultValue === ''){
                    resultValue += value;
                }
                else{
                    resultValue += ', ' + value;
                }
            });
        
        addressField.setValue(resultValue);
    },
    
    clearComboBox: function(comboBox){
        var me = this;
        
        comboBox.clearValue();
        comboBox.store.removeAll();
        comboBox.setEditable(true);
        me.addressDictionary.delete(comboBox.propertyName);
        me.fieldFillingAddressDictionary.delete(comboBox.level);
    }
});