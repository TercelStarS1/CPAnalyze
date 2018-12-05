/**
* cvi_busy_lib.js 1.21 (20-Mar-2009) (c) by Christian Effenberger 
* All Rights Reserved. Source: busy.netzgesta.de
* Distributed under Netzgestade Software License Agreement.
* This license permits free of charge use on non-commercial 
* and private web sites only under special conditions. 
* Read more at... http://www.netzgesta.de/cvi/LICENSE.txt
* syntax:
 
Add:
OBJECT = getBusyOverlay(parent[,overlay[,busy]]);

parent		== element to add the overlay (e.g. document.getElementById(id) or 'viewport')

overlay 	== OBJECT e.g. {color: 'black', opacity: 0.5, ...}
color	== STR 'black' or '#000000' or 'rgb(0,0,0)' Default: 'white'
opacity	== FLOAT 0.0 - 1.0  Default: 0.0
text	== STR e.g. "loading" Default: ''
style	== STR e.g. "color: black;" or "my_text_class" Default: ''

busy		== OBJECT e.g. {color: '#fff', size: 48, ...}
color	== STR '#000000' - '#ffffff' or '#000' - '#fff' Default: '#000'
size	== INT 16 - 512 (pixel) Default: 32
type	== STR 'circle|oval|polygon|rectangle|tube' or 'c|o|p|r|t' Default: 'tube'
iradius	== INT 6 - 254 (pixel) Default: 8
weight	== INT 1 - 254 (pixel) Default: 3
count	== INT 5 - 36 (rays) Default: 12
speed	== INT 30 - 1000 (millisec) Default: 96
minopac	== FLOAT 0.0 - 0.5  Default: 0.25

Remove:
OBJECT.remove();
	
Set Overlay text:
OBJECT.settext(string);

*
**/
 

function onIEWinResize(event) {
    function parseWidth(val) { return (isNaN(parseInt(val, 10)) ? 0 : parseInt(val, 10)); }
    if (!event) { event = window.event; } var i, cs, parent = this, div = parent.getElementsByTagName("div");
    if (div.length > 0) {
        if (parent.currentStyle) { cs = parent.currentStyle; } else if (document.defaultView && document.defaultView.getComputedStyle) { cs = document.defaultView.getComputedStyle(parent, ""); } else { cs = parent.style; }
        for (i = 0; i < div.length; i++) {
            if (div[i].className == 'buzy_ele') {
                div[i].style.height = (parent.offsetHeight - parseWidth(cs.borderBottomWidth) - parseWidth(cs.borderTopWidth));
                div[i].style.width = (parent.offsetWidth - parseWidth(cs.borderLeftWidth) - parseWidth(cs.borderRightWidth));
                div[i].firstChild.style.height = div[i].style.height; div[i].firstChild.style.width = div[i].style.width;
                break;
            }
        }
    }
}
function onIEVPResize(event) {
    if (!event) { event = window.event; } var vp = document.getElementById('viewport'); if (vp) {
        if (typeof document.documentElement != 'undefined') { vp.style.width = document.documentElement.clientWidth + 'px'; vp.style.height = document.documentElement.clientHeight + 'px'; }
        else { vp.style.width = document.getElementsByTagName('body')[0].clientWidth + 'px'; vp.style.height = document.getElementsByTagName('body')[0].clientHeight + 'px'; }
    }
}
function onIEVPScroll(event) {
    if (!event) { event = window.event; } var vp = document.getElementById('viewport'); if (vp) {
        if (typeof document.documentElement != 'undefined') { vp.style.left = document.documentElement.scrollLeft + 'px'; vp.style.top = document.documentElement.scrollTop + 'px'; }
        else { vp.style.left = document.getElementsByTagName('body')[0].scrollLeft + 'px'; vp.style.top = document.getElementsByTagName('body')[0].scrollTop + 'px'; }
    }
}

