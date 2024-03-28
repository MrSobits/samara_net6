Ext.define('B4.controller.administration.Operator', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.form.ComboBox',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.administration.Operator'
    ],

    models: [
        'administration.Operator',
        'dict.Inspector',
        'dict.Municipality',
        'Contragent'
    ],

    stores: [
        'administration.Operator',
        'administration.operator.Contragent',
        'administration.operator.Inspector',
        'administration.operator.Municipality',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'contragent.ContragentForSelect',
        'contragent.ContragentForSelected'
    ],

    views: [
        'administration.operator.MainPanel',
        'administration.operator.Grid',
        'administration.operator.EditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'administration.operator.MainPanel',
    mainViewSelector: 'administrationOperatorPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'administrationOperatorPanel'
        }
    ],

    aspects: [
        {
            xtype: 'operatoradminperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'operatorGridWindowAspect',
            gridSelector: '#operatorGrid',
            editFormSelector: '#operatorEditWindow',
            storeName: 'administration.Operator',
            modelName: 'administration.Operator',
            editWindowView: 'administration.operator.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.operatorId = record.getId();
                this.updateControls(asp.controller.operatorId);
            },
            otherActions: function (actions) {
                actions['#operatorEditWindow #btnCreatePassword'] = { 'click': { fn: this.createNewPassword, scope: this } };
            },
            createNewPassword: function (btn) {
                var me = this;
                B4.Ajax.request({
                    url: B4.Url.action('GenerateNewPassword', 'Operator'),
                    method: 'POST',
                    timeout: 100 * 60 * 60 * 3
                })
                    .next(function (response) {
                        var obj = Ext.JSON.decode(response.responseText);
                        me.unmask();
                        Ext.Msg.alert('Новый пароль', obj.password.bold() + "<br><br>Не забудьте сохранить запись");
                        var editWindow = btn.up('#operatorEditWindow');
                        var newpass = editWindow.down('#tfpassword');
                        var newpassconfirm = editWindow.down('#tfnewPassword');
                        newpass.setValue(obj.password);
                        newpassconfirm.setValue(obj.password);
                        //   grid.getStore().load();
                    })
                    .error(function (error) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка расчета', error.message || 'Ошибка при расчете даты');
                    });
            },
            updateControls: function (objectId) {
                var frm = this.getForm();

                var readOnly = objectId == 0;

                frm.down('#operatorInspectorsTrigerField').setReadOnly(readOnly);
                frm.down('#operatorMunicipalitiesTrigerField').setReadOnly(readOnly);
                frm.down('#operatorContragentTrigerField').setReadOnly(readOnly);
            },

            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var operatorId = record.getId(),
                        fieldInspectors,
                        fieldMunicipalities,
                        fieldContragents,
                        fieldToken;

                    asp.controller.operatorId = operatorId;

                    this.updateControls(operatorId);

                    fieldInspectors = form.down('#operatorInspectorsTrigerField');
                    fieldMunicipalities = form.down('#operatorMunicipalitiesTrigerField');
                    fieldContragents = form.down('#operatorContragentTrigerField');
                    fieldToken = form.down('textarea[name=RisToken]');

                    if (operatorId > 0) {
                        // если токен рис указан, то больше его редактировать нельзя
                        if (fieldToken.getValue()) {
                            fieldToken.hide();
                        }

                        asp.controller.mask('Загрузка', form);
                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('GetInfo', 'Operator'),
                            params: {
                                operatorId: asp.controller.operatorId
                            }
                        }).next(function (response) {
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText);

                            fieldInspectors.updateDisplayedText(obj.inspectorNames);
                            fieldInspectors.setValue(obj.inspectorIds);

                            fieldMunicipalities.value = obj.municipalityIds;
                            fieldMunicipalities.updateDisplayedText(obj.municipalityNames);

                            fieldContragents.updateDisplayedText(obj.contragentNames);
                            fieldContragents.setValue(obj.contragentIds);

                            asp.controller.unmask();
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        fieldInspectors.updateDisplayedText(null);
                        fieldInspectors.setValue(null);

                        fieldMunicipalities.updateDisplayedText(null);
                        fieldMunicipalities.setValue(null);

                        fieldContragents.updateDisplayedText(null);
                        fieldContragents.setValue(null);
                    }
                },
                getdata: function (asp, record) {
                    var me = this,
                        form = me.getForm(),
                        operatorId = record.getId(),
                        muIds = [],
                        operatorIds = [],
                        inspectorIds = [];

                    if (operatorId > 0) {
                        var muValue = form.down('#operatorMunicipalitiesTrigerField').getValue();

                        if (muValue) {
                            var value = muValue.toString().trim();

                            if (value.length) {
                                muIds = value.split(',');
                            }
                        }

                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('AddMunicipalities', 'Operator'),
                            params: {
                                objectIds: Ext.encode(muIds),
                                operatorId: operatorId
                            }
                        }).next(function () {
                            var orgsValue = form.down('#operatorContragentTrigerField').getValue();

                            if (orgsValue) {
                                var value = orgsValue.toString().trim();

                                if (value.length) {
                                    operatorIds = value.split(',');
                                }
                            }

                            B4.Ajax.request({
                                method: 'POST',
                                url: B4.Url.action('AddContragents', 'Operator'),
                                params: {
                                    objectIds: Ext.encode(operatorIds),
                                    operatorId: operatorId
                                }
                            }).next(function () {
                                var inspectorsValue = form.down('#operatorInspectorsTrigerField').getValue();

                                if (inspectorsValue) {
                                    var val = inspectorsValue.toString().trim();

                                    if (val.length) {
                                        inspectorIds = val.split(',');
                                    }
                                }

                                B4.Ajax.request({
                                    method: 'POST',
                                    url: B4.Url.action('AddInspectors', 'Operator'),
                                    params: {
                                        objectIds: Ext.encode(inspectorIds),
                                        operatorId: operatorId
                                    }
                                }).next(function () {
                                    return true;
                                });
                            });
                        });

                        return false;
                    }
                }
            }
        },
        {
            /*
           аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
           по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
           По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
           и сохраняем инспекторов через серверный метод /Operator/AddInspectors
           */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'operatorInspectorMultiSelectWindowAspect',
            fieldSelector: '#operatorEditWindow #operatorInspectorsTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#operatorInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'administration.operator.Inspector',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            onSelectedBeforeLoad: function (store, operation) {
                operation.params['operatorId'] = this.controller.operatorId;
            } //,
            //listeners: {
            //    getdata: function (asp, records) {
            //        var recordIds = [];
            //        records.each(function (rec) { recordIds.push(rec.get('Id')); });
            //        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
            //        B4.Ajax.request({
            //            method: 'POST',
            //            url: B4.Url.action('AddInspectors', 'Operator'),
            //            params: {
            //                objectIds: Ext.encode(recordIds),
            //                operatorId: asp.controller.operatorId
            //            }
            //        }).next(function () {
            //            Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
            //            asp.controller.unmask();
            //            return true;
            //        }).error(function () {
            //            asp.controller.unmask();
            //        });

            //        return true;
            //    }
            //}
        },
        {
            /*
           аспект взаимодействия триггер-поля организации с массовой формой выбора 
           по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
           По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
           и сохраняем инспекторов через серверный метод /Operator/AddManOrgs
           */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'operatorManOrgMultiSelectWindowAspect',
            fieldSelector: '#operatorEditWindow #operatorContragentTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#operatorManOrgSelectWindow',
            storeSelect: 'contragent.ContragentForSelect',
            storeSelected: 'administration.operator.Contragent',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                {
                    header: 'Муниципальный район', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: {
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
                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, sortable: false },
                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор контрагентов',
            titleGridSelect: 'Контрагенты для отбора',
            titleGridSelected: 'Выбранные контрагенты',
            onSelectedBeforeLoad: function (store, operation) {
                operation.params['operatorId'] = this.controller.operatorId;
            }//,
            //listeners: {
            //    getdata: function (asp, records) {
            //        var recordIds = [];
            //        records.each(function (rec) { recordIds.push(rec.get('Id')); });
            //        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
            //        B4.Ajax.request({
            //            method: 'POST',
            //            url: B4.Url.action('AddContragents', 'Operator'),
            //            params: {
            //                objectIds: Ext.encode(recordIds),
            //                operatorId: asp.controller.operatorId
            //            }
            //        }).next(function () {
            //            Ext.Msg.alert('Сохранение!', 'Контрагенты сохранены успешно');
            //            asp.controller.unmask();
            //            return true;
            //        }).error(function () {
            //            asp.controller.unmask();
            //        });

            //        return true;
            //    }
            //}
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('administrationOperatorPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('administration.Operator').load();
    }
});