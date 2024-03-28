Ext.define('B4.controller.ShortProgram', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateButton',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'version.ProgramVersion',
        'shortprogram.RealityObject',
        'shortprogram.Record',
        'shortprogram.DefectList',
        'shortprogram.Protocol'
    ],
    
    stores: [
        'shortprogram.RealityObject',
        'shortprogram.Record',
        'shortprogram.DefectList',
        'shortprogram.Protocol',
        'shortprogram.RealObjSelect',
        'shortprogram.RealObjSelected'
    ],
    
    views: [
        'shortprogram.Panel',
        'shortprogram.RealityObjectGrid',
        'shortprogram.RecordGrid',
        'shortprogram.RecordEditWindow',
        'shortprogram.DefectListGrid',
        'shortprogram.DefectListEditWindow',
        'shortprogram.ProtocolGrid',
        'shortprogram.ProtocolEditWindow',
        'SelectWindow.MultiSelectWindow',
        'shortprogram.MassStateChangeWindow'
    ],

    mainView: 'shortprogram.Panel',
    mainViewSelector: 'shortprogrampanel',
    massStateChangeSelector: 'shortprogramstatechangewin',
    massStateChangeView: 'shortprogram.MassStateChangeWindow',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.ShortProgram.MassStateChange', applyTo: 'button[action="MassStateChange"]', selector: 'shortprogrampanel' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'shortprogramstatepermissionaspect',
            permissions: [
                { name: 'Ovrhl.ShortProgram.ActualizeVersion', applyTo: 'button[action="ActualizeVersion"]', selector: 'shortprogrampanel' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'shortprogramrelobjstateperm',
            permissions: [
                { name: 'Ovrhl.ShortProgram.RealityObject.Create', applyTo: 'b4addbutton', selector: 'shortprogramrecordgrid' },
                { name: 'Ovrhl.ShortProgram.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: 'shortprogramrecordgrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                { name: 'Ovrhl.ShortProgram.RealityObject.Protocol.Create', applyTo: 'b4addbutton', selector: 'shortprogramprotocolgrid' },
                { name: 'Ovrhl.ShortProgram.RealityObject.Protocol.Update', applyTo: 'b4savebutton', selector: 'shortprogramprotocolwindow' },
                { name: 'Ovrhl.ShortProgram.RealityObject.Protocol.Delete', applyTo: 'b4deletecolumn', selector: 'shortprogramprotocolgrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                { name: 'Ovrhl.ShortProgram.RealityObject.DefectList.Create', applyTo: 'b4addbutton', selector: 'shortprogramdefectlistgrid' },
                { name: 'Ovrhl.ShortProgram.RealityObject.DefectList.Update', applyTo: 'b4savebutton', selector: 'shortprogramdefectlistwindow' },
                { name: 'Ovrhl.ShortProgram.RealityObject.DefectList.Delete', applyTo: 'b4deletecolumn', selector: 'shortprogramdefectlistgrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                }

            ]
        },
        {   /* Статус для грида домов краткосрочки*/
            xtype: 'b4_state_contextmenu',
            name: 'stateTransferAspect',
            gridSelector: 'shortprogramrogrid',
            menuSelector: 'shortprogramrostatemenu',
            stateType: 'ovrhl_short_prog_object'
        },
        {   /* Статус для грида дефектных ведомостей */
            xtype: 'b4_state_contextmenu',
            name: 'stateTransferDefectListAspect',
            gridSelector: 'shortprogramdefectlistgrid',
            menuSelector: 'shortprogramdefectliststatemenu',
            stateType: 'ovrhl_short_prog_defect_list'
        },
        {
            /*Аспект взаимодействия грида Работ и формы редактирования*/
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'multiselectaspect',
            gridSelector: 'shortprogramrecordgrid',
            storeName: 'shortprogram.Record',
            modelName: 'shortprogram.Record',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#shortprogramworkmultiselectwindow',
            storeSelect: 'shortprogram.WorkSelect',
            storeSelected: 'shortprogram.WorkSelected',
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Работы для отбора',
            titleGridSelected: 'Выбранные работы',
            editFormSelector: '#shortprogramrecordwindow',
            editWindowView: 'shortprogram.RecordEditWindow',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
            ],
            setFormData: function (rec) {
                var form = this.getEditForm();
                form.setTitle(rec.get('WorkName'));

                if (this.fireEvent('beforesetformdata', this, rec, form) !== false) {
                    form.loadRecord(rec);
                }

                this.fireEvent('aftersetformdata', this, rec, form);
            },
            updateGrid: function () {
                this.controller.getMainPanel().down('shortprogramrecordgrid').getStore().load();
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        view = asp.controller.getMainPanel();

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainPanel());
                        B4.Ajax.request({
                            url: B4.Url.action('AddWorks', 'ShortProgramRecord'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                roId: view.params.objectId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            B4.QuickMsg.msg('Успешно', 'Работы успешно добавлены', 'success');

                            me.updateGrid();

                            return true;
                        }).error(function (e) {
                            asp.controller.unmask();
                            B4.QuickMsg.msg('Ошибка', e.message ? e.message : 'Во время сохранения работ произошла ошибка', 'error');
                        });
                    } else {
                        B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать работы', 'error');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования дефектных ведомостей
            */
            xtype: 'grideditwindowaspect',
            name: 'shortprogDefectListGridWindowAspect',
            gridSelector: 'shortprogramdefectlistgrid',
            editFormSelector: 'shortprogramdefectlistwindow',
            modelName: 'shortprogram.DefectList',
            editWindowView: 'shortprogram.DefectListEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var view = asp.controller.getMainPanel();
                    if (view.params && !record.data.Id) {
                        record.data.ShortObject = view.params.objectId;
                    }
                },
                aftersetformdata: function (asp, record) {
                    // для статусов
                    //this.controller.getAspect('defectListStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                }
            }
        },
        {
            /**
            * Аспект взаимодействия таблицы и формы редактирования протоколов
            */
            xtype: 'grideditwindowaspect',
            name: 'shortprogProtocolGridWindowAspect',
            gridSelector: 'shortprogramprotocolgrid',
            editFormSelector: 'shortprogramprotocolwindow',
            modelName: 'shortprogram.Protocol',
            editWindowView: 'shortprogram.ProtocolEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var view = asp.controller.getMainPanel();
                    if (view.params && !record.data.Id) {
                        record.data.ShortObject = view.params.objectId;
                    }
                }
            }
        },
        {
            /*Аспект взаимодействия массовой формы смены статуса*/
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'shortprogramromultiselectwindowasp',
            fieldSelector: 'shortprogramstatechangewin gkhtriggerfield[name="RealObjs"]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#shortprogramromultiselectwindow',
            storeSelect: 'shortprogram.RealObjSelect',
            storeSelected: 'shortprogram.RealObjSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Жилые дома для отбора',
            titleGridSelected: 'Выбранные жилые дома',
            textProperty: 'Address',
            columnsGridSelect: [
                { header: 'Статус', xtype: 'gridcolumn', dataIndex: 'State', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Статус', xtype: 'gridcolumn', dataIndex: 'State', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            onBeforeLoad: function (store, operation) {
                var mainView = this.controller.getMainPanel(),
                    muId = mainView.down('b4combobox[name=Municipality]').getValue(),
                    year = mainView.down('b4combobox[name=Year]').getValue(),
                    form = Ext.ComponentQuery.query(this.controller.massStateChangeSelector)[0],
                    stateId = form.down('#cbCurrentState').getValue();
                
                operation.params.muId = muId;
                operation.params.stateId = stateId;
                operation.params.year = year;
            }
        }
    ],
    
    refs: [
        { ref: 'mainPanel', selector: 'shortprogrampanel' },
        { ref: 'roGrid', selector: 'shortprogramrogrid' },
        { ref: 'recordTitle', selector: 'shortprogrampanel breadcrumbs' },
        { ref: 'recordGrid', selector: 'shortprogramrecordgrid' },
        { ref: 'recordWindow', selector: '#shortprogramrecordwindow' },
        { ref: 'defectListGrid', selector: 'shortprogramdefectlistgrid' },
        { ref: 'defectListWindow', selector: 'shortprogramdefectlistwindow' },
        { ref: 'protocolGrid', selector: 'shortprogramprotocolgrid' },
        { ref: 'protocolWindow', selector: 'shortprogramprotocolwindow' }
    ],

    init: function() {
        var me = this,
            actions = {
                'shortprogrampanel button[action="ActualizeVersion"]': { click: { fn: me.onActualizeVersion, scope: me } },
                'shortprogrampanel button[action="MassStateChange"]': { click: { fn: me.onClickMassStateChange, scope: me } },
                'shortprogrampanel b4combobox[name=Municipality]': {
                    render: { fn: me.onRenderMunicipality, scope: me },
                    change: { fn: me.onChangeMunicipality, scope: me }
                },
                'shortprogrampanel b4combobox[name=Year]': {
                    render: { fn: me.onRenderYear, scope: me },
                    change: { fn: me.onChangeYear, scope: me }
                },
                'shortprogramrogrid': {
                     select: { fn: me.onSelectRo, scope: me }
                },
                'shortprogramrecordwindow': {
                     render: { fn: me.onRenderWindow, cope: me }
                },
                'shortprogramdefectlistwindow': {
                    render: { fn: me.onRenderWindow, cope: me }
                },
                'shortprogramprotocolwindow': {
                    render: { fn: me.onRenderWindow, cope: me }
                },
                'shortprogramstatechangewin #cbCurrentState': {
                    'change': { fn: me.onChangeCurrentState, scope: me },
                    'storebeforeload': { fn: me.onBeforeLoadCurrentState, scope: me }
                },
                'shortprogramstatechangewin #cbNextState': {
                    'change': { fn: me.onChangeNextState, scope: me },
                    'storebeforeload': { fn: me.onBeforeLoadNextState, scope: me }
                },
                'shortprogramstatechangewin #btnChangeState': {
                    'click': { fn: me.onClickBtnChange, scope: me }
                },
                'shortprogramstatechangewin b4savebutton': {
                    'click': { fn: me.onSaveStateChange, scope: me }
                },
                'shortprogramstatechangewin b4closebutton': {
                    'click': { fn: me.onCloseStateChange, scope: me }
                },
                'shortprogramprotocolwindow b4selectfield[name=Contragent]': {
                    'beforeload': { fn: me.onBeforeLoadContragent, scope: me }
                },
                'shortprogramdefectlistwindow b4selectfield[name=Work]': {
                    'beforeload': { fn: me.onBeforeLoadWorks, scope: me }
                }
            };

        me.control(actions);

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainPanel(),
            store;

        if (!view) {
            view = Ext.widget('shortprogrampanel');
            me.bindContext(view);
            me.application.deployView(view);

            store = view.down('shortprogramrogrid').getStore();
            store.on('beforeload', me.onBeforeLoadRealityObject, me);
            store.on('load', me.onLoadRealityObject, me);

            view.down('shortprogramrecordgrid').getStore().on('beforeload', me.onBeforeLoadRecord, me);
            view.down('shortprogramdefectlistgrid').getStore().on('beforeload', me.onBeforeLoadDefectList, me);
            view.down('shortprogramprotocolgrid').getStore().on('beforeload', me.onBeforeLoadProtocol, me);

            view.params = {};
        }

        view.down('hidden[name=MunicipalityId]').setValue(id);
    },
    
    onBeforeLoadContragent: function (store, operation) {
        operation.params = operation.params || {};
        operation.params.showAll = true;
    },
    
    onBeforeLoadWorks: function (store, operation) {
        var view = this.getMainPanel();
        operation.params = operation.params || {};
        operation.params.objectId = view.params.objectId;
    },
    
    onBeforeLoadRecord: function (store, operation) {
        var view = this.getMainPanel();
        operation.params.objectId = view.params.objectId;
    },

    onBeforeLoadDefectList: function (store, operation) {
        var view = this.getMainPanel();
        operation.params.objectId = view.params.objectId;
    },

    onBeforeLoadProtocol: function (store, operation) {
        var view = this.getMainPanel();
        operation.params.objectId = view.params.objectId;
    },
    
    onBeforeLoadCurrentState: function (field, store, operation) {
        operation.params.typeId = 'ovrhl_short_prog_object';
    },

    onBeforeLoadNextState: function (field, store, operation) {
        var form = field.up(),
            currStateField = form.down('#cbCurrentState');
        operation.params.currentStateId = currStateField.getValue();
    },
    
    onBeforeLoadRealityObject: function (store, operation) {
        var me = this;
        
        operation.params.municipalityId = me.getMainPanel().down('b4combobox[name="Municipality"]').getValue();
        operation.params.year = me.getMainPanel().down('b4combobox[name="Year"]').getValue();
    },

    onSaveStateChange: function (btn) {
        var me = this,
            form = btn.up('shortprogramstatechangewin'),
            nextStateId = form.down('#cbNextState').getValue(),
            realObjFldValue = form.down('gkhtriggerfield[name="RealObjs"]').getValue(), recordIds;

        if (!realObjFldValue) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать жилые дома', 'error');
            return;
        }

        recordIds = realObjFldValue.split(',');

        me.mask('Сохранение', me.getMainPanel());
            B4.Ajax.request({
                url: B4.Url.action('MassStateChange', 'ShortProgramRecord'),
                method: 'POST',
                params: {
                    objectIds: Ext.encode(recordIds),
                    nextStateId: nextStateId
                }
            }).next(function () {
                me.unmask();
                me.getMainPanel().down('shortprogramrogrid').getStore().load();
                B4.QuickMsg.msg('Успешно', 'Статусы успешно переведены', 'success');
                return true;
            }).error(function (e) {
                me.unmask();
                B4.QuickMsg.msg('Ошибка', e.message ? e.message : 'Во время изменения статуса произошла ошибка', 'error');
            });
    },
    
    onCloseStateChange: function (btn) {
        btn.up('shortprogramstatechangewin').close();
    },

    onClickMassStateChange: function () {
        var me = this;

        if (me.massStateChangeSelector) {
            var editWindow = Ext.ComponentQuery.query(me.massStateChangeSelector)[0];

            if (!editWindow) {
                editWindow = me.getView(me.massStateChangeView).create({ constrain: true, autoDestroy: true });

                editWindow.show();
                editWindow.center();
            }

            return editWindow;
        }
        return null;
    },
    
    onChangeCurrentState: function (field, newValue) {
        var form = field.up(),
            cbNextState = form.down('#cbNextState'),
            realObjFld = form.down('gkhtriggerfield[name="RealObjs"]');

        if (newValue) {
            cbNextState.setDisabled(false);
        } else {
            cbNextState.setDisabled(true);
        }
        
        cbNextState.setValue(null);
        realObjFld.setValue(null);
        cbNextState.store.load();
    },
    
    onChangeNextState: function (field, newValue, oldValue) {
        var realObjFld = field.up().down('gkhtriggerfield[name="RealObjs"]');
        if (!newValue) {
            realObjFld.setDisabled(true);
            realObjFld.setValue(null);
        } else {
            realObjFld.setDisabled(false);
        }
    },
    
    onClickBtnChange: function () {
        var me = this,
            controller = me.controller,
            store = controller.getStore(this.storeName);

        var ids = [];

        Ext.each(store.data.items, function (item) {
            ids.push(item.get('Id'));
        });

        if (ids.length == 0) {
            Ext.Msg.alert('Ошибка!', 'Не выбрано ни одного объекта');
            return;
        }

        controller.mask('Загрузка', controller.getMainComponent());
        B4.Ajax.request(B4.Url.action('MassChangeState', 'ObjectCr', {
            ids: Ext.JSON.encode(ids),
            newStateId: controller.getMainComponent().down('#cbNextState').getValue()
        })).next(function () {
            Ext.Msg.alert('Успешно!', 'Статусы выбранных объектов успешно переведены!');
            store.removeAll();
            controller.unmask();
        }).error(function () {
            controller.unmask();
        });
    },
    
    onActualizeVersion: function () {
        var me = this,
            pnl = me.getMainPanel(),
            moId = pnl.down('b4combobox[name=Municipality]').getValue(),
            year = pnl.down('b4combobox[name=Year]').getValue();
       
        Ext.Msg.confirm('Внимание!', Ext.String.format('Актуализация ДПКР будет проведена за {0} год. Продолжить?', year), function (result) {
            if (result == 'yes') {
                me.mask('Актуализация ДПКР', me.getMainPanel());
                B4.Ajax.request({
                    url: B4.Url.action('ActualizeVersion', 'ShortProgramRecord'),
                    method: 'POST',
                    timeout: 9999999,
                    params: { municipality_id: moId, year: year }
                }).next(function() {
                    me.unmask();
                    B4.QuickMsg.msg('Сообщение', Ext.String.format('Актуализация ДПКР за {0} выполнена успешно', year), 'success');
                }).error(function(e) {
                    me.unmask();
                    Ext.Msg.alert("Ошибка!", e.message ? e.message : 'Во время выполнения актуализации ДПКР произошла ошибка');
                });
            }
         }, me);
        },
    
    onChangeMunicipality: function (cmp, newValue) {
        var me = this,
            model = me.getModel('version.ProgramVersion'),
            store = cmp.up('shortprogrampanel').down('shortprogramrogrid').getStore(),
            permissionAspect = me.getAspect('shortprogramstatepermissionaspect');

        me.mask('Загрузка..', me.getMainPanel());
        B4.Ajax.request(B4.Url.action('GetMainVersionByMunicipality', 'ProgramVersion', {
            muId: newValue
        })).next(function (response) {
            var obj = Ext.decode(response.responseText),
                versionId = obj.Id;
            
            me.unmask();
            permissionAspect.setPermissionsByRecord(new model({ Id: versionId }));
            store.load();
        }).error(function(e) {
            me.unmask();
            B4.QuickMsg.msg('Сообщение', e.message || 'Актуализация ДПКР выполнена успешно', 'success');
        });
    },
    
    onChangeYear: function(cmp) {
        var store = cmp.up('shortprogrampanel').down('shortprogramrogrid').getStore();
        
        store.load();
    },

    onRenderYear: function (cmp) {
        var me = this,
            store = cmp.getStore();

        store.on('load', me.onLoadYear, me, { single: true });
        store.load();
    },

    onLoadYear: function (store) {
        var me = this,
            panel = me.getMainPanel(),
            cmd = panel.down('b4combobox[name=Year]'),
            record;

        if (store.getCount() > 0) {

            record = store.findRecord('Default', true, false, true, true);

            cmd.setValue(record.get('Name'));
        }
    },

    onRenderMunicipality: function(cmp) {
        var me = this,
            store = cmp.getStore();

        store.on('load', me.onLoadMunicipality, me, { single: true });
        store.load();
    },
    
    onLoadMunicipality: function (store, records) {
        var me = this,
            panel = me.getMainPanel(),
            cmd = panel.down('b4combobox[name="Municipality"]'),
            muId = panel.down('hidden[name=MunicipalityId]').getValue(),
            record;
        
        if (store.getCount() > 0) {
            
            if (muId) {
                record = store.findRecord('Id', muId, false, true, true);
            }

            cmd.setValue(record ? record.getData() : records[0].data);
        }
    },
    
    onSelectRo: function(grid, record) {
        var me = this,
            view = me.getMainPanel(),
            recordGrid = me.getRecordGrid(),
            defectListGrid = me.getDefectListGrid(),
            protocolGrid = me.getProtocolGrid(),
            permAspect = me.getAspect('shortprogramrelobjstateperm');
        
        view.params.objectId = record.getId();
        me.getRecordTitle().update({ text: record.get('Address') });
        recordGrid.getStore().load();
        defectListGrid.getStore().load();
        protocolGrid.getStore().load();

        permAspect.setPermissionsByRecord(record);
    },
    
    onLoadRealityObject: function(store, records) {
        var me = this,
            recordGrid = me.getRecordGrid(),
            defectListGrid = me.getDefectListGrid(),
            protocolGrid = me.getProtocolGrid();

        if (store.getCount() > 0) {
            me.onSelectRo(me.getRoGrid(), records[0]);
            recordGrid.setDisabled(false);
            defectListGrid.setDisabled(false);
            protocolGrid.setDisabled(false);
        } else {
            me.getRecordTitle().update({ text: 'Не выбран дом' });
            recordGrid.setDisabled(true);
            defectListGrid.setDisabled(true);
            protocolGrid.setDisabled(true);
            recordGrid.getStore().removeAll();
            defectListGrid.getStore().removeAll();
            protocolGrid.getStore().removeAll();
        }
    },

    onRenderWindow: function (wnd) {
        var me = this,
            view = me.getMainPanel(),
            permissions = ['Ovrhl.ShortProgram.RealityObject.Edit'];

        B4.Ajax.request({
            url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
            method: 'POST',
            params: {
                permissions: Ext.encode(permissions),
                ids: Ext.encode([view.params.objectId])
            }
        }).next(function(response) {
            var grants = Ext.decode(response.responseText)[0];
            wnd.down('b4savebutton').setDisabled(!grants[0]);
        }).error(function() {
            wnd.down('b4savebutton').setDisabled(true);
        });
    }
});