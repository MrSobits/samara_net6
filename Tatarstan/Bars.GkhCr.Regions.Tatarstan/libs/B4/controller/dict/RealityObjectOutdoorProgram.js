Ext.define('B4.controller.dict.RealityObjectOutdoorProgram', {
    /*
    * Контроллер раздела программы благоустройства двора
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.RealityObjectOutdoorProgram',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'dict.RealityObjectOutdoorProgram',
        'dict.Period',
        'dict.RealityObjectOutdoorProgramChangeJournal'
    ],
    stores: [
        'dict.RealityObjectOutdoorProgram',
        'dict.RealityObjectOutdoorProgramChangeJournal'
    ],
    views: [
        'dict.realityobjectoutdoorprogram.EditWindow',
        'dict.realityobjectoutdoorprogram.CopyWindow',
        'dict.realityobjectoutdoorprogram.Grid',
        'SelectWindow.MultiSelectWindow',
        'dict.realityobjectoutdoorprogram.ChangeJournalGrid'
    ],

    mainView: 'dict.realityobjectoutdoorprogram.Grid',
    mainViewSelector: 'outdoorprogramgrid',

    aspects: [
        {
            xtype: "outdoorprogramperm"
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'outdoorProgramGridWindowAspect',
            gridSelector: 'outdoorprogramgrid',
            editFormSelector: 'outdoorprogrameditwindow',
            copyFormSelector: 'outdoorprogramcopywindow',
            modelName: 'dict.RealityObjectOutdoorProgram',
            editWindowView: 'dict.realityobjectoutdoorprogram.EditWindow',

            onSaveSuccess: function (asp, record) {
                asp.getForm(asp.editFormSelector).close();
            },

            otherActions: function (actions) {
                actions[this.copyFormSelector + ' [name=copyProgramButton]'] = { 'click': { fn: this.onCopyBtnClick, scope: this } };
                actions[this.copyFormSelector + ' b4closebutton'] = { 'click': { fn: this.closeCopyWin, scope: this } };
            },

            listeners: {
                aftersetformdata: function (asp, record) {
                    var id = record.getId(),
                        editForm = asp.getForm(asp.editFormSelector),
                        editFormStore = editForm.down('outdoorprogramchangejournalgrid').getStore(),
                        isEdit = id > 0;

                    editForm.down('outdoorprogramchangejournalgrid').setDisabled(!isEdit);
                    if (isEdit) {
                        asp.controller.setContextValue(asp.controller.getMainView(), 'outdoorProgramId', id);
                        editFormStore.on('beforeload', this.onBeforeLoad, this);
                        editFormStore.load();
                    } 
                }
            },

            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                        case 'copy':
                            this.copyRecord(record);
                            break;
                    }
                }
            },

            copyRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model = this.getModel(record);

                me.controller.setContextValue(me.controller.getMainView(), 'outdoorProgramId', id);

                id ? model.load(id, {
                    success: function (rec) {
                        me.setCopyFormData(rec);
                    },
                    scope: this
                })
                    : this.setFormData(new model({ Id: 0 }));
            },

            setCopyFormData: function (rec) {
                var me = this,
                    form = this.copyFormSelector ? Ext.widget(me.copyFormSelector) : null;
                if (form == null || me.fireEvent('beforesetformdata', me, rec, form) === false) {
                    return false;
                }

                Ext.Object.each(rec.data, function (property, value) {
                    var component = form.down('[name=' + property + ']'),
                        newComponent = form.down('[name=New' + property + ']');
                    if (component) {
                        component.setValue(value);
                    }
                    if (newComponent && property !== 'Name') {
                        newComponent.setValue(value);
                    }

                    //просьба аналитиков добавлять в наименование слово 'Копия'
                    if (newComponent && property === 'Name') {
                        newComponent.setValue('Копия ' + value);
                    }
                });

                form.show();
            },

            onCopyBtnClick: function (btn) {
                var asp = this,
                    copyWin = btn.up(asp.copyFormSelector),
                    programId = asp.controller.getContextValue(asp.controller.getMainView(), 'outdoorProgramId'),
                    programName = copyWin.down('[name=NewName]').getValue(),
                    programPeriod = copyWin.down('[name=NewPeriod]').getValue(),
                    isProgramNameValid = programName != null && programName != '',
                    isProgramPeriodValid = programPeriod != null;

                if (!(isProgramNameValid && isProgramPeriodValid)) {
                    var commonMessagePart = ' Это поле обязательно для заполнения<br>',
                        errormessage = 'Следующие поля содержат ошибки:<br>' +
                            (isProgramNameValid ? '' : '<b>Наименование:</b>' + commonMessagePart) + 
                            (isProgramPeriodValid ? '' : '<b>Период:</b>' + commonMessagePart);

                    Ext.Msg.alert('Ошибка копирования!', errormessage);
                    return false;
                }

                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(
                    {
                        method: 'POST',
                        url: B4.Url.action('CopyProgram', 'RealityObjectOutdoorProgram'),
                        params: {
                            oldOutdoorProgramId: programId,
                            Name: programName,
                            Code: copyWin.down('[name=NewCode]').getValue(),
                            Period: programPeriod,
                            Visible: copyWin.down('[name=NewTypeVisibilityProgram]').getValue(),
                            Type: copyWin.down('[name=NewTypeProgram]').getValue(),
                            State: copyWin.down('[name=NewTypeProgramState]').getValue(),
                            IsNotAddOutdoor: copyWin.down('[name=NewIsNotAddOutdoor]').getValue(),
                            Description: copyWin.down('[name=NewDescription]').getValue()
                        },
                        timeout: 999999
                    }).next(function () {
                        copyWin.close();
                        asp.updateGrid();
                        asp.controller.unmask();
                        Ext.Msg.alert('Копирование программы', 'Копирование успешно завершено');
                    }).error(function (e) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка!', 'Не удалось скопировать программу: ' + e.message);
                    });
            },

            onBeforeLoad: function (store, operation) {
                operation.params.outdoorProgramId = this.controller.getContextValue(this.controller.getMainView(), 'outdoorProgramId');
            },

            closeCopyWin: function (btn) {
                btn.up(this.copyFormSelector).close();
            },
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'outdoorprogramchangejournalgrid b4updatebutton': {
                change: {
                    fn: me.updateJournal,
                    scope: me
                }
            }
        });
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = this.getMainView() || Ext.widget(me.mainViewSelector);
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    },

    updateJournal: function (btn) {
        btn.up('grid').getStore().load();
    }
});