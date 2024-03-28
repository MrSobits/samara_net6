Ext.define('B4.controller.belaypolicy.MkdInclude', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: ['belaypolicy.Mkd'],
    stores: [
        'belaypolicy.MkdInclude',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],
    views: [
        'SelectWindow.MultiSelectWindow',
        'belaypolicy.MkdIncludeGrid'
    ],

    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'manorgRealobjGridAspect',
            gridSelector: '#belayPolicyMkdIncludeGrid',
            storeName: 'belaypolicy.MkdInclude',
            modelName: 'belaypolicy.Mkd',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#belayPolicyMkdIncludeMultiSelectWindow',
            storeSelect: 'realityobj.RealityObjectForSelect',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Жилые дома для отбора',
            titleGridSelected: 'Выбранные жилые дома',
            columnsGridSelect: [
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1 }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.manOrgId = this.controller.manOrgId;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('AddPolicyMkdObjects', 'BelayPolicyMkd'),
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
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать жилые дома');
                        return false;
                    }
                    return true;
                }
            },
            /* переопределен метод deleteRecord. Удаление записи 
            * заменяем сохранением с другим типом TypeCondition
            */
            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Исключение из договора!', 'Вы действительно хотите исключить дом из договора?', function(res) {
                    if (res == 'yes') {
                        record.set('IsExcluded', true);
                        record.save({ Id: record.getId() })
                            .next(function() {
                                Ext.Msg.alert('Исключение из договора!', 'Объект перенесен в исключенные договора');
                                this.controller.getStore('belaypolicy.MkdInclude').load();
                            }, this)
                            .error(function(result) {
                                Ext.Msg.alert('Исключение из договора!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            }, this);
                    }
                }, me);
            }
        }
    ],

    params: null,
    mainView: 'belaypolicy.MkdIncludeGrid',
    mainViewSelector: '#belayPolicyMkdIncludeGrid',
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    manOrgId: 0,

    init: function () {
        var actions = {};
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.getStore('belaypolicy.MkdInclude').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('belaypolicy.MkdInclude').load();
    },

    onMainViewAfterRender: function () {
        if (this.params) {
            var me = this;
            me.mask('Загрузка', me.getMainComponent());

            B4.Ajax.request(B4.Url.action('GetInfo', 'BelayPolicy', {
                belayPolicyId: this.params.get('Id')
            })).next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                me.manOrgId = obj.manOrgId;
                me.unmask();
            }).error(function () {
                me.unmask();
            });
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.belayPolicyId = this.params.get('Id');
            operation.params.isExcluded = false;
        }
    }
});