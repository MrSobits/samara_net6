Ext.define('B4.controller.report.MkdChargePaymentReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.MkdChargePaymentReportPanel',
    mainViewSelector: 'mkdchargepaymentreportpanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected' 
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    parentMuTfs: 'mkdchargepaymentreportpanel [name = ParentMu]',
    startDateTfs: 'mkdchargepaymentreportpanel [name = StartDate]',
    endDateTfs: 'mkdchargepaymentreportpanel [name = EndDate]',
    fundTfs: 'mkdchargepaymentreportpanel [name = Fund]',
    hasInDpkr: 'mkdchargepaymentreportpanel [name = HasInDpkr]',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'parentMuMultiselectwindowaspect',
            fieldSelector: 'mkdchargepaymentreportpanel [name = ParentMu]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#mkdchargepaymentreportParentMuSelectWindow',
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
                }
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
        var parentMu = Ext.ComponentQuery.query(this.parentMuTfs)[0];
        var startDate = Ext.ComponentQuery.query(this.startDateTfs)[0];
        var endDate = Ext.ComponentQuery.query(this.endDateTfs)[0];
        
        return (parentMu && parentMu.isValid() && startDate && startDate.isValid() && endDate && endDate.isValid());
        
    },

    getParams: function () {
        var parentMu = Ext.ComponentQuery.query(this.parentMuTfs)[0],
            startDate = Ext.ComponentQuery.query(this.startDateTfs)[0],
            endDate = Ext.ComponentQuery.query(this.endDateTfs)[0],
            fund = Ext.ComponentQuery.query(this.fundTfs)[0],
            hasInDpkr = Ext.ComponentQuery.query(this.hasInDpkr)[0];

        return {
            parentMu: (parentMu ? parentMu.getValue() : null),
            startDate: (startDate ? startDate.getValue() : null),
            endDate: (endDate ? endDate.getValue() : null),
            fund: (fund ? fund.getValue() : null),
            hasInDpkr: (hasInDpkr ? hasInDpkr.getValue() : null)
        };
    }
});