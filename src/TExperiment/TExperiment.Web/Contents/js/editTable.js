(function (window, $) {
    'use strict';


    function EditTable(element, options) {
        this.init(element, options);
    }

    EditTable.DEFAULTS = {
        'fields': [],
        'data': [],
        'templates': {
            'main': '{{~it.data :d:di}}'
            + '<tr>{{~it.fields :f1}}<td>{{? d._edited }}<input name="{{=f1.name}}" class="form-control input-sm" />{{??}}{{=d[f1.name]}}{{?}}</td>{{~}}<td>'
            + '{{? d._edited }}'
            + '<button type="button" data-index="{{=di}}" class="btn btn-sm btn-success button-save-selector">保存</button>'
            + '<button type="button" data-index="{{=di}}" class="btn btn-sm btn-default button-cancel-selector">取消</button>'
            + '{{??}}<button type="button" data-index="{{=di}}" class="btn btn-sm btn-warning button-edit-selector">编辑</button>'
            + '{{? d._new }}<button type="button" data-index="{{=di}}" class="btn btn-sm btn-danger button-remove-selector">移除</button>{{?}}{{?}}'
            + '</td></tr>'
            + '{{~}}'
            + '<tr>'
            + '{{~it.fields :f2}}'
            + '<td><input name="{{=f2.name}}" class="form-control input-sm" /></td>'
            + '{{~}}'
            + '<td><button type="button" class="btn btn-sm btn-info button-add-selector">新增</button></td>'
            + '</tr>'
        },
        'selectors': {
            'add': '.button-add-selector',
            'edit': '.button-edit-selector',
            'save': '.button-save-selector',
            'cancel': '.button-cancel-selector',
            'remove': '.button-remove-selector'
        }
    };

    EditTable.prototype.getDefaults = function (element, options) {
        return EditTable.DEFAULTS;
    };
    EditTable.prototype.init = function (element, options) {
        this.$element = $(element);
        this.options = $.extend({}, this.getDefaults(), options);
        this.render = doT.template(this.options.templates.main);

        this._bindEvents();
        this._refresh();
    };

    EditTable.prototype._bindEvents = function () {
        var s = this;
        var o = this.options;
        var $e = this.$element;

        $e.on('click', o.selectors.add, function () {
            o.data.push({ '_new': true });
            s._refresh();
        });
        $e.on('click', o.selectors.edit, function () {
            var i = parseInt($(this).attr('data-index'));
            o.data[i]._edited = true;
            s._refresh();
        });
        $e.on('click', o.selectors.save, function () {
            var i = parseInt($(this).attr('data-index'));
            o.data[i]._edited = false;
            s._refresh();
        });
        $e.on('click', o.selectors.cancel, function () {
            var i = parseInt($(this).attr('data-index'));
            o.data[i]._edited = false;
            s._refresh();
        });
        $e.on('click', o.selectors.remove, function () {
            var i = parseInt($(this).attr('data-index'));
            o.data.splice(i, 1);
            s._refresh();
        });
    };
    EditTable.prototype._getContentContainer = function () {
        var $e = this.$element;

        var $c = $e.find('tbody');
        if ($c.length === 0) {
            $e.append('<tbody></tbody>');
            $c = $e.find('tbody');
        }

        return $c;
    };
    EditTable.prototype._refresh = function () {
        var o = this.options;
        var $e = this.$element;
        var $c = this._getContentContainer();

        $c.html(this.render(o));
    };


    function Plugin(option) {
        var options = typeof option == 'object' && option

        var $first = this.first();
        var data = $first.data('edit_table')
        if (!data) {
            $first.data('edit_table', (data = new EditTable(this, options)))
        }

        return data;
    }

    $.fn.editTable = Plugin
    $.fn.editTable.Constructor = EditTable
})(window, jQuery);