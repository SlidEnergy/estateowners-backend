(function () {

    const url = '/api/v1/usersignatures',
        canvas = document.getElementById('paintarea'),
        vcanvas = document.createElement('canvas'),
        ctx = canvas.getContext('2d'),
        vctx = vcanvas.getContext('2d'),
        pad = 35;

    let brushPath = []
    drawing = false,
        dotSize = 10,
        color = '#000000',
        maxP = null,
        minP = null;

    attachListeners();

    function submitButton() {
        crop_vcanvas = document.createElement('canvas');
        const width = maxP.x - minP.x + pad * 2,
            height = maxP.y - minP.y + pad * 2;
        crop_vcanvas.width = width;
        crop_vcanvas.height = height;
        crop_vctx = crop_vcanvas.getContext('2d');
        crop_vctx.drawImage(vcanvas,
            minP.x - pad, minP.y - pad, width, height,
            0, 0, width, height);

        crop_vcanvas.toBlob(submitWrap, 'image/webp', 0.1);
    }

    function submitWrap(blob) {
        var reader = new FileReader();
        reader.readAsDataURL(blob);
        reader.onloadend = function () {
            var base64data = reader.result;
            console.log(base64data);
            submit(base64data)
        }
    }

    function submit(base64data) {
        console.log("Submitting");
        //let formData = new FormData();

        //formData.append('salt', TelegramGameProxy.initParams['salt']);
        //formData.append('payload', TelegramGameProxy.initParams['payload']);
        //formData.append('drawing', imgBlob);

        let model = {
            salt: TelegramGameProxy.initParams['salt'],
            payload: TelegramGameProxy.initParams['payload'],
            base64Image: base64data
        }

        let XHR = new XMLHttpRequest();

        XHR.addEventListener('load', event => {
            if (event.target.status === 202) {
                console.log("Request complete!");
                document.getElementById('submitted').style.visibility = 'visible';
                detachListeners();
            }
        });
        XHR.addEventListener('error', event => {
            console.log("Status: " + event.target.status);
        });
        XHR.open('POST', url);
        XHR.setRequestHeader('Content-type', 'application/json; charset=utf-8');
        XHR.send(JSON.stringify(model));
    }


    function attachListeners() {
        // Drawing buttons
        //document.getElementById('small-dot').onclick = () => { dotSize = 3 };
        //document.getElementById('medium-dot').onclick = () => { dotSize = 6 };
        //document.getElementById('large-dot').onclick = () => { dotSize = 10 };
        //document.getElementById('color').onchange = (e) => { color = e.target.value };
        document.getElementById('submit').onclick = submitButton;

        // Drawing event handlers (bound to mouse, redirected from touch)
        canvas.addEventListener('mousedown', onMouseDown, false);
        canvas.addEventListener('mouseup', onMouseUp, false);
        canvas.addEventListener('mousemove', onMouseMove, false);

        // Touch event redirect
        canvas.addEventListener('touchstart', onTouchStart, false);
        canvas.addEventListener('touchend', onTouchEnd, false);
        canvas.addEventListener('touchcancel', onTouchEnd, false);
        canvas.addEventListener('touchmove', onTouchMove, false);

        // Dynamic canvas size
        window.addEventListener('resize', resizeCanvas, false);
        window.addEventListener("load", init, false);
        //window.addEventListener("orientationchange", rotatePage, false);

        document.getElementById('mobile-start').onclick = mobileInit;
    }

    function detachListeners() {
        //document.getElementById('small-dot').onclick = null;
        //document.getElementById('medium-dot').onclick = null;
        //document.getElementById('large-dot').onclick = null;
        //document.getElementById('color').onchange = null;
        document.getElementById('submit').onclick = null;
        canvas.removeEventListener('mousedown', onMouseDown, false);
        canvas.removeEventListener('mouseup', onMouseUp, false);
        canvas.removeEventListener('mousemove', onMouseMove, false);
        canvas.removeEventListener('touchstart', onTouchStart, false);
        canvas.removeEventListener('touchend', onTouchEnd, false);
        canvas.removeEventListener('touchcancel', onTouchEnd, false);
        canvas.removeEventListener('touchmove', onTouchMove, false);
        window.removeEventListener('resize', resizeCanvas, false);
        window.removeEventListener("load", init, false);
        //window.removeEventListener("orientationchange", rotatePage, false);

        document.getElementById('mobile-start').onclick = null;
    }

    function onMouseDown(e) {
        brushPath.push({ x: e.clientX, y: e.clientY });

        // Temporary canvas drawing
        drawing = true;
        ctx.beginPath();
        ctx.arc(e.clientX, e.clientY, dotSize / 2, 0, Math.PI * 2);
        ctx.fillStyle = color;
        ctx.fill();
        ctx.beginPath();
        ctx.moveTo(e.clientX, e.clientY);
    }

    function onMouseUp(e) {
        drawing = false;
        finalizePath();
    }

    function onMouseMove(e) {
        if (drawing) {
            // Virtual Canvas handling
            brushPath.push({ x: e.clientX, y: e.clientY });

            // Temporary canvas handling
            ctx.lineTo(e.clientX, e.clientY);
            ctx.lineWidth = dotSize;
            ctx.lineCap = 'round';
            ctx.strokeStyle = color;
            ctx.stroke();
        }
    }

    function finalizePath() {
        if (!maxP || !minP) {
            maxP = { x: 0, y: 0 };
            minP = { x: window.innerWidth, y: window.innerHeight };
        }
        for (point in brushPath) {
            maxP.x = Math.max(maxP.x, brushPath[point].x);
            maxP.y = Math.max(maxP.y, brushPath[point].y);
            minP.x = Math.min(minP.x, brushPath[point].x);
            minP.y = Math.min(minP.y, brushPath[point].y);
        }

        if (brushPath.length == 1) {
            vctx.beginPath();
            vctx.arc(brushPath[0].x, brushPath[0].y,
                dotSize / 2, 0, Math.PI * 2);
            vctx.fillStyle = color;
            vctx.fill();
        } else {
            vctx.beginPath();
            vctx.moveTo(brushPath[0].x, brushPath[0].y);
            for (i = 1; i < brushPath.length - 2; i++) {
                let mid = {
                    x: (brushPath[i].x + brushPath[i + 1].x) / 2,
                    y: (brushPath[i].y + brushPath[i + 1].y) / 2
                };
                vctx.quadraticCurveTo(brushPath[i].x, brushPath[i].y, mid.x, mid.y);
            }
            ctx.quadraticCurveTo(brushPath[i].x, brushPath[i].y,
                brushPath[i + 1].x, brushPath[i + 1].y);
            vctx.lineWidth = dotSize;
            vctx.lineCap = 'round';
            vctx.strokeStyle = color;
            vctx.stroke();
        }
        brushPath = [];
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(vcanvas, 0, 0);
    }


    function onTouchStart(e) {
        preventTouchClick(e);
        canvas.dispatchEvent(new MouseEvent('mousedown', {
            clientX: e.touches[0].clientX,
            clientY: e.touches[0].clientY
        }));
    }

    function onTouchEnd(e) {
        preventTouchClick(e);
        canvas.dispatchEvent(new MouseEvent('mouseup', {}));
    }

    function onTouchMove(e) {
        preventTouchClick(e);
        canvas.dispatchEvent(new MouseEvent('mousemove', {
            clientX: e.touches[0].clientX,
            clientY: e.touches[0].clientY
        }));
    }
    function preventTouchClick(e) {
        if (e.target === canvas) {
            e.preventDefault();
        }
    }

    function init() {
        //color = document.getElementById('color').value;

        if (isMobile()) {
            document.getElementById("mobile-start").style.visibility = 'visible';
        } else {
            show();
        }
    }

    function isMobile() {
        return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    }

    function mobileInit() {
        fullScreen();
        rotatePage();

    }

    function show() {
        document.getElementById("paintarea").style.visibility = 'visible';
        document.getElementById("buttonbar").style.visibility = 'visible';

        resizeCanvas();
    }

    function rotatePage() {
        if (isPortrait()) {
            screen.orientation.lock("landscape-primary")
                .then(function () {
                    show();
                    document.getElementById("mobile-start").style.visibility = 'hidden';
                })
                .catch(function (error) {
                    alert(error);
                });
            //document.body.style.transform = 'rotate(-90deg)';
        }
    }

    function fullScreen() {
        // we still need prefixed methods for Chrome & Safari
        if (document.querySelector("body").requestFullscreen)
            document.querySelector("body").requestFullscreen();
        else if (document.querySelector("body").webkitRequestFullScreen)
            document.querySelector("body").webkitRequestFullScreen();
    }

    function isPortrait() {
        if (screen != undefined) {
            return screen.availHeight > screen.availWidth;
        }

        return window.innerHeight > window.innerWidth;
    }

    function resizeCanvas() {
        canvas.width = window.innerWidth;
        vcanvas.width = window.innerWidth;
        canvas.height = window.innerHeight;
        vcanvas.height = window.innerHeight;
        maxP = null;
        minP = null;
    }

})();
