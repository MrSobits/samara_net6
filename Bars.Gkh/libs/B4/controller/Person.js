Ext.define('B4.controller.Person', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
        'B4.aspects.StateContextMenu',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.store.contragent.Contact',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.FieldRequirementAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        loader: 'B4.mixins.LayoutControllerLoader',
        context: 'B4.mixins.Context'
    },

    models: ['Person'],
    stores: ['Person'],
    views: [
        'person.Grid',
        'person.AddWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'person.Grid',
    mainViewSelector: 'personGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'personGrid'
        },
        {
            ref: 'addView',
            selector: 'personaddwindow'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Person.Create', applyTo: 'b4addbutton', selector: 'personGrid' }
            ]
        },
        {
            xtype: 'gkhpermissionaspect',
            editFormAspectName: 'personGridWindowAspect',
            permissions: [
                { name: 'Gkh.Person.AddInContragent', applyTo: 'button[actionName=addContact]', selector: 'personaddwindow' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.Person.Delete' }],
            name: 'deletePersonStatePerm'
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.PersonRegisterGji.Field.Inn_Rqrd', applyTo: '[name=Inn]', selector: 'personaddwindow' },
                { name: 'GkhGji.PersonRegisterGji.Field.TypeIdentityDocument_Rqrd', applyTo: '[name=TypeIdentityDocument]', selector: 'personaddwindow' },
                { name: 'GkhGji.PersonRegisterGji.Field.IdIssuedDate_Rqrd', applyTo: '[name=IdIssuedDate]', selector: 'personaddwindow' },
                { name: 'GkhGji.PersonRegisterGji.Field.IdSerial_Rqrd', applyTo: '[name=IdSerial]', selector: 'personaddwindow' },
                { name: 'GkhGji.PersonRegisterGji.Field.IdNumber_Rqrd', applyTo: '[name=IdNumber]', selector: 'personaddwindow' },
                { name: 'GkhGji.PersonRegisterGji.Field.IdIssuedBy_Rqrd', applyTo: '[name=IdIssuedBy]', selector: 'personaddwindow' },
                { name: 'GkhGji.PersonRegisterGji.Field.Contragent_Rqrd', applyTo: '[name=Contragent]', selector: 'personaddwindow' },
                { name: 'GkhGji.PersonRegisterGji.Field.StartDate_Rqrd', applyTo: '[name=StartDate]', selector: 'personaddwindow' }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'personStateTransferAspect',
            gridSelector: 'personGrid',
            menuSelector: 'personGridStateMenu',
            stateType: 'gkh_person'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'QualificationCertificatePrintAspect',
            buttonSelector: 'personGrid #btnPrint',
            codeForm: 'QualificationCertificate',
            getUserParams: function(reportId) {

            }
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'personGridWindowAspect',
            gridSelector: 'personGrid',
            editFormSelector: '#personAddWindow',
            storeName: 'Person',
            modelName: 'Person',
            editWindowView: 'person.AddWindow',
            controllerEditName: 'B4.controller.person.Navi',
            deleteWithRelatedEntities: true,
            listeners: {
                savesuccess: function(asp, rec) {
                    var form = asp.getForm(),
                        values = form.getValues();

                    if (values.Contragent || values.StartDate || values.EndDate || values.Position)
                    {
                        asp.controller.mask('Сохранение', form);
                        B4.Ajax.request(B4.Url.action('AddWorkPlace',
                            'Person',
                            {
                                personId: rec.getId(),
                                contragentId: values.Contragent,
                                startDate: values.StartDate,
                                endDate: values.EndDate,
                                positionId: values.Position
                            })).next(function() {
                            asp.onSaveSuccess(asp, rec);
                            asp.controller.unmask();
                            return true;
                        }).error(function(e) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка!', e.message);
                        });
                        return false;
                    }
                    return true;
                }
            },

            rowAction: function(grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                    }
                }
            },
            deleteRecord: function (record) {
                var me = this,
                    grants,
                    model,
                    rec;

                if (record.getId()) {
                    me.controller.getAspect('deletePersonStatePerm').loadPermissions(record)
                        .next(function (response) {
                            grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        model = me.getModel(record);
                                        rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function () {
                                                me.fireEvent('deletesuccess', me);
                                                me.updateGrid();
                                                me.unmask();
                                            }, me)
                                            .error(function (result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, me);
                                    }
                                }, me);
                            }

                        }, me);
                }
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model;


                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('personedit/{0}', id));
                    } else {
                        model.load(id, {
                            success: function(rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        },
        {
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'personAddConactAspect',
            buttonSelector: '#personAddWindow button[actionName=addContact]',
            multiSelectWindowSelector: '#personAddConactSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'contragent.Contact',
            storeSelected: 'contragent.Contact',
            selModelMode: 'SINGLE',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'FullName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Контрагент', xtype: 'gridcolumn', dataIndex: 'Contragent', flex: 1, filter: { xtype: 'textfield' } },
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentMunicipality',
                    flex: 1,
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'ContragentAddress', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'FullName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор контактной информации',
            titleGridSelect: 'Контактная информация для отбора',
            titleGridSelected: 'Выбранные контакты',
            listeners: {
                getdata: function(asp, records) {
                    var contactId,
                        addWindow = asp.controller.getAddView();

                    Ext.Array.each(records.items,
                        function(item) {
                            contactId = item.get('Id');
                        });

                    asp.controller.mask('Получение информации', addWindow);
                    if (contactId) {
                        B4.Ajax.request(B4.Url.action('GetContactDetails', 'Person', {
                            contactId: contactId
                        })).next(function(response) {
                            var resp = Ext.JSON.decode(response.responseText);
                            addWindow.getForm().setValues(resp.data);
                            asp.controller.unmask();
                            return true;
                        }).error(function(e) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка!', e.message);
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать контактную информацию контрагента');
                        return false;
                    }

                    return true;
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'personButtonExportAspect',
            gridSelector: 'personGrid',
            buttonSelector: 'personGrid #btnExport',
            controllerName: 'Person',
            actionName: 'Export',
            usePost: true
        }
    ],

    init: function() {
        this.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('personGrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();


        this.getAspect('QualificationCertificatePrintAspect').loadReportStore();
    }
});