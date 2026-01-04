window.dialog = {
    toast: (text, duration) => {
        const elem = document.getElementById("toast");
        if (elem) {
            elem.innerHTML = text;
            elem.style.opacity = 1;

            setTimeout(() => {
                elem.style.opacity = 0;
            }, duration);
        }
    }
};
