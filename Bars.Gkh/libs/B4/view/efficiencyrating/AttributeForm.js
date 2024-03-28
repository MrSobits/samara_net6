Ext.define('B4.view.efficiencyrating.AttributeForm',
{
    extend: 'Ext.form.Panel',
    alias: 'widget.efAttributeFormPanel',

    requires: [
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.TreeSelectField',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.efficiencyrating.DataValueType'
    ],

    title: 'Настройка атрибута',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: '5',
    layout: 'form',

    COEFFICIENT_FORM: 1,
    ATTRIBUTE_FORM: 2,

    initComponent: function() {
        var me = this;

        Ext.apply(me,
        {
            items: [
                {
                    xtype: 'form',
                    flex: 1,
                    bodyStyle: Gkh.bodyStyle,
                    border: null,
                    layout: { type: 'vbox', align: 'stretch' },

                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },

                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Наименование',
                            name: 'Name',
                            allowBlank: false

                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Код атрибута',
                            name: 'Code',
                            allowBlank: false,
                            regex: /^[A-ZА-Я]{1,3}[a-zа-я0-9]{0,4}$/,
                            regexText:
                                'Формат ввода код: AAAxxxx, где AAA - русские или латинские буквы, хххx - русские или латинские буквы, числа'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Код коэффициента',
                            name: 'CodeCoef',
                            readOnly: true
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            fieldLabel: 'Вес коэффициента',
                            name: 'Weight',
                            minValue: 0,
                            negativeText: 'Значение не может быть отрицательным',
                            allowBlank: false
                        },
                        {
                            xtype: 'textarea',
                            fieldLabel: 'Формула расчета',
                            name: 'Formula'
                        },
                        {
                            xtype: 'b4combobox',
                            fieldLabel: 'Тип значения',
                            name: 'DataValueType',
                            editable: false,
                            displayField: 'Display',
                            valueField: 'Value',
                            allowBlank: false,

                            /* Если будет такое, что будем использовать все типы, то просто убрать это отсюда */
                            items: Ext.Array.filter(B4.enums.efficiencyrating.DataValueType.getItems(),
                                function(element) {
                                    var avaliableEnums = [
                                        B4.enums.efficiencyrating.DataValueType.Number,
                                        B4.enums.efficiencyrating.DataValueType.Dictionary
                                    ];

                                    return avaliableEnums.indexOf(element[0]) >= 0;
                                }),

                            listeners: {
                                'change': {
                                    fn: function(cbox, newVal, oldVal) {
                                        var me = this,
                                            systemTreeSf = me.down('treeselectfield[name=DataFillerName]'),
                                            isDictionary = newVal === B4.enums.efficiencyrating.DataValueType.Dictionary;

                                        systemTreeSf.setVisible(isDictionary);
                                        systemTreeSf.allowBlank = !isDictionary;

                                        if (isDictionary) {
                                            systemTreeSf.isValid();
                                        } else {
                                            systemTreeSf.setValue(null);
                                        }
                                    },
                                    scope: me
                                }
                            }
                        },
                        {
                            xtype: 'treeselectfield',
                            fieldLabel: 'Системное значение',
                            name: 'DataFillerName',
                            textProperty: 'Name',
                            idProperty: 'Code',
                            store: 'B4.store.metavalueconstructor.DataFillerTree',
                            allowBlank: false,
                            hidden: true,
                            titleWindow: 'Системное значение',
                            windowHeight: 550,

                            columns: [
                                {
                                    xtype: 'treecolumn',
                                    dataIndex: 'Name',
                                    editor: 'textfield',
                                    text: 'Наименование',
                                    flex: 1,
                                    menuDisabled: true,
                                    sortable: false,
                                    draggable: false
                                }
                            ],

                            // хз зачем во вьюхе так назвали хендлер, ну ок
                            onSearchWork: function(t, e) {
                                var me = this,
                                    value = me.selectWindow.down('textfield[name="tfSearch"]').getValue();

                                if (e.keyCode === 13 && value) {
                                    me.filterBy(value, me.textProperty);
                                }
                            },

                            filterBy: function(text, by) {
                                var me = this,
                                    treeView = me.getPanel().getView(),
                                    treePanel = me.getPanel(),
                                    nodesAndParents = [];

                                me.clearFilter();

                                // Find the nodes which match the search term, expand them.
                                // Then add them and their parents to nodesAndParents.
                                me.getPanel().getRootNode().cascadeBy(function() {
                                        var currNode = this;
                                        if (currNode.data[by] &&
                                            currNode.data[by].toString().toLowerCase().indexOf(text.toLowerCase()) >
                                            -1) {
                                            treePanel.expandPath(currNode.getPath());

                                            if (currNode.hasChildNodes()) {
                                                currNode.eachChild(function(child) {
                                                    nodesAndParents.push(child.id);
                                                });
                                            }
                                            while (currNode.parentNode) {
                                                nodesAndParents.push(currNode.id);
                                                currNode = currNode.parentNode;
                                            }
                                        }
                                    }, null, [treePanel, treeView]);

                                // Hide all of the nodes which aren't in nodesAndParents
                                me.getPanel().getRootNode().cascadeBy(function(tree, view) {
                                            var uiNode = view.getNodeByRecord(this);
                                            if (uiNode && !Ext.Array.contains(nodesAndParents, this.id)) {
                                                Ext.get(uiNode).setDisplayed('none');
                                            }
                                    }, null, [treePanel, treeView]);
                            },

                            clearFilter: function() {
                                var me = this,
                                    treeView = me.getPanel().getView();
                                me.getPanel().collapseAll();
                                me.getPanel()
                                    .getRootNode()
                                    .cascadeBy(function(tree, view) {
                                            var uiNode = view.getNodeByRecord(this);
                                            if (uiNode) {
                                                Ext.get(uiNode).setDisplayed('table-row');
                                            }
                                        },
                                        null,
                                        [me, treeView]);

                                me.getPanel().expandAll();
                            },

                            getPanel: function() {
                                return this.selectWindow.down('treepanel');
                            },

                            onUpdateClick: function () {
                                var me = this;
                                me.fireEvent('beforeshowselectwindow', me, me.selectWindow);
                            }
                        },
                        {
                            xtype: 'numberfield',
                            fieldLabel: 'Максимальная длина',
                            name: 'MaxLength',
                            minValue: 0,
                            hidden: true
                        },
                        {
                            xtype: 'numberfield',
                            fieldLabel: 'Минимальная длина',
                            name: 'MinLength',
                            minValue: 0,
                            hidden: true
                        },
                        {
                            xtype: 'numberfield',
                            fieldLabel: 'Знаков после запятой',
                            name: 'Decimals',
                            minValue: 0
                        },
                        {
                            xtype: 'checkboxfield',
                            fieldLabel: 'Обязательное',
                            name: 'Required'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                }
            ],

            setFormSettings: function(level) {
                var isAttribute = level === me.ATTRIBUTE_FORM,
                    tfCode = me.down('textfield[name=Code]'),
                    tfDataFillerName = me.down('textfield[name=DataFillerName]'),
                    tfCodeCoef = me.down('textfield[name=CodeCoef]'),
                    tfFormula = me.down('textfield[name=Formula]'),
                    tfWeight = me.down('textfield[name=Weight]'),
                    tfDataValueType= me.down('textfield[name=DataValueType]'),
                    cbTypes = me.down('b4combobox[name=DataValueType]');

                tfCodeCoef.setVisible(isAttribute);

                if (tfDataValueType.getValue() !== B4.enums.efficiencyrating.DataValueType.Dictionary) {
                    tfDataFillerName.hide();
                }

                tfFormula.setVisible(!isAttribute);

                tfWeight.setVisible(!isAttribute);
                tfWeight.allowBlank = isAttribute;

                if (isAttribute) {
                    cbTypes.getStore().clearFilter();
                    tfCode.setFieldLabel('Код атрибута');
                } else {
                    cbTypes.getStore().filter('Value', 1);
                    tfCode.setFieldLabel('Код коэффициента');
                }

            }
        });

        me.callParent(arguments);
    }
});