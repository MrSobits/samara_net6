Ext.define('B4.controller.InfoAboutUseCommonFacilities', {
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.infoaboutusecommonfacilities.State'
    ],

    models:
    [
        'DisclosureInfoRealityObj',
        'InfoAboutUseCommonFacilities'
    ],
    stores: ['InfoAboutUseCommonFacilities'],

    views: [
        'infoaboutusecommonfacilities.EditPanel',
        'infoaboutusecommonfacilities.EditWindow'
    ],

    mainView: 'infoaboutusecommonfacilities.EditPanel',
    mainViewSelector: '#infoAboutUseCommonFacilitiesEditPanel',

    aspects: [
        {
            xtype: 'infoaboutusecommonfacilitiesstateperm',
            name: 'infoAboutUseCommonFacilitiesPermissionAspect'
        },
        {
            xtype: 'gkheditpanel',
            name: 'infoAboutUseCommonFacilitiesEditPanelAspect',
            modelName: 'DisclosureInfoRealityObj',
            editPanelSelector: '#infoAboutUseCommonFacilitiesEditPanel',

            listeners: {
                beforesave: function (asp, record) {
                    record.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                }
            },
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' b4addbutton'] = { 'click': { fn: this.onAddBtnClick, scope: this} };
                actions[this.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: this.onUpdateBtnClick, scope: this} };
                actions[this.editPanelSelector + ' #cbPlaceGeneralUse'] = { 'change': { fn: this.changePlaceGeneralUse, scope: this } };
            },
            onAddBtnClick: function () {
                this.controller.getAspect('infoAboutUseCommonFacilitiesGridWindowAspect').editRecord();
            },
            onUpdateBtnClick: function () {
                this.setData(this.controller.params.disclosureInfoRealityObjId);
                if (Ext.ComponentQuery.query('#cbPlaceGeneralUse')[0].value == 10) {
                    this.controller.getAspect('infoAboutUseCommonFacilitiesGridWindowAspect').updateGrid();
                }
            },

            changePlaceGeneralUse: function (field, newValue, oldValue) {
                //При первом заходе не сохраняем
                if (oldValue !== undefined) {
                    this.saveRequestHandler();
                }

                this.setDisableGrid(field);
            },

            setDisableGrid: function (field) {

                var grid = Ext.ComponentQuery.query('#infoAboutUseCommonFacilitiesGrid')[0];
                var editPanel = Ext.ComponentQuery.query('#infoAboutUseCommonFacilitiesEditPanel')[0];
                var addInfoAboutUseCommonFacilitiesButton = Ext.ComponentQuery.query('#addInfoAboutUseCommonFacilitiesButton')[0];

                if (field.getValue() != 10) {
                    grid.setDisabled(true);
                    grid.getStore().removeAll();
                    addInfoAboutUseCommonFacilitiesButton.hide();
                }
                else {
                    grid.setDisabled(false);
                    grid.getStore().load();
                    addInfoAboutUseCommonFacilitiesButton.show();
                }
                editPanel.doLayout();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'infoAboutUseCommonFacilitiesGridWindowAspect',
            gridSelector: '#infoAboutUseCommonFacilitiesGrid',
            editFormSelector: 'infoaboutusecommonfacilitieseditwindow',
            storeName: 'InfoAboutUseCommonFacilities',
            modelName: 'InfoAboutUseCommonFacilities',
            editWindowView: 'infoaboutusecommonfacilities.EditWindow',
            otherActions: function (actions) {
                var me = this;
                actions['infoaboutusecommonfacilitieseditwindow b4enumcombo[name="LesseeType"]'] = { 'change': { fn: me.onChangeCombo, scope: me } };
                actions['infoaboutusecommonfacilitieseditwindow checkboxfield[name="IsLastDayMonthPeriodIn"]'] = { 'change': { fn: me.onChangeIsLastDayMonthPeriodIn, scope: me } };
                actions['infoaboutusecommonfacilitieseditwindow checkboxfield[name="IsLastDayMonthPeriodOut"]'] = { 'change': { fn: me.onChangeIsLastDayMonthPeriodOut, scope: me } };
            },
            onChangeCombo: function (field) {
                var form = field.up(),
                    surnameField = form.down('textfield[name="Surname"]'),
                    nameField = form.down('textfield[name="Name"]'),
                    patronymicField = form.down('textfield[name="Patronymic"]'),
                    genderField = form.down('b4enumcombo[name="Gender"]'),
                    birthDateField = form.down('datefield[name="BirthDate"]'),
                    birthPlaceField = form.down('textfield[name="BirthPlace"]'),
                    snilsField = form.down('textfield[name="Snils"]'),
                    ogrnField = form.down('textfield[name="Ogrn"]'),
                    innField = form.down('textfield[name="Inn"]'),
                    lesseeField = form.down('textfield[name="Lessee"]'),
                    lesseeType = form.down('b4enumcombo[name="LesseeType"]').getValue();

                var legal = lesseeType === B4.enums.LesseeTypeDi.Legal,
                    physical = lesseeType === B4.enums.LesseeTypeDi.Individual;
                
                surnameField.setDisabled(legal);
                nameField.setDisabled(legal);
                patronymicField.setDisabled(legal);
                genderField.setDisabled(legal);
                birthDateField.setDisabled(legal);
                birthPlaceField.setDisabled(legal);
                snilsField.setDisabled(legal);
                ogrnField.setDisabled(physical);
                innField.setDisabled(physical);
                lesseeField.setDisabled(physical);
            },
            onChangeIsLastDayMonthPeriodIn: function (field) {
                var form = field.up(),
                    dayMonthPeriodInField = form.down('numberfield[name="DayMonthPeriodIn"]'),
                    isLastDayMonthPeriodIn = form.down('checkboxfield[name="IsLastDayMonthPeriodIn"]').getValue();

                dayMonthPeriodInField.setDisabled(isLastDayMonthPeriodIn);
            },
            onChangeIsLastDayMonthPeriodOut: function (field) {
                var form = field.up(),
                    dayMonthPeriodOutField = form.down('numberfield[name="DayMonthPeriodOut"]'),
                    isLastDayMonthPeriodOut = form.down('checkboxfield[name="IsLastDayMonthPeriodOut"]').getValue();

                dayMonthPeriodOutField.setDisabled(isLastDayMonthPeriodOut);
            },
            listeners: {
                beforesave: function (asp, record) {
                    var periodDiStartDateString = asp.controller.params.periodDiDateStart,
                        periodDiEndDateString = asp.controller.params.periodDiDateEnd,
                        periodDiStartDate = Ext.Date.parse(periodDiStartDateString, 'd.m.Y'),
                        periodDiEndDate = Ext.Date.parse(periodDiEndDateString, 'd.m.Y'),
                        recordDateStart = record.data.DateStart,
                        recordDateEnd = record.data.DateEnd;

                    if (!((recordDateStart != null && periodDiStartDateString != null && recordDateStart >= periodDiStartDate || periodDiStartDateString == null)
                        && (periodDiEndDateString != null && recordDateStart != null && periodDiEndDate >= recordDateStart || periodDiEndDateString == null)
                        || (recordDateStart != null && periodDiStartDateString != null && periodDiStartDate >= recordDateStart || recordDateStart == null)
                    && (recordDateEnd != null && periodDiStartDateString != null && recordDateEnd >= periodDiStartDate || recordDateEnd == null))) {
                        Ext.Msg.alert('Ошибка сохранения', 'Следующие поля содержат ошибки:<br><b>Дата начала - дата окончания:</b> Дата начала - дата окончания не входит в период раскрытия информации.');
                        return false;
                    }

                    record.set('DisclosureInfoRealityObj', asp.controller.params.disclosureInfoRealityObjId);
                },
                aftersetformdata: function(asp, record, form) {
                    var isYearGte2015 = false;
                    try {
                        var yearFromParams = parseInt(asp.controller.params.year.substring(0, 4));
                        if (yearFromParams >= 2015) {
                            isYearGte2015 = true;
                        }
                    } catch (e) {
                    }
                    // 1 - KindCommomFacilities
                    // Наименование общего имущества - 2015 и более
                    // Вид общего имущества - 2014 и менее
                    var kindCommomFacilitiesComponent = form.down("textfield[name=KindCommomFacilities]");
                    if (kindCommomFacilitiesComponent) {
                        var textForKindCommomFacilitiesComponent;
                        if (isYearGte2015) {
                            textForKindCommomFacilitiesComponent = "Наименование общего имущества:";
                        } else {
                            textForKindCommomFacilitiesComponent = "Вид общего имущества:";
                        }
                        kindCommomFacilitiesComponent.labelEl.update(textForKindCommomFacilitiesComponent);
                    }

                    // 2 - AppointmentCommonFacilities
                    // 2015 и более - показывать
                    // 2014 и менее - не показывать
                    asp.controller.setVisibleByParam(form.down("textfield[name=AppointmentCommonFacilities]"), isYearGte2015);

                    // 3 - AreaOfCommonFacilities
                    // 2015 и более - показывать
                    // 2014 и менее - не показывать
                    asp.controller.setVisibleByParam(form.down("numberfield[name=AreaOfCommonFacilities]"), isYearGte2015);

                    // 4 - ContractNumber
                    // 2015 и более - показывать
                    // 2014 и менее - не показывать
                    asp.controller.setVisibleByParam(form.down("textfield[name=ContractNumber]"), isYearGte2015);

                    // 5 - ContractNumber
                    // 2015 и более - показывать
                    // 2014 и менее - не показывать
                    asp.controller.setVisibleByParam(form.down("datefield[name=ContractDate]"), isYearGte2015);

                    // 6 - CostByContractInMonth
                    // 2015 и более - показывать
                    // 2014 и менее - не показывать
                    asp.controller.setVisibleByParam(form.down("numberfield[name=CostByContractInMonth]"), isYearGte2015);

                    // 7 - form height
                    // 2015 и более - 540
                    // 2014 и менее - 340
                    if (isYearGte2015) {
                        form.setSize(form.width, form.height + 200);
                    } else {
                        form.setSize(form.width, form.height);
                    }

                    this.onChangeCombo(form.down('b4enumcombo[name="LesseeType"]'));
                    this.onChangeIsLastDayMonthPeriodIn(form.down('checkboxfield[name="IsLastDayMonthPeriodIn"]'));
                    this.onChangeIsLastDayMonthPeriodOut(form.down('checkboxfield[name="IsLastDayMonthPeriodOut"]'));

                    // Скрывать/показывать Период внесения платы по договору
                    var isPeriodOfMakingPayments = true;
                    if (form.down("numberfield[name=DayMonthPeriodIn]").hidden &&
                        form.down("checkboxfield[name=IsLastDayMonthPeriodIn]").hidden &&
                        form.down("checkboxfield[name=IsNextMonthPeriodIn]").hidden &&
                        form.down("numberfield[name=DayMonthPeriodOut]").hidden &&
                        form.down("checkboxfield[name=IsLastDayMonthPeriodOut]").hidden &&
                        form.down("checkboxfield[name=IsNextMonthPeriodOut]").hidden)
                    {
                        isPeriodOfMakingPayments = false;
                    }
                    // Увелить высоту окна или скрыть группу
                    if (isPeriodOfMakingPayments)
                    {
                        form.setSize(form.width, form.height + 90);
                    }
                    else
                    {
                        form.down('fieldset[name="PeriodOfMakingPayments"]').hide();
                    }
                }
            }
        }
    ],

    init: function () {
        this.getStore('InfoAboutUseCommonFacilities').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('infoAboutUseCommonFacilitiesEditPanelAspect').setData(this.params.disclosureInfoRealityObjId);
            var me = this;
            me.params.getId = function () { return me.params.disclosureInfoId; };
            this.getAspect('infoAboutUseCommonFacilitiesPermissionAspect').setPermissionsByRecord(this.params);
            me.getStore('InfoAboutUseCommonFacilities').load();
            
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
        }
    },

    setVisibleByParam: function (component, isVisible) {
        if (component) {
            component.setVisible(isVisible);
        }
    }
});
