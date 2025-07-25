var StarRating = (function () {
    "use strict";
    function e(e, t) {
        if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function");
    }
    function t(e, t) {
        for (var i = 0; i < t.length; i++) {
            var s = t[i];
            (s.enumerable = s.enumerable || !1), (s.configurable = !0), "value" in s && (s.writable = !0), Object.defineProperty(e, s.key, s);
        }
    }
    function i(e, i, s) {
        return i && t(e.prototype, i), s && t(e, s), e;
    }
    var s = { classNames: { active: "gl-active", base: "gl-star-rating", selected: "gl-selected" }, clearable: !0, maxStars: 10, prebuilt: !1, stars: null, tooltip: "Select a Rating" },
        n = function (e, t, i) {
            e.classList[t ? "add" : "remove"](i);
        },
        a = function (e) {
            var t = document.createElement("span");
            for (var i in (e = e || {})) t.setAttribute(i, e[i]);
            return t;
        },
        l = function (e, t, i) {
            var s = a(i);
            return e.parentNode.insertBefore(s, t ? e.nextSibling : e), s;
        },
        r = function e() {
            for (var t = arguments.length, i = new Array(t), s = 0; s < t; s++) i[s] = arguments[s];
            var n = {};
            return (
                i.forEach(function (t) {
                    Object.keys(t || {}).forEach(function (s) {
                        if (void 0 !== i[0][s]) {
                            var a = t[s];
                            "Object" !== o(a) || "Object" !== o(n[s]) ? (n[s] = a) : (n[s] = e(n[s], a));
                        }
                    });
                }),
                n
            );
        },
        o = function (e) {
            return {}.toString.call(e).slice(8, -1);
        },
        u = (function () {
            function t(i, s) {
                var n, a, l;
                e(this, t),
                    (this.direction = window.getComputedStyle(i, null).getPropertyValue("direction")),
                    (this.el = i),
                    (this.events = {
                        change: this.onChange.bind(this),
                        keydown: this.onKeyDown.bind(this),
                        mousedown: this.onPointerDown.bind(this),
                        mouseleave: this.onPointerLeave.bind(this),
                        mousemove: this.onPointerMove.bind(this),
                        reset: this.onReset.bind(this),
                        touchend: this.onPointerDown.bind(this),
                        touchmove: this.onPointerMove.bind(this),
                    }),
                    (this.indexActive = null),
                    (this.indexSelected = null),
                    (this.props = s),
                    (this.tick = null),
                    (this.ticking = !1),
                    (this.values = (function (e) {
                        var t = [];
                        return (
                            [].forEach.call(e.options, function (e) {
                                var i = parseInt(e.value, 10) || 0;
                                i > 0 && t.push({ index: e.index, text: e.text, value: i });
                            }),
                            t.sort(function (e, t) {
                                return e.value - t.value;
                            })
                        );
                    })(i)),
                    (this.widgetEl = null),
                    this.el.widget && this.el.widget.destroy(),
                    (n = this.values.length),
                    (a = 1),
                    (l = this.props.maxStars),
                    /^\d+$/.test(n) && a <= n && n <= l ? this.build() : this.destroy();
            }
            return (
                i(t, [
                    {
                        key: "build",
                        value: function () {
                            this.destroy(), this.buildWidget(), this.selectValue((this.indexSelected = this.selected()), !1), this.handleEvents("add"), (this.el.widget = this);
                        },
                    },
                    {
                        key: "buildWidget",
                        value: function () {
                            var e,
                                t,
                                i = this;
                            this.props.prebuilt
                                ? ((e = this.el.parentNode), (t = e.querySelector("." + this.props.classNames.base + "--stars")))
                                : ((e = l(this.el, !1, { class: this.props.classNames.base })).appendChild(this.el),
                                    (t = l(this.el, !0, { class: this.props.classNames.base + "--stars" })),
                                    this.values.forEach(function (e, s) {
                                        var n = a({ "data-index": s, "data-value": e.value });
                                        "function" == typeof i.props.stars && i.props.stars.call(i, n, e, s),
                                            [].forEach.call(n.children, function (e) {
                                                return (e.style.pointerEvents = "none");
                                            }),
                                            (t.innerHTML += n.outerHTML);
                                    })),
                                (e.dataset.starRating = ""),
                                e.classList.add(this.props.classNames.base + "--" + this.direction),
                                this.props.tooltip && t.setAttribute("role", "tooltip"),
                                (this.widgetEl = t);
                        },
                    },
                    {
                        key: "changeIndexTo",
                        value: function (e, t) {
                            var i = this;
                            if (this.indexActive !== e || t) {
                                if (
                                    ([].forEach.call(this.widgetEl.children, function (t, s) {
                                        n(t, s <= e, i.props.classNames.active), n(t, s === i.indexSelected, i.props.classNames.selected);
                                    }),
                                        this.widgetEl.setAttribute("data-rating", e + 1),
                                        "function" == typeof this.props.stars || this.props.prebuilt || (this.widgetEl.classList.remove("s" + 10 * (this.indexActive + 1)), this.widgetEl.classList.add("s" + 10 * (e + 1))),
                                        this.props.tooltip)
                                ) {
                                    var s,
                                        a = e < 0 ? this.props.tooltip : null === (s = this.values[e]) || void 0 === s ? void 0 : s.text;
                                    this.widgetEl.setAttribute("aria-label", a);
                                }
                                this.indexActive = e;
                            }
                            this.ticking = !1;
                        },
                    },
                    {
                        key: "destroy",
                        value: function () {
                            (this.indexActive = null), (this.indexSelected = this.selected());
                            var e = this.el.parentNode;
                            e.classList.contains(this.props.classNames.base) &&
                                (this.props.prebuilt
                                    ? ((this.widgetEl = e.querySelector("." + this.props.classNames.base + "--stars")), e.classList.remove(this.props.classNames.base + "--" + this.direction), delete e.dataset.starRating)
                                    : e.parentNode.replaceChild(this.el, e),
                                    this.handleEvents("remove")),
                                delete this.el.widget;
                        },
                    },
                    {
                        key: "eventListener",
                        value: function (e, t, i, s) {
                            var n = this;
                            i.forEach(function (i) {
                                return e[t + "EventListener"](i, n.events[i], s || !1);
                            });
                        },
                    },
                    {
                        key: "handleEvents",
                        value: function (e) {
                            var t = this.el.closest("form");
                            t && "FORM" === t.tagName && this.eventListener(t, e, ["reset"]),
                                this.eventListener(this.el, e, ["change"]),
                                ("add" === e && this.el.disabled) || (this.eventListener(this.el, e, ["keydown"]), this.eventListener(this.widgetEl, e, ["mousedown", "mouseleave", "mousemove", "touchend", "touchmove"], !1));
                        },
                    },
                    {
                        key: "indexFromEvent",
                        value: function (e) {
                            var t,
                                i,
                                s = (null === (t = e.touches) || void 0 === t ? void 0 : t[0]) || (null === (i = e.changedTouches) || void 0 === i ? void 0 : i[0]) || e,
                                n = document.elementFromPoint(s.clientX, s.clientY);
                            return [].slice.call(n.parentNode.children).indexOf(n);
                        },
                    },
                    {
                        key: "onChange",
                        value: function () {
                            this.changeIndexTo(this.selected(), !0);
                        },
                    },
                    {
                        key: "onKeyDown",
                        value: function (e) {
                            var t = e.key.slice(5);
                            if (~["Left", "Right"].indexOf(t)) {
                                var i = "Left" === t ? -1 : 1;
                                "rtl" === this.direction && (i *= -1);
                                var s = this.values.length - 1,
                                    n = Math.min(Math.max(this.selected() + i, -1), s);
                                this.selectValue(n, !0);
                            }
                        },
                    },
                    {
                        key: "onPointerDown",
                        value: function (e) {
                            e.preventDefault();
                            var t = this.indexFromEvent(e);
                            this.props.clearable && t === this.indexSelected && (t = -1), this.selectValue(t, !0);
                        },
                    },
                    {
                        key: "onPointerLeave",
                        value: function (e) {
                            var t = this;
                            e.preventDefault(),
                                cancelAnimationFrame(this.tick),
                                requestAnimationFrame(function () {
                                    return t.changeIndexTo(t.indexSelected);
                                });
                        },
                    },
                    {
                        key: "onPointerMove",
                        value: function (e) {
                            var t = this;
                            e.preventDefault(),
                                this.ticking ||
                                ((this.tick = requestAnimationFrame(function () {
                                    return t.changeIndexTo(t.indexFromEvent(e));
                                })),
                                    (this.ticking = !0));
                        },
                    },
                    {
                        key: "onReset",
                        value: function () {
                            var e,
                                t = this.valueIndex(null === (e = this.el.querySelector("[selected]")) || void 0 === e ? void 0 : e.value);
                            this.selectValue(t || -1, !1);
                        },
                    },
                    {
                        key: "selected",
                        value: function () {
                            return this.valueIndex(this.el.value);
                        },
                    },
                    {
                        key: "selectValue",
                        value: function (e, t) {
                            var i;
                            (this.el.value = (null === (i = this.values[e]) || void 0 === i ? void 0 : i.value) || ""),
                                (this.indexSelected = this.selected()),
                                !1 === t ? this.changeIndexTo(this.selected(), !0) : this.el.dispatchEvent(new Event("change"));
                        },
                    },
                    {
                        key: "valueIndex",
                        value: function (e) {
                            return this.values.findIndex(function (t) {
                                return t.value === +e;
                            });
                        },
                    },
                ]),
                t
            );
        })();
    return (function () {
        function t(i, s) {
            e(this, t), (this.destroy = this.destroy.bind(this)), (this.props = s), (this.rebuild = this.build.bind(this)), (this.selector = i), (this.widgets = []), this.build();
        }
        return (
            i(t, [
                {
                    key: "build",
                    value: function () {
                        var e = this;
                        this.queryElements(this.selector).forEach(function (t) {
                            var i = r(s, e.props, JSON.parse(t.getAttribute("data-options")));
                            "SELECT" !== t.tagName || t.widget || (!i.prebuilt && t.parentNode.classList.contains(i.classNames.base) && e.unwrap(t), e.widgets.push(new u(t, i)));
                        });
                    },
                },
                {
                    key: "destroy",
                    value: function () {
                        this.widgets.forEach(function (e) {
                            return e.destroy();
                        }),
                            (this.widgets = []);
                    },
                },
                {
                    key: "queryElements",
                    value: function (e) {
                        return "HTMLSelectElement" === o(e) ? [e] : "NodeList" === o(e) ? [].slice.call(e) : "String" === o(e) ? [].slice.call(document.querySelectorAll(e)) : [];
                    },
                },
                {
                    key: "rebuild",
                    value: function () {
                        this.destroy(), this.build();
                    },
                },
                {
                    key: "unwrap",
                    value: function (e) {
                        var t = e.parentNode,
                            i = t.parentNode;
                        i.insertBefore(e, t), i.removeChild(t);
                    },
                },
            ]),
            t
        );
    })();
})();
