Ext.define('B4.controller.JurInstitution', {
    /*
    * Контроллер раздела программы КР
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.JurInstitutionType',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'dict.JurInstitution',
        'dict.JurInstitutionRealObj'
    ],
    stores: [
        'dict.JurInstitution',
        'dict.JurInstitutionRealObj',
        'dict.RealObjForJurInst',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],
    views: [
        'jurinstitution.EditWindow',
        'jurinstitution.Grid',
        'SelectWindow.MultiSelectWindow'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'jurinstitution.Grid',
    mainViewSelector: 'jurinstitutiongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'jurinstitutiongrid'
        }
    ],

    aspects: [{
        xtype: 'grideditctxwindowaspect',
        name: 'jurInstitutionGridWindowAspect',
        gridSelector: 'jurinstitutiongrid',
        editFormSelector: 'jurinstitutioneditwin',
        modelName: 'dict.JurInstitution',
        editWindowView: 'jurinstitution.EditWindow',
        otherActions: function (actions) {
            var me = this;
            actions['jurinstitutioneditwin [name=JurInstitutionType]'] = { 'change': { fn: me.onChangeJurInst, scope: me } };
        },
        onSaveSuccess: function (asp, record) {
            var mainView = asp.controller.getMainView(),
                    view = asp.getForm(),
                    roGrid = view.down('jurinstitutionrealobjgrid'),
                    roStore = roGrid.getStore();

            if (!record.phantom
                && record.get('JurInstitutionType') == B4.enums.JurInstitutionType.Court) {
                roGrid.enable();
            } else {
                roGrid.disable();
            }

            asp.controller.setContextValue(mainView, 'jurInstId', record.getId());

            roStore.clearFilter(true);
            roStore.filter('jurInstId', record.getId());
        },
        listeners: {
            aftersetformdata: function (asp, record) {
                var mainView = asp.controller.getMainView(),
                    view = asp.getForm(),
                    showStlClaimWork,
                    judgeFieldSet = view.down('fieldset[type=Judge]'),
                    jurInstField = view.down('[name=JurInstitutionType]'),
                    roGrid = view.down('jurinstitutionrealobjgrid'),
                    roStore = roGrid.getStore(),
                    settlColumn = roGrid.down('[dataIndex=Settlement]'),
                    courtTypeField = view.down('[name=CourtType]');

                if (record.get('JurInstitutionType') == B4.enums.JurInstitutionType.Court) {
                    judgeFieldSet.show();
                    courtTypeField.show();
                } else {
                    judgeFieldSet.hide();
                    courtTypeField.hide();
                }

                jurInstField.setReadOnly(!record.phantom);

                asp.controller.setContextValue(mainView, 'jurInstId', record.getId());
                showStlClaimWork = asp.controller.getContextValue(mainView, 'showStlClaimWork');

                if (showStlClaimWork) {
                    settlColumn.show();
                } else {
                    settlColumn.hide();
                }

                if (!record.phantom
                    && record.get('JurInstitutionType') == B4.enums.JurInstitutionType.Court) {
                    roGrid.enable();
                } else {
                    roGrid.disable();
                }

                roStore.clearFilter(true);
                roStore.filter('jurInstId', record.getId());
            }
        },
        onChangeJurInst: function (field, newValue) {
            var view = this.getForm(),
                judgeFieldSet = view.down('fieldset[type=Judge]'),
                courtTypeField = view.down('[name=CourtType]');

            if (newValue == 10) {
                judgeFieldSet.show();
                courtTypeField.show();
            } else {
                judgeFieldSet.hide();
                courtTypeField.hide();
            }
        }
    },
    {
        xtype: 'gkhgridmultiselectwindowaspect',
        name: 'jurInstRealObjGridAspect',
        gridSelector: 'jurinstitutionrealobjgrid',
        storeName: 'dict.JurInstitutionRealObj',
        modelName: 'dict.JurInstitutionRealObj',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#jurInstRealObjGridMultiSelectWindow',
        storeSelect: 'dict.RealObjForJurInst',
        storeSelected: 'realityobj.RealityObjectForSelected',
        titleSelectWindow: 'Выбор жилых домов',
        titleGridSelect: 'Дома',
        titleGridSelected: 'Выбранные дома',
        otherActions: function (actions) {
            var me = this;
            actions['#jurInstRealObjGridMultiSelectWindow [name=Even]'] = { 'change': { fn: me.updateSelectGrid, scope: me } };
            actions['#jurInstRealObjGridMultiSelectWindow [name=Odd]'] = { 'change': { fn: me.updateSelectGrid, scope: me } };
        },
        onBeforeLoad: function (store, operation) {
            var me = this,
                form = me.getForm(),
                selectGrid = me.getSelectGrid(),
                from = selectGrid.down('[name=From]').getValue(),
                to = selectGrid.down('[name=To]').getValue(),
                isEven = form.down('[name=Even]').getValue(),
                isOdd = form.down('[name=Odd]').getValue();

            operation.params.from = from;
            operation.params.to = to;
            operation.params.isEven = isEven;
            operation.params.isOdd = isOdd;
        },
        formAfterrender: function (panel) {
            var me = this,
                mainView = me.controller.getMainView(),
                showStlClaimWork = me.controller.getContextValue(mainView, 'showStlClaimWork'),
                grid = panel.down('#multiSelectGrid'),
                settlColumn = grid.down('[dataIndex=Settlement]');

            if (showStlClaimWork) {
                settlColumn.show();
            } else {
                settlColumn.hide();
            }
        },
        columnsGridSelect: [
            {
                xtype: 'gridcolumn',
                dataIndex: 'Municipality',
                flex: 1,
                text: 'Муниципальный район',
                filter: {
                    xtype: 'b4combobox',
                    operand: CondExpr.operands.eq,
                    storeAutoLoad: false,
                    hideLabel: true,
                    editable: false,
                    valueField: 'Name',
                    emptyItem: { Name: '-' },
                    url: '/Municipality/ListMoAreaWithoutPaging'
                }
            },
            {
                xtype: 'gridcolumn',
                dataIndex: 'Settlement',
                flex: 1,
                text: 'Муниципальное образование',
                filter: {
                    xtype: 'b4combobox',
                    operand: CondExpr.operands.eq,
                    storeAutoLoad: false,
                    hideLabel: true,
                    editable: false,
                    valueField: 'Name',
                    emptyItem: { Name: '-' },
                    url: '/Municipality/ListWithoutPaging'
                }
            },
            {
                xtype: 'gridcolumn',
                dataIndex: 'PlaceName',
                flex: 1,
                text: 'Населенный пункт',
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'gridcolumn',
                dataIndex: 'StreetName',
                flex: 1,
                text: 'Улица',
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'gridcolumn',
                dataIndex: 'House',
                width: 40,
                text: 'Номер дома',
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'gridcolumn',
                dataIndex: 'Letter',
                width: 40,
                text: 'Литер',
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'gridcolumn',
                dataIndex: 'Housing',
                width: 45,
                text: 'Корпус',
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'gridcolumn',
                dataIndex: 'Building',
                width: 45,
                text: 'Секция',
                filter: { xtype: 'textfield' }
            }
        ],
        columnsGridSelected: [
            { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
        ],
        leftTopToolbarConfig: {
            xtype: 'toolbar',
            dock: 'top',
            items: [
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    allowDecimals: false,
                    name: 'From',
                    labelAlign: 'right',
                    fieldLabel: 'Номера дома     c',
                    width: 150
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    allowDecimals: false,
                    name: 'To',
                    labelAlign: 'right',
                    labelWidth: 40,
                    fieldLabel: 'по',
                    width: 100
                },
                {
                    xtype: 'component',
                    width: 25
                },
                { xtype: 'b4updatebutton' },
                {
                    xtype: 'tbfill'
                }
            ]
        },
        leftGridConfig: {
            cls: 'x-large-head',
            flex: 2
        },
        toolbarItems: [
            {
                xtype: 'checkbox',
                name: 'Even',
                fieldLabel: 'Четные номера',
                labelAlign: 'right',
                checked: true
            },
            {
                xtype: 'checkbox',
                name: 'Odd',
                fieldLabel: 'Нечетные номера',
                labelAlign: 'right',
                checked: true
            }
        ],
        listeners: {
            getdata: function (asp, records) {
                var mainView = asp.controller.getMainView(),
                    recordIds = [];

                records.each(function (rec) {
                    recordIds.push(rec.get('Id'));
                });

                if (recordIds[0] > 0) {
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddRealObjs', 'JurInstitution', {
                        realObjIds: Ext.encode(recordIds),
                        jurInstId: asp.controller.getContextValue(mainView, 'jurInstId')
                    })).next(function () {
                        asp.getGrid().getStore().load();
                        asp.controller.unmask();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                }
                else {
                    Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                    return false;
                }
                return true;
            }
        }
    }
    ],

    index: function () {
        var me = this,
            json,
            view = me.getMainView() || Ext.widget('jurinstitutiongrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();

        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GkhParams')
        }).next(function (response) {
            json = Ext.JSON.decode(response.responseText);

            me.setContextValue(view, 'showStlClaimWork', json.ShowStlClaimWork);
        }).error(function () {
            Ext.Msg.alert('Ошибка!', 'Ошибка получения параметров приложения');
        });
    }
});