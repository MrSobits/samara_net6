Ext.define('B4.view.administration.operator.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
         'B4.form.TreeSelectField',
        'B4.form.SelectField',
        'B4.store.Role',
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhTriggerField',
        'B4.enums.TypeWorkplace',
        'B4.store.dict.Inspector',
        'B4.view.dict.inspector.Grid',
        'B4.store.dict.MunicipalityTree'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    bodyPadding: 5,
    itemId: 'operatorEditWindow',
    title: 'Оператор',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            fieldDefaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Login',
                    fieldLabel: 'Логин',
                    allowBlank: false,
                    maxLength: 100
                },
                {
                    xtype: 'textfield',
                    id: 'pass',
                    name: 'Password',
                    fieldLabel: 'Пароль',
                    inputType: 'password',
                    maxLength: 100
                },
                {
                    fieldLabel: 'Подтверждение',
                    xtype: 'textfield',
                    name: 'NewPassword',
                    vtype: 'password',
                    inputType: 'password',
                    initialPassField: 'pass',
                    msgTarget: 'side',
                    autoFitErrors: false,
                    maxLength: 100
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Role',
                    fieldLabel: 'Роль',
                    store: 'B4.store.Role',
                    editable: false,
                    allowBlank: false,
                    blankText: 'Это поле обязательно для заполнения',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
                {
                    // todo refactor, вынести опции в treeselectfield для multiselect
                    xtype: 'treeselectfield',
                    name: 'municipalityInspectors',
                    itemId: 'operatorMunicipalitiesTrigerField',
                    fieldLabel: 'Муниципальные образования',
                    titleWindow: 'Выбор муниципального образования',
                    store: 'B4.store.dict.MunicipalitySelectTree',
                    allowBlank: true,
                    editable: false,
                    onTrigger1Click: function () {
                        // Создаем необходимые контролы - окно и панель с деревом в нем
                        var me = this,
                            store = me.store;

                        if (!store) {
                            store = Ext.StoreMgr.lookup('ext-empty-store');
                        } else if (Ext.isString(store)) {
                            store = Ext.StoreMgr.lookup(store);
                            if (!store) {
                                store = me.store = Ext.create(me.store);
                            }
                            me.store = store;
                        } else {
                            me.store.load();
                        }

                        if (!Ext.isEmpty(store) && Ext.isFunction(store.on)) {
                            store.on('beforeload', me.onStoreBeforeLoad, me);
                        }

                        if (!me.treePanel) {
                            me.treePanel = Ext.create('Ext.tree.Panel', {
                                region: 'west',
                                rootVisible: me.rootVisible,
                                animate: me.animate,
                                autoScroll: me.autoScroll,
                                useArrows: me.useArrows,
                                containerScroll: me.containerScroll,
                                loadMask: me.loadMask,
                                treetype: 'parttree',
                                viewConfig: {
                                    loadMask: true
                                },
                                store: me.store,
                                listeners: {
                                    itemdblclick: me.onSelectItem,
                                    itemappend: { fn: me.onNodeAppend, scope: me },
                                    checkchange: function (node, checked) {
                                        var tree = me.treePanel;

                                        if (checked) {
                                            var checkedNodes = tree.getChecked();

                                        }
                                    }
                                },
                                scope: me
                            }
                            );
                        }

                        if (!me.selectWindow) {
                            me.selectWindow = Ext.create('Ext.window.Window', {
                                layout: 'fit',
                                width: me.windowWidth,
                                height: me.windowHeight,
                                title: me.titleWindow,
                                constrain: true,
                                modal: false,
                                closeAction: 'destroy',
                                renderTo: B4.getBody().getActiveTab().getEl(),
                                items: [
                                    me.treePanel
                                ],
                                tbar: [
                                    {
                                        flex: 1,
                                        xtype: 'textfield',
                                        name: 'tfSearch',
                                        tooltip: 'Найти элемент',
                                        emptyText: 'Поиск',
                                        enableKeyEvents: true,
                                        listeners: {
                                            scope: me,
                                            specialkey: function (f, e) {
                                                if (e.getKey() == e.ENTER) {
                                                    var val = me.selectWindow.down('textfield[name="tfSearch"]').getValue(),
                                                     treeStore = me.treePanel.store;

                                                    treeStore.load({
                                                        params: {
                                                            search: val
                                                        }
                                                    });
                                                }
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'b4updatebutton',
                                        tooltip: 'Обновить',
                                        handler: function () {
                                            var val = me.selectWindow.down('textfield[name="tfSearch"]').getValue(),
                                                treeStore = me.treePanel.store;

                                            treeStore.load({
                                                params: {
                                                    search: val
                                                }
                                            });
                                        },
                                        scope: me
                                    },
                                    {
                                        xtype: 'b4savebutton',
                                        tooltip: 'Выбрать',
                                        text: 'Выбрать',
                                        handler: me.onSelectItem,
                                        scope: me
                                    },
                                    {
                                        xtype: 'b4closebutton',
                                        text: 'Закрыть',
                                        tooltip: 'Закрыть',
                                        handler: me.onSelectWindowClose,
                                        scope: me
                                    }
                                ],
                                listeners: {
                                    close: function () {
                                        delete me.treePanel;
                                        delete me.selectWindow;
                                    },
                                    scope: me
                                }
                            });

                            me.store.sorters.clear();
                        }

                        me.selectWindow.show();
                    },

                    setValue: function (data) {

                        var me = this,
                            oldValue = me.getValue(),
                            isValid = me.getErrors() != '';

                        if (me.store && data) {
                            var array = data.split(',');

                            if (array.length > 0) {

                                if (typeof me.store != 'object') {
                                    me.store = Ext.create(me.store);
                                    me.store.load();
                                }

                                var text = me.store.getById(array[0]);

                                for (var i = 1; i < array.length; i++) {
                                    text += ',' + me.store.getById(array[i]).raw.text;
                                }

                                data = text;
                            }
                        }

                        me.value = data;
                        me.updateDisplayedText(data);

                        me.fireEvent('validitychange', me, isValid);
                        me.fireEvent('change', me, data, oldValue);
                        me.validate();
                        return me;
                    },
                    
                    onNodeAppend: function (asp, node) {

                        var me = this;
                        if (me.value) {

                            var vals = me.value.replace(/ /g,'').split(',');
                            var id = node.raw.id.toString();

                            if (vals.indexOf(id) >= 0) {
                                node.set('checked', true);
                            }
                        }

                    },
                    
                    onSelectItem: function () {

                        var me = this,
                            tree = me.treePanel,
                            selection = tree.getSelectionModel();

                        var checkedNodes = tree.getChecked();

                        if (checkedNodes.length > 0) {
                            if (!tree.getSelectionModel().getSelection()[0]) {
                                return;
                            }

                            var ids = checkedNodes[0].raw.id;
                            var text = checkedNodes[0].raw.Name;

                            for (var i = 1; i < checkedNodes.length; i++) {

                                ids += ',' + checkedNodes[i].raw.id;
                                text += ',' + checkedNodes[i].raw.text;
                            }

                            me.value = ids;
                            me.setRawValue.call(me, text);
                        }

                        me.onSelectWindowClose();
                    }
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'operatorManOrgs',
                    itemId: 'operatorContragentTrigerField',
                    fieldLabel: 'Организации'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'operatorInspectors',
                    itemId: 'operatorInspectorsTrigerField',
                    fieldLabel: 'Инспекторы'
                },
                {
                    xtype: 'checkbox',
                    name: 'ShowUnassigned',
                    fieldLabel: 'Показывать неназначенные обращения'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Inspector',
                    itemId: 'sflInspector',
                    fieldLabel: 'Инспектор этого оператора',
                    textProperty: 'Fio',
                    store: 'B4.store.dict.Inspector',
                    columns: [
                        { text: 'ФИО', dataIndex: 'Fio', flex: 1 },
                        { text: 'Должность', dataIndex: 'Position', flex: 1 }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    itemId: 'sflContragent',
                    fieldLabel: 'Контрагент (для заполнения информации по постановлению 1468)',
                    store: 'B4.store.Contragent',
                    textProperty: 'Name',
                    columns: [
                        {
                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Юридический адрес', dataIndex: 'JuridicalAddress', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Имя пользователя',
                    allowBlank: false,
                    maxLength: 1000
                },
                {
                    xtype: 'textfield',
                    name: 'Email',
                    fieldLabel: 'E-mail',
                    vtype: 'email',
                    maxLength: 250
                },
                {
                    xtype: 'textfield',
                    name: 'Phone',
                    fieldLabel: 'Телефон',
                    maxLength: 50
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Тип рабочего места',
                    store: B4.enums.TypeWorkplace.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeWorkplace',
                    allowBlank: false
                },
                {
                    xtype: 'checkbox',
                    name: 'IsActive',
                    fieldLabel: 'Пользователь активен'
                }
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
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });


        Ext.apply(Ext.form.field.VTypes, {

            password: function (val, field) {
                if (field.initialPassField) {
                    var pwd = Ext.getCmp('pass');
                    return (val == pwd.getValue());
                }
                return true;
            },

            passwordText: 'Пароли не совпадают'
        });

        me.callParent(arguments);
    }


});

