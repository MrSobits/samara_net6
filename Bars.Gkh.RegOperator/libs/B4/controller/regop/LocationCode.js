Ext.define('B4.controller.regop.LocationCode', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.view.regop.loccode.Grid',
        'B4.view.regop.loccode.EditWindow',
        'B4.aspects.GridEditCtxWindow',
        'B4.model.regop.LocationCode',
        'B4.store.regop.LocationCode',
        'B4.enums.TypeAccountNumber'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'loccodegrid'
        }
    ],

    models: [
        'regop.LocationCode',
        'regop.Fias'
    ],

    stores: [
        'regop.LocationCode'
    ],

    aspects: [
        {
            xtype: 'grideditctxwindowaspect',
            name: 'loccodegrideditwinaspect',
            gridSelector: 'loccodegrid',
            editFormSelector: 'loccodewindow',
            storeName: 'regop.LocationCode',
            editWindowView: 'regop.loccode.EditWindow',
            modelName: 'regop.LocationCode',
            listeners: {
                beforesave: function (asp, record) {
                    var fias = record.get('FiasLevel3') || false,
                        aoGuid = fias.AOGuid || '',
                        name = fias.FormalName || '';
                    if (fias && (aoGuid.Length !== 0 || name.Length !== 0)) {
                        record.set('FiasLevel3', name);
                        record.set('AOGuid', aoGuid);
                    }

                },
                aftersetformdata: function(asp) {
                    var me = this,
                        win = asp.getForm();

                    var sfFiasLevel3 = Ext.ComponentQuery.query('loccodewindow #sfFiasLevel3')[0];

                    if (sfFiasLevel3) {
                        var store = sfFiasLevel3.getStore();

                        if (store) {
                            store.on('beforeload', me.onBeforeFias3Load, me);
                        }
                    }

                    var sfFiasLevel2 = Ext.ComponentQuery.query('loccodewindow #sfFiasLevel2')[0];

                    if (sfFiasLevel2 && sfFiasLevel2.store) {
                        if (typeof sfFiasLevel2.store == 'string') {
                            sfFiasLevel2.store = Ext.create(sfFiasLevel2.store);
                        }

                        sfFiasLevel2.store.on('beforeload', me.onBeforeFias2Load, me);

                    }

                    // В зависимости от настройки Способ генерации лиц. счетов делаем необязательными поля
                    me.checkRequiredFields(win);
                }
            },
            
            onBeforeFias2Load: function (store, operation) {
                
                var sfFiasLevel1 = Ext.ComponentQuery.query('loccodewindow #sfMunicipality')[0];

                    if (sfFiasLevel1 && sfFiasLevel1.getValue()) {
                        operation.params.parentMoId = sfFiasLevel1.getValue();
                    }
            },


            onBeforeFias3Load: function (store, operation) {
                
                var sfFiasLevel2 = Ext.ComponentQuery.query('loccodewindow #sfFiasLevel2')[0];

                if (sfFiasLevel2 && sfFiasLevel2.getValue()) {
                    operation.params.municipalityId = sfFiasLevel2.getValue();

                }
            },
            

            setRequireFields: function (require, win) {
                for (var i = 1; i < 4; i++) {
                    switch (i) {
                        case 2:
                            win.down('treeselectfield[name="FiasLevel' + i + '"]').allowBlank = !require;
                            if (!require) {
                                win.down('treeselectfield[name="FiasLevel' + i + '"]').clearInvalid();
                            }
                            break;
                        case 3:
                            win.down('b4selectfield[name="FiasLevel' + i + '"]').allowBlank = !require;
                            if (!require) {
                                win.down('b4selectfield[name="FiasLevel' + i + '"]').clearInvalid();
                            }
                            break;
                    }
                        
                    win.down('textfield[name="CodeLevel' + i + '"]').allowBlank = !require;
                    
                    if (!require) {
                        var field = win.down('textfield[name="CodeLevel' + i + '"]');
                        if (field.getValue() == '') {
                            win.down('textfield[name="CodeLevel' + i + '"]').setValue(0);
                        }
                    }
                }
            },
            
            checkRequiredFields: function (win) {
                var me = this;
                
                me.controller.mask();
                B4.Ajax.request({
                    url: B4.Url.action('GetParamByKey', 'RegoperatorParams'),
                    params: {
                        key: 'TypeAccountNumber'
                    }
                }).next(function (response) {
                    var typeAccountNumber = Ext.JSON.decode(response.responseText).data;

                    var fieldRequired = typeAccountNumber == 10;
                    if (win.items.length > 0) {
                        me.setRequireFields(fieldRequired, win);
                    }
                    
                    me.controller.unmask();
                }).error(function (e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка!', e.message);
                });
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('loccodegrid');
        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});