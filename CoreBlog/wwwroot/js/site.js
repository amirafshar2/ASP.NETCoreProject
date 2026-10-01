// CoreBlog – kleine Helfer für Website und Panels
(function () {
    "use strict";

    // Hinweise (Toasts) automatisch ausblenden
    document.querySelectorAll(".flash").forEach(function (el) {
        var close = function () { el.remove(); };
        el.querySelector("[data-flash-close]")?.addEventListener("click", close);
        setTimeout(close, 6000);
    });

    // Mobiles Menü der Website
    var toggle = document.querySelector("[data-nav-toggle]");
    if (toggle) {
        toggle.addEventListener("click", function () {
            var menu = document.getElementById("site-menu");
            var open = menu.classList.toggle("is-open");
            toggle.setAttribute("aria-expanded", open);
        });
    }

    // Seitenleiste im Panel (mobil)
    document.querySelectorAll("[data-sidebar-toggle]").forEach(function (btn) {
        btn.addEventListener("click", function (e) {
            e.stopPropagation();
            document.querySelector(".sidebar")?.classList.toggle("is-open");
        });
    });
    document.addEventListener("click", function (e) {
        var sb = document.querySelector(".sidebar.is-open");
        if (sb && !sb.contains(e.target)) sb.classList.remove("is-open");
    });

    // Newsletter-Anmeldung per AJAX
    document.querySelectorAll("form[data-newsletter]").forEach(function (form) {
        form.addEventListener("submit", function (e) {
            e.preventDefault();
            var result = form.querySelector(".result");
            var button = form.querySelector("button");
            button.disabled = true;
            fetch(form.action, { method: "POST", body: new FormData(form) })
                .then(function (r) { return r.json(); })
                .then(function (data) {
                    result.textContent = data.message;
                    if (data.ok) form.querySelector("input[type=email]").value = "";
                })
                .catch(function () { result.textContent = "Die Anmeldung hat nicht funktioniert. Bitte versuchen Sie es erneut."; })
                .finally(function () { button.disabled = false; });
        });
    });

    // Bestätigungsdialog für Löschen & Co.: <form data-confirm="Text" data-confirm-title="…" data-confirm-button="…">
    var modalEl = document.getElementById("confirmModal");
    if (modalEl && window.bootstrap) {
        var modal = new bootstrap.Modal(modalEl);
        var pending = null;
        document.addEventListener("submit", function (e) {
            var form = e.target;
            if (!form.matches("form[data-confirm]") || form.dataset.confirmed === "1") return;
            e.preventDefault();
            pending = form;
            document.getElementById("confirmTitle").textContent = form.dataset.confirmTitle || "Wirklich löschen?";
            document.getElementById("confirmText").textContent = form.dataset.confirm;
            document.getElementById("confirmOk").textContent = form.dataset.confirmButton || "Löschen";
            modal.show();
        }, true);
        document.getElementById("confirmOk").addEventListener("click", function () {
            if (!pending) return;
            pending.dataset.confirmed = "1";
            modal.hide();
            pending.requestSubmit ? pending.requestSubmit() : pending.submit();
        });
    }

    // Bildvorschau bei Datei-Upload: <input type=file data-preview="#img">
    document.querySelectorAll("input[type=file][data-preview]").forEach(function (input) {
        input.addEventListener("change", function () {
            var file = input.files && input.files[0];
            var img = document.querySelector(input.dataset.preview);
            if (!file || !img) return;
            img.src = URL.createObjectURL(file);
            document.querySelectorAll("input[name=Image][type=radio]").forEach(function (r) { r.checked = false; });
        });
    });

    // Galerie-Auswahl aktualisiert die Vorschau
    document.querySelectorAll("input[type=radio][data-gallery]").forEach(function (radio) {
        radio.addEventListener("change", function () {
            var img = document.querySelector(radio.dataset.gallery);
            if (img) img.src = radio.value;
            var file = document.querySelector("input[type=file][data-preview]");
            if (file) file.value = "";
        });
    });

    // Zeichenzähler: <textarea data-count="#id">
    document.querySelectorAll("[data-count]").forEach(function (field) {
        var out = document.querySelector(field.dataset.count);
        var max = field.getAttribute("maxlength");
        var update = function () {
            var words = field.value.trim() ? field.value.trim().split(/\s+/).length : 0;
            out.textContent = field.value.length.toLocaleString("de-DE") + (max ? " / " + Number(max).toLocaleString("de-DE") : "") + " Zeichen" + (field.dataset.words ? ", " + words + " Wörter, ca. " + Math.max(1, Math.ceil(words / 200)) + " Min. Lesezeit" : "");
        };
        field.addEventListener("input", update);
        update();
    });
})();
