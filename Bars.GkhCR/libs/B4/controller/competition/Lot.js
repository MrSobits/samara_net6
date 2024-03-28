Ext.define('B4.controller.competition.Lot', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['competition.Lot'],

    stores: [
        'dict.Municipality',
        'competition.Lot',
        'competition.Document',
        'competition.LotTypeWorkByProgrammCr'
    ],

    views: [
        'competition.LotGrid',
        'competition.LotEditWindow',
        'competition.LotTypeWorkGrid',
        'competition.LotBidGrid',
        'competition.LotBidEditWindow',
        'competition.MultiSelectWindowTypeWork'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'competitionlotgrid'
        },
        {
            ref: 'lotTypeWorkGrid',
            selector: 'competitionlottypeworkgrid'
        }
    ],

    mainView: 'competition.LotGrid',
    mainViewSelector: 'competitionlotgrid',

    parentCtrlCls: 'B4.controller.competition.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'competitionlotpermission',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'GkhCr.Competition.Lot.Edit', applyTo: 'b4savebutton', selector: 'competitionloteditwindow' },
                { name: 'GkhCr.Competition.Lot.Delete', applyTo: 'b4deletecolumn', selector: 'competitionlotgrid' },
                { name: 'GkhCr.Competition.Lot.Create', applyTo: 'b4addbutton', selector: 'competitionlotgrid' }
            ]
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'competitionLotGridWindowAspect',
            gridSelector: 'competitionlotgrid',
            editFormSelector: 'competitionloteditwindow',
            modelName: 'competition.Lot',
            editWindowView: 'competition.LotEditWindow',
            onSaveSuccess: function () {
                // перекрыл метод потому что ненужно чтобы форма закрывалась 
            },
            listeners: {
                getdata: function(asp, record) {
                    var me = this;
                    if (!record.getId()) {
                        record.set('Competition', me.controller.getContextValue(me.controller.getMainComponent(), 'competitionId'));
                    }
                },
                aftersetformdata: function(asp, record) {
                    var form = asp.getForm(),
                        mainView = asp.controller.getMainComponent(),
                        gridTw = form.down('competitionlottypeworkgrid'),
                        gridBid = form.down('competitionlotbidgrid'),
                        fldContractNumber = form.down('[name=ContractNumber]'),
                        fldContractDate = form.down('[name=ContractDate]'),
                        fldContractFactPrice = form.down('[name=ContractFactPrice]'),
                        fldContractFile = form.down('[name=ContractFile]'),
                        storeTw, storeBid;

                    if (record.getId()) {
                        asp.controller.setContextValue(mainView, 'lotId', record.getId());

                        gridTw.setDisabled(false);
                        gridBid.setDisabled(false);

                        storeTw = gridTw.getStore();
                        storeTw.clearFilter(true);
                        storeTw.filter('lotId', record.getId());

                        storeBid = gridBid.getStore();
                        storeBid.clearFilter(true);
                        storeBid.filter('lotId', record.getId());

                    } else {
                        gridTw.setDisabled(true);
                        gridBid.setDisabled(true);
                    }

                    if (record.get('WinnerId')) {
                        fldContractNumber.setDisabled(false);
                        fldContractDate.setDisabled(false);
                        fldContractFactPrice.setDisabled(false);
                        fldContractFile.setDisabled(false);
                    } else {
                        fldContractNumber.setDisabled(true);
                        fldContractDate.setDisabled(true);
                        fldContractFactPrice.setDisabled(true);
                        fldContractFile.setDisabled(true);
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'competitionLotBidGridWindowAspect',
            gridSelector: 'competitionlotbidgrid',
            editFormSelector: 'competitionlotbideditwindow',
            modelName: 'competition.LotBid',
            editWindowView: 'competition.LotBidEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    var me = this;
                    if (!record.getId()) {
                        record.set('Lot', me.controller.getContextValue(me.controller.getMainComponent(), 'lotId'));
                    }
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы ДЛ с массовой формой выбора ДЛ
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'lotTypeWorkAspect',
            gridSelector: 'competitionlottypeworkgrid',
            modelName: 'competition.LotTypeWork',
            multiSelectWindow: 'competition.MultiSelectWindowTypeWork',
            multiSelectWindowSelector: '#lotTypeWorkMultiSelectWindow',
            storeSelect: 'competition.LotTypeWorkByProgrammCr',
            storeSelected: 'competition.LotTypeWorkByProgrammCr',
            titleSelectWindow: 'Выбор объектов капитального ремонта',
            titleGridSelect: 'Объекты капитального ремонта',
            titleGridSelected: 'Выбранные объекты КР',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RoMunicipality',
                    width: 160,
                    text: 'Муниципальный район',
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RoAddress', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Вид работы', xtype: 'gridcolumn', dataIndex: 'TypeWork', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RoAddress', flex: 1, sortable: false },
                { header: 'Вид работы', xtype: 'gridcolumn', dataIndex: 'TypeWork', flex: 1, sortable: false }
            ],
            otherActions: function(actions) {
                var me = this;

                actions[me.multiSelectWindowSelector + ' [name=ProgramCr]'] = {
                    'change': { fn: me.onProgramCrChange, scope: me }
                };
            },
            onProgramCrChange: function(fld, newValue) {
                var me = this;

                me.updateSelectGrid();
            },
            onBeforeLoad: function(store, operation) {
                var me = this,
                    form = me.getForm(),
                    programId = form.down('[name=ProgramCr]').getValue();

                operation.params.programId = programId;
            },
            listeners: {
                getdata: function(asp, records) {
                    var me = this,
                        recordIds = [],
                        lotId = me.controller.getContextValue(me.controller.getMainComponent(), 'lotId'),
                        lotTypeWorkGrid = asp.controller.getLotTypeWorkGrid(),
                        form = me.getForm();

                    records.each(function(rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', form);
                        B4.Ajax.request(B4.Url.action('AddWorks', 'CompetitionLotTypeWork', {
                            typeWorkIds: Ext.encode(recordIds),
                            lotId: lotId
                        })).next(function(response) {
                            asp.controller.unmask();
                            lotTypeWorkGrid.getStore().load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать объекты капитального ремонта');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    index: function(id, lotId) {
        var me = this,
            view = me.getMainView() || Ext.widget('competitionlotgrid'),
            competitionModel = me.getModel('Competition'),
            lotModel = me.getModel('competition.Lot'),
            store, record;
            

        me.bindContext(view);
        me.setContextValue(view, 'competitionId', id);
        me.application.deployView(view, 'competition_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('competitionId', id);

        me.getAspect('competitionlotpermission').setPermissionsByRecord(new competitionModel({ Id: id }));

        if (!Ext.isEmpty(lotId)) {
            record = new lotModel({ Id: lotId });
            me.getAspect('competitionLotGridWindowAspect').editRecord(record);
        }
    }
});