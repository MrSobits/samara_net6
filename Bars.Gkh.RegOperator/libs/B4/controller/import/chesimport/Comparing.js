Ext.define('B4.controller.import.chesimport.Comparing', {
    extend: 'B4.base.Controller',
    requires: [
    ],

    models: [
        'AddressMatch',
        'import.chesimport.ChesNotMatchAddress',
        'import.chesimport.ChesMatchAccountOwner',
        'import.chesimport.ChesMatchLegalAccountOwner',
        'import.chesimport.ChesMatchIndAccountOwner',
        'import.chesimport.ChesNotMatchAccountOwner',
        'import.chesimport.ChesNotMatchLegalAccountOwner',
        'import.chesimport.ChesNotMatchIndAccountOwner'
    ],
    mixins: { 
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody' 
    },

    mainView: 'import.chesimport.ComparingPanel',
    mainViewSelector: 'chesimportcomparingpanel',

    aspects: [
        
    ],

    refs: [
        {
            ref: 'addressToMatchGrid',
            selector: 'chesimportcomparingpanel chesaddresstomatchgrid'
        },
        {
            ref: 'realityObjectGrid',
            selector: 'chesimportcomparingpanel chesimportrealityobjectgrid'
        },
        {
            ref: 'indOwnerToMatchGrid',
            selector: 'chesimportcomparingpanel chesimportindownertomatchgrid'
        },
        {
            ref: 'indAccountOwnerGrid',
            selector: 'chesimportcomparingpanel chesimportindaccountownergrid'
        },
        {
            ref: 'legalOwnerToMatchGrid',
            selector: 'chesimportcomparingpanel chesimportlegalownertomatchgrid'
        },
        {
            ref: 'legalAccountOwnerGrid',
            selector: 'chesimportcomparingpanel chesimportlegalaccountownergrid'
        }
    ],

    onMatchAddressClick: function(btn) {
        var me = this,
            addressToMatchGrid = me.getAddressToMatchGrid(),
            realityObjectGrid = me.getRealityObjectGrid(),
            selectionExternal = addressToMatchGrid.getSelectionModel().getSelection(),
            selectedInternal = realityObjectGrid.getSelectionModel().getSelection()[0],
            autoComparing = btn.action === 'automatch';

        if (!selectionExternal || !selectionExternal.length) {
            Ext.Msg.alert('Ошибка!', 'Не выбран адрес из файла');
            return;
        }

        if (!autoComparing) {
            if (selectionExternal.length !== 1) {
                Ext.Msg.alert('Ошибка!', 'Необходимо выбрать только один адрес из файла');
                return;
            }

            if (!selectedInternal) {
                Ext.Msg.alert('Ошибка!', 'Не выбран адрес в системе');
                return;
            }
        }

        if (autoComparing) {
            me.matchAddressAuto(selectionExternal);
        } else {
            me.matchAddressManual(selectionExternal[0], selectedInternal);
        }
    },

    matchAddressAuto: function(selectedExternal) {
        var me = this,
            view = me.getMainView(),
            addressToMatchGrid = me.getAddressToMatchGrid();

        me.mask('Сопоставление...', view);
        B4.Ajax.request({
                url: B4.Url.action('MatchAddresses', 'ChesImport'),
                timeout: 1000 * 60 * 5, // 5 минут
                params: {
                    ids: Ext.encode(Ext.Array.map(selectedExternal, function(el) { return el.get('Id') }))
                }
        })
        .next(function(response) {
            var result = Ext.decode(response.responseText);

            me.unmask();
            Ext.Msg.alert('Успех!', result.message || 'Сопоставление произведено успешно');

            addressToMatchGrid.getStore().load();
        }, me)
        .error(function(result) {
            me.unmask();

            addressToMatchGrid.getStore().load();
            Ext.Msg.alert('Ошибка сохранения!', result.message || 'Сопоставление произведено с ошибками');
        },
        me);
    },

    matchAddressManual: function(selectedExternal, selectedInternal) {
        var me = this,
            view = me.getMainView(),
            addressToMatchGrid = me.getAddressToMatchGrid(),
            model = me.getModel('AddressMatch');

        me.mask('Сохранение сопоставлений', view);
        model
            .create({
                ExternalAddress: selectedExternal.get('ExternalAddress'),
                HouseGuid: selectedExternal.get('HouseGuid'),
                RealityObject: selectedInternal.getId()
            })
            .save()
            .next(function(result) {
                me.unmask();
                Ext.Msg.alert('Успех!', 'Сопоставление произведено успешно');
                addressToMatchGrid.getStore().load();
            }, me)
            .error(function (result) {
                me.unmask();

                Ext.Msg.alert('Сопоставление невозможно!', result.message || 'Ошибка во время сопоставления');
            }, me);
    },

    onMatchAccountClick: function(btn) {
        var me = this,
            toMatchGrid = btn.up('grid'),
            internalGrid = toMatchGrid.up('[panelSelector=comparingPanel]').down('[name=ComparingGrid]'),
            selectionExternal = toMatchGrid.getSelectionModel().getSelection(),
            selectedInternal = internalGrid.getSelectionModel().getSelection()[0],
            autoComparing = btn.action === 'automatch';

        if (!selectionExternal || !selectionExternal.length) {
            Ext.Msg.alert('Ошибка!', 'Не выбран абонент из файла');
            return;
        }

        if (!autoComparing) {
            if (selectionExternal.length !== 1) {
                Ext.Msg.alert('Ошибка!', 'Необходимо выбрать только одного абонента из файла');
                return;
            }

            if (!selectedInternal) {
                Ext.Msg.alert('Ошибка!', 'Не выбран абонент в системе');
                return;
            }
        }

        if (autoComparing) {
            me.matcOwnerAuto(selectionExternal, toMatchGrid);
        } else {
            me.matcOwnerManual(selectionExternal[0], selectedInternal, toMatchGrid);
        }
    },

   matcOwnerAuto: function(selectedExternal, gridToReload) {
        var me = this,
            view = me.getMainView();

        me.mask('Сопоставление...', view);
        B4.Ajax.request({
                url: B4.Url.action('MatchOwners', 'ChesImport'),
                timeout: 1000 * 60 * 5, // 5 минут
                params: {
                    ids: Ext.encode(Ext.Array.map(selectedExternal, function(el) { return el.get('Id') }))
                }
            })
            .next(function(response) {
                var result = Ext.decode(response.responseText);

                me.unmask();
                Ext.Msg.alert('Успех!', result.message || 'Сопоставление произведено успешно');

                gridToReload.getStore().load();
            }, me)
            .error(function(result) {
                    me.unmask();

                    gridToReload.getStore().load();
                    Ext.Msg.alert('Ошибка сохранения!', result.message || 'Сопоставление произведено с ошибками');
                },
                me);
    },

   matcOwnerManual: function(selectedExternal, selectedInternal, gridToReload) {
        var me = this,
            view = me.getMainView();

        me.mask('Сохранение сопоставлений', view);
       B4.Ajax.request({
               url: B4.Url.action('MatchOwner', 'ChesNotMatchAccountOwner'),
               timeout: 1000 * 60 * 5, // 5 минут
               params: {
                   notMatchId: selectedExternal.getId(),
                   ownerId: selectedInternal.getId()
               }
           })
           .next(function(response) {
               var result = Ext.decode(response.responseText);

               me.unmask();
               Ext.Msg.alert('Успех!', result.message || 'Сопоставление произведено успешно');

               gridToReload.getStore().load();
           }, me)
           .error(function(result) {
                   me.unmask();

                   gridToReload.getStore().load();
                   Ext.Msg.alert('Ошибка сохранения!', result.message || 'Сопоставление произведено с ошибками');
               },
               me);
    },

    onBeforeStoreLoad: function(store, operation) {
        var me = this,
            view = me.getMainView();

        operation.params.periodId = me.getContextValue(view, 'periodId');
    },

    init: function() {
        var me = this;

        me.control({
            'chesimportcomparingpanel': {
                'afterrender': {
                    fn: function(view) {
                        var matchAddressStore = view.down('chesaddresstomatchgrid').getStore(),
                            matcOwnerStore = view.down('chesimportindownertomatchgrid').getStore(),
                            matchLegalAccountStore = view.down('chesimportlegalownertomatchgrid').getStore();

                        matchAddressStore.on('beforeload', me.onBeforeStoreLoad, me);
                        matcOwnerStore.on('beforeload', me.onBeforeStoreLoad, me);
                        matchLegalAccountStore.on('beforeload', me.onBeforeStoreLoad, me);
                    }
                }
            },
            'chesimportcomparingpanel b4updatebutton': {
                'click': function(btn) {
                    btn.up('grid').getStore().load();
                }
            },
            'chesimportcomparingpanel chesaddresstomatchgrid menuitem': {
                'click': {
                    fn: me.onMatchAddressClick,
                    scope: me
                }
            },
            'chesimportcomparingpanel [name=MatchGrid] menuitem': {
                'click': {
                    fn: me.onMatchAccountClick,
                    scope: me
                }
            },

            'chesimportcomparingpanel grid' : 
            { 
                'viewready':  function(view) {
                    view.getStore().load();
                } 
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view, 'chesPeriodId_Info');
        me.setContextValue(view, 'periodId', id);
    }
});