function getBusyOverlay(id, overlay, busy, divId) {

    if ((typeof (id) === 'string') && document.getElementsByTagName) {
        function parseWidth(val) {
            return (isNaN(parseInt(val, 10)) ? 0 : parseInt(val, 10));
        }
        var parent, isIE, isVL, isCV, isWK, isGE, i, b, o, lt, rt, lb, rb, cz, cs, size, viewport, inner, outer, string, canvas, context, ctrl, opacity, color, text, styles, waiting = true;
        //创建id为viewport的DIV，并为其赋样式
        if (divId != "") {
            console.log(document.getElementById(divId));
            //新写的
            viewport = document.createElement('div'); //不存在则创建
            viewport.id = 'viewport';
            cz = viewport.style; //开始设置div样式
            cz.backgroundColor = 'transparent'; //属性设置元素的背景颜色，transparent默认。背景颜色为透明
            cz.position = 'relative'; //一个固定定位（position属性的值为fixed）元素会相对于视窗来定位，这意味着即便页面滚动，它还是会停留在相同的位置。
            cz.overflow = 'hidden'; //内容会被修剪，并且其余内容是不可见的。
            cz.display = 'block'; //此元素将显示为块级元素，此元素前后会带有换行符。
            cz.zIndex = 999999;
            cz.left = '0px';
            cz.top = '0px';
            cz.zoom = 1;
            cz.width = '100%';
            cz.height = document.getElementById(divId).offsetHeight == 0 ? '150px' : document.getElementById(divId).offsetHeight - 10 + 'px';
            cz.margin = '0px'; //外边距
            cz.padding = '0px'; //内边距
            document.getElementById(divId).appendChild(viewport);
            parent = viewport; //给parent赋予div的意义
        } else {
            //原逻辑
            if (id == 'viewport') {
                viewport = document.getElementById('viewport'); //获取DOM对象
                if (!viewport) {
                    viewport = document.createElement('div'); //不存在则创建
                    viewport.id = 'viewport';
                    cz = viewport.style; //开始设置div样式
                    cz.backgroundColor = 'transparent'; //属性设置元素的背景颜色，transparent默认。背景颜色为透明
                    cz.position = 'fixed'; //一个固定定位（position属性的值为fixed）元素会相对于视窗来定位，这意味着即便页面滚动，它还是会停留在相同的位置。
                    cz.overflow = 'hidden'; //内容会被修剪，并且其余内容是不可见的。
                    cz.display = 'block'; //此元素将显示为块级元素，此元素前后会带有换行符。
                    cz.zIndex = 999999;
                    cz.left = '0px';
                    cz.top = '0px';
                    cz.zoom = 1;
                    cz.width = '100%';
                    cz.height = '100%';
                    cz.margin = '0px'; //外边距
                    cz.padding = '0px'; //内边距
                    if (document.all && !window.opera && !window.XMLHttpRequest) {//此处走不通，!window.XMLHttpRequest=false
                        cz.position = 'absolute';
                        if (typeof document.documentElement != 'undefined') {
                            cz.width = document.documentElement.clientWidth + 'px';
                            cz.height = document.documentElement.clientHeight + 'px';
                        }
                        else {
                            cz.width = document.getElementsByTagName('body')[0].clientWidth + 'px';
                            cz.height = document.getElementsByTagName('body')[0].clientHeight + 'px';
                        }
                    }
                    document.getElementsByTagName("body")[0].appendChild(viewport); //创建的viewportDIV添加到body中
                } else {
                    viewport.style.display = 'block';
                    if (document.all && !window.opera && !window.XMLHttpRequest) {
                        if (typeof document.documentElement != 'undefined') {
                            viewport.style.width = document.documentElement.clientWidth + 'px';
                            viewport.style.height = document.documentElement.clientHeight + 'px';
                        }
                        else {
                            viewport.style.width = document.getElementsByTagName('body')[0].clientWidth + 'px';
                            viewport.style.height = document.getElementsByTagName('body')[0].clientHeight + 'px';
                        }
                    }
                }
                parent = viewport; //给parent赋予div的意义
            }
        }

        //创建CSS样式
        if (parent.currentStyle) {//parent.currentStyle = undefined，进不去if
            cs = parent.currentStyle;
        }
        else if (document.defaultView && document.defaultView.getComputedStyle) {
            cs = document.defaultView.getComputedStyle(parent, ""); //创建一个CSSStyleDeclaration对象
        } else {
            cs = parent.style;
        }
        while (cs.display.search(/block|inline-block|table|inline-table|list-item/i) < 0) {//此处值=0，无法进入if
            parent = parent.parentNode;
            if (parent.currentStyle) { cs = parent.currentStyle; }
            else if (document.defaultView && document.defaultView.getComputedStyle) { cs = document.defaultView.getComputedStyle(parent, ""); }
            else { cs = parent.style; } if (parent.tagName.toUpperCase() == 'BODY') { parent = ""; }
        }

        if (typeof (parent) === 'object') {
            //=false，无法进入
            if (!overlay) {
                overlay = new Object();
                overlay['opacity'] = 0;
                overlay['color'] = 'white';
                overlay['text'] = '';
                overlay['style'] = '';
            }
            //=false，无法进入
            if (!busy) {
                busy = new Object();
                busy['size'] = 32;
                busy['color'] = '#000';
                busy['type'] = 'tube';
                busy['iradius'] = 8;
                busy['weight'] = 3;
                busy['count'] = 12;
                busy['speed'] = 96;
                busy['minopac'] = .25;
            }

            //此处是传进来的4个参数
            //overlay['opacity']为传入的参数=0.75，opacity=0.75
            opacity = Math.max(0.0, Math.min(1.0, (typeof overlay['opacity'] === 'number' ? overlay['opacity'] : 0) || 0));
            //获取设置的颜色
            color = (typeof overlay['color'] === 'string' ? overlay['color'] : 'white');
            text = (typeof overlay['text'] === 'string' ? overlay['text'] : '');
            styles = (typeof overlay['style'] === 'string' ? overlay['style'] : '');

            //在网页上创建Canvas元素时，他会创建一块矩形区域。默认情况下该区域300px*150px, 使用Canvas编程，首先要获取其上下文(context)。
            //接着在上下文中知心动作，最后将这些动作应用到上下文。可以将Canvas的这种编程方式想象成数据库事物：开发人员发起一个事务然后执行某些操作，最后提交事务。
            //Canvas的坐标是从左上角开始，X轴沿水平方向，Y轴沿垂直向下延伸，左上角坐标为X=0，Y=0 的点称作原点。
            canvas = document.createElement("canvas"); //创建canvas元素
            isCV = canvas.getContext ? 1 : 0; //=1
            isWK = navigator.userAgent.indexOf('WebKit') > -1 ? 1 : 0; //=1 //navigator.userAgent.indexOf('WebKit')=48
            isGE = navigator.userAgent.indexOf('Gecko') > -1 && window.updateCommands ? 1 : 0; //=0 //navigator.userAgent.indexOf('Gecko')=68 window.updateCommands=undefined
            isIE = navigator.appName == 'Microsoft Internet Explorer' && window.navigator.systemLanguage && !window.opera ? 1 : 0; //=0 //navigator.appName=Netscape，window.navigator.systemLanguage=undefined，!window.opera=true
            isVL = document.all && document.namespaces ? 1 : 0; //=0 //document.namespaces=undefined


            //outer 遮罩层
            outer = document.createElement('div');
            parent.style.position = (cs.position == 'static' ? 'relative' : cs.position); //=fixed
            cz = parent.style.zIndex >= 0 ? (parent.style.zIndex - 0 + 2) : 2; //=1000001 //parent.style.zIndex=999999
            //因为isIE=0，所以无法进入if
            if (isIE && !cs.hasLayout) {
                parent.style.zoom = 1;
            }

            outer.style.position = 'relative'; //生成绝对定位的元素，相对于 static 定位以外的第一个父元素进行定位。元素的位置通过 "left", "top", "right" 以及 "bottom" 属性进行规定。
            outer.style.overflow = 'hidden'; //内容会被修剪，并且其余内容是不可见的。
            outer.style.display = 'block'; //此元素将显示为块级元素，此元素前后会带有换行符。
            outer.style.zIndex = cz; //=1000001
            outer.style.left = 0 + 'px';
            outer.style.top = 0 + 'px';
            outer.style.width = '100%';
            outer.style.height = '100%';
            outer.style.textAlign = "center"; //后添加，为了让canvas元素的动态元素居中

            //isIE=0，无法进入if
            if (isIE) {
                outer.className = 'buzy_ele';
                outer.style.zoom = 1;
                outer.style.margin = '0px';
                outer.style.padding = '0px';
                outer.style.height = (parent.offsetHeight - parseWidth(cs.borderBottomWidth) - parseWidth(cs.borderTopWidth));
                outer.style.width = (parent.offsetWidth - parseWidth(cs.borderLeftWidth) - parseWidth(cs.borderRightWidth));
            }

            //cs.borderRadius=0px，所以类型等于string，无法进入if
            if (typeof (cs.borderRadius) == "undefined") {
                if (typeof (cs.MozBorderRadius) != "undefined") {
                    lt = parseFloat(cs.MozBorderRadiusTopleft) - Math.min(parseFloat(cs.borderLeftWidth), parseFloat(cs.borderTopWidth));
                    rt = parseFloat(cs.MozBorderRadiusTopright) - Math.min(parseFloat(cs.borderRightWidth), parseFloat(cs.borderTopWidth));
                    lb = parseFloat(cs.MozBorderRadiusBottomleft) - Math.min(parseFloat(cs.borderLeftWidth), parseFloat(cs.borderBottomWidth));
                    rb = parseFloat(cs.MozBorderRadiusBottomright) - Math.min(parseFloat(cs.borderRightWidth), parseFloat(cs.borderBottomWidth));
                    outer.style.MozBorderRadiusTopleft = lt + "px";
                    outer.style.MozBorderRadiusTopright = rt + "px";
                    outer.style.MozBorderRadiusBottomleft = lb + "px";
                    outer.style.MozBorderRadiusBottomright = rb + "px";
                } else if (typeof (cs.WebkitBorderRadius) != "undefined") {
                    lt = parseFloat(cs.WebkitBorderTopLeftRadius) - Math.min(parseFloat(cs.borderLeftWidth), parseFloat(cs.borderTopWidth));
                    rt = parseFloat(cs.WebkitBorderTopRightRadius) - Math.min(parseFloat(cs.borderRightWidth), parseFloat(cs.borderTopWidth));
                    lb = parseFloat(cs.WebkitBorderBottomLeftRadius) - Math.min(parseFloat(cs.borderLeftWidth), parseFloat(cs.borderBottomWidth));
                    rb = parseFloat(cs.WebkitBorderBottomRightRadius) - Math.min(parseFloat(cs.borderRightWidth), parseFloat(cs.borderBottomWidth));
                    outer.style.WebkitBorderTopLeftRadius = lt + "px";
                    outer.style.WebkitBorderTopRightRadius = rt + "px";
                    outer.style.WebkitBorderBottomLeftRadius = lb + "px";
                    outer.style.WebkitBorderBottomRightRadius = rb + "px";
                }
            } else {
                //此处全是0
                lt = parseFloat(cs.borderTopLeftRadius) - Math.min(parseFloat(cs.borderLeftWidth), parseFloat(cs.borderTopWidth));
                rt = parseFloat(cs.borderTopRightRadius) - Math.min(parseFloat(cs.borderRightWidth), parseFloat(cs.borderTopWidth));
                lb = parseFloat(cs.borderBottomLeftRadius) - Math.min(parseFloat(cs.borderLeftWidth), parseFloat(cs.borderBottomWidth));
                rb = parseFloat(cs.borderBottomRightRadius) - Math.min(parseFloat(cs.borderRightWidth), parseFloat(cs.borderBottomWidth));
                outer.style.borderTopLeftRadius = lt + "px";
                outer.style.borderTopRightRadius = rt + "px";
                outer.style.borderBottomLeftRadius = lb + "px";
                outer.style.borderBottomRightRadius = rb + "px";
            }
            parent.appendChild(outer);


            //inner
            inner = document.createElement('div');
            inner.style.position = 'absolute'; //生成绝对定位的元素，相对于 static 定位以外的第一个父元素进行定位。元素的位置通过 "left", "top", "right" 以及 "bottom" 属性进行规定。
            inner.style.cursor = 'progress'; //cursor 属性规定所显示的指针（光标）的类型。
            inner.style.display = 'block';
            inner.style.zIndex = (cz - 1); //=100000
            inner.style.left = 0 + 'px';
            inner.style.top = 0 + 'px';
            inner.style.width = 100 + '%';
            inner.style.height = 100 + '%';
            inner.style.backgroundColor = color; //传递的参数设置颜色

            //isIE=0，无法进入if
            if (isIE) {
                inner.style.zoom = 1;
                inner.style.margin = '0px';
                inner.style.padding = '0px';
                inner.style.height = outer.style.height;
                inner.style.width = outer.style.width;
            }

            //cs.borderRadius=0px，类型为string，无法进入if
            if (typeof (cs.borderRadius) == "undefined") {
                if (typeof (cs.MozBorderRadius) != "undefined") {
                    inner.style.MozBorderRadiusTopleft = lt + "px";
                    inner.style.MozBorderRadiusTopright = rt + "px";
                    inner.style.MozBorderRadiusBottomleft = lb + "px";
                    inner.style.MozBorderRadiusBottomright = rb + "px";
                } else if (typeof (cs.WebkitBorderRadius) != "undefined") {
                    inner.style.WebkitBorderTopLeftRadius = lt + "px";
                    inner.style.WebkitBorderTopRightRadius = rt + "px";
                    inner.style.WebkitBorderBottomLeftRadius = lb + "px";
                    inner.style.WebkitBorderBottomRightRadius = rb + "px";
                }
            } else {
                inner.style.borderTopLeftRadius = lt + "px";
                inner.style.borderTopRightRadius = rt + "px";
                inner.style.borderBottomLeftRadius = lb + "px";
                inner.style.borderBottomRightRadius = rb + "px";
            }

            if (isIE) {
                inner.style.filter = "progid:DXImageTransform.Microsoft.Alpha(opacity=" + parseInt(opacity * 100) + ")";
            } else {
                inner.style.opacity = opacity; //设置 div 元素的不透明级别：从 0.0 （完全透明）到 1.0（完全不透明）。
            }
            outer.appendChild(inner);


            //busy['size']为设置的参数100，所以size=100
            size = Math.max(16, Math.min(512, (typeof busy['size'] === 'number' ? (busy['size'] == 0 ? 32 : busy['size']) : 32)));

            //isVL=0，无法进入if
            if (isVL) {
                if (document.namespaces['v'] == null) {
                    var e = ["shape", "shapetype", "group", "background", "path", "formulas", "handles", "fill", "stroke", "shadow", "textbox", "textpath", "imagedata", "line", "polyline", "curve", "roundrect", "oval", "rect", "arc", "image"], s = document.createStyleSheet();
                    for (i = 0; i < e.length; i++) {
                        s.addRule("v\\:" + e[i], "behavior: url(#default#VML);");
                    }
                    document.namespaces.add("v", "urn:schemas-microsoft-com:vml");
                }
            }

            //isCV=1,!isCV无法进入if
            if (!isCV) {
                canvas = document.createElement("div"); //创建div
            }

            canvas.style.position = 'relative'; //生成绝对定位的元素，相对于 static 定位以外的第一个父元素进行定位。元素的位置通过 "left", "top", "right" 以及 "bottom" 属性进行规定。
            canvas.style.cursor = 'progress'; //cursor 属性规定所显示的指针（光标）的类型。
            canvas.style.zIndex = (cz - 0 + 1); //=1000002
            canvas.style.top = '35%';   //15
            canvas.style.left = '0%';
            //canvas.style.marginTop = '-' + (size / 2) + 'px'; //=-50px //size=100
            //canvas.style.marginLeft = '-' + (size / 2) + 'px'; //=-50px //size=100
            canvas.width = size; //100
            canvas.height = size; //100
            canvas.style.width = size / 2 + "px"; //100px
            canvas.style.height = size / 2 + "px"; //100px
            outer.appendChild(canvas); //生成一个100*100的canvas元素，居中在outer背景中

            //居中显示的汉字
            //text = "viewport: loading..."   text为传递过来的参数
            if (text != "") {
                string = document.createElement('div');
                string.style.position = 'absolute';
                string.style.overflow = 'hidden';
                string.style.cursor = 'progress';
                string.style.zIndex = (cz - 0 + 1); //=1000002
                string.style.top = '50%';
                string.style.left = '0px';
                string.style.marginTop = 2 + (size / 2) + 'px'; //52px
                string.style.textAlign = 'center'; //居中
                string.style.width = 100 + '%';
                string.style.height = 'auto'; //默认。浏览器会计算出实际的高度。
                //传递过来的参数
                if (styles != "") {
                    //"<span style="text-shadow: 0 0 3px black;font-weight:bold;font-size:16px;color:white">viewport: loading...</span>"
                    string.innerHTML = '<span ' + (styles.match(/:/i) ? 'style' : 'class') + '="' + styles + '">' + text + '</span>';
                }
                else {
                    string.innerHTML = '<span>' + text + '</span>';
                }
                outer.appendChild(string);
            }
            
            //-moz-user-select: none; /*火狐*/
            //-khtml-user-select: none; /*早期浏览器*/
            //isGE=0，无法进入if,  isWK=1,可以进入第二个else if
            if (isGE) {
                outer.style.MozUserSelect = "none";
                inner.style.MozUserSelect = "none";
                canvas.style.MozUserSelect = "none";
            } else if (isWK) {
                outer.style.KhtmlUserSelect = "none"; //用户不能选择文本
                inner.style.KhtmlUserSelect = "none"; //用户不能选择文本
                canvas.style.KhtmlUserSelect = "none"; //用户不能选择文本
            } else if (isIE) {
                outer.unselectable = "on";
                inner.unselectable = "on";
                canvas.unselectable = "on";
            }
            //isVL=0,无法进入if，isCV=1，可以进入else if
            if (isVL) {
                ctrl = getBusyVL(canvas, busy['color'], busy['size'], busy['type'], busy['iradius'], busy['weight'], busy['count'], busy['speed'], busy['minopac']);
                ctrl.start();
            } else if (isCV) {
                //busy['color']=#ff0，busy['size']=100，busy['type']=t，后面全是undefined
                ctrl = getBusyCV(canvas.getContext("2d"), busy['color'], busy['size'], busy['type'], busy['iradius'], busy['weight'], busy['count'], busy['speed'], busy['minopac']);
                ctrl.start();
            }
            else {
                ctrl = getBusy(canvas, busy['color'], busy['size'], busy['type'], busy['iradius'], busy['weight'], busy['count'], busy['speed'], busy['minopac']);
                ctrl.start();
            }

            if (isIE) {
                parent.onresize = onIEWinResize;
                if (parent.id == 'viewport' && !window.XMLHttpRequest) {
                    window.onresize = onIEVPResize;
                    window.onscroll = onIEVPScroll;
                }
            }
            return {
                remove: function() {
                    if (waiting) {
                        waiting = false;
                        ctrl.stop();
                        delete ctrl;
                        parent.removeChild(outer); //去掉遮罩层
                        //if (parent.id == 'viewport') {
                        parent.style.display = 'none'; //隐藏div
                        //}
                    }
                },
                settext: function(v) {
                    if (string && typeof (v) == 'string') {
                        string.firstChild.innerHTML = v;
                        return false;
                    }
                }
            };
        }
    }
}
function getBusy(obj, cl, sz, tp, ir, w, ct, sp, mo) {
    function getHEX(v) {
        var col = v || '#000000';
        if (!col.match(/^#[0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f]$/i)) {
            if (v.match(/^#[0-9a-f][0-9a-f][0-9a-f]$/i)) { col = '#' + v.substr(1, 1) + v.substr(1, 1) + v.substr(2, 1) + v.substr(2, 1) + v.substr(3, 1) + v.substr(3, 1); }
        } return col;
    }
    var running = false, i = 0, os = 0, al = 0, f = 100, c, h, p, t, x, y, v, hp, ph, sh, ele = new Array();
    c = getHEX(cl); tp = tp || "t"; t = (tp.match(/^[coprt]/i) ? tp.substr(0, 1).toLowerCase() : 't');
    ct = Math.max(5, Math.min(36, ct || 12)); sp = Math.max(30, Math.min(1000, sp || 96));
    sz = Math.max(16, Math.min(512, sz || 32)); ir = Math.max(1, Math.min((sz / 2) - 2, ir || sz / 4));
    w = Math.max(1, Math.min((sz / 2) - ir, w || sz / 10)); mo = Math.max(0, Math.min(0.5, mo || 0.25));
    al = 360 / ct; hp = (Math.PI / 2) * -1; ph = Math.PI / 180; w = (t != 'c' ? parseInt((w / 2) * 3) : w); v = parseInt((sz / 2) - (w / 2));
    for (i = 0; i < ct; i++) {
        sh = document.createElement('div'); x = Math.round(v + v * Math.cos(hp + (i + 1) * al * ph)); y = Math.round(v + v * Math.sin(hp + (i + 1) * al * ph));
        sh.style.position = 'absolute'; sh.style.margin = '0px'; sh.style.width = w + 'px'; sh.style.height = w + 'px';
        sh.style.lineHeight = '1px'; sh.style.fontSize = '0px'; sh.style.top = y + 'px'; sh.style.left = x + 'px'; sh.style.backgroundColor = c;
        if (document.all && !window.opera) {
            sh.style.filter = "progid:DXImageTransform.Microsoft.Alpha(opacity=" + parseInt(Math.min(1, Math.max(mo, 1 - ((ct + 1 - i) / (ct + 1)))) * 100) + ")";
        } else { sh.style.opacity = Math.min(1, Math.max(mo, 1 - ((ct + 1 - i) / (ct + 1)))); }
        obj.appendChild(sh); ele[i] = sh;
    }
    function nextLoop() {
        if (!running) { return; } os = (os + 1) % ct;
        if (document.all && !window.opera) { for (i = 0; i < ct; i++) { al = ((os + i) % ct); ele[al].style.filter = "progid:DXImageTransform.Microsoft.Alpha(opacity=" + parseInt(Math.min(1, Math.max(mo, 1 - ((ct + 1 - i) / (ct + 1)))) * 100) + ")"; } }
        else { for (i = 0; i < ct; i++) { al = ((os + i) % ct); ele[al].style.opacity = Math.min(1, Math.max(mo, 1 - ((ct + 1 - i) / (ct + 1)))); } }
        setTimeout(nextLoop, sp);
    }
    nextLoop(0);
    return {
        start: function() { if (!running) { running = true; nextLoop(0); } },
        stop: function() { running = false; for (i = 0; i < ct; i++) { if (document.all && !window.opera) { ele[i].style.filter = "progid:DXImageTransform.Microsoft.Alpha(opacity=0)"; } else { ele[i].setAttribute('opacity', 0); } } },
        pause: function() { running = false; }
    };
}
function getBusyVL(obj, cl, sz, tp, ir, w, ct, sp, mo) {
    function getHEX(v) {
        var col = v || '#000000';
        if (!col.match(/^#[0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f]$/i)) {
            if (v.match(/^#[0-9a-f][0-9a-f][0-9a-f]$/i)) { col = '#' + v.substr(1, 1) + v.substr(1, 1) + v.substr(2, 1) + v.substr(2, 1) + v.substr(3, 1) + v.substr(3, 1); }
        } return col;
    }
    var running = false, os = 0, al = 0, f = 100, c, i, h, p, t, x, y, hs, qs, hw, qw, rp, sh, fl, ele = new Array();
    c = getHEX(cl); tp = tp || "t"; t = (tp.match(/^[coprt]/i) ? tp.substr(0, 1).toLowerCase() : 't');
    ct = Math.max(5, Math.min(36, ct || 12)); sp = Math.max(30, Math.min(1000, sp || 96));
    sz = Math.max(16, Math.min(512, sz || 32)); ir = Math.max(1, Math.min((sz / 2) - 2, ir || sz / 4));
    w = Math.max(1, Math.min((sz / 2) - ir, w || sz / 10)); mo = Math.max(0, Math.min(0.5, mo || 0.25));
    h = (sz / 2) - ir; x = sz / 2; y = x; al = 360 / ct; hs = parseInt((sz / 2) * f); qs = parseInt(hs / 2);
    hw = parseInt((w / 2) * f); qw = parseInt(hw / 2); rp = hs - parseInt(ir * f);
    switch (t) {
        case "c": p = 'm ' + hs + ',' + (rp - hw) + ' ar ' + (hs - hw) + ',' + (rp - hw - hw) + ',' + (hs + hw) + ',' + rp + ',' + (hs - hw) + ',' + (rp - hw - hw) + ',' + (hs - hw) + ',' + (rp - hw - hw) + ' e'; break;
        case "p": p = 'm ' + (hs - qw) + ',0 l ' + (hs - hw) + ',' + rp + ',' + (hs + hw) + ',' + rp + ',' + (hs + qw) + ',0 x e'; break;
        case "o": p = 'm ' + hs + ',' + (rp - qs) + ' ar ' + (hs - hw) + ',0,' + (hs + hw) + ',' + rp + ',' + (hs - hw) + ',0,' + (hs - hw) + ',0 e'; break;
        case "t": p = 'm ' + (hs - hw) + ',' + rp + ' l ' + (hs - hw) + ',' + hw + ' qy ' + hs + ',0 qx ' + (hs + hw) + ',' + hw + ' l ' + (hs + hw) + ',' + rp + ' x e'; break;
        default: p = 'm ' + (hs - hw) + ',0 l ' + (hs - hw) + ',' + rp + ',' + (hs + hw) + ',' + rp + ',' + (hs + hw) + ',0 x e'; break;
    }
    for (i = 0; i < ct; i++) {
        sh = document.createElement('v:shape'); sh.setAttribute('filled', 't'); sh.setAttribute('stroked', 'f');
        sh.setAttribute('coordorigin', '0,0'); sh.setAttribute('coordsize', (sz * f) + ',' + (sz * f));
        sh.setAttribute('path', p); sh.style.rotation = (i * al); sh.style.position = 'absolute'; sh.style.margin = '0px';
        sh.style.width = sz + 'px'; sh.style.height = sz + 'px'; sh.style.top = '-1px'; sh.style.left = '-1px';
        obj.appendChild(sh); fl = document.createElement('v:fill');
        fl.setAttribute('opacity', Math.min(1, Math.max(mo, 1 - ((ct + 1 - i) / (ct + 1)))));
        fl.setAttribute('color', c); sh.appendChild(fl); ele[i] = fl;
    }
    function nextLoop() {
        if (!running) { return; } os = (os + 1) % ct;
        if (document.documentMode == 8) {
            for (i = 0; i < ct; i++) { al = ((os + i) % ct); ele[al].opacity = Math.min(1, Math.max(mo, 1 - ((ct + 1 - i) / (ct + 1)))); }
        } else {
            for (i = 0; i < ct; i++) { al = ((os + i) % ct); ele[al].setAttribute('opacity', Math.min(1, Math.max(mo, 1 - ((ct + 1 - i) / (ct + 1))))); }
        }
        setTimeout(nextLoop, sp);
    }
    nextLoop(0);
    return {
        start: function() { if (!running) { running = true; nextLoop(0); } },
        stop: function() { running = false; for (i = 0; i < ct; i++) { ele[i].setAttribute('opacity', 0); } },
        pause: function() { running = false; }
    };
}

function getBusyCV(ctx, cl, sz, tp, ir, w, ct, sp, mo) {
    //获取颜色TGB码
    function getRGB(v) {
        function hex2dec(h) {
            return (Math.max(0, Math.min(parseInt(h, 16), 255)));
        }
        var r = 0, g = 0, b = 0;
        v = v || '#000';
        //可以进入if
        if (v.match(/^#[0-9a-f][0-9a-f][0-9a-f]$/i)) {
            r = hex2dec(v.substr(1, 1) + v.substr(1, 1)), g = hex2dec(v.substr(2, 1) + v.substr(2, 1)), b = hex2dec(v.substr(3, 1) + v.substr(3, 1));
        } else if (v.match(/^#[0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f]$/i)) {
            r = hex2dec(v.substr(1, 2)), g = hex2dec(v.substr(3, 2)), b = hex2dec(v.substr(5, 2));
        }
        return r + ',' + g + ',' + b;
    }
    function drawOval(ctx, w, h) {
        ctx.beginPath();
        ctx.moveTo(-w / 2, h / 2);
        ctx.quadraticCurveTo(-w / 2, 0, 0, 0);
        ctx.quadraticCurveTo(w / 2, 0, w / 2, h / 2);
        ctx.quadraticCurveTo(w / 2, h, 0, h);
        ctx.quadraticCurveTo(-w / 2, h, -w / 2, h / 2);
        ctx.fill();
    }
    function drawTube(ctx, w, h) {
        ctx.beginPath();
        ctx.moveTo(w / 2, 0);
        ctx.lineTo(-w / 2, 0);
        ctx.lineTo(-w / 2, h - (w / 2));
        ctx.quadraticCurveTo(-w / 2, h, 0, h);
        ctx.quadraticCurveTo(w / 2, h, w / 2, h - (w / 2));
        ctx.fill();
    }
    function drawPoly(ctx, w, h) {
        ctx.beginPath();
        ctx.moveTo(w / 2, 0);
        ctx.lineTo(-w / 2, 0);
        ctx.lineTo(-w / 4, h);
        ctx.lineTo(w / 4, h);
        ctx.fill();
    }
    function drawCirc(ctx, r, z) {
        ctx.beginPath();
        ctx.arc(r, r, r, 0, Math.PI * 2, false);
        ctx.fill();
    }

    var running = false, os = 0, al = 0, c, i, h, t, x, y;

    c = getRGB(cl); //颜色
    tp = tp || "t"; //类型
    t = (tp.match(/^[coprt]/i) ? tp.substr(0, 1).toLowerCase() : 't');
    ct = Math.max(5, Math.min(36, ct || 12)); //12
    sp = Math.max(30, Math.min(1000, sp || 96)); //96
    sz = Math.max(16, Math.min(512, sz || 32)); //100
    ir = Math.max(1, Math.min((sz / 2) - 2, ir || sz / 4)); //25
    w = Math.max(1, Math.min((sz / 2) - ir, w || sz / 10)); //10
    mo = Math.max(0, Math.min(0.5, mo || 0.25)); //0.25
    h = (sz / 2) - ir; //25
    x = sz / 2; //50
    y = x; //50

    //小圆圈
    function nextLoop() {
        //!running=true
        if (!running) {
            return;
        }
        os = (os + 1) % ct;
        ctx.clearRect(0, 0, sz, sz);
        ctx.save();
        ctx.translate(x, y);
        for (i = 0; i < ct; i++) {
            al = 2 * ((os + i) % ct) * Math.PI / ct;
            ctx.save();
            ctx.translate(ir * Math.sin(-al), ir * Math.cos(-al));
            ctx.rotate(al);
            ctx.fillStyle = 'rgba(' + c + ',' + Math.min(1, Math.max(mo, 1 - ((ct + 1 - i) / (ct + 1)))) + ')';
            switch (t) {
                case "c": drawCirc(ctx, w / 2, h); break;
                case "o": drawOval(ctx, w, h); break;
                case "p": drawPoly(ctx, w, h); break;
                case "t": drawTube(ctx, w, h); break;
                default: ctx.fillRect(-w / 2, 0, w, h); break;
            }
            ctx.restore();
        }
        ctx.restore();
        setTimeout(nextLoop, sp);
    }
    nextLoop(0);
    return {
        start: function() { if (!running) { running = true; nextLoop(0); } },
        stop: function() { running = false; ctx.clearRect(0, 0, sz, sz); },
        pause: function() { running = false; }
    };
}



var xval;
function GetAjaxLoad(item,divid) {
    xval = getBusyOverlay('viewport', { color: 'white', opacity: 0.75, text: '', style: 'text-shadow: 0 0 3px black;font-weight:bold;font-size:16px;color:white' }, { color: '#a1a3a6', size: 100, type: 'o' }, divid);

    xval.settext(item);

} 