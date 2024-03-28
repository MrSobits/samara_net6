Ext.define('B4.controller.belaypolicy.Risk', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: ['belaypolicy.Risk'],
    stores: [
        'belaypolicy.Risk',
        'dict.KindRiskForSelect',
        'dict.KindRiskForSelected'
    ],
    views: [
        'SelectWindow.MultiSelectWindow',
        'belaypolicy.RiskGrid'
    ],
    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'manorgRealobjGridAspect',
            gridSelector: '#belayPolicyRiskGrid',
            storeName: 'belaypolicy.Risk',
            modelName: 'belaypolicy.Risk',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#kindRiskMultiSelectWindow',
            storeSelect: 'dict.KindRiskForSelect',
            storeSelected: 'dict.KindRiskForSelected',
            titleSelectWindow: 'Выбор видов рисков',
            titleGridSelect: 'Виды риска для отбора',
            titleGridSelected: 'Выбранные виды рисков',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });
                    
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddKindRisk', 'BelayPolicyRisk'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                belayPolicyId: asp.controller.params.get('Id')
                            }
                        }).next(function() {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать виды рисков');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    params: null,
    mainView: 'belaypolicy.RiskGrid',
    mainViewSelector: '#belayPolicyRiskGrid',
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    init: function () {
        this.getStore('belaypolicy.Risk').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('belaypolicy.Risk').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.belayPolicyId = this.params.get('Id');
        }
    }
});