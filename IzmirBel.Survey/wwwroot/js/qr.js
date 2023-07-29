﻿window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute("data-url");

    new QRCode(document.getElementById("qrCode"), {
        text: uri,
        width: 128,
        height: 128
    });
});