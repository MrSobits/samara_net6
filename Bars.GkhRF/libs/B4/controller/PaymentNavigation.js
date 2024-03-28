Ext.define('B4.controller.PaymentNavigation', {
    /*
    * Контроллер взаимодействия формы редактирования с навигационной панелью
    */
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GkhNavigationPanel',
        'B4.aspects.GkhManyGridEditWindow'],

    models: [
        'payment.Item',
        'Payment'
    ],

    stores: ['payment.NavigationMenu',
        'payment.HireRegFund',
        'payment.Cr',
        'payment.Cr185',
        'payment.BuildingCurrentRepair',
        'payment.SanitaryEngineeringRepair',
        'payment.HeatingRepair',
        'payment.ElectricalRepair',
        'payment.BuildingRepair'
    ],

    views: [
        'payment.ItemEditWindow',
        'payment.NavigationPanel',
        'payment.ItemGrid',
        'payment.Grid'
    ],

    aspects: [
        {
            /**
            * Аспект панели навигации с переопределенным методом onMenuItemClick
            */
            xtype: 'gkhnavigationpanelaspect',
            name: 'paymentNavigationAspect',
            panelSelector: '#paymentNavigationPanel',
            treeSelector: '#paymentMenuTree',
            tabSelector: '#paymentMainTab',
            storeName: 'payment.NavigationMenu',
            onMenuItemClick: function(view, record) {
                //полученные параметры дополняем параметром перечисления, переопределяем метод создания вкладок
                var me = this;
                var params = record.get('options');
                me.controller.params.Id = me.controller.params.get('Id');
                me.controller.params.typePayment = params.type;

                me.controller.tabCreate(me.tabSelector, params.type, params.title);
            }
        },
        {
            /**
            * Аспект взаимодействия таблицы и формы редактирования.
            * Имеем одно представление для грида и одно представление для формы редактирования
            * В зависимости от типа записей отображаем их в соответствующей таб панели
            */
            xtype: 'gkhmanygrideditwindow',
            name: 'paymentItemGkhManyGridEditWindowAspect',
            gridSelectors: [
                '#paymentItemGridCr',
                '#paymentItemGridHireRegFund',
                '#paymentItemGridCr185',
                '#paymentItemGridBuildingCurrentRepair',
                '#paymentItemGridSanitaryEngineeringRepair',
                '#paymentItemGridHeatingRepair',
                '#paymentItemGridElectricalRepair',
                '#paymentItemGridBuildingRepair'
            ],
            editFormSelector: '#paymentItemEditWindow',
            modelName: 'payment.Item',
            editWindowView: 'payment.ItemEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (this.controller.params && !record.get('Id')) {
                        record.set('Payment', this.controller.params.get('Id'));
                        record.set('TypePayment', asp.typePayment);
                    }
                }
            }
        }
    ],

    refs: [
        { ref: 'menuTree', selector: '#paymentMenuTree' },
        { ref: 'infoLabel', selector: '#paymentInfoLabel' },
        { ref: 'mainTab', selector: '#paymentMainTab' },
        { ref: 'grid', selector: '#paymentGrid' }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'payment.NavigationPanel',
    mainViewSelector: '#paymentNavigationPanel',

    typePayment: null,
    params: null,
    title: 'Сведения об оплате КР',

    init: function() {
        this.getStore('payment.Cr').on('beforeload', this.onBeforeLoad, this);
        this.getStore('payment.HireRegFund').on('beforeload', this.onBeforeLoad, this);
        this.getStore('payment.Cr185').on('beforeload', this.onBeforeLoad, this);
        this.getStore('payment.BuildingCurrentRepair').on('beforeload', this.onBeforeLoad, this);
        this.getStore('payment.SanitaryEngineeringRepair').on('beforeload', this.onBeforeLoad, this);
        this.getStore('payment.HeatingRepair').on('beforeload', this.onBeforeLoad, this);
        this.getStore('payment.ElectricalRepair').on('beforeload', this.onBeforeLoad, this);
        this.getStore('payment.BuildingRepair').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        var mainView = this.getMainComponent();
        if (mainView)
            mainView.setTitle(this.title);

        var label = this.getInfoLabel();
        if (label) {
            if (this.params.get('RealityObject'))
                label.update({ text: this.params.get('RealityObject').Address });
            else
                label.update({ text: this.params.get('Address') });
        }

        this.getAspect('paymentNavigationAspect').reload();
    },

    /**
    * Метод ручного создания таба
    */
    tabCreate: function(containerSelector, type, title) {
        if (type) {
            var container = Ext.ComponentQuery.query(containerSelector)[0];
            var component = Ext.create('B4.view.payment.ItemGrid', {
                store: 'payment.' + type,
                title: title,
                itemId: 'paymentItemGrid' + type
            });
            component.store.load();
            if (!container.items.containsKey(component.itemId)) {
                container.add(component);
            }
            container.setActiveTab(component);
            container.doLayout();
        }
    },

    onBeforeLoad: function(store, operation) {
        if (this.params) {
            operation.params.paymentId = this.params.getId();
            operation.params.typePayment = store.typePayment;
        }
    }
});