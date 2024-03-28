Ext.define('B4.controller.report.ProtocolResponsibility_2', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ProtocolResponsibility_2Panel',
    mainViewSelector: '#protocolResponsibility_2Panel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#protocolResponsibility_2Panel #tfMunicipality'
        },
        {
            ref: 'DateStartField',
            selector: '#protocolResponsibility_2Panel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#protocolResponsibility_2Panel #dfDateEnd'
        },
        {
            ref: 'CanceledField',
            selector: '#protocolResponsibility_2Panel #cbCanceled'
        },
        {
            ref: 'ReturnedField',
            selector: '#protocolResponsibility_2Panel #cbReturned'
        },
        {
            ref: 'RemarkedField',
            selector: '#protocolResponsibility_2Panel #cbRemarked'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'protocolResponsibility_2PanelMultiselectwindowaspect',
            fieldSelector: '#protocolResponsibility_2Panel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocolResponsibility_2PanelMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
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
                { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    validateParams: function () {
        var muId = this.getMunicipalityTriggerField();
        var muIdNotSet = muId && muId.isValid();
        
        var dateSt = this.getDateStartField();
        var dateStNotSet = dateSt && dateSt.isValid();
        
        var dateEnd = this.getDateEndField();
        var dateEndNotSet = dateEnd && dateEnd.isValid();
        
        var canc = this.getCanceledField();
        var cancNotSet = canc && canc.isValid();

        var ret = this.getReturnedField();
        var retNotSet = ret && ret.isValid();

        var rem = this.getRemarkedField();
        var remNotSet = rem && rem.isValid();

        return (muIdNotSet && dateStNotSet && dateEndNotSet && cancNotSet && retNotSet && remNotSet);
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();
        var canceled = this.getCanceledField();
        var returned = this.getReturnedField();
        var remarked = this.getRemarkedField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null),
            canceled: (canceled ? canceled.getValue() : null),
            returned: (returned ? returned.getValue() : null),
            remarked: (remarked ? remarked.getValue() : null)
        };
    }
});