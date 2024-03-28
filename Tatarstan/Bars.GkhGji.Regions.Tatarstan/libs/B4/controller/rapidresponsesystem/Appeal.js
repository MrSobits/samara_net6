Ext.define('B4.controller.rapidresponsesystem.Appeal', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.rapidresponsesystem.AppealPermission',
        'B4.aspects.permission.rapidresponsesystem.AppealRequirement',
        'B4.aspects.StateButton',
        'B4.aspects.GkhEditPanel'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'rapidresponsesystem.AppealDetails',
        'rapidresponsesystem.AppealResponse'
    ],

    stores: [
        'rapidresponsesystem.AppealDetails',
        'contragent.ContragentForSelect',
        'contragent.ContragentForSelected',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],

    views: [
        'rapidresponsesystem.AppealGrid',
        'rapidresponsesystem.AppealEditWindow',
        'rapidresponsesystem.AppealResponsePanel'
    ],

    mainView: 'rapidresponsesystem.AppealGrid',
    mainViewSelector: 'rapidResponseSystemAppealGrid',

    refs: [
        {
            ref: 'RapidResponseSystemAppealFilterPanel',
            selector: '#appealFilterPanel'
        }
    ],

    aspects: [
        {
            xtype: 'statebuttonaspect',
            name: 'appealStateButtonAspect',
            stateButtonSelector: '#soprAppealEditWindow #btnState'
        },
        {
            xtype: 'appealperm'
        },
        {
            xtype: 'appealrequirement'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'appealContragentMultiselectwindowaspect',
            fieldSelector: 'rapidResponseSystemAppealGrid #tfContragent',
            valueProperty: 'Id',
            textProperty: 'Name',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#appealContragentSelectWindow',
            storeSelect: 'contragent.ContragentForSelect',
            storeSelected: 'contragent.ContragentForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false, filter: {xtype: 'textfield'} },
                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, sortable: false, filter: {xtype: 'textfield'} },
            ],
            columnsGridSelected: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор контрагентов',
            titleGridSelect: 'Контрагенты для отбора',
            titleGridSelected: 'Выбранные контрагенты'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'appealRealityObjectMultiselectwindowaspect',
            fieldSelector: 'rapidResponseSystemAppealGrid #tfRealityObject',
            valueProperty: 'Id',
            textProperty: 'Address',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#appealRealityObjectSelectWindow',
            storeSelect: 'realityobj.RealityObjectForSelect',
            storeSelected: 'realityobj.RealityObjectForSelected',
            columnsGridSelect: [
                { 
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    sortable: false ,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    },
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false, filter: {xtype: 'textfield'} },
            ],
            columnsGridSelected: [{ header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'appealGridWindowAspect',
            gridSelector: 'rapidResponseSystemAppealGrid',
            modelName: 'rapidresponsesystem.AppealDetails',
            editWindowView: 'rapidresponsesystem.AppealEditWindow',
            editFormSelector: '#soprAppealEditWindow',
            saveRequestHandler: function() {},
            onAfterSetFormData: function (aspect, rec, form) {},
            onSaveSuccess: function (aspect) {},
            otherActions: function (actions) {
                var me = this;
                
                actions[this.editFormSelector + ' [action=redirectToHouse]'] = 
                    { 
                        'click': 
                        { 
                            fn: function(){
                                var id = me.getForm().getRecord().get('RealityObjectId');
                                
                                if(id){
                                    me.controller.application.redirectTo(Ext.String.format('realityobjectedit/{0}', id));
                                }
                            }, scope: me 
                        } 
                    };

                actions[this.editFormSelector + ' [action=redirectToContragent]'] =
                    { 
                        'click':
                        {
                            fn: function(){
                                var id = me.getForm().getRecord().get('ContragentId');

                                if(id){
                                    me.controller.application.redirectTo(Ext.String.format('contragentedit/{0}', id));
                                }
                            }, scope: me
                        }
                    };

                actions[this.editFormSelector + ' [action=AppealFileDownload]'] =
                    {
                        'click':
                            {
                                fn: function(){
                                    var id = me.getForm().getRecord().get('AppealFileId');

                                    if(id){
                                        window.open(B4.Url.content(Ext.String.format('FileUpload/Download?id={0}', id)));
                                    }
                                }, scope: me
                            }
                    };
                
                actions[this.editFormSelector + ' [action=TakeToWork]'] = 
                    {
                        'click': 
                            {
                                fn: function (){
                                    // Статус "В работе"
                                    me.changeState(2);
                                }, scope: me
                            }
                    };

                actions[this.editFormSelector + ' [action=NotifyGji]'] =
                    {
                        'click':
                            {
                                fn: function (){
                                    // Статус "Обработано"
                                    me.changeState(3);
                                }, scope: me
                            }
                    };
            },
            listeners: {
                beforesetformdata: function(asp, rec, form){
                    var appealResponseAspect = asp.controller.getAspect('appealResponsePanelAspect');

                    asp.controller.setContextValue(form, 'appealDetailsId', rec.getId());
                    appealResponseAspect.setData();
                },
                aftersetformdata: function(asp, rec, form){
                    var takeToWorkButton = form.down('[action=TakeToWork]'),
                        notifyGjiButton = form.down('[action=NotifyGji]'),
                        saveButton = form.down('b4savebutton'),
                        responseFieldSet = form.down('#AppealResponseSet'),
                        state = rec.get('State');
                    
                    if(state){
                        switch(state.Code) {
                            // Новое
                            case '1':
                                takeToWorkButton.show();
                                notifyGjiButton.hide();
                                responseFieldSet.hide();
                                saveButton.hide();
                                
                                break;

                            // В работе
                            case '2':
                                takeToWorkButton.hide();
                                notifyGjiButton.show();
                                responseFieldSet.show();
                                
                                if(saveButton.allowed){
                                    saveButton.show();
                                }
                                
                                break;

                            // Обработано
                            case '3':
                                takeToWorkButton.hide();
                                notifyGjiButton.hide();
                                responseFieldSet.hide();
                                responseFieldSet.show();
                                saveButton.hide();
                                
                                break;

                            // Не обработано
                            case '4':
                                takeToWorkButton.hide();
                                notifyGjiButton.hide();
                                responseFieldSet.hide();
                                saveButton.hide();
                                
                                break;
                        }

                        // Если скрыты оба элемента группы кнопок - скрываем саму группу
                        notifyGjiButton.up('buttongroup').setVisible((notifyGjiButton.isVisible() || saveButton.isVisible() || takeToWorkButton.isVisible()));
                        
                        this.controller.getAspect('appealStateButtonAspect').setStateData(rec.getId(), state);
                    }
                }
            },
            changeState: function (stateCode){
                var me = this,
                    form = me.getForm(),
                    entityId = form.getRecord().get('Id'),
                    model = me.getModel();

                B4.Ajax.request({
                    method: 'GET',
                    url: B4.Url.action('ChangeState', "RapidResponseSystemAppeal", {
                        stateCode: stateCode,
                        entityId: entityId
                    })
                }).next(function(response) {
                    var result = Ext.decode(response.responseText);
                    if (result.success){
                        me.editRecord(new model({ Id: entityId }))
                        me.controller.updateGrid();
                    }
                }).error(function(result) {
                    Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) 
                        ? result.responseData 
                        : result.responseData.message);
                });
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'rapidResponseSystemAppealButtonExportAspect',
            gridSelector: 'rapidResponseSystemAppealGrid',
            buttonSelector: 'rapidResponseSystemAppealGrid #btnExport',
            controllerName: 'RapidResponseSystemAppeal',
            actionName: 'Export'
        },
        {
            xtype: 'gkheditpanel',
            name: 'appealResponsePanelAspect',
            editPanelSelector: '#appealResponsePanel',
            modelName: 'rapidresponsesystem.AppealResponse',
            otherActions: function (actions) {
                var me = this;
                
                actions['#soprAppealEditWindow b4savebutton'] = { 'click': { fn: me.saveRequestHandler, scope: me } };
                actions[this.editPanelSelector + ' b4filefield'] = { 'fileclear': { fn: me.onClearFileTrigger, scope: this } };
            },
            setData: function() {
                var me = this,
                    panel = me.getPanel(),
                    model;

                panel.setDisabled(true);
                model = me.getModel();
                
                model.load(0, {
                    params: {
                        appealDetailsId: me.controller.getContextValue(me.getPanel(), 'appealDetailsId')
                    },
                    success: function(rec) {
                        if(rec){
                            me.setPanelData(rec);
                        } else {
                            me.setPanelData(new model({ Id: 0 }))
                        }
                    },
                    scope: me
                });
            },
            listeners: {
                beforesave: function(asp, rec){
                    if(rec.getId() === 0 && !rec.get('RapidResponseSystemAppeal')){
                        rec.set('RapidResponseSystemAppealDetails', {Id: asp.controller.getContextValue(asp.getPanel(), 'appealDetailsId')});
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('rapidresponsesystem.AppealDetails').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (params) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);
        me.updateGrid();

        if (params && params.id > 0) {
            var aspect = me.getAspect('appealGridWindowAspect'),
                model = aspect.getModel();

            aspect.editRecord(new model({ Id: params.id }));
        }
    },

    updateGrid: function () {
        this.getMainView().getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            filterPanel = me.getRapidResponseSystemAppealFilterPanel();
        
        if(filterPanel)
        {
            operation.params.appealDateFrom = filterPanel.down('[name=AppealDateFrom]').getValue();
            operation.params.appealDateTo = filterPanel.down('[name=AppealDateTo]').getValue();
            operation.params.controlPeriodFrom = filterPanel.down('[name=ControlPeriodFrom]').getValue();
            operation.params.controlPeriodTo = filterPanel.down('[name=ControlPeriodTo]').getValue();
            operation.params.roIds = Ext.encode(filterPanel.down('[name=RealityObjects]').getValue());
            operation.params.contragentIds = Ext.encode(filterPanel.down('[name=Contragents]').getValue());
        }
    }
});