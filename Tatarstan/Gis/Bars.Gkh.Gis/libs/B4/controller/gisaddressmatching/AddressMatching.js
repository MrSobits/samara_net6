Ext.define('B4.controller.gisaddressMatching.AddressMatching', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.aspects.GridEditWindow',
        'B4.enums.TypeAddressMatched',
        'B4.view.gisaddressmatching.FiasEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: ['gisaddressmatching.GisAddress', 'gisaddressmatching.FiasAddress'],
    views: [
        'gisaddressmatching.GisGrid',
        'gisaddressmatching.FiasGrid',
        'gisaddressmatching.HouseType',
        'gisaddressmatching.HouseMunicipality'
    ],

    mainView: 'gisaddressmatching.AddressMatchingPanel',
    mainViewSelector: 'gisgrid',

    refs: [
        {
            ref: 'mainPanel',
            selector: 'addressmatchingpnl'
        },
        {
            ref: 'addTypeWin',
            selector: 'gisaddressmatchinghouseType'
        },
        {
            ref: 'addMoWin',
            selector: 'gisaddressmatchinghousemunicipality'
        }
    ],

    aspects: [
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'addressMatchGridWindowAspect',
            gridSelector: 'gisgrid',
            storeName: 'gisaddressmatching.GisAddress',
            modelName: 'gisaddressmatching.GisAddress',
            rowAction: function(grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                    case 'match':
                        this.controller.matchAddress(record);
                        break;
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'fiasAddressAddAspect',
            gridSelector: 'fiasgrid',
            editFormSelector: 'gisaddressmatchingfiaseditwindow',
            modelName: 'gisaddressmatching.FiasAddress',
            editWindowView: 'gisaddressmatching.FiasEditWindow',
            otherActions: function(actions) {
                actions[this.editFormSelector + ' b4savebutton'] = { 'click': { fn: this.onSaveBtnClick, scope: this } };
            },
            //сохранение нового адреса ФИАС
            onSaveBtnClick: function(btn) {
                var asp = this,
                    gridStore = Ext.ComponentQuery.query(asp.gridSelector)[0].getStore(),
                    form = btn.up(asp.editFormSelector).getForm();

                if (!form.isValid()) {
                    B4.QuickMsg.msg('Внимание', 'Имеются ошибки заполнения формы', 'error');
                    return;
                }

                asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                B4.Ajax.request(
                    {
                        method: 'POST',
                        url: B4.Url.action('CreateFiasAddress', 'GisAddress'),
                        params: {
                            areaCode: form.getValues().Area,
                            placeCode: form.getValues().Place,
                            streetName: btn.up(asp.editFormSelector).down('b4selectfield[name=Street]').getRawValue(),
                            streetShortName: form.getValues().StreetShortName,
                            house: form.getValues().House,
                            building: form.getValues().Building
                        },
                        timeout: 999999
                    }).next(function() {
                        asp.controller.unmask();
                        B4.QuickMsg.msg('Успешно', 'Адрес успешно сохранен', 'success');
                        gridStore.load();
                        asp.closeWindowHandler();

                    }).error(function(response) {
                        asp.controller.unmask();                        
                        B4.QuickMsg.msg('Ошибка!', response.message, 'error');
                    });
            }
        }
    ],

    index: function() {
        var view = this.getMainPanel()
            || Ext.widget('addressmatchingpnl'),
            billingInfoPanel = view.down('breadcrumbs[name = "billingAddrInfo"]'),
            fiasInfoPanel = view.down('breadcrumbs[name = "fiasAddrInfo"]');

        this.bindContext(view);
        this.application.deployView(view);

        if (billingInfoPanel) {
            billingInfoPanel.update({ text: 'Адрес домов ГИС' });
        }
        if (fiasInfoPanel) {
            fiasInfoPanel.update({ text: 'Адрес домов ФИАС' });
        }
    },

    init: function() {
        var me = this,
            actions = {
                'gisgrid, fiasgrid': {
                    render: { fn: me.onRenderGrid, scope: me }
                },
                'gisgrid combobox[name="cmbTypeAddressMatched"]': {
                    change: { fn: me.onChangeTypeAddressMatched, scope: me }
                },
                'gisgrid button[name=addTypeButton]': {
                    click: me.onAddTypeButtonClick
                },
                'gisgrid button[name=addMoButton]': {
                    click: me.onAddMoButtonClick
                },
                'gisaddressmatchinghouseType': {
                    render: me.onHouseTypeWinRender
                },
                'gisaddressmatchinghousemunicipality': {
                    render: me.onHouseMunicipalityWinRender
                },
                'gisaddressmatchinghouseType b4closebutton': {
                    click: function(button) {
                        button.up('window').close();
                    }
                },
                'gisaddressmatchinghousemunicipality b4closebutton': {
                    click: function(button) {
                        button.up('window').close();
                    }
                },
                'gisaddressmatchinghouseType b4savebutton': {
                    click: me.onHouseTypesSave
                },
                'gisaddressmatchinghousemunicipality b4savebutton': {
                    click: me.onHouseMoSave
                },
                'gisaddressmatchingfiaseditwindow': {
                    render: { fn: me.onRenderEditWindow, scope: me }
                },
                'gisaddressmatchingfiaseditwindow b4selectfield[name="Area"]': {
                    change: { fn: me.onSelectFieldAreaChanged, scope: me },
                    beforewindowcreated: function (asp) {
                        asp.getStore().currentPage = 1;
                    }
                },
                'gisaddressmatchingfiaseditwindow b4selectfield[name="Place"]': {
                    change: { fn: me.onSelectFieldPlaceChanged, scope: me },
                    beforewindowcreated: function (asp) {
                        //костыль.при открытии грида - нужно скинуть на 1 страницу.
                        //потом придумать что то получше
                        asp.getStore().loadPage(1);
                    }
                },
                'gisaddressmatchingfiaseditwindow b4selectfield[name="Street"]': {
                    change: { fn: me.onSelectFieldStreetChanged, scope: me },
                    beforewindowcreated: function (asp) {
                        //костыль.при открытии грида - нужно скинуть на 1 страницу.
                        //потом придумать что то получше
                        asp.getStore().loadPage(1);
                    }
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    onRenderGrid: function(grid) {
        var store = grid.getStore();

        store.on('beforeload', this.onBeforeLoadAddresses, this);
        store.load();
    },

    onBeforeLoadAddresses: function(store, operation) {
        operation.params.typeAddressMatched = this.getMainView().down('combobox[name="cmbTypeAddressMatched"]').value;
    },

    onChangeTypeAddressMatched: function(cmb) {
        var grid = cmb.up('addressmatchingpnl').down('gisgrid'),
            gridStore = grid.getStore();

        if (gridStore) {
            gridStore.load();
        }
    },

    //вызов функции сопоставления адресов
    matchAddress: function(record) {
        var me = this,
            fiasGrid = Ext.ComponentQuery.query('fiasgrid')[0],
            fiasSelectRecord = fiasGrid.getSelectionModel().getSelection()[0],
            id = record ? record.getId() : null;

        if (!fiasSelectRecord) {
            B4.QuickMsg.msg('Внимание', 'Выберите ФИАС запись дома', 'warning');
            return;
        }
        Ext.MessageBox.show({
            title: 'Сопоставление адреса',
            modal: true,
            msg: 'Вы уверены что хотите сопоставить выбранные адреса?',
            buttonText: { yes: "Да", no: "Нет" },
            fn: function(btn) {
                switch (btn) {
                case 'yes':
                    me.DetectSimilarAddresses(id, fiasSelectRecord.getId());
                    break;
                }
            }
        });
    },
    
    //определить возможность массового сопоставления
    DetectSimilarAddresses: function(houseId, fiasAddressId) {
        var me = this;

        me.mask();

        B4.Ajax.request(
            {
                method: 'GET',
                url: B4.Url.action('SimilarAddressesDetected', 'GisAddressMatching'),
                params: {
                    addrMatchId: houseId,
                    fiasId: fiasAddressId
                },
                timeout: 999999
            }).next(function(response) {
                me.unmask();
                if (response.responseText == 'true') {
                    Ext.MessageBox.show({
                        title: 'Сопоставление адреса',
                        modal: true,
                        msg: 'Обнаружены похожие адреса. Выполнить массовое сопоставление?',
                        buttonText: { yes: "Да", no: "Нет", cancel: 'Отмена' },
                        fn: function(btn) {

                            switch (btn) {
                            case 'yes':
                                //массовое сопоставление
                                me.matchSelectedAddresses(houseId, fiasAddressId, true);
                                break;
                            case 'no':
                                //разовое сопоставление
                                me.matchSelectedAddresses(houseId, fiasAddressId, false);
                                break;
                            }
                        }
                    });
                } else {
                    //разовое сопоставление
                    me.matchSelectedAddresses(houseId, fiasAddressId, false);
                }
            }).error(function() {
                me.unmask();
                B4.QuickMsg.msg('Ошибка!', 'Не удалось определить похожие адреса', 'error');
            });
    },
    
    //сопоставить выбранные адреса
    matchSelectedAddresses: function(billingId, fiasAddressId, isMassMatch) {
        var me = this,
            gisGridStore = Ext.ComponentQuery.query('gisgrid')[0].getStore(),
            action = isMassMatch ? 'MassManualMathAddress' : 'ManualMathAddress';

        me.mask('Сохранение', me.getMainComponent());

        B4.Ajax.request(
            {
                method: 'POST',
                url: B4.Url.action(action, 'GisAddressMatching'),
                params: {
                    addrMatchId: billingId,
                    fiasId: fiasAddressId
                },
                timeout: 999999
            }).next(function (response) {
                var responseObj = Ext.decode(response.responseText);
                me.unmask();
                B4.QuickMsg.msg(
                    responseObj.success ? 'Успешно' : 'Внимание!', //'Ошибка!',
                    //responseObj.success ? 'Адрес успешно сопоставлен' : responseObj.message,
                    responseObj.success ? responseObj.message : 'Не удалось сопоставить выбранные адреса',
                    //responseObj.success ? 'success' : 'error');
                    responseObj.success ? 'success' : 'warning');
                gisGridStore.load();
            }).error(function (response) {
                me.unmask();
                B4.QuickMsg.msg('Ошибка!', 'Не удалось сохранить новый адрес', 'error');
            });
    },
    
    //добавить тип домам - открыть окно
    onAddTypeButtonClick: function(button) {
        Ext.widget('gisaddressmatchinghouseType').show();
    },
    
    //отрисовка окна
    onHouseTypeWinRender: function(win) {
        var me = this,
            store = win.down('b4selectfield[name=MatchedHouses]').store;

        store.on('beforeload', function(storeParam, operation) {
            operation.params.typeAddressMatched = B4.enums.TypeAddressMatched.getStore().findRecord('Name', 'MatchedFound').get('Value');
            //сохраняем фильтр 
            me.setContextValue(me.getAddTypeWin(), 'complexFilter', operation.params.complexFilter);
        });

        //костыль - почему то не срабатыват allowBlank при первой загрузки
        win.getForm().isValid();
    },
    
    //присвоить выбранным домам тип
    onHouseTypesSave: function(button) {

        if (!button.up('window').getForm().isValid()) {
            B4.QuickMsg.msg('Предупреждение!', 'Не заполнены обязательные поля', 'warning');
            return;
        }

        var me = this,
            win = button.up('window'),
            houseMatched = win.down('b4selectfield[name=MatchedHouses]'),
            houseIdList = houseMatched.getValue(),
            houseType = win.down('b4combobox[name=houseTypes]').getValue();
        me.mask('Сохранение...');
        B4.Ajax.request(
            {
                method: 'POST',
                url: B4.Url.action("SaveHouseType", 'GisAddressMatching'),
                params: {
                    houseIdList: Ext.encode(houseIdList),
                    houseType: houseType,
                    all: houseIdList == 'All',
                    complexFilter: me.getContextValue(me.getAddTypeWin(), 'complexFilter')
                }
            }).next(function() {
                me.unmask();
                B4.QuickMsg.msg('Успешно', 'Тип успешно присвоен', 'success');
            }).error(function() {
                me.unmask();
                B4.QuickMsg.msg('Ошибка!', 'Не удалось присвоить выбранный тип', 'error');
            });
    },
    
    //открыть окно добавления МО
    onAddMoButtonClick: function(button) {
        Ext.widget('gisaddressmatchinghousemunicipality').show();
    },
    
    //отрисовка окна выбора МО для домов
    onHouseMunicipalityWinRender: function(win) {
        var me = this,
            store = win.down('b4selectfield[name=MatchedHouses]').store;

        store.on('beforeload', function(storeParam, operation) {
            operation.params.typeAddressMatched = B4.enums.TypeAddressMatched.getStore().findRecord('Name', 'MatchedFound').get('Value');
            //сохраняем фильтр 
            me.setContextValue(me.getAddMoWin(), 'complexFilter', operation.params.complexFilter);
        });

        //костыль - почему то не срабатыват allowBlank при первой загрузки
        win.getForm().isValid();
    },
    
    //присвоить выбранным домам МО
    onHouseMoSave: function(button) {

        if (!button.up('window').getForm().isValid()) {
            B4.QuickMsg.msg('Предупреждение!', 'Не заполнены обязательные поля', 'warning');
            return;
        }

        var me = this,
            win = button.up('window'),
            houseMatched = win.down('b4selectfield[name=MatchedHouses]'),
            houseIdList = houseMatched.getValue(),
            houseMunicipality = win.down('b4combobox[name=cmbMunicipality]').getValue();
        me.mask('Сохранение...');
        B4.Ajax.request(
            {
                method: 'POST',
                url: B4.Url.action("SaveHouseMunicipality", 'GisAddressMatching'),
                params: {
                    houseIdList: Ext.encode(houseIdList),
                    municipality: houseMunicipality,
                    all: houseIdList == 'All',
                    complexFilter: me.getContextValue(me.getAddTypeWin(), 'complexFilter')
                }
            }).next(function() {
                me.unmask();
                B4.QuickMsg.msg('Успешно', 'Муниципальное образование успешно присвоено', 'success');
            }).error(function() {
                me.unmask();
                B4.QuickMsg.msg('Ошибка!', 'Не удалось присвоить выбранное муниципальное образование', 'error');
            });
    },
    
    //загрузка окна добавления фиас адреса
    onRenderEditWindow: function (win) {
        var me = this,
            storePlace = win.down('b4selectfield[name="Place"]').getStore(),
            streetStore = win.down('b4selectfield[name="Street"]').getStore();

        storePlace.on('beforeload', me.onPlaceStoreBeforeLoad, me);
        streetStore.on('beforeload', function (store, operation) {
            operation.params.placeIdList = Ext.encode(me.getContextValue(me.getMainView(), 'placeIdList'));
        });

    },

    onPlaceStoreBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.municipalIdList = me.getContextValue(me.getMainView(), 'municipalIdList');
    },

    //выбор района (форма добавления адреса ФИАС)
    onSelectFieldAreaChanged: function (field, data, oldField) {
        var me = this;
        me.setContextValue(me.getMainView(), 'municipalIdList', [data ? data.Id : undefined]);
        me.setContextValue(me.getMainView(), 'placeIdList', [data ? data.Id : undefined]);

        field.up('window').down('b4selectfield[name="Place"]').setValue(undefined);                
    },
    
    //выбор нас. пункта (форма добавления адреса ФИАС)
    onSelectFieldPlaceChanged: function (field, data, oldField) {
        var me = this,
            areas = field.up('window').down('b4selectfield[name="Area"]').getValue();
        me.setContextValue(me.getMainView(), 'placeIdList', [data ? data.Id : (areas ? areas : undefined)]);
    },
    
    onSelectFieldStreetChanged: function(field, data, oldField) {
        var me = this;
        if (data) {
            field.up('window').down('b4combobox[name=StreetShortName]').setValue(data.ShortName);
        }
    }
});