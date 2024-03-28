Ext.define('B4.controller.BaseStatement', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.BaseStatement'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['BaseStatement'],
    stores: [
        'BaseStatement'
    ],
    views: [
        'basestatement.MainPanel',
        'basestatement.Grid',
        'basestatement.AddWindow',
        'basestatement.FilterPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'basestatement.MainPanel',
    mainViewSelector: 'baseStatementPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'baseStatementPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Inspection.BaseStatement.Create', applyTo: 'b4addbutton', selector: '#baseStatementGrid' },
                { name: 'GkhGji.Inspection.BaseStatement.Edit', applyTo: 'b4savebutton', selector: '#baseStatementEditPanel' },
                { name: 'GkhGji.Inspection.BaseStatement.Delete', applyTo: 'b4deletecolumn', selector: '#baseStatementGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhGji.Inspection.BaseStatement.CheckBoxShowCloseInsp', applyTo: '#cbShowCloseInspections', selector: '#baseStatementGrid',
                   applyBy: function (component, allowed) {
                       if (allowed) {
                           this.controller.params.showCloseInspections = false;
                           this.controller.getStore('BaseStatement').load();
                           component.show();
                       } else {
                           this.controller.params.showCloseInspections = true;
                           this.controller.getStore('BaseStatement').load();
                           component.hide();
                       }
                   }
               }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'baseStatementStateTransferAspect',
            gridSelector: '#baseStatementGrid',
            menuSelector: 'baseStatementStateMenu',
            stateType: 'gji_inspection'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'baseStatementExportAspect',
            gridSelector: '#baseStatementGrid',
            buttonSelector: '#baseStatementGrid #btnExport',
            controllerName: 'BaseStatement',
            actionName: 'Export'
        },
        {
            /*
            аспект взаимодействия таблицы проверок по обращениям граждан, формы добавления и Панели редактирования,
            открывающейся в боковой вкладке
            */
            xtype: 'gkhgrideditformaspect',
            name: 'baseStatementGridWindowAspect',
            gridSelector: '#baseStatementGrid',
            editFormSelector: '#baseStatementAddWindow',
            storeName: 'BaseStatement',
            modelName: 'BaseStatement',
            editWindowView: 'basestatement.AddWindow',
            controllerEditName: 'B4.controller.basestatement.Navigation',
            otherActions: function (actions) {
                actions['#baseStatementFilterPanel #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
                actions['#baseStatementFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };

                actions[this.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: this.onChangeType, scope: this } };
                actions[this.editFormSelector + ' #cbPersonInspection'] = { 'change': { fn: this.onChangePerson, scope: this } };
                actions[this.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions['#baseStatementGrid #cbShowCloseInspections'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('BaseStatement');
                str.currentPage = 1;
                str.load();
            },
            onChangeRealityObject: function (field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.realityObjectId = newValue.Id;
                } else {
                    this.controller.params.realityObjectId = null;
                }
            },
            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = this.controller.params.typeJurOrg;
            },
            onChangeType: function (field, newValue, oldValue) {
                this.controller.params = this.controller.params || {};
                this.controller.params.typeJurOrg = newValue;
                this.getForm().down('#sfContragent').setValue(null);
                this.getForm().down('#tfPhysicalPerson').setValue(null);
            },
            onChangePerson: function (field, newValue, oldValue) {
                var form = this.getForm(),
                    sfContragent = form.down('#sfContragent'),
                    tfPhysicalPerson = form.down('#tfPhysicalPerson'),
                    cbTypeJurPerson = form.down('#cbTypeJurPerson');
                sfContragent.setValue(null);
                tfPhysicalPerson.setValue(null);
                cbTypeJurPerson.setValue(10);
                switch (newValue) {
                    case 10://физлицо
                        sfContragent.setDisabled(true);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(true);
                        break;
                    case 20://организацияы
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(true);
                        cbTypeJurPerson.setDisabled(false);
                        break;
                    case 30://должностное лицо
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(false);
                        break;
                }
            },
            onChangeCheckbox: function (field, newValue) {
                this.controller.params.showCloseInspections = newValue;
                this.controller.getStore('BaseStatement').load();
            },
            saveRecordHasNotUpload: function (rec) {
                var me = this;
                var frm = me.getForm();
                me.mask('Сохранение', frm);
                B4.Ajax.request({
                    url: B4.Url.action('CreateNewBaseStatment', 'RegionSpecific'),
                    method: 'POST',
                    params: {
                        baseStatement: Ext.encode(rec.data),
                        contragentId: rec.get('Contragent'),
                        appealCitizenId: rec.get('AppealCit')
                    }
                }).
                next(function (response) {
                    var res = Ext.JSON.decode(response.responseText);
                    me.unmask();
                    me.updateGrid();

                    var model = me.getModel(rec);

                    model.load(res.data.Id, {
                        success: function (record) {
                            me.editRecord(record);
                        }
                    });

                    me.fireEvent('savesuccess', me, res);
                }, this)
                .error(function (result) {
                    me.unmask();
                    me.fireEvent('savefailure', result.record, result.responseData);

                    Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                }, this);
            }
        }
    ],

    init: function () {
        this.params = {};
        this.getStore('BaseStatement').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('baseStatementPanel');
        this.bindContext(view);
        this.application.deployView(view);
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.subjIds = this.params.subjIds;
            operation.params.realityObjectId = this.params.realityObjectId;
            operation.params.showCloseInspections = this.params.showCloseInspections;
        }
    }
});