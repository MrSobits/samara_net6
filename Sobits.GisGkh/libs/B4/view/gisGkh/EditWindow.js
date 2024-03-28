Ext.define('B4.view.gisGkh.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.gisGkh.TaskGridStore',
        'B4.store.dict.PlanJurPersonGji',
        'B4.view.gisGkh.TaskGrid',
        'B4.enums.GisGkhTypeRequest',
        'B4.enums.GisGkhPlanType',
        'B4.enums.YesNo',
        'B4.enums.GisGkhListGroup',
        //'B4.enums.exportDataProviderNsiItemRequestRegistryNumber',
        'B4.enums.GisGkhDataProviderNsiType',
        'Ext.ux.CheckColumn'
        //'B4.enums.ContragentForGisGkhExportStore'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'gisGkhEditWindow',
    title: 'Информация о задаче в ГИС ЖКХ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            xtype: 'container',
            layout: 'vbox',
            defaults: {
                margin: '0',
                labelWidth: 140,
                labelAlign: 'right',
                //readOnly: true,
            },
            items: [
                //{
                //    xtype: 'textfield',
                //    name: 'MessageGUID',
                //    itemId: 'dfMessageGUID',
                //    fieldLabel: 'MessageGUID',
                //    allowBlank: true,
                //    disabled: true,
                //    flex: 1,
                //    maxLength: 40,
                //    editable: false,
                //},
                //{
                //    xtype: 'textfield',
                //    name: 'RequesterMessageGUID',
                //    itemId: 'dfRequesterMessageGUID',
                //    fieldLabel: 'RequesterMessageGUID',
                //    allowBlank: true,
                //    disabled: false,
                //    flex: 1,
                //    maxLength: 40,
                //    editable: false,
                //},
                {
                    xtype: 'combobox',
                    name: 'TypeRequest',
                    fieldLabel: 'Тип запроса',
                    displayField: 'Display',
                    itemId: 'dfTypeRequest',
                    flex: 1,
                    store: B4.enums.GisGkhTypeRequest.getStore(),
                    valueField: 'Value',
                    allowBlank: false,
                    editable: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'RealityObject',
                            fieldLabel: 'Дом',
                            store: 'B4.store.gisGkh.ROForGisGkhExportStore',
                            editable: false,
                            flex: 1,
                            itemId: 'dfRealityObject',
                            textProperty: 'Address',
                            idProperty: 'Id',
                            allowBlank: true,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right',
                            width: 800,
                            columns: [
                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Жилых помещений', dataIndex: 'NumberApartments', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                                //{ text: 'ФИАС GUID', dataIndex: 'HouseGuid', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Жилых сопоставлено', dataIndex: 'NumberGisGkhMatchedApartments', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                                { text: 'Нежилых помещений', dataIndex: 'NumberNonResidentialPremises', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                                { text: 'Нежилых сопоставлено', dataIndex: 'NumberGisGkhMatchedNonResidental', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } },
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'ListGroup',
                            fieldLabel: 'Тип',
                            displayField: 'Display',
                            itemId: 'dfListGroup',
                            flex: 1,
                            labelWidth: 120,
                            store: B4.enums.GisGkhListGroup.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'numberfield',
                            name: 'RegNumber',
                            fieldLabel: 'Реестровый номер',
                            editable: true,
                            flex: 1,
                            itemId: 'dfRegNumber',
                            allowBlank: false,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'DataProviderNsiItem',
                            fieldLabel: 'Реестровый номер справочника поставщика',
                            displayField: 'Display',
                            itemId: 'dfDataProviderNsiItem',
                            flex: 1,
                            labelWidth: 120,
                            //store: B4.enums.exportDataProviderNsiItemRequestRegistryNumber.getStore(),
                            store: B4.enums.GisGkhDataProviderNsiType.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        },
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'PlanType',
                            fieldLabel: 'КПР / МАПКР / РАПКР',
                            displayField: 'Display',
                            itemId: 'dfPlanType',
                            flex: 1,
                            labelWidth: 120,
                            store: B4.enums.GisGkhPlanType.getStore(),
                            valueField: 'Value',
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Municipality',
                            fieldLabel: 'Муниципальное образование',
                            store: 'B4.store.gisGkh.MOForGisGkhExportStore',
                            editable: false,
                            flex: 1,
                            itemId: 'dfMunicipality',
                            textProperty: 'Name',
                            idProperty: 'Oktmo',
                            allowBlank: true,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right',
                            width: 800,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'ОКТМО', dataIndex: 'Oktmo', flex: 1, filter: { xtype: 'textfield' } },
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            allowBlank: false,
                            name: 'DateFrom',
                            fieldLabel: 'Дата, с',
                            itemId: 'dfDateFrom',
                            flex: 1,
                        },
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            allowBlank: false,
                            name: 'DateTo',
                            fieldLabel: 'Дата, по',
                            itemId: 'dfDateTo',
                            value: new Date(),
                            flex: 1,
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Program',
                            fieldLabel: 'Региональная программа',
                            store: 'B4.store.gisGkh.ProgramForGisGkhExportStore',
                            editable: false,
                            flex: 1,
                            itemId: 'dfProgram',
                            textProperty: 'Name',
                            idProperty: 'GisGkhGuid',
                            allowBlank: true,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right',
                            width: 800,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                //{ text: 'ОКТМО', dataIndex: 'Oktmo', flex: 1, filter: { xtype: 'textfield' } },
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Examination',
                            fieldLabel: 'Проверка (распоряжение)',
                            store: 'B4.store.gisGkh.DisposalForGisGkhStore',
                            editable: false,
                            flex: 1,
                            itemId: 'dfExamination',
                            textProperty: 'DocumentNumber',
                            idProperty: 'Id',
                            allowBlank: true,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right',
                            width: 800,
                            selectionMode: 'MULTI',
                            columns: [
                                { text: 'Номер', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                                { xtype: 'datecolumn', text: 'Дата', dataIndex: 'DocumentDate', format: 'd.m.Y', flex: 1, filter: { xtype: 'datefield' } },
                                { xtype: 'b4enumcolumn', text: 'Состояние выгрузки', dataIndex: 'State', flex: 1, enumName: 'B4.enums.GisGkhExaminationState', filter: true }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'ExaminationPlan',
                            fieldLabel: 'План проверок',
                            store: 'B4.store.dict.PlanJurPersonGji',
                            editable: false,
                            flex: 1,
                            itemId: 'dfExaminationPlan',
                            textProperty: 'Name',
                            idProperty: 'Id',
                            allowBlank: true,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right',
                            width: 800,
                            selectionMode: 'MULTI',
                            columns: [
                                { text: 'Название', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                //{ xtype: 'b4enumcolumn', text: 'Состояние выгрузки', dataIndex: 'State', flex: 1, enumName: 'B4.enums.GisGkhExaminationState', filter: true }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Resolution',
                            fieldLabel: 'Постановление',
                            store: 'B4.store.gisGkh.ResolutionForGisGkhStore',
                            editable: false,
                            flex: 1,
                            itemId: 'dfResolution',
                            textProperty: 'DocumentNumber',
                            idProperty: 'Id',
                            allowBlank: true,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right',
                            width: 800,
                            selectionMode: 'MULTI',
                            columns: [
                                { text: 'Номер', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                                { xtype: 'datecolumn', format: 'd.m.Y', text: 'Дата', dataIndex: 'DocumentDate', flex: 1, filter: { xtype: 'datefield' } }
                                //{ xtype: 'b4enumcolumn', text: 'Состояние выгрузки', dataIndex: 'State', flex: 1, enumName: 'B4.enums.GisGkhExaminationState', filter: true }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Plan',
                            fieldLabel: 'КПР',
                            store: 'B4.store.dict.ProgramCr',
                            editable: false,
                            flex: 1,
                            itemId: 'dfPlan',
                            textProperty: 'Name',
                            idProperty: 'Id',
                            allowBlank: true,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right',
                            width: 800,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Период', dataIndex: 'PeriodName', flex: 1, filter: { xtype: 'textfield' } },
                                { xtype: 'b4enumcolumn', text: 'Видимость', dataIndex: 'TypeVisibilityProgramCr', flex: 1, enumName: 'B4.enums.TypeVisibilityProgramCr', filter: true },
                                { xtype: 'b4enumcolumn', text: 'Состояние', dataIndex: 'TypeProgramStateCr', flex: 1, enumName: 'B4.enums.TypeProgramStateCr', filter: true },
                                //{ text: 'ОКТМО', dataIndex: 'Oktmo', flex: 1, filter: { xtype: 'textfield' } },
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'PlanForContract',
                    fieldLabel: 'КПР',
                    store: 'B4.store.gisGkh.ProgramImportStore',
                    editable: false,
                    flex: 1,
                    itemId: 'dfPlanForContract',
                    textProperty: 'Name',
                    idProperty: 'Id',
                    allowBlank: false,
                    readOnly: false,
                    editable: false,
                    labelWidth: 120,
                    labelAlign: 'right',
                    width: 800,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Период', dataIndex: 'PeriodName', flex: 1, filter: { xtype: 'textfield' } },
                        { xtype: 'b4enumcolumn', text: 'Видимость', dataIndex: 'TypeVisibilityProgramCr', flex: 1, enumName: 'B4.enums.TypeVisibilityProgramCr', filter: true },
                        { xtype: 'b4enumcolumn', text: 'Состояние', dataIndex: 'TypeProgramStateCr', flex: 1, enumName: 'B4.enums.TypeProgramStateCr', filter: true }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ObjectCr',
                    fieldLabel: 'Дом',
                    store: 'B4.store.gisGkh.ObjectCrImportStore',
                    editable: false,
                    flex: 1,
                    itemId: 'dfObjectCr',
                    textProperty: 'Address',
                    idProperty: 'Id',
                    allowBlank: false,
                    readOnly: false,
                    editable: false,
                    labelWidth: 120,
                    labelAlign: 'right',
                    width: 800,
                    columns: [
                        { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contract',
                    fieldLabel: 'Договор',
                    store: 'B4.store.gisGkh.BuildContractImportStore',
                    editable: false,
                    flex: 1,
                    itemId: 'dfBuildContract',
                    textProperty: 'DocumentNum',
                    idProperty: 'Id',
                    allowBlank: false,
                    readOnly: false,
                    editable: false,
                    labelWidth: 120,
                    labelAlign: 'right',
                    width: 800,
                    columns: [
                        { text: 'Номер', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } },
                        { xtype: 'datecolumn', text: 'Договор от', dataIndex: 'DocumentDateFrom', format: 'd.m.Y', flex: 1, filter: { xtype: 'datefield' } },
                        { text: 'Сумма', dataIndex: 'Sum', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'BuildContractForAct',
                    fieldLabel: 'Договор',
                    store: 'B4.store.gisGkh.BuildContractForActImportStore',
                    editable: false,
                    flex: 1,
                    itemId: 'dfBuildContractForAct',
                    textProperty: 'DocumentNum',
                    idProperty: 'Id',
                    allowBlank: false,
                    readOnly: false,
                    editable: false,
                    labelWidth: 120,
                    labelAlign: 'right',
                    width: 800,
                    columns: [
                        { text: 'Номер', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } },
                        { xtype: 'datecolumn', text: 'Договор от', dataIndex: 'DocumentDateFrom', format: 'd.m.Y', flex: 1, filter: { xtype: 'datefield' } },
                        { text: 'Сумма', dataIndex: 'Sum', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'PerfWorkAct',
                    fieldLabel: 'Акт выполненных работ',
                    store: 'B4.store.gisGkh.PerfWorkActImportStore',
                    editable: false,
                    flex: 1,
                    itemId: 'dfPerfWorkAct',
                    textProperty: 'DocumentNum',
                    idProperty: 'Id',
                    allowBlank: false,
                    readOnly: false,
                    editable: false,
                    labelWidth: 120,
                    labelAlign: 'right',
                    width: 800,
                    columns: [
                        { text: 'Номер', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } },
                        { xtype: 'datecolumn', text: 'Акт от', dataIndex: 'DateFrom', format: 'd.m.Y', flex: 1, filter: { xtype: 'datefield' } },
                        { text: 'Сумма', dataIndex: 'Sum', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                //{
                //    xtype: 'container',
                //    layout: 'hbox',
                //    anchor: '100%',
                //    defaults: {
                //        margin: '0',
                //        labelWidth: 120,
                //        labelAlign: 'right',
                //    },
                //    items: [
                //        {
                //            xtype: 'b4selectfield',
                //            name: 'Plan',
                //            fieldLabel: 'КПР',
                //            store: 'B4.store.dict.Period',
                //            editable: false,
                //            flex: 1,
                //            itemId: 'dfPlan',
                //            textProperty: 'Name',
                //            idProperty: 'Id',
                //            allowBlank: true,
                //            readOnly: false,
                //            labelWidth: 120,
                //            labelAlign: 'right',
                //            width: 800,
                //            columns: [
                //                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                //                { text: 'Начало', dataIndex: 'DateStart', flex: 1, filter: { xtype: 'textfield' } },
                //                { text: 'Окончание', dataIndex: 'DateEnd', flex: 1, filter: { xtype: 'textfield' } }
                //            ]
                //        }
                //    ]
                //},
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Plan',
                            fieldLabel: 'Период',
                            store: 'B4.store.regop.ChargePeriod',
                            editable: false,
                            flex: 1,
                            itemId: 'dfChargePeriod',
                            textProperty: 'Name',
                            idProperty: 'Id',
                            allowBlank: true,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right',
                            width: 800,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Начало', dataIndex: 'StartDate', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Окончание', dataIndex: 'EndDate', flex: 1, filter: { xtype: 'textfield' } },
                                {
                                    xtype: 'booleancolumn', text: 'Период закрыт', dataIndex: 'IsClosed', flex: 1, trueText: 'Закрыт',
                                    falseText: 'Открыт',
                                    listeners: {
                                        beforecheckchange: function () {
                                            return false; // HERE
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'Rewrite',
                            fieldLabel: 'С изменением уже выгруженных данных',
                            flex: 1,
                            itemId: 'cbRewrite',
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Contragent',
                            fieldLabel: 'Организация',
                            store: 'B4.store.gisGkh.ContragentForGisGkhExportStore',
                            editable: false,
                            flex: 1,
                            itemId: 'dfContragent',
                            textProperty: 'Name',
                            idProperty: 'Id',
                            allowBlank: true,
                            readOnly: false,
                            labelWidth: 120,
                            labelAlign: 'right',
                            width: 800,
                            columns: [
                                { text: 'Организация', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                //{ text: 'ОКТМО', dataIndex: 'Oktmo', flex: 1, filter: { xtype: 'textfield' } },
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    anchor: '100%',
                    defaults: {
                        margin: '0',
                        labelWidth: 120,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'Page',
                            fieldLabel: 'Номер страницы',
                            flex: 1,
                            itemId: 'nfPage',
                            allowBlank: false,
                            value: 1,
                            allowDecimals: false
                        }
                    ]
                },
                //{
                //    xtype: 'container',
                //    layout: 'hbox',
                //    defaults: {
                //        xtype: 'combobox',
                //        //     margin: '10 0 5 0',
                //        labelWidth: 90,
                //        labelAlign: 'right',
                //    },
                //    items: [
                //        {
                //            xtype: 'datefield',
                //            format: 'd.m.Y',
                //            allowBlank: false,
                //            name: 'StartDate',
                //            fieldLabel: 'Дата, с',
                //            itemId: 'dfStartDate',
                //            flex: 1,
                //        },
                //        {
                //            xtype: 'datefield',
                //            format: 'd.m.Y',
                //            allowBlank: false,
                //            name: 'EndDate',
                //            fieldLabel: 'Дата, по',
                //            itemId: 'dfEndDate',
                //            value: new Date(),
                //            flex: 1,
                //        }
                //    ]
                //}
                //{
                //    xtype: 'container',
                //    layout: 'hbox',
                //    anchor: '100%',
                //    defaults: {
                //        margin: '0',
                //        labelWidth: 120,
                //        labelAlign: 'right',
                //    },
                //    items: [
                //        {
                //            xtype: 'b4selectfield',
                //            name: 'Plan',
                //            fieldLabel: 'КПР',
                //            store: 'B4.store.dict.ProgramCr',
                //            editable: false,
                //            flex: 1,
                //            itemId: 'dfPlan',
                //            textProperty: 'Name',
                //            idProperty: 'Id',
                //            allowBlank: true,
                //            readOnly: false,
                //            labelWidth: 120,
                //            labelAlign: 'right',
                //            width: 800,
                //            columns: [
                //                { text: 'Название программы', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                //                { text: 'Период', dataIndex: 'PeriodName', flex: 1, filter: { xtype: 'textfield' } }
                //            ]
                //        }
                //    ]
                //}
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [

                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        },
                        //{
                        //    xtype: 'buttongroup',
                        //    columns: 2,
                        //    items: [
                        //        {
                        //            xtype: 'button',
                        //            text: 'Синхронизация справочников',
                        //            tooltip: 'Синхронизация справочников',
                        //            iconCls: 'icon-accept',
                        //            width: 170,
                        //            itemId: 'btnGetDictionaries'
                        //        }
                        //    ]
                        //},
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});