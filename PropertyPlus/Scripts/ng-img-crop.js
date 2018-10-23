/*! ngImgCrop v0.3.2 License: MIT */ ! function() {
    "use strict";
    var e = angular.module("ngImgCrop", []);
    e.factory("cropAreaCircle", ["cropArea", function(e) {
        var t = function() {
            e.apply(this, arguments), this._boxResizeBaseSize = 20, this._boxResizeNormalRatio = .9, this._boxResizeHoverRatio = 1.2, this._iconMoveNormalRatio = .9, this._iconMoveHoverRatio = 1.2, this._boxResizeNormalSize = this._boxResizeBaseSize * this._boxResizeNormalRatio, this._boxResizeHoverSize = this._boxResizeBaseSize * this._boxResizeHoverRatio, this._posDragStartX = 0, this._posDragStartY = 0, this._posResizeStartX = 0, this._posResizeStartY = 0, this._posResizeStartSize = 0, this._boxResizeIsHover = !1, this._areaIsHover = !1, this._boxResizeIsDragging = !1, this._areaIsDragging = !1
        };
        return t.prototype = new e, t.prototype._calcCirclePerimeterCoords = function(e) {
            var t = this._size / 2,
                i = e * (Math.PI / 180),
                r = this._x + t * Math.cos(i),
                s = this._y + t * Math.sin(i);
            return [r, s]
        }, t.prototype._calcResizeIconCenterCoords = function() {
            return this._calcCirclePerimeterCoords(-45)
        }, t.prototype._isCoordWithinArea = function(e) {
            return Math.sqrt((e[0] - this._x) * (e[0] - this._x) + (e[1] - this._y) * (e[1] - this._y)) < this._size / 2
        }, t.prototype._isCoordWithinBoxResize = function(e) {
            var t = this._calcResizeIconCenterCoords(),
                i = this._boxResizeHoverSize / 2;
            return e[0] > t[0] - i && e[0] < t[0] + i && e[1] > t[1] - i && e[1] < t[1] + i
        }, t.prototype._drawArea = function(e, t, i) {
            e.arc(t[0], t[1], i / 2, 0, 2 * Math.PI)
        }, t.prototype.draw = function() {
            e.prototype.draw.apply(this, arguments), this._cropCanvas.drawIconMove([this._x, this._y], this._areaIsHover ? this._iconMoveHoverRatio : this._iconMoveNormalRatio), this._cropCanvas.drawIconResizeBoxNESW(this._calcResizeIconCenterCoords(), this._boxResizeBaseSize, this._boxResizeIsHover ? this._boxResizeHoverRatio : this._boxResizeNormalRatio)
        }, t.prototype.processMouseMove = function(e, t) {
            var i = "default",
                r = !1;
            if (this._boxResizeIsHover = !1, this._areaIsHover = !1, this._areaIsDragging) this._x = e - this._posDragStartX, this._y = t - this._posDragStartY, this._areaIsHover = !0, i = "move", r = !0, this._events.trigger("area-move");
            else if (this._boxResizeIsDragging) {
                i = "nesw-resize";
                var s, o, a;
                o = e - this._posResizeStartX, a = this._posResizeStartY - t, s = o > a ? this._posResizeStartSize + 2 * a : this._posResizeStartSize + 2 * o, this._size = Math.max(this._minSize, s), this._boxResizeIsHover = !0, r = !0, this._events.trigger("area-resize")
            } else this._isCoordWithinBoxResize([e, t]) ? (i = "nesw-resize", this._areaIsHover = !1, this._boxResizeIsHover = !0, r = !0) : this._isCoordWithinArea([e, t]) && (i = "move", this._areaIsHover = !0, r = !0);
            return this._dontDragOutside(), angular.element(this._ctx.canvas).css({
                cursor: i
            }), r
        }, t.prototype.processMouseDown = function(e, t) {
            this._isCoordWithinBoxResize([e, t]) ? (this._areaIsDragging = !1, this._areaIsHover = !1, this._boxResizeIsDragging = !0, this._boxResizeIsHover = !0, this._posResizeStartX = e, this._posResizeStartY = t, this._posResizeStartSize = this._size, this._events.trigger("area-resize-start")) : this._isCoordWithinArea([e, t]) && (this._areaIsDragging = !0, this._areaIsHover = !0, this._boxResizeIsDragging = !1, this._boxResizeIsHover = !1, this._posDragStartX = e - this._x, this._posDragStartY = t - this._y, this._events.trigger("area-move-start"))
        }, t.prototype.processMouseUp = function() {
            this._areaIsDragging && (this._areaIsDragging = !1, this._events.trigger("area-move-end")), this._boxResizeIsDragging && (this._boxResizeIsDragging = !1, this._events.trigger("area-resize-end")), this._areaIsHover = !1, this._boxResizeIsHover = !1, this._posDragStartX = 0, this._posDragStartY = 0
        }, t
    }]), e.factory("cropAreaSquare", ["cropArea", function(e) {
        var t = function() {
            e.apply(this, arguments), this._resizeCtrlBaseRadius = 10, this._resizeCtrlNormalRatio = .75, this._resizeCtrlHoverRatio = 1, this._iconMoveNormalRatio = .9, this._iconMoveHoverRatio = 1.2, this._resizeCtrlNormalRadius = this._resizeCtrlBaseRadius * this._resizeCtrlNormalRatio, this._resizeCtrlHoverRadius = this._resizeCtrlBaseRadius * this._resizeCtrlHoverRatio, this._posDragStartX = 0, this._posDragStartY = 0, this._posResizeStartX = 0, this._posResizeStartY = 0, this._posResizeStartSize = 0, this._resizeCtrlIsHover = -1, this._areaIsHover = !1, this._resizeCtrlIsDragging = -1, this._areaIsDragging = !1
        };
        return t.prototype = new e, t.prototype._calcSquareCorners = function() {
            var e = this._size / 2;
            return [
                [this._x - e, this._y - e],
                [this._x + e, this._y - e],
                [this._x - e, this._y + e],
                [this._x + e, this._y + e]
            ]
        }, t.prototype._calcSquareDimensions = function() {
            var e = this._size / 2;
            return {
                left: this._x - e,
                top: this._y - e,
                right: this._x + e,
                bottom: this._y + e
            }
        }, t.prototype._isCoordWithinArea = function(e) {
            var t = this._calcSquareDimensions();
            return e[0] >= t.left && e[0] <= t.right && e[1] >= t.top && e[1] <= t.bottom
        }, t.prototype._isCoordWithinResizeCtrl = function(e) {
            for (var t = this._calcSquareCorners(), i = -1, r = 0, s = t.length; s > r; r++) {
                var o = t[r];
                if (e[0] > o[0] - this._resizeCtrlHoverRadius && e[0] < o[0] + this._resizeCtrlHoverRadius && e[1] > o[1] - this._resizeCtrlHoverRadius && e[1] < o[1] + this._resizeCtrlHoverRadius) {
                    i = r;
                    break
                }
            }
            return i
        }, t.prototype._drawArea = function(e, t, i) {
            var r = i / 2;
            e.rect(t[0] - r, t[1] - r, i, i)
        }, t.prototype.draw = function() {
            e.prototype.draw.apply(this, arguments), this._cropCanvas.drawIconMove([this._x, this._y], this._areaIsHover ? this._iconMoveHoverRatio : this._iconMoveNormalRatio);
            for (var t = this._calcSquareCorners(), i = 0, r = t.length; r > i; i++) {
                var s = t[i];
                this._cropCanvas.drawIconResizeCircle(s, this._resizeCtrlBaseRadius, this._resizeCtrlIsHover === i ? this._resizeCtrlHoverRatio : this._resizeCtrlNormalRatio)
            }
        }, t.prototype.processMouseMove = function(e, t) {
            var i = "default",
                r = !1;
            if (this._resizeCtrlIsHover = -1, this._areaIsHover = !1, this._areaIsDragging) this._x = e - this._posDragStartX, this._y = t - this._posDragStartY, this._areaIsHover = !0, i = "move", r = !0, this._events.trigger("area-move");
            else if (this._resizeCtrlIsDragging > -1) {
                var s, o;
                switch (this._resizeCtrlIsDragging) {
                    case 0:
                        s = -1, o = -1, i = "nwse-resize";
                        break;
                    case 1:
                        s = 1, o = -1, i = "nesw-resize";
                        break;
                    case 2:
                        s = -1, o = 1, i = "nesw-resize";
                        break;
                    case 3:
                        s = 1, o = 1, i = "nwse-resize"
                }
                var a, n = (e - this._posResizeStartX) * s,
                    h = (t - this._posResizeStartY) * o;
                a = n > h ? this._posResizeStartSize + h : this._posResizeStartSize + n;
                var c = this._size;
                this._size = Math.max(this._minSize, a);
                var l = (this._size - c) / 2;
                this._x += l * s, this._y += l * o, this._resizeCtrlIsHover = this._resizeCtrlIsDragging, r = !0, this._events.trigger("area-resize")
            } else {
                var u = this._isCoordWithinResizeCtrl([e, t]);
                if (u > -1) {
                    switch (u) {
                        case 0:
                            i = "nwse-resize";
                            break;
                        case 1:
                            i = "nesw-resize";
                            break;
                        case 2:
                            i = "nesw-resize";
                            break;
                        case 3:
                            i = "nwse-resize"
                    }
                    this._areaIsHover = !1, this._resizeCtrlIsHover = u, r = !0
                } else this._isCoordWithinArea([e, t]) && (i = "move", this._areaIsHover = !0, r = !0)
            }
            return this._dontDragOutside(), angular.element(this._ctx.canvas).css({
                cursor: i
            }), r
        }, t.prototype.processMouseDown = function(e, t) {
            var i = this._isCoordWithinResizeCtrl([e, t]);
            i > -1 ? (this._areaIsDragging = !1, this._areaIsHover = !1, this._resizeCtrlIsDragging = i, this._resizeCtrlIsHover = i, this._posResizeStartX = e, this._posResizeStartY = t, this._posResizeStartSize = this._size, this._events.trigger("area-resize-start")) : this._isCoordWithinArea([e, t]) && (this._areaIsDragging = !0, this._areaIsHover = !0, this._resizeCtrlIsDragging = -1, this._resizeCtrlIsHover = -1, this._posDragStartX = e - this._x, this._posDragStartY = t - this._y, this._events.trigger("area-move-start"))
        }, t.prototype.processMouseUp = function() {
            this._areaIsDragging && (this._areaIsDragging = !1, this._events.trigger("area-move-end")), this._resizeCtrlIsDragging > -1 && (this._resizeCtrlIsDragging = -1, this._events.trigger("area-resize-end")), this._areaIsHover = !1, this._resizeCtrlIsHover = -1, this._posDragStartX = 0, this._posDragStartY = 0
        }, t
    }]), e.factory("cropArea", ["cropCanvas", function(e) {
        var t = function(t, i) {
            this._ctx = t, this._events = i, this._minSize = 80, this._cropCanvas = new e(t), this._image = new Image, this._x = 0, this._y = 0, this._size = 200
        };
        return t.prototype.getImage = function() {
            return this._image
        }, t.prototype.setImage = function(e) {
            this._image = e
        }, t.prototype.getX = function() {
            return this._x
        }, t.prototype.setX = function(e) {
            this._x = e, this._dontDragOutside()
        }, t.prototype.getY = function() {
            return this._y
        }, t.prototype.setY = function(e) {
            this._y = e, this._dontDragOutside()
        }, t.prototype.getSize = function() {
            return this._size
        }, t.prototype.setSize = function(e) {
            this._size = Math.max(this._minSize, e), this._dontDragOutside()
        }, t.prototype.getMinSize = function() {
            return this._minSize
        }, t.prototype.setMinSize = function(e) {
            this._minSize = e, this._size = Math.max(this._minSize, this._size), this._dontDragOutside()
        }, t.prototype._dontDragOutside = function() {
            var e = this._ctx.canvas.height,
                t = this._ctx.canvas.width;
            this._size > t && (this._size = t), this._size > e && (this._size = e), this._x < this._size / 2 && (this._x = this._size / 2), this._x > t - this._size / 2 && (this._x = t - this._size / 2), this._y < this._size / 2 && (this._y = this._size / 2), this._y > e - this._size / 2 && (this._y = e - this._size / 2)
        }, t.prototype._drawArea = function() {}, t.prototype.draw = function() {
            this._cropCanvas.drawCropArea(this._image, [this._x, this._y], this._size, this._drawArea)
        }, t.prototype.processMouseMove = function() {}, t.prototype.processMouseDown = function() {}, t.prototype.processMouseUp = function() {}, t
    }]), e.factory("cropCanvas", [function() {
        var e = [
                [-.5, -2],
                [-3, -4.5],
                [-.5, -7],
                [-7, -7],
                [-7, -.5],
                [-4.5, -3],
                [-2, -.5]
            ],
            t = [
                [.5, -2],
                [3, -4.5],
                [.5, -7],
                [7, -7],
                [7, -.5],
                [4.5, -3],
                [2, -.5]
            ],
            i = [
                [-.5, 2],
                [-3, 4.5],
                [-.5, 7],
                [-7, 7],
                [-7, .5],
                [-4.5, 3],
                [-2, .5]
            ],
            r = [
                [.5, 2],
                [3, 4.5],
                [.5, 7],
                [7, 7],
                [7, .5],
                [4.5, 3],
                [2, .5]
            ],
            s = [
                [-1.5, -2.5],
                [-1.5, -6],
                [-5, -6],
                [0, -11],
                [5, -6],
                [1.5, -6],
                [1.5, -2.5]
            ],
            o = [
                [-2.5, -1.5],
                [-6, -1.5],
                [-6, -5],
                [-11, 0],
                [-6, 5],
                [-6, 1.5],
                [-2.5, 1.5]
            ],
            a = [
                [-1.5, 2.5],
                [-1.5, 6],
                [-5, 6],
                [0, 11],
                [5, 6],
                [1.5, 6],
                [1.5, 2.5]
            ],
            n = [
                [2.5, -1.5],
                [6, -1.5],
                [6, -5],
                [11, 0],
                [6, 5],
                [6, 1.5],
                [2.5, 1.5]
            ],
            h = {
                areaOutline: "#fff",
                resizeBoxStroke: "#fff",
                resizeBoxFill: "#444",
                resizeBoxArrowFill: "#fff",
                resizeCircleStroke: "#fff",
                resizeCircleFill: "#444",
                moveIconFill: "#fff"
            };
        return function(c) {
            var l = function(e, t, i) {
                    return [i * e[0] + t[0], i * e[1] + t[1]]
                },
                u = function(e, t, i, r) {
                    c.save(), c.fillStyle = t, c.beginPath();
                    var s, o = l(e[0], i, r);
                    c.moveTo(o[0], o[1]);
                    for (var a in e) a > 0 && (s = l(e[a], i, r), c.lineTo(s[0], s[1]));
                    c.lineTo(o[0], o[1]), c.fill(), c.closePath(), c.restore()
                };
            this.drawIconMove = function(e, t) {
                u(s, h.moveIconFill, e, t), u(o, h.moveIconFill, e, t), u(a, h.moveIconFill, e, t), u(n, h.moveIconFill, e, t)
            }, this.drawIconResizeCircle = function(e, t, i) {
                var r = t * i;
                c.save(), c.strokeStyle = h.resizeCircleStroke, c.lineWidth = 2, c.fillStyle = h.resizeCircleFill, c.beginPath(), c.arc(e[0], e[1], r, 0, 2 * Math.PI), c.fill(), c.stroke(), c.closePath(), c.restore()
            }, this.drawIconResizeBoxBase = function(e, t, i) {
                var r = t * i;
                c.save(), c.strokeStyle = h.resizeBoxStroke, c.lineWidth = 2, c.fillStyle = h.resizeBoxFill, c.fillRect(e[0] - r / 2, e[1] - r / 2, r, r), c.strokeRect(e[0] - r / 2, e[1] - r / 2, r, r), c.restore()
            }, this.drawIconResizeBoxNESW = function(e, r, s) {
                this.drawIconResizeBoxBase(e, r, s), u(t, h.resizeBoxArrowFill, e, s), u(i, h.resizeBoxArrowFill, e, s)
            }, this.drawIconResizeBoxNWSE = function(t, i, s) {
                this.drawIconResizeBoxBase(t, i, s), u(e, h.resizeBoxArrowFill, t, s), u(r, h.resizeBoxArrowFill, t, s)
            }, this.drawCropArea = function(e, t, i, r) {
                var s = e.width / c.canvas.width,
                    o = e.height / c.canvas.height,
                    a = t[0] - i / 2,
                    n = t[1] - i / 2;
                c.save(), c.strokeStyle = h.areaOutline, c.lineWidth = 2, c.beginPath(), r(c, t, i), c.stroke(), c.clip(), i > 0 && c.drawImage(e, a * s, n * o, i * s, i * o, a, n, i, i), c.beginPath(), r(c, t, i), c.stroke(), c.clip(), c.restore()
            }
        }
    }]), e.service("cropEXIF", [function() {
        function e(e) {
            return !!e.exifdata
        }

        function t(e, t) {
            t = t || e.match(/^data\:([^\;]+)\;base64,/im)[1] || "", e = e.replace(/^data\:([^\;]+)\;base64,/gim, "");
            for (var i = atob(e), r = i.length, s = new ArrayBuffer(r), o = new Uint8Array(s), a = 0; r > a; a++) o[a] = i.charCodeAt(a);
            return s
        }

        function i(e, t) {
            var i = new XMLHttpRequest;
            i.open("GET", e, !0), i.responseType = "blob", i.onload = function() {
                (200 == this.status || 0 === this.status) && t(this.response)
            }, i.send()
        }

        function r(e, r) {
            function a(t) {
                var i = s(t),
                    a = o(t);
                e.exifdata = i || {}, e.iptcdata = a || {}, r && r.call(e)
            }
            if (e.src)
                if (/^data\:/i.test(e.src)) {
                    var n = t(e.src);
                    a(n)
                } else if (/^blob\:/i.test(e.src)) {
                var h = new FileReader;
                h.onload = function(e) {
                    a(e.target.result)
                }, i(e.src, function(e) {
                    h.readAsArrayBuffer(e)
                })
            } else {
                var c = new XMLHttpRequest;
                c.onload = function() {
                    if (200 != this.status && 0 !== this.status) throw "Could not load image";
                    a(c.response), c = null
                }, c.open("GET", e.src, !0), c.responseType = "arraybuffer", c.send(null)
            } else if (window.FileReader && (e instanceof window.Blob || e instanceof window.File)) {
                var h = new FileReader;
                h.onload = function(e) {
                    u && console.log("Got file of length " + e.target.result.byteLength), a(e.target.result)
                }, h.readAsArrayBuffer(e)
            }
        }

        function s(e) {
            var t = new DataView(e);
            if (u && console.log("Got file of length " + e.byteLength), 255 != t.getUint8(0) || 216 != t.getUint8(1)) return u && console.log("Not a valid JPEG"), !1;
            for (var i, r = 2, s = e.byteLength; s > r;) {
                if (255 != t.getUint8(r)) return u && console.log("Not a valid marker at offset " + r + ", found: " + t.getUint8(r)), !1;
                if (i = t.getUint8(r + 1), u && console.log(i), 225 == i) return u && console.log("Found 0xFFE1 marker"), l(t, r + 4, t.getUint16(r + 2) - 2);
                r += 2 + t.getUint16(r + 2)
            }
        }

        function o(e) {
            var t = new DataView(e);
            if (u && console.log("Got file of length " + e.byteLength), 255 != t.getUint8(0) || 216 != t.getUint8(1)) return u && console.log("Not a valid JPEG"), !1;
            for (var i = 2, r = e.byteLength, s = function(e, t) {
                    return 56 === e.getUint8(t) && 66 === e.getUint8(t + 1) && 73 === e.getUint8(t + 2) && 77 === e.getUint8(t + 3) && 4 === e.getUint8(t + 4) && 4 === e.getUint8(t + 5)
                }; r > i;) {
                if (s(t, i)) {
                    var o = t.getUint8(i + 7);
                    o % 2 !== 0 && (o += 1), 0 === o && (o = 4);
                    var n = i + 8 + o,
                        h = t.getUint16(i + 6 + o);
                    return a(e, n, h)
                }
                i++
            }
        }

        function a(e, t, i) {
            for (var r, s, o, a, n, h = new DataView(e), l = {}, u = t; t + i > u;) 28 === h.getUint8(u) && 2 === h.getUint8(u + 1) && (a = h.getUint8(u + 2), a in _ && (o = h.getInt16(u + 3), n = o + 5, s = _[a], r = c(h, u + 5, o), l.hasOwnProperty(s) ? l[s] instanceof Array ? l[s].push(r) : l[s] = [l[s], r] : l[s] = r)), u++;
            return l
        }

        function n(e, t, i, r, s) {
            var o, a, n, c = e.getUint16(i, !s),
                l = {};
            for (n = 0; c > n; n++) o = i + 12 * n + 2, a = r[e.getUint16(o, !s)], !a && u && console.log("Unknown tag: " + e.getUint16(o, !s)), l[a] = h(e, o, t, i, s);
            return l
        }

        function h(e, t, i, r, s) {
            var o, a, n, h, l, u, g = e.getUint16(t + 2, !s),
                d = e.getUint32(t + 4, !s),
                f = e.getUint32(t + 8, !s) + i;
            switch (g) {
                case 1:
                case 7:
                    if (1 == d) return e.getUint8(t + 8, !s);
                    for (o = d > 4 ? f : t + 8, a = [], h = 0; d > h; h++) a[h] = e.getUint8(o + h);
                    return a;
                case 2:
                    return o = d > 4 ? f : t + 8, c(e, o, d - 1);
                case 3:
                    if (1 == d) return e.getUint16(t + 8, !s);
                    for (o = d > 2 ? f : t + 8, a = [], h = 0; d > h; h++) a[h] = e.getUint16(o + 2 * h, !s);
                    return a;
                case 4:
                    if (1 == d) return e.getUint32(t + 8, !s);
                    for (a = [], h = 0; d > h; h++) a[h] = e.getUint32(f + 4 * h, !s);
                    return a;
                case 5:
                    if (1 == d) return l = e.getUint32(f, !s), u = e.getUint32(f + 4, !s), n = new Number(l / u), n.numerator = l, n.denominator = u, n;
                    for (a = [], h = 0; d > h; h++) l = e.getUint32(f + 8 * h, !s), u = e.getUint32(f + 4 + 8 * h, !s), a[h] = new Number(l / u), a[h].numerator = l, a[h].denominator = u;
                    return a;
                case 9:
                    if (1 == d) return e.getInt32(t + 8, !s);
                    for (a = [], h = 0; d > h; h++) a[h] = e.getInt32(f + 4 * h, !s);
                    return a;
                case 10:
                    if (1 == d) return e.getInt32(f, !s) / e.getInt32(f + 4, !s);
                    for (a = [], h = 0; d > h; h++) a[h] = e.getInt32(f + 8 * h, !s) / e.getInt32(f + 4 + 8 * h, !s);
                    return a
            }
        }

        function c(e, t, i) {
            for (var r = "", s = t; t + i > s; s++) r += String.fromCharCode(e.getUint8(s));
            return r
        }

        function l(e, t) {
            if ("Exif" != c(e, t, 4)) return u && console.log("Not valid EXIF data! " + c(e, t, 4)), !1;
            var i, r, s, o, a, h = t + 6;
            if (18761 == e.getUint16(h)) i = !1;
            else {
                if (19789 != e.getUint16(h)) return u && console.log("Not valid TIFF data! (no 0x4949 or 0x4D4D)"), !1;
                i = !0
            }
            if (42 != e.getUint16(h + 2, !i)) return u && console.log("Not valid TIFF data! (no 0x002A)"), !1;
            var l = e.getUint32(h + 4, !i);
            if (8 > l) return u && console.log("Not valid TIFF data! (First offset less than 8)", e.getUint32(h + 4, !i)), !1;
            if (r = n(e, h, h + l, d, i), r.ExifIFDPointer) {
                o = n(e, h, h + r.ExifIFDPointer, g, i);
                for (s in o) {
                    switch (s) {
                        case "LightSource":
                        case "Flash":
                        case "MeteringMode":
                        case "ExposureProgram":
                        case "SensingMethod":
                        case "SceneCaptureType":
                        case "SceneType":
                        case "CustomRendered":
                        case "WhiteBalance":
                        case "GainControl":
                        case "Contrast":
                        case "Saturation":
                        case "Sharpness":
                        case "SubjectDistanceRange":
                        case "FileSource":
                            o[s] = p[s][o[s]];
                            break;
                        case "ExifVersion":
                        case "FlashpixVersion":
                            o[s] = String.fromCharCode(o[s][0], o[s][1], o[s][2], o[s][3]);
                            break;
                        case "ComponentsConfiguration":
                            o[s] = p.Components[o[s][0]] + p.Components[o[s][1]] + p.Components[o[s][2]] + p.Components[o[s][3]]
                    }
                    r[s] = o[s]
                }
            }
            if (r.GPSInfoIFDPointer) {
                a = n(e, h, h + r.GPSInfoIFDPointer, f, i);
                for (s in a) {
                    switch (s) {
                        case "GPSVersionID":
                            a[s] = a[s][0] + "." + a[s][1] + "." + a[s][2] + "." + a[s][3]
                    }
                    r[s] = a[s]
                }
            }
            return r
        }
        var u = !1,
            g = this.Tags = {
                36864: "ExifVersion",
                40960: "FlashpixVersion",
                40961: "ColorSpace",
                40962: "PixelXDimension",
                40963: "PixelYDimension",
                37121: "ComponentsConfiguration",
                37122: "CompressedBitsPerPixel",
                37500: "MakerNote",
                37510: "UserComment",
                40964: "RelatedSoundFile",
                36867: "DateTimeOriginal",
                36868: "DateTimeDigitized",
                37520: "SubsecTime",
                37521: "SubsecTimeOriginal",
                37522: "SubsecTimeDigitized",
                33434: "ExposureTime",
                33437: "FNumber",
                34850: "ExposureProgram",
                34852: "SpectralSensitivity",
                34855: "ISOSpeedRatings",
                34856: "OECF",
                37377: "ShutterSpeedValue",
                37378: "ApertureValue",
                37379: "BrightnessValue",
                37380: "ExposureBias",
                37381: "MaxApertureValue",
                37382: "SubjectDistance",
                37383: "MeteringMode",
                37384: "LightSource",
                37385: "Flash",
                37396: "SubjectArea",
                37386: "FocalLength",
                41483: "FlashEnergy",
                41484: "SpatialFrequencyResponse",
                41486: "FocalPlaneXResolution",
                41487: "FocalPlaneYResolution",
                41488: "FocalPlaneResolutionUnit",
                41492: "SubjectLocation",
                41493: "ExposureIndex",
                41495: "SensingMethod",
                41728: "FileSource",
                41729: "SceneType",
                41730: "CFAPattern",
                41985: "CustomRendered",
                41986: "ExposureMode",
                41987: "WhiteBalance",
                41988: "DigitalZoomRation",
                41989: "FocalLengthIn35mmFilm",
                41990: "SceneCaptureType",
                41991: "GainControl",
                41992: "Contrast",
                41993: "Saturation",
                41994: "Sharpness",
                41995: "DeviceSettingDescription",
                41996: "SubjectDistanceRange",
                40965: "InteroperabilityIFDPointer",
                42016: "ImageUniqueID"
            },
            d = this.TiffTags = {
                256: "ImageWidth",
                257: "ImageHeight",
                34665: "ExifIFDPointer",
                34853: "GPSInfoIFDPointer",
                40965: "InteroperabilityIFDPointer",
                258: "BitsPerSample",
                259: "Compression",
                262: "PhotometricInterpretation",
                274: "Orientation",
                277: "SamplesPerPixel",
                284: "PlanarConfiguration",
                530: "YCbCrSubSampling",
                531: "YCbCrPositioning",
                282: "XResolution",
                283: "YResolution",
                296: "ResolutionUnit",
                273: "StripOffsets",
                278: "RowsPerStrip",
                279: "StripByteCounts",
                513: "JPEGInterchangeFormat",
                514: "JPEGInterchangeFormatLength",
                301: "TransferFunction",
                318: "WhitePoint",
                319: "PrimaryChromaticities",
                529: "YCbCrCoefficients",
                532: "ReferenceBlackWhite",
                306: "DateTime",
                270: "ImageDescription",
                271: "Make",
                272: "Model",
                305: "Software",
                315: "Artist",
                33432: "Copyright"
            },
            f = this.GPSTags = {
                0: "GPSVersionID",
                1: "GPSLatitudeRef",
                2: "GPSLatitude",
                3: "GPSLongitudeRef",
                4: "GPSLongitude",
                5: "GPSAltitudeRef",
                6: "GPSAltitude",
                7: "GPSTimeStamp",
                8: "GPSSatellites",
                9: "GPSStatus",
                10: "GPSMeasureMode",
                11: "GPSDOP",
                12: "GPSSpeedRef",
                13: "GPSSpeed",
                14: "GPSTrackRef",
                15: "GPSTrack",
                16: "GPSImgDirectionRef",
                17: "GPSImgDirection",
                18: "GPSMapDatum",
                19: "GPSDestLatitudeRef",
                20: "GPSDestLatitude",
                21: "GPSDestLongitudeRef",
                22: "GPSDestLongitude",
                23: "GPSDestBearingRef",
                24: "GPSDestBearing",
                25: "GPSDestDistanceRef",
                26: "GPSDestDistance",
                27: "GPSProcessingMethod",
                28: "GPSAreaInformation",
                29: "GPSDateStamp",
                30: "GPSDifferential"
            },
            p = this.StringValues = {
                ExposureProgram: {
                    0: "Not defined",
                    1: "Manual",
                    2: "Normal program",
                    3: "Aperture priority",
                    4: "Shutter priority",
                    5: "Creative program",
                    6: "Action program",
                    7: "Portrait mode",
                    8: "Landscape mode"
                },
                MeteringMode: {
                    0: "Unknown",
                    1: "Average",
                    2: "CenterWeightedAverage",
                    3: "Spot",
                    4: "MultiSpot",
                    5: "Pattern",
                    6: "Partial",
                    255: "Other"
                },
                LightSource: {
                    0: "Unknown",
                    1: "Daylight",
                    2: "Fluorescent",
                    3: "Tungsten (incandescent light)",
                    4: "Flash",
                    9: "Fine weather",
                    10: "Cloudy weather",
                    11: "Shade",
                    12: "Daylight fluorescent (D 5700 - 7100K)",
                    13: "Day white fluorescent (N 4600 - 5400K)",
                    14: "Cool white fluorescent (W 3900 - 4500K)",
                    15: "White fluorescent (WW 3200 - 3700K)",
                    17: "Standard light A",
                    18: "Standard light B",
                    19: "Standard light C",
                    20: "D55",
                    21: "D65",
                    22: "D75",
                    23: "D50",
                    24: "ISO studio tungsten",
                    255: "Other"
                },
                Flash: {
                    0: "Flash did not fire",
                    1: "Flash fired",
                    5: "Strobe return light not detected",
                    7: "Strobe return light detected",
                    9: "Flash fired, compulsory flash mode",
                    13: "Flash fired, compulsory flash mode, return light not detected",
                    15: "Flash fired, compulsory flash mode, return light detected",
                    16: "Flash did not fire, compulsory flash mode",
                    24: "Flash did not fire, auto mode",
                    25: "Flash fired, auto mode",
                    29: "Flash fired, auto mode, return light not detected",
                    31: "Flash fired, auto mode, return light detected",
                    32: "No flash function",
                    65: "Flash fired, red-eye reduction mode",
                    69: "Flash fired, red-eye reduction mode, return light not detected",
                    71: "Flash fired, red-eye reduction mode, return light detected",
                    73: "Flash fired, compulsory flash mode, red-eye reduction mode",
                    77: "Flash fired, compulsory flash mode, red-eye reduction mode, return light not detected",
                    79: "Flash fired, compulsory flash mode, red-eye reduction mode, return light detected",
                    89: "Flash fired, auto mode, red-eye reduction mode",
                    93: "Flash fired, auto mode, return light not detected, red-eye reduction mode",
                    95: "Flash fired, auto mode, return light detected, red-eye reduction mode"
                },
                SensingMethod: {
                    1: "Not defined",
                    2: "One-chip color area sensor",
                    3: "Two-chip color area sensor",
                    4: "Three-chip color area sensor",
                    5: "Color sequential area sensor",
                    7: "Trilinear sensor",
                    8: "Color sequential linear sensor"
                },
                SceneCaptureType: {
                    0: "Standard",
                    1: "Landscape",
                    2: "Portrait",
                    3: "Night scene"
                },
                SceneType: {
                    1: "Directly photographed"
                },
                CustomRendered: {
                    0: "Normal process",
                    1: "Custom process"
                },
                WhiteBalance: {
                    0: "Auto white balance",
                    1: "Manual white balance"
                },
                GainControl: {
                    0: "None",
                    1: "Low gain up",
                    2: "High gain up",
                    3: "Low gain down",
                    4: "High gain down"
                },
                Contrast: {
                    0: "Normal",
                    1: "Soft",
                    2: "Hard"
                },
                Saturation: {
                    0: "Normal",
                    1: "Low saturation",
                    2: "High saturation"
                },
                Sharpness: {
                    0: "Normal",
                    1: "Soft",
                    2: "Hard"
                },
                SubjectDistanceRange: {
                    0: "Unknown",
                    1: "Macro",
                    2: "Close view",
                    3: "Distant view"
                },
                FileSource: {
                    3: "DSC"
                },
                Components: {
                    0: "",
                    1: "Y",
                    2: "Cb",
                    3: "Cr",
                    4: "R",
                    5: "G",
                    6: "B"
                }
            },
            _ = {
                120: "caption",
                110: "credit",
                25: "keywords",
                55: "dateCreated",
                80: "byline",
                85: "bylineTitle",
                122: "captionWriter",
                105: "headline",
                116: "copyright",
                15: "category"
            };
        this.getData = function(t, i) {
            return (t instanceof Image || t instanceof HTMLImageElement) && !t.complete ? !1 : (e(t) ? i && i.call(t) : r(t, i), !0)
        }, this.getTag = function(t, i) {
            return e(t) ? t.exifdata[i] : void 0
        }, this.getAllTags = function(t) {
            if (!e(t)) return {};
            var i, r = t.exifdata,
                s = {};
            for (i in r) r.hasOwnProperty(i) && (s[i] = r[i]);
            return s
        }, this.pretty = function(t) {
            if (!e(t)) return "";
            var i, r = t.exifdata,
                s = "";
            for (i in r) r.hasOwnProperty(i) && (s += "object" == typeof r[i] ? r[i] instanceof Number ? i + " : " + r[i] + " [" + r[i].numerator + "/" + r[i].denominator + "]\r\n" : i + " : [" + r[i].length + " values]\r\n" : i + " : " + r[i] + "\r\n");
            return s
        }, this.readFromBinaryFile = function(e) {
            return s(e)
        }
    }]), e.factory("cropHost", ["$document", "cropAreaCircle", "cropAreaSquare", "cropEXIF", function(e, t, i, r) {
        var s = function(e) {
            var t = e.getBoundingClientRect(),
                i = document.body,
                r = document.documentElement,
                s = window.pageYOffset || r.scrollTop || i.scrollTop,
                o = window.pageXOffset || r.scrollLeft || i.scrollLeft,
                a = r.clientTop || i.clientTop || 0,
                n = r.clientLeft || i.clientLeft || 0,
                h = t.top + s - a,
                c = t.left + o - n;
            return {
                top: Math.round(h),
                left: Math.round(c)
            }
        };
        return function(o, a, n) {
            function h() {
                c.clearRect(0, 0, c.canvas.width, c.canvas.height), null !== l && (c.drawImage(l, 0, 0, c.canvas.width, c.canvas.height), c.save(), c.fillStyle = "rgba(0, 0, 0, 0.65)", c.fillRect(0, 0, c.canvas.width, c.canvas.height), c.restore(), u.draw())
            }
            var c = null,
                l = null,
                u = null,
                g = [100, 100],
                d = [300, 300],
                f = 200,
                p = "image/png",
                _ = null,
                m = function() {
                    if (null !== l) {
                        u.setImage(l);
                        var e = [l.width, l.height],
                            t = l.width / l.height,
                            i = e;
                        i[0] > d[0] ? (i[0] = d[0], i[1] = i[0] / t) : i[0] < g[0] && (i[0] = g[0], i[1] = i[0] / t), i[1] > d[1] ? (i[1] = d[1], i[0] = i[1] * t) : i[1] < g[1] && (i[1] = g[1], i[0] = i[1] * t), o.prop("width", i[0]).prop("height", i[1]).css({
                            "margin-left": -i[0] / 2 + "px",
                            "margin-top": -i[1] / 2 + "px"
                        }), u.setX(c.canvas.width / 2), u.setY(c.canvas.height / 2), u.setSize(Math.min(200, c.canvas.width / 2, c.canvas.height / 2))
                    } else o.prop("width", 0).prop("height", 0).css({
                        "margin-top": 0
                    });
                    h()
                },
                v = function(e) {
                    return angular.isDefined(e.changedTouches) ? e.changedTouches : e.originalEvent.changedTouches
                },
                S = function(e) {
                    if (null !== l) {
                        var t, i, r = s(c.canvas);
                        "touchmove" === e.type ? (t = v(e)[0].pageX, i = v(e)[0].pageY) : (t = e.pageX, i = e.pageY), u.processMouseMove(t - r.left, i - r.top), h()
                    }
                },
                z = function(e) {
                    if (e.preventDefault(), e.stopPropagation(), null !== l) {
                        var t, i, r = s(c.canvas);
                        "touchstart" === e.type ? (t = v(e)[0].pageX, i = v(e)[0].pageY) : (t = e.pageX, i = e.pageY), u.processMouseDown(t - r.left, i - r.top), h()
                    }
                },
                I = function(e) {
                    if (null !== l) {
                        var t, i, r = s(c.canvas);
                        "touchend" === e.type ? (t = v(e)[0].pageX, i = v(e)[0].pageY) : (t = e.pageX, i = e.pageY), u.processMouseUp(t - r.left, i - r.top), h()
                    }
                };
            this.getResultImageDataURI = function() {
                var e, t;
                return t = angular.element("<canvas></canvas>")[0], e = t.getContext("2d"), t.width = f, t.height = f, null !== l && e.drawImage(l, (u.getX() - u.getSize() / 2) * (l.width / c.canvas.width), (u.getY() - u.getSize() / 2) * (l.height / c.canvas.height), u.getSize() * (l.width / c.canvas.width), u.getSize() * (l.height / c.canvas.height), 0, 0, f, f), null !== _ ? t.toDataURL(p, _) : t.toDataURL(p)
            }, this.setNewImageSource = function(e) {
                if (l = null, m(), n.trigger("image-updated"), e) {
                    var t = new Image;
                    "http" === e.substring(0, 4).toLowerCase() && (t.crossOrigin = "anonymous"), t.onload = function() {
                        n.trigger("load-done"), r.getData(t, function() {
                            var e = r.getTag(t, "Orientation");
                            if ([3, 6, 8].indexOf(e) > -1) {
                                var i = document.createElement("canvas"),
                                    s = i.getContext("2d"),
                                    o = t.width,
                                    a = t.height,
                                    h = 0,
                                    c = 0,
                                    u = 0;
                                switch (e) {
                                    case 3:
                                        h = -t.width, c = -t.height, u = 180;
                                        break;
                                    case 6:
                                        o = t.height, a = t.width, c = -t.height, u = 90;
                                        break;
                                    case 8:
                                        o = t.height, a = t.width, h = -t.width, u = 270
                                }
                                i.width = o, i.height = a, s.rotate(u * Math.PI / 180), s.drawImage(t, h, c), l = new Image, l.src = i.toDataURL("image/png")
                            } else l = t;
                            m(), n.trigger("image-updated")
                        })
                    }, t.onerror = function() {
                        n.trigger("load-error")
                    }, n.trigger("load-start"), t.src = e
                }
            }, this.setMaxDimensions = function(e, t) {
                if (d = [e, t], null !== l) {
                    var i = c.canvas.width,
                        r = c.canvas.height,
                        s = [l.width, l.height],
                        a = l.width / l.height,
                        n = s;
                    n[0] > d[0] ? (n[0] = d[0], n[1] = n[0] / a) : n[0] < g[0] && (n[0] = g[0], n[1] = n[0] / a), n[1] > d[1] ? (n[1] = d[1], n[0] = n[1] * a) : n[1] < g[1] && (n[1] = g[1], n[0] = n[1] * a), o.prop("width", n[0]).prop("height", n[1]).css({
                        "margin-left": -n[0] / 2 + "px",
                        "margin-top": -n[1] / 2 + "px"
                    });
                    var f = c.canvas.width / i,
                        p = c.canvas.height / r,
                        _ = Math.min(f, p);
                    u.setX(u.getX() * f), u.setY(u.getY() * p), u.setSize(u.getSize() * _)
                } else o.prop("width", 0).prop("height", 0).css({
                    "margin-top": 0
                });
                h()
            }, this.setAreaMinSize = function(e) {
                e = parseInt(e, 10), isNaN(e) || (u.setMinSize(e), h())
            }, this.setResultImageSize = function(e) {
                e = parseInt(e, 10), isNaN(e) || (f = e)
            }, this.setResultImageFormat = function(e) {
                p = e
            }, this.setResultImageQuality = function(e) {
                e = parseFloat(e), !isNaN(e) && e >= 0 && 1 >= e && (_ = e)
            }, this.setAreaType = function(e) {
                var r = u.getSize(),
                    s = u.getMinSize(),
                    o = u.getX(),
                    a = u.getY(),
                    g = t;
                "square" === e && (g = i), u = new g(c, n), u.setMinSize(s), u.setSize(r), u.setX(o), u.setY(a), null !== l && u.setImage(l), h()
            }, c = o[0].getContext("2d"), u = new t(c, n), e.on("mousemove", S), o.on("mousedown", z), e.on("mouseup", I), e.on("touchmove", S), o.on("touchstart", z), e.on("touchend", I), this.destroy = function() {
                e.off("mousemove", S), o.off("mousedown", z), e.off("mouseup", S), e.off("touchmove", S), o.off("touchstart", z), e.off("touchend", S), o.remove()
            }
        }
    }]), e.factory("cropPubSub", [function() {
        return function() {
            var e = {};
            this.on = function(t, i) {
                return t.split(" ").forEach(function(t) {
                    e[t] || (e[t] = []), e[t].push(i)
                }), this
            }, this.trigger = function(t, i) {
                return angular.forEach(e[t], function(e) {
                    e.call(null, i)
                }), this
            }
        }
    }]), e.directive("imgCrop", ["$timeout", "cropHost", "cropPubSub", function(e, t, i) {
        return {
            restrict: "E",
            scope: {
                images: "=",
                resultImage: "=",
                changeOnFly: "=",
                areaType: "@",
                areaMinSize: "=",
                resultImageSize: "=",
                resultImageFormat: "@",
                resultImageQuality: "=",
                onChange: "&",
                onLoadBegin: "&",
                onLoadDone: "&",
                onLoadError: "&"
            },
            template: "<canvas></canvas>",
            controller: ["$scope", function(e) {
                e.events = new i
            }],
            link: function(i, r) {
                var s, o = i.events,
                    a = new t(r.find("canvas"), {}, o),
                    n = function(e) {
                        var t = a.getResultImageDataURI();
                        s !== t && (s = t, angular.isDefined(e.resultImage) && (e.resultImage = t), e.onChange({
                            $dataURI: e.resultImage
                        }))
                    },
                    h = function(t) {
                        return function() {
                            e(function() {
                                i.$apply(function(e) {
                                    t(e)
                                })
                            })
                        }
                    };
                o.on("load-start", h(function(e) {
                    e.onLoadBegin({})
                })).on("load-done", h(function(e) {
                    e.onLoadDone({})
                })).on("load-error", h(function(e) {
                    e.onLoadError({})
                })).on("area-move area-resize", h(function(e) {
                    e.changeOnFly && n(e)
                })).on("area-move-end area-resize-end image-updated", h(function(e) {
                    n(e)
                })), i.$watch("images", function() {
                    a.setNewImageSource(i.images)
                }), i.$watch("areaType", function() {
                    a.setAreaType(i.areaType), n(i)
                }), i.$watch("areaMinSize", function() {
                    a.setAreaMinSize(i.areaMinSize), n(i)
                }), i.$watch("resultImageSize", function() {
                    a.setResultImageSize(i.resultImageSize), n(i)
                }), i.$watch("resultImageFormat", function() {
                    a.setResultImageFormat(i.resultImageFormat), n(i)
                }), i.$watch("resultImageQuality", function() {
                    a.setResultImageQuality(i.resultImageQuality), n(i)
                }), i.$watch(function() {
                    return [r[0].clientWidth, r[0].clientHeight]
                }, function(e) {
                    a.setMaxDimensions(e[0], e[1]), n(i)
                }, !0), i.$on("$destroy", function() {
                    a.destroy()
                })
            }
        }
    }])
}